import { Formik } from "formik";
import { useState } from "react";
import { Button } from "react-bootstrap";
import { Form } from "semantic-ui-react";
import MyTextInput from "../../../common/form/MyTextInput";
import InPageLoadingComponent from "../../../app/layout/InPageLoadingComponent";
import agent from "../../../api/agent";
import { useDispatch } from "react-redux";
import { handleSetUserDetails } from "../../../app/redux/actions/userSlice";
import { handleToggleModal } from "../../../app/redux/actions/modalSlice";

const ConfirmEmailPopUp = () => {
  const [reloadError, setReloadError] = useState(false);
  const [invalidTokenError, setInvalidTokenError] = useState(false);
  const [resendingToken, setResendingToken] = useState(false);
  const dispatch = useDispatch();

  const resetErrorState = () => {
    setReloadError(false);
    setInvalidTokenError(false);
  };

  const submitEmailToken = async (token: string) => {
    try {
      resetErrorState();
      var result = await agent.Authentication.confirmEmail(token);
      dispatch(handleSetUserDetails(result));
      dispatch(
        handleToggleModal({
          open: false,
        })
      );
    } catch (err) {
      console.log(err);
      setInvalidTokenError(true);
      //TODO: Do something with the error
    }
  };

  const resendEmailToken = async () => {
    try {
      resetErrorState();
      setResendingToken(true);
      await agent.Authentication.resendEmailToken();
      setResendingToken(false);
    } catch (err) {
      //TODO: Do something with the error
    }
  };

  return (
    <div id="auth-modal">
      <div className="navbar-logo d-flex flex-column justify-content-center align-content-center">
        <div className="d-flex justify-content-center pb-2"></div>
        <div className="d-flex mb-4 mt-2">
          <h2 className="header-title mt-1">Confirm your email</h2>
        </div>

        <p>
          Enter the token which has been sent to your email address so we can
          confirm your account.
        </p>
        <Formik
          initialValues={{ token: "", error: null }}
          onSubmit={async (values) => {
            await submitEmailToken(values.token);
          }}
        >
          {({ handleSubmit, isSubmitting, values }) => {
            return (
              <Form
                className="ui form error fade-in"
                onSubmit={handleSubmit}
                autoComplete="off"
              >
                <div className="form-body">
                  <MyTextInput
                    name="token"
                    placeholder=""
                    fieldClassName="mt-2"
                    disabled={isSubmitting}
                  />
                </div>
                {invalidTokenError ? (
                  <h5 className="inverse text-center fade-in mb-0 mt-3">
                    The token you have entered is incorrect, please try again..
                  </h5>
                ) : null}
                {reloadError ? (
                  <h5 className="inverse text-center fade-in mb-0 mt-3">
                    There was an error performing this action, please close the
                    pop up and try logging in again..
                  </h5>
                ) : null}

                {isSubmitting || resendingToken ? (
                  <div className="pb-2 pt-3">
                    <InPageLoadingComponent />
                  </div>
                ) : (
                  <div className="d-flex flex-column align-content-center">
                    <Button
                      variant=""
                      className="black-button align-self-center fade-in mt-3 w-100"
                      type="submit"
                      disabled={isSubmitting || !values.token}
                    >
                      <h5>Confirm token</h5>
                    </Button>
                    <div className="d-flex justify-content-center mt-2">
                      <Button onClick={() => resendEmailToken()} variant="link">
                        Request new token
                      </Button>
                    </div>
                  </div>
                )}
              </Form>
            );
          }}
        </Formik>
      </div>
    </div>
  );
};

export default ConfirmEmailPopUp;