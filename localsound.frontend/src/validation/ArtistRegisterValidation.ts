import * as Yup from "yup";
import YupPassword from "yup-password";
YupPassword(Yup);

const artistRegisterValidation = Yup.object({
  profileUrl: Yup.string()
    .required("You must set a profile url.")
    .test({
      name: "profileUrlFormat",
      message:
        "Your url must have alphanumeric characters and cannot end in a hypen.",
      test: (value) =>
        new RegExp("(^[A-Za-z0-9]+(-[A-Za-z0-9]+)*$)").test(value!) ||
        value === undefined ||
        (value !== undefined && value.trim().length === 0),
    })
    .test({
      name: "profileUrlFormat",
      message: "Your url cannot be more than 30 characters",
      test: (value) => value.length < 30,
    }),
  email: Yup.string()
    .required("Your email address is required.")
    .email()
    .typeError("You must enter a correct email address."),
  password: Yup.string()
    .min(6, "Password must be at least 6 characters.")
    .required("You must enter a password.")
    .minUppercase(1, "Your password must have at least 1 uppercase letter.")
    .minLowercase(1, "Your password must have at least 1 lowercase letter.")
    .minNumbers(1, "Your password must have at least 1 number.")
    .minSymbols(1, "Your password must have at least 1 symbol."),
  checkPassword: Yup.string()
    .required("You must re-enter your password.")
    .oneOf([Yup.ref("password")], "Passwords must match."),
  name: Yup.string().required("You must enter your name as a performer."),
  address: Yup.string()
    .required("You must enter a valid address.")
    .test({
      name: "addressFormat",
      message: "You must select a valid address.",
      test: (value) => {
        if (value === undefined || value.trim() === "") {
          return false;
        }
        return true;
      },
    }),
  phoneNumber: Yup.string()
    .required("You must enter a valid mobile number.")
    .test({
      name: "numberFormat",
      message:
        "Your mobile number must be in the the following format e.g. 0400 000 000.",
      test: (value) =>
        new RegExp("(04[0-9]{2} [0-9]{3} [0-9]{3})").test(value!) ||
        value === undefined ||
        (value !== undefined && value.trim().length === 0),
    }),
  youtubeUrl: Yup.string().test({
    name: "urlFormat",
    message: "Your youtube url must start with https://www.youtube.com/@",
    test: (value) =>
      !value || new RegExp("(https://www.youtube.com/@)").test(value!),
  }),
  spotifyUrl: Yup.string().test({
    name: "urlFormat",
    message:
      "Your spotify url must start with https://open.spotify.com/artist/",
    test: (value) =>
      !value || new RegExp("(https://open.spotify.com/artist/)").test(value!),
  }),
  soundcloudUrl: Yup.string().test({
    name: "urlFormat",
    message: "Your soundcloud url must start with https://soundcloud.com/",
    test: (value) =>
      !value || new RegExp("(https://soundcloud.com/)").test(value!),
  }),
});

export default artistRegisterValidation;
