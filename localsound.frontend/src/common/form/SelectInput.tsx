import { useField } from "formik";
import { Form, Label, Select } from "semantic-ui-react";

interface Props {
  disabled?: boolean;
  placeholder: string;
  name: string;
  options: any;
  label?: string;
}

const SelectInput = (props: Props) => {
  const [field, meta, helpers] = useField(props);

  return (
    <Form.Field error={meta.touched && !!meta.error}>
      <label>{props.label}</label>
      <Select
        clearable
        options={props.options}
        value={field.value || null}
        onChange={(_event, data) => helpers.setValue(data.value)}
        onBlur={() => helpers.setTouched(true)}
        placeholder={props.placeholder}
        disabled={props.disabled}
      />
      {meta.touched && meta.error ? (
        <Label basic color="red">
          {meta.error}
        </Label>
      ) : null}
    </Form.Field>
  );
};

export default SelectInput;
