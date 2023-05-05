import { useField } from "formik";
import { Form, Label } from "semantic-ui-react";

interface Props {
  value: any;
  disabled?: boolean;
  placeholder?: string;
  name: string;
  label?: string;
  type?: string;
  className?: string;
  fieldClassName?: string;
  checked: boolean;
  onChange: any;
}

const MyRadioInput = (props: Props) => {
  const {
    placeholder,
    name,
    label,
    type,
    className,
    disabled,
    value,
    checked,
    onChange,
  } = props;
  const fieldProps = { placeholder, name, label, type, className };
  const [field, meta] = useField(fieldProps);

  return (
    <Form.Field
      error={meta.touched && !!meta.error}
      className={props.fieldClassName}
    >
      <label className="radio-label">
        <input
          {...field}
          {...props}
          disabled={disabled}
          type="radio"
          value={value}
          name={name}
          checked={checked}
          onChange={onChange}
          className="radio-input m-2"
        />
        {label}
      </label>
      {meta.touched && meta.error ? (
        <Label basic color="red">
          {meta.error}
        </Label>
      ) : null}
    </Form.Field>
  );
};

export default MyRadioInput;
