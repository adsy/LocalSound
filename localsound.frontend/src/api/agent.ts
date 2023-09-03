import axios, { AxiosError, AxiosRequestConfig, AxiosResponse } from "axios";
import { UserRegistrationModel } from "../app/model/dto/user-registration.model";
import { UserLoginModel } from "../app/model/dto/user-login.model";
import { UserModel } from "../app/model/dto/user.model";
import { UpdateArtistModel } from "../app/model/dto/update-artist.model";
import { GenreModel } from "../app/model/dto/genre.model";

// let localStore: StoreType;

// export const injectStore = (_store: StoreType) => {
//   localStore = _store;
// };

const axiosApiInstance = axios.create();

const sleep = (delay: number) => {
  return new Promise((resolve) => {
    setTimeout(resolve, delay);
  });
};

axiosApiInstance.defaults.baseURL = import.meta.env.VITE_API_URL;

axiosApiInstance.interceptors.request.use(
  (config: InternalAxiosRequestConfig) => {
    config.withCredentials = true;
    return config;
  }
);

axiosApiInstance.interceptors.response.use(
  async (response: AxiosResponse) => {
    if (import.meta.env.NODE_ENV === "development") {
      await sleep(2000);
    }
    return response;
  },
  async (error: AxiosError) => {
    if (import.meta.env.NODE_ENV === "development") {
    }
    if (error && error.response) {
      //TODO: Refresh token functionality
      // const { status, config, headers } = error.response;

      // Handle refresh token functionality
      // if (
      //   status === 401 &&
      //   headers["www-authenticate"]?.includes("invalid_token")
      // ) {
      //   try {
      //     await Authentication.refreshToken();
      //     return await requests.retry(config);
      //   } catch (err) {
      //     localStore.dispatch(handleUserLogout());
      //     localStore.dispatch(handleResetState());
      //     history.push("/");
      //   }
      // } else {
      return Promise.reject(error.response.data);
      // }
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
  login: (details: UserLoginModel) => requests.post(`account/login`, details),
  register: (details: UserRegistrationModel) =>
    requests.post(`account/register`, details),
  // checkCurrentUser: () =>
  //   requests.get<StoreLoginState | UserLoginState>("account"),
  // refreshToken: () => requests.post<null>("token/refresh-token", {}),
  // confirmEmail: (token: string) =>
  //   requests.post<UserLoginState | StoreLoginState>("token/confirm-email", {
  //     token,
  //   }),
  // resendEmailToken: () => requests.post("token/resend-email-token", {}),
  signOut: () => requests.post<null>("account/sign-out", null),
};

const Profile = {
  getProfile: (profileUrl: string) =>
    requests.get<UserModel>(`account/get-profile-details/${profileUrl}`),
  uploadProfileImage: (memberId: string, formData: FormData) =>
    requests.put(`account/update-profile-image/${memberId}`, formData),
};

const Artist = {
  updateArtistDetails: (memberId: string, editArtist: UpdateArtistModel) =>
    requests.put<null>(`artist/${memberId}`, editArtist),
};

const Genre = {
  searchGenre: (type: string) =>
    requests.get<GenreModel[]>(`genre/search-genre/${type}`),
};

const agent = {
  Authentication,
  Profile,
  Artist,
  Genre,
};

export default agent;
