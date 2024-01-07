import * as Yup from "yup";

//   bookingLength: "",
//   eventType: "",
//   bookingDate: null,
const createBookingValidation = Yup.object({
  bookingLength: Yup.number().required("You must provide a booking length."),
  eventType: Yup.string().required("You must select an event type."),
  bookingDate: Yup.date()
    .required("You must select an event date.")
    .test({
      name: "futureDate",
      message: "You cannot select a past date.",
      test: (value) => {
        const currentDate = new Date().setHours(0, 0, 0, 0);
        if (value.getTime() < currentDate) return false;
        return true;
      },
    }),
});

export default createBookingValidation;
