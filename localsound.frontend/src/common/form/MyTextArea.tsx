import { useField } from "formik";
import { Form, Label } from "semantic-ui-react";

interface Props {
  placeholder: string;
  name: string;
  rows: number;
  label?: string;
  fieldClassName?: string;
  disabled?: boolean;
}

const MyTextArea = (props: Props) => {
  const [field, meta] = useField(props);

  return (
    <Form.Field
      error={meta.touched && !!meta.error}
      className={props.fieldClassName}
    >
      <label>{props.label}</label>
      <textarea {...field} {...props} disabled={props.disabled} />
      {meta.touched && meta.error ? (
        <Label basic color="red">
          {meta.error}
        </Label>
      ) : null}
    </Form.Field>
  );
};

export default MyTextArea;
