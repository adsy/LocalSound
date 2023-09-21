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
import { history } from "../common/history/history";
import { resetState } from "../app/redux/store/store";
import { UpdateArtistProfileDetailsModel } from "../app/model/dto/update-artist-profile.model";
import { EventTypeModel } from "../app/model/dto/eventType.model";
import { TrackUploadSASModel } from "../app/model/dto/track-upload-sas.model";

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

axiosApiInstance.interceptors.response.use(
  async (response: AxiosResponse) => {
    if (import.meta.env.NODE_ENV === "development") {
      await sleep(2000);
    }
    return response;
  },
  async (error: AxiosError) => {
    if (error && error.response) {
      const { status, config, headers } = error.response;
      if (
        status === 401 &&
        headers["www-authenticate"]?.includes("invalid_token")
      ) {
        try {
          await Authentication.refreshToken();
          return await requests.retry(config);
        } catch (err) {
          resetState();
          history.push("/");
        }
      } else {
        return Promise.reject(error.response.data);
      }
    } else {
      return Promise.reject(
        "An unexpected error has occured, please try again.."
      );
    }
  }
);

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

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
};

const Genre = {
  searchGenre: (type: string) =>
    requests.get<GenreModel[]>(`genre/search-genre/${type}`),
};

const EventType = {
  searchEventType: (type: string) =>
    requests.get<EventTypeModel[]>(`event-type/search-event-type/${type}`),
};

const Tracks = {
  getTrackData: (memberId: string) =>
    requests.get<TrackUploadSASModel>(
      `upload-track/member/${memberId}/upload-token`
    ),
  uploadTrackSupportingData: (
    memberId: string,
    trackId: string,
    formData: FormData
  ) =>
    requests.post(`upload-track/member/${memberId}/track/${trackId}`, formData),
};

const agent = {
  Authentication,
  Profile,
  Artist,
  Genre,
  EventType,
  Tracks,
};

export default agent;
