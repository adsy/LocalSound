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
import { ArtistTrackUploadModel } from "../app/model/dto/artist-track-upload.model";
import { TrackListResponse } from "../app/model/dto/track-list-response.model";
import { FollowerListResponse } from "../app/model/dto/follower-list-response.model";
import { ArtistPackageModel } from "../app/model/dto/artist-package.model";
import { BookingSubmissionModel } from "../app/model/dto/booking-submission.model";
import { BookingListResponse } from "../app/model/dto/booking-list.-response.model";

const axiosApiInstance = axios.create();

const sleep = (delay: number) => {
  return new Promise((resolve) => {
    setTimeout(resolve, delay);
  });
};

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

const subscribeTokenRefresh = (cb: () => void) => {
  refreshSubscribers.push(cb);
};

const onRefreshed = () => {
  refreshSubscribers.map((cb) => cb());
  refreshSubscribers = [];
};

axiosApiInstance.interceptors.response.use(
  async (response: AxiosResponse) => {
    if (import.meta.env.MODE === "development") {
      await sleep(2000);
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
            .catch(async (err) => await Authentication.signOut());
        }

        const retryOrigReq = new Promise((resolve, reject) => {
          subscribeTokenRefresh(async () => {
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

const Profile = {
  getProfile: (profileUrl: string) =>
    requests.get<UserModel>(`account/get-profile-details/${profileUrl}`),
  uploadProfileImage: (
    memberId: string,
    formData: FormData,
    accountImageType: AccountImageTypes
  ) =>
    requests.put<AccountImageModel>(
      `account/update-account-image/${memberId}/image-type/${accountImageType}`,
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
};

const Artist = {
  updateArtistPersonalDetails: (
    memberId: string,
    editArtist: UpdateArtistPersonalDetailsModel
  ) =>
    requests.put<null>(
      `artist/member/${memberId}/personal-details`,
      editArtist
    ),
  updateArtistProfileDetails: (
    memberId: string,
    editArtist: UpdateArtistProfileDetailsModel
  ) => requests.put(`artist/member/${memberId}/profile-details`, editArtist),
  followArtist: (userId: string, artistId: string) =>
    requests.post<null>(
      `artist/follow-artist/member/${userId}/artist/${artistId}`,
      null
    ),
  unfollowArtist: (userId: string, artistId: string) =>
    requests.post<null>(
      `artist/unfollow-artist/member/${userId}/artist/${artistId}`,
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
  uploadTrackSupportingData: (
    memberId: string,
    trackId: string,
    formData: FormData
  ) => requests.post(`track/member/${memberId}/track/${trackId}`, formData),
  editTrackSupportingDetails: (
    memberId: string,
    trackId: string,
    formData: FormData
  ) => requests.put(`track/member/${memberId}/track/${trackId}`, formData),
  getArtistUploads: (memberId: string, page: number) =>
    requests.get<TrackListResponse>(`track/member/${memberId}?page=${page}`),
  getTrackDetails: (memberId: string, trackId: string) =>
    requests.get<ArtistTrackUploadModel>(
      `track/member/${memberId}/track/${trackId}`
    ),
  deleteTrack: (memberId: string, trackId: string) =>
    requests.delete(`track/member/${memberId}/track/${trackId}`),
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
    page: number,
    bookingConfirmed: boolean | null
  ) => {
    let url = `bookings/member/${memberId}/get-bookings?page=${page}`;

    if (bookingConfirmed !== null) {
      url += "&bookingConfirmed=" + bookingConfirmed;
    }

    return requests.get<BookingListResponse>(url);
  },
  getCompletedBookings: (memberId: string, page: number) =>
    requests.get<BookingListResponse>(
      `bookings/member/${memberId}/get-completed-bookings?page=${page}`
    ),
  acceptBooking: (memberId: string, bookingId: string) =>
    requests.put(
      `bookings/member/${memberId}/booking/${bookingId}/accept-booking`,
      {}
    ),
  cancelBooking: (memberId: string, bookingId: string) =>
    requests.put(
      `bookings/member/${memberId}/booking/${bookingId}/cancel-booking`,
      {}
    ),
};

const Notifications = {
  removeNotification: (notificationId: string) =>
    requests.delete(
      `notifications/notification/${notificationId}/delete-notification`
    ),
};

const agent = {
  Authentication,
  Profile,
  Artist,
  Genre,
  EventType,
  Tracks,
  Packages,
  Bookings,
  Notifications,
};

export default agent;
