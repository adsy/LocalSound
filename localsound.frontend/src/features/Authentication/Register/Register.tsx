import { useEffect, useState } from "react";
import { Button } from "react-bootstrap";
import { CustomerTypes } from "../../../app/model/enums/customerTypes";
import ArtistRegister from "./ArtistRegister";
import NonArtistRegister from "./NonArtistRegister";
import { useDispatch } from "react-redux";
import { handleToggleModal } from "../../../app/redux/actions/modalSlice";

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

  return (
    <div id="auth-modal" className="fade-in">
      <div className="d-flex flex-row mb-2 header">
        <h2 className="header-title mt-1 align-self-center">
          Register for an account
        </h2>
      </div>
      {!showRegisterForm ? (
        <div className="d-flex header justify-content-end mt-2">
          <Button
            className="purple-button mr-2"
            onClick={() => handleUserTypeSelection(CustomerTypes.Artist)}
          >
            I play the music
          </Button>
          <Button
            className="purple-button"
            onClick={() => handleUserTypeSelection(CustomerTypes.NonArtist)}
          >
            I listen to the music
          </Button>
        </div>
      ) : customerType === CustomerTypes.Artist ? (
        <ArtistRegister />
      ) : (
        <NonArtistRegister />
      )}
    </div>
  );
};

export default Register;
