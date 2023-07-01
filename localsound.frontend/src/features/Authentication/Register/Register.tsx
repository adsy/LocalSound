import { useEffect, useState } from "react";
import { Button } from "react-bootstrap";
import { CustomerTypes } from "../../../app/model/enums/customerTypes";
import ArtistRegister from "./ArtistRegister";
import NonArtistRegister from "./NonArtistRegister";
import { useDispatch } from "react-redux";
import { handleToggleModal } from "../../../app/redux/actions/modalSlice";
import { handleSetUserDetails } from "../../../app/redux/actions/userSlice";
import agent from "../../../api/agent";
import { RegistrationModel } from "../../../app/model/dto/user-registration.model";

const Register = () => {
  const [showRegisterForm, setShowRegisterForm] = useState(false);
  const [customerType, setCustomerType] = useState<CustomerTypes | null>(null);
  const dispatch = useDispatch();

  const handleUserTypeSelection = (userType: CustomerTypes) => {
    setCustomerType(userType);
    setShowRegisterForm(true);
    dispatch(handleToggleModal({ size: "large" }));
  };

  useEffect(() => {
    return () => {
      setCustomerType(null);
      setShowRegisterForm(false);
    };
  }, []);

  const handleRegisterRequest = async (
    values: RegistrationModel,
    customerType: CustomerTypes,
    setStatus: (value: any) => void
  ) => {
    try {
      var result = await agent.Authentication.register({
        customerType: customerType,
        registrationDto: values,
      });

      dispatch(handleSetUserDetails(result));
      dispatch(
        handleToggleModal({
          open: false,
        })
      );

      // TODO: redirect once logged in
    } catch (error) {
      if (error) {
        setStatus(error);
      } else {
        setStatus({
          error:
            "An error occured while trying to register your account, please try again..",
        });
      }
    }
  };

  return (
    <div id="auth-modal" className="fade-in">
      <div className="d-flex flex-row header">
        <h2 className="header-title mt-1 align-self-center">
          Register for an account
        </h2>
      </div>
      {!showRegisterForm ? (
        <div className="d-flex header justify-content-end mt-2">
          <Button
            className="black-button mr-2"
            onClick={() => handleUserTypeSelection(CustomerTypes.Artist)}
          >
            I play the music
          </Button>
          <Button
            className="black-button"
            onClick={() => handleUserTypeSelection(CustomerTypes.NonArtist)}
          >
            I listen to the music
          </Button>
        </div>
      ) : customerType === CustomerTypes.Artist ? (
        <ArtistRegister handleRegisterRequest={handleRegisterRequest} />
      ) : (
        <NonArtistRegister handleRegisterRequest={handleRegisterRequest} />
      )}
    </div>
  );
};

export default Register;
