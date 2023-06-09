import * as Yup from "yup";
import YupPassword from "yup-password";
YupPassword(Yup);

const nonArtistRegisterValidation = Yup.object({
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
  firstName: Yup.string().required("You must enter your first name."),
  lastName: Yup.string().required("You must enter your surname."),
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
});

export default nonArtistRegisterValidation;
