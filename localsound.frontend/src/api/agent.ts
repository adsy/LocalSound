import axios, {
  AxiosError,
  AxiosResponse,
  InternalAxiosRequestConfig,
} from "axios";
import { UserRegistrationModel } from "../app/model/dto/user-registration.model";
import { UserLoginModel } from "../app/model/dto/user-login.model";

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
  get: <T>(url: string, config?: InternalAxiosRequestConfig) =>
    axiosApiInstance.get<T>(url, config).then(responseBody),
  post: <T>(
    url: string,
    body: {} | null,
    config?: InternalAxiosRequestConfig
  ) => axiosApiInstance.post<T>(url, body, config).then(responseBody),
  put: <T>(url: string, body: {}, config?: InternalAxiosRequestConfig) =>
    axiosApiInstance.put<T>(url, body, config).then(responseBody),
  delete: <T>(
    url: string,
    config?: AxiosRequeInternalAxiosRequestConfigstConfig
  ) => axiosApiInstance.delete<T>(url, config).then(responseBody),
  retry: (config: InternalAxiosRequestConfig) => axiosApiInstance(config),
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

const agent = {
  Authentication,
};

export default agent;
