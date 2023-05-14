import { useEffect, useState } from "react";
import { Button } from "react-bootstrap";
import { CustomerTypes } from "../../../app/model/enums/customerTypes";
import { Formik } from "formik";
import { Form } from "semantic-ui-react";
import MyTextInput from "../../../common/form/MyTextInput";

const Login = () => {
  const [showLoginForm, setShowLoginForm] = useState(false);
  const [customerType, setCustomerType] = useState<CustomerTypes | null>(null);
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    return () => {
      setShowLoginForm(false);
    };
  }, []);

  const handleUserTypeSelection = (userType: CustomerTypes) => {
    setCustomerType(userType);
    setShowLoginForm(true);
  };

  return (
    <div id="auth-modal" className="fade-in">
      <div className="d-flex flex-row mb-2 header">
        <h2 className="header-title mt-1 align-self-center">
          Login to your account
        </h2>
      </div>
      {!showLoginForm ? (
        <div className="d-flex header justify-content-end mt-2">
          <Button
            className="purple-button mr-2"
            onClick={() => handleUserTypeSelection(CustomerTypes.Artist)}
          >
            I play the music
          </Button>
          <Button
            className="purple-button"
            onClick={() => handleUserTypeSelection(CustomerTypes.Artist)}
          >
            I listen to the music
          </Button>
        </div>
      ) : (
        <div className="w-100 header mt-2 fade-in">
          <Formik
            initialValues={{ email: "", password: "", error: null }}
            onSubmit={async (values, { setStatus }) => {
              setIsLoading(true);
              setStatus(null);
              // await handleLoginRequest(values, setStatus);
            }}
          >
            {({ handleSubmit, isSubmitting, status, values }) => {
              return (
                <Form
                  className="ui form error fade-in"
                  onSubmit={handleSubmit}
                  autoComplete="off"
                >
                  <div className="form-body">
                    <div className="form-label">EMAIL</div>
                    <MyTextInput
                      name="email"
                      placeholder=""
                      fieldClassName="mt-2"
                      disabled={isSubmitting}
                    />
                    <div className="form-label">PASSWORD</div>
                    <MyTextInput
                      name="password"
                      placeholder=""
                      type="password"
                      fieldClassName="mb-1"
                      disabled={isSubmitting}
                    />
                  </div>
                  {status?.error ? (
                    <h4 className="text-center fade-in mb-0">{status.error}</h4>
                  ) : null}

                  {
                    !isSubmitting ? (
                      <Button
                        className="purple-button w-100 align-self-center mt-5"
                        type="submit"
                        disabled={
                          isLoading ||
                          values.email === "" ||
                          values.password === ""
                        }
                      >
                        <strong>LOGIN</strong>
                      </Button>
                    ) : null
                    // <InPageLoadingComponent />
                  }
                  {/* <Divider /> */}
                  {/* <Button
                    onClick={() =>
                      dispatch(
                        handleToggleModal({
                          open: true,
                          body: <Register />,
                          size: "large",
                        })
                      )
                    }
                    variant="outline-dark"
                    className="white-btn w-100 mt-3"
                    disabled={isSubmitting}
                  >
                    <strong>GO TO REGISTER</strong>
                  </Button> */}
                </Form>
              );
            }}
          </Formik>
        </div>
      )}
    </div>
  );
};

export default Login;
