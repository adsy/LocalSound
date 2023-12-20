import * as Yup from "yup";

//   bookingLength: "",
//   eventType: "",
//   bookingDate: null,
const createBookingValidation = Yup.object({
  bookingLength: Yup.number().required("You must provide a booking length."),
  eventType: Yup.string().required("You must select an event type."),
  bookingDate: Yup.date().required("You must select an event date."),
});

export default createBookingValidation;
