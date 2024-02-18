import { useField } from "formik";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import { Form, Label } from "semantic-ui-react";

interface Props {
  name: string;
  date: Date | null;
  setFieldValue: (field: string, value: any, shouldValidate?: boolean) => void;
  setFieldTouched: (
    field: string,
    isTouched?: boolean,
    shouldValidate?: boolean
  ) => void;
  disabled?: boolean;
  placeholder: string;
  label?: string;
  type?: string;
  className?: string;
  fieldClassName?: string;
}

const DateInput = ({
  name,
  date,
  setFieldValue,
  setFieldTouched,
  placeholder,
  label,
  type,
  className,
  fieldClassName,
}: Props) => {
  const fieldProps = {
    placeholder,
    name,
    label,
    type,
    className,
  };
  const [_field, meta] = useField(fieldProps);

  return (
    <Form.Field error={meta.touched && !!meta.error} className={fieldClassName}>
      <DatePicker
        selected={date}
        onChange={(date) => {
          setFieldTouched(name, true, true);
          setFieldValue(name, date, true);
        }}
        onBlur={(date) => {
          setFieldTouched(name, true, true);
          setFieldValue(
            name,
            date.target.value ? new Date(date.target.value) : null,
            true
          );
        }}
      />
      {meta.touched && meta.error ? (
        <Label basic color="red">
          {meta.error}
        </Label>
      ) : null}
    </Form.Field>
  );
};

export default DateInput;
