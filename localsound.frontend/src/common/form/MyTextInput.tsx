import { useField } from "formik";
import { Form, Label } from "semantic-ui-react";

interface Props {
  disabled?: boolean;
  placeholder: string;
  name: string;
  label?: string;
  type?: string;
  className?: string;
  fieldClassName?: string;
  onBlur?: React.FocusEventHandler<HTMLInputElement>;
  onChange?: React.ChangeEventHandler<HTMLInputElement>;
  ref?: React.LegacyRef<HTMLInputElement>;
}

const MyTextInput = (props: Props) => {
  const {
    placeholder,
    name,
    label,
    type,
    className,
    disabled,
    ref,
    fieldClassName,
    onBlur,
    onChange,
  } = props;
  const fieldProps = {
    placeholder,
    name,
    label,
    type,
    className,
  };
  const [field, meta] = useField(fieldProps);

  return (
    <Form.Field error={meta.touched && !!meta.error} className={fieldClassName}>
      <label>{props.label}</label>
      {onBlur && onChange ? (
        <input
          {...field}
          {...fieldProps}
          onBlur={onBlur}
          onChange={onChange}
          disabled={disabled}
          ref={ref}
        />
      ) : (
        <input {...field} {...fieldProps} disabled={disabled} ref={ref} />
      )}
      {meta.touched && meta.error ? (
        <Label basic color="red">
          {meta.error}
        </Label>
      ) : null}
    </Form.Field>
  );
};

export default MyTextInput;
