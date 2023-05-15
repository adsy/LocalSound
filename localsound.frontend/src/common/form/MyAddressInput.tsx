import { useField } from "formik";
import { useState } from "react";
import PlacesAutocomplete from "react-places-autocomplete";
import { Form, Label } from "semantic-ui-react";

interface Props {
  disabled?: boolean;
  placeholder: string;
  name: string;
  label?: string;
  setFieldValue: (field: string, value: any, shouldValidate?: boolean) => void;
  setFieldTouched: (
    field: string,
    isTouched?: boolean,
    shouldValidate?: boolean
  ) => void;
  setAddressError: (addressError: boolean) => void;
}

const MyAddressInput = (props: Props) => {
  const {
    setFieldValue,
    setFieldTouched,
    disabled,
    placeholder,
    name,
    label,
    setAddressError,
  } = props;
  const customProps = { disabled, placeholder, name, label };
  const [address, setAddress] = useState("");
  const [field, meta] = useField(props);
  const [pickedAddress, setPickedAddress] = useState(false);
  const [addressSelected, setAddressSelected] = useState("");

  const handleChange = (address: string) => {
    setFieldTouched("address", true);
    setFieldValue("address", address);
    setAddress(address);
    if (address !== addressSelected) {
      setAddressError(true);
      setPickedAddress(false);
    }
  };

  const handleSelect = (address: string, placeId: string) => {
    if (!placeId) return;

    setAddress(address);
    setAddressSelected(address);
    setFieldValue("address", address);
    setFieldTouched("address", true);
    setPickedAddress(true);
    setAddressError(false);
  };

  return (
    <Form.Field error={meta.touched && !pickedAddress}>
      <PlacesAutocomplete
        value={address}
        onChange={handleChange}
        onSelect={handleSelect}
        searchOptions={{
          componentRestrictions: {
            country: "AU",
          },
        }}
      >
        {({ getInputProps, suggestions, getSuggestionItemProps, loading }) => {
          return (
            <div className="position-relative">
              <input
                {...field}
                {...getInputProps({
                  placeholder: `${props.placeholder}`,
                  className: "location-search-input",
                })}
                {...customProps}
                disabled={disabled}
                onBlur={(e) => {
                  setFieldTouched("address", true);
                }}
                className="mb-2"
              />
              {meta.touched && !pickedAddress ? (
                <Label basic color="red">
                  You must select a valid address
                </Label>
              ) : null}
              {suggestions.length ? (
                <div className="autocomplete-dropdown-container">
                  {loading && <div>Loading...</div>}
                  {suggestions.map((suggestion, index) => {
                    const className = suggestion.active
                      ? "suggestion-item--active"
                      : "suggestion-item";

                    const style = suggestion.active
                      ? { backgroundColor: "#fafafa", cursor: "pointer" }
                      : { backgroundColor: "#ffffff", cursor: "pointer" };
                    return (
                      <div
                        {...getSuggestionItemProps(suggestion, {
                          className,
                          style,
                        })}
                        key={index}
                      >
                        <span>{suggestion.description}</span>
                      </div>
                    );
                  })}
                </div>
              ) : null}
            </div>
          );
        }}
      </PlacesAutocomplete>
    </Form.Field>
  );
};

export default MyAddressInput;
