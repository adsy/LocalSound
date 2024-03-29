import axios, {
  AxiosError,
  AxiosRequestConfig,
  AxiosRequestHeaders,
  AxiosResponse,
} from "axios";
import { UserRegistrationModel } from "../app/model/dto/user-registration.model";
import { UserLoginModel } from "../app/model/dto/user-login.model";
import { UserModel } from "../app/model/dto/user.model";
import { UpdateArtistPersonalDetailsModel } from "../app/model/dto/update-artist-personal.model";
import { GenreModel } from "../app/model/dto/genre.model";
import { AccountImageTypes } from "../app/model/enums/accountImageTypes";
import { AccountImageModel } from "../app/model/dto/account-image.model";
import { UpdateArtistProfileDetailsModel } from "../app/model/dto/update-artist-profile.model";
import { EventTypeModel } from "../app/model/dto/eventType.model";
import { TrackUploadSASModel } from "../app/model/dto/track-upload-sas.model";
import { ArtistTrackModel } from "../app/model/dto/artist-track-upload.model";
import { TrackListResponse } from "../app/model/dto/track-list-response.model";
import { FollowerListResponse } from "../app/model/dto/follower-list-response.model";
import { ArtistPackageModel } from "../app/model/dto/artist-package.model";
import { BookingSubmissionModel } from "../app/model/dto/booking-submission.model";
import { BookingListResponse } from "../app/model/dto/booking-list.-response.model";
import { NotificationListResponseModel } from "../app/model/dto/notification-list-response.model";
import { toast } from "react-toastify";
import { OnboardingDataModel } from "../app/model/dto/onboarding-data.model";
import { MessageTypes } from "../app/model/enums/messageTypes";
import { PlaylistTypes } from "../app/model/enums/playlistTypes";
import { TrackLikeModel } from "../app/model/dto/track-like.model";

const axiosApiInstance = axios.create();

// const sleep = (delay: number) => {
//   return new Promise((resolve) => {
//     setTimeout(resolve, delay);
//   });
// };

interface AdaptAxiosRequestConfig extends AxiosRequestConfig {
  headers: AxiosRequestHeaders;
}

axiosApiInstance.defaults.baseURL = import.meta.env.VITE_API_URL;

axiosApiInstance.interceptors.request.use((config: AdaptAxiosRequestConfig) => {
  config.withCredentials = true;
  return config;
});

let isRefreshing = false;
let refreshSubscribers: any[] = [];

let isWaiting = false;
let waitingSubscribers: any[] = [];

const subscribeTokenRefresh = (cb: () => void) => {
  refreshSubscribers.push(cb);
};

const onRefreshed = () => {
  refreshSubscribers.map((cb) => cb());
  refreshSubscribers = [];
};

const subscribeWaitingPeriodRefresh = (cb: () => void) => {
  waitingSubscribers.push(cb);
};

const onWaited = () => {
  waitingSubscribers.map((cb) => cb());
  waitingSubscribers = [];
};

axiosApiInstance.interceptors.response.use(
  async (response: AxiosResponse) => {
    if (import.meta.env.MODE === "development") {
      // await sleep(2000);
    }
    return response;
  },
  async (err: AxiosError) => {
    if (err && err.response) {
      const { config, headers } = err.response;

      if (
        err.response!.status === 401 &&
        headers["www-authenticate"]?.includes("invalid_token")
      ) {
        if (!isRefreshing) {
          isRefreshing = true;
          Authentication.refreshToken()
            .then(async () => {
              isRefreshing = false;
              onRefreshed();
            })
            .catch(async () => await Authentication.signOut());
        }

        const retryOrigReq = new Promise((resolve) => {
          subscribeTokenRefresh(async () => {
            resolve(requests.retry(config));
          });
        });
        return retryOrigReq;
      } else if (err.response!.status === 429) {
        toast(err.response.data as string);
        if (!isWaiting) {
          isWaiting = true;

          setTimeout(() => {
            isWaiting = false;
            onWaited();
          }, 5000);
        }

        const retryOrigReq = new Promise((resolve) => {
          subscribeWaitingPeriodRefresh(async () => {
            resolve(requests.retry(config));
          });
        });
        return retryOrigReq;
      } else {
        return Promise.reject(err.response.data);
      }
    } else {
      return Promise.reject(
        "An unexpected error has occured, please try again.."
      );
    }
  }
);

const responseBody = <T>(response: AxiosResponse<T>) => response?.data;

const requests = {
  get: <T>(url: string, config?: AxiosRequestConfig) =>
    axiosApiInstance.get<T>(url, config).then(responseBody),
  post: <T>(url: string, body: {} | null, config?: AxiosRequestConfig) =>
    axiosApiInstance.post<T>(url, body, config).then(responseBody),
  put: <T>(url: string, body: {}, config?: AxiosRequestConfig) =>
    axiosApiInstance.put<T>(url, body, config).then(responseBody),
  delete: <T>(url: string, config?: AxiosRequestConfig) =>
    axiosApiInstance.delete<T>(url, config).then(responseBody),
  retry: (config: AxiosRequestConfig) => axiosApiInstance(config),
};

const Authentication = {
  login: (details: UserLoginModel) =>
    requests.post<UserModel>(`account/login`, details),
  register: (details: UserRegistrationModel) =>
    requests.post(`account/register`, details),
  checkCurrentUser: () => requests.get<UserModel>("account"),
  refreshToken: () => requests.post<null>("token/refresh-token", {}),
  confirmEmail: (token: string) =>
    requests.post<UserModel>("token/confirm-email", {
      token,
    }),
  resendEmailToken: () => requests.post("token/resend-email-token", {}),
  signOut: () => requests.post<null>("account/sign-out", null),
};

const Account = {
  getProfile: (profileUrl: string) =>
    requests.get<UserModel>(`account/get-profile-details/${profileUrl}`),
  uploadProfileImage: (
    memberId: string,
    formData: FormData,
    accountImageType: AccountImageTypes
  ) =>
    requests.put<AccountImageModel>(
      `account/update-account-image/member/${memberId}/image-type/${accountImageType}`,
      formData
    ),
  getProfileFollowers: (memberId: string, page: number) =>
    requests.get<FollowerListResponse>(
      `account/get-profile-followers/member/${memberId}?page=${page}`
    ),
  getProfileFollowing: (memberId: string, page: number) =>
    requests.get<FollowerListResponse>(
      `account/get-profile-following/member/${memberId}?page=${page}`
    ),
  saveOnboardingData: (memberId: string, onboardingData: OnboardingDataModel) =>
    requests.post(
      `account/member/${memberId}/save-onboarding-data`,
      onboardingData
    ),
  updatePersonalDetails: (
    memberId: string,
    editArtist: UpdateArtistPersonalDetailsModel
  ) =>
    requests.put<null>(
      `account/member/${memberId}/personal-details`,
      editArtist
    ),
  updateProfileDetails: (
    memberId: string,
    editArtist: UpdateArtistProfileDetailsModel
  ) => requests.put(`account/member/${memberId}/profile-details`, editArtist),
  followArtist: (userId: string, artistId: string) =>
    requests.post<null>(
      `account/follow-artist/member/${userId}/artist/${artistId}`,
      null
    ),
  unfollowArtist: (userId: string, artistId: string) =>
    requests.post<null>(
      `account/unfollow-artist/member/${userId}/artist/${artistId}`,
      null
    ),
};

const Genre = {
  searchGenre: (type: string) =>
    requests.get<GenreModel[]>(`genre/search-genre/${type}`),
};

const EventType = {
  searchEventType: (type: string) =>
    requests.get<EventTypeModel[]>(`event-type/search-event-type/${type}`),
  getEventTypes: () =>
    requests.get<EventTypeModel[]>(`event-type/get-event-types`),
};

const Tracks = {
  getTrackData: (memberId: string) =>
    requests.get<TrackUploadSASModel>(`track/member/${memberId}/upload-token`),
  uploadTrackSupportingData: (memberId: string, formData: FormData) =>
    requests.post<number>(`track/member/${memberId}/upload-track`, formData),
  editTrackSupportingDetails: (
    memberId: string,
    trackId: number,
    formData: FormData
  ) => requests.put(`track/member/${memberId}/track/${trackId}`, formData),
  getTracks: (
    memberId: string,
    playlistType: PlaylistTypes,
    trackId?: number
  ) => {
    var url = `track/member/${memberId}/playlist-type/${playlistType}`;

    if (trackId) {
      url += `?lastTrackId=${trackId}`;
    }
    return requests.get<TrackListResponse>(url);
  },
  getTrackDetails: (memberId: string, trackId: number) =>
    requests.get<ArtistTrackModel>(`track/member/${memberId}/track/${trackId}`),
  deleteTrack: (memberId: string, trackId: number) =>
    requests.delete(`track/member/${memberId}/track/${trackId}`),
  likeSong: (memberId: string, data: TrackLikeModel) =>
    requests.put(`track/member/${memberId}/likes`, data, {}),
  unlikeSong: (memberId: string, songLikeId: number) =>
    requests.delete(`track/member/${memberId}/likes/${songLikeId}`, {}),
};

const Packages = {
  getPackages: (memberId: string) =>
    requests.get<ArtistPackageModel[]>(`packages/member/${memberId}`),
  createPackage: (memberId: string, formData: FormData) =>
    requests.post(`packages/member/${memberId}`, formData),
  updatePackage: (memberId: string, packageId: string, formData: FormData) =>
    requests.put(`packages/member/${memberId}/package/${packageId}`, formData),
  deletePackage: (memberId: string, packageId: string) =>
    requests.delete(`packages/member/${memberId}/package/${packageId}`),
};

const Bookings = {
  createBooking: (memberId: string, bookingData: BookingSubmissionModel) =>
    requests.post(`bookings/member/${memberId}/create-booking`, bookingData),
  getNonCompletedBookings: (
    memberId: string,
    bookingConfirmed: boolean | null,
    lastBookingId?: number
  ) => {
    let url = `bookings/member/${memberId}/get-bookings`;

    if (bookingConfirmed !== null || lastBookingId) {
      url += "?";
      if (bookingConfirmed !== null) {
        url += "bookingConfirmed=" + bookingConfirmed + "&";
      }

      if (lastBookingId) {
        url += `lastBookingId=${lastBookingId}`;
      }
    }

    return requests.get<BookingListResponse>(url);
  },
  getCompletedBookings: (memberId: string, lastBookingId?: number) => {
    let url = `bookings/member/${memberId}/get-completed-bookings`;

    if (lastBookingId) {
      url += `?lastBookingId=${lastBookingId}`;
    }

    return requests.get<BookingListResponse>(url);
  },
  acceptBooking: (memberId: string, bookingId: number) =>
    requests.put(
      `bookings/member/${memberId}/booking/${bookingId}/accept-booking`,
      {}
    ),
  cancelBooking: (memberId: string, bookingId: number) =>
    requests.put(
      `bookings/member/${memberId}/booking/${bookingId}/cancel-booking`,
      {}
    ),
};

const Notifications = {
  getMoreNotifications: (memberId: string, lastNotificationId?: number) => {
    var url = `notifications/member/${memberId}/get-more-notifications`;

    if (lastNotificationId) {
      url += `?lastNotificationId=${lastNotificationId}`;
    }

    return requests.get<NotificationListResponseModel>(url);
  },
  clickNotification: (memberId: string, notificationId: number) =>
    requests.put(
      `notifications/member/${memberId}/notification/${notificationId}/click-notification`,
      {}
    ),
};

const Messages = {
  dismissMessage: (memberId: string, messageType: MessageTypes) =>
    requests.post(
      `message/member/${memberId}/dismiss-message/${messageType}`,
      {}
    ),
};

const agent = {
  Authentication,
  Account,
  Genre,
  EventType,
  Tracks,
  Packages,
  Bookings,
  Notifications,
  Messages,
};

export default agent;
