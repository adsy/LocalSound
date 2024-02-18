import { useState } from "react";
import { Button } from "react-bootstrap";
import { Formik } from "formik";
import { Form } from "semantic-ui-react";
import TextInput from "../../../common/form/TextInput";
import { UserLoginModel } from "../../../app/model/dto/user-login.model";
import agent from "../../../api/agent";
import InPageLoadingComponent from "../../../app/layout/InPageLoadingComponent";
import { useDispatch } from "react-redux";
import { handleSetUserDetails } from "../../../app/redux/actions/userSlice";
import { handleToggleModal } from "../../../app/redux/actions/modalSlice";
import ConfirmEmailPopUp from "../ConfirmEmail/ConfirmEmailPopUp";
import ErrorBanner from "../../../common/banner/ErrorBanner";
import { handleAppLoading } from "../../../app/redux/actions/applicationSlice";

const Login = () => {
  const [isLoading, setIsLoading] = useState(false);
  const dispatch = useDispatch();

  const handleLoginRequest = async (
    values: UserLoginModel,
    setStatus: (status?: any) => void
  ) => {
    try {
      var result = await agent.Authentication.login({
        email: values.email,
        password: values.password,
      });

      if (result.emailConfirmed) {
        dispatch(handleSetUserDetails(result));
        dispatch(handleAppLoading(false));
        dispatch(
          handleToggleModal({
            open: false,
          })
        );
      } else {
        dispatch(
          handleToggleModal({
            open: true,
            body: <ConfirmEmailPopUp />,
            size: "mini",
          })
        );
      }
    } catch (error) {
      if (error) {
        setStatus({ error: error });
      } else {
        setStatus({
          error:
            "An error occured while trying to log you in, please try again..",
        });
      }
    }
    setIsLoading(false);
  };

  return (
    <div id="modal-popup" className="fade-in">
      <div className="d-flex flex-row">
        <h2 className="header-title align-self-center">
          Login to your account
        </h2>
      </div>
      <div className="w-100 header mt-2 fade-in">
        <Formik
          initialValues={{ email: "", password: "", error: null }}
          onSubmit={async (values, { setStatus }) => {
            setIsLoading(true);
            setStatus(null);
            await handleLoginRequest(values, setStatus);
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
                  <div className="d-flex">
                    <p className="form-label">EMAIL</p>
                  </div>
                  <TextInput
                    name="email"
                    placeholder=""
                    fieldClassName="mt-2"
                    disabled={isSubmitting}
                  />
                  <div className="d-flex">
                    <p className="form-label">PASSWORD</p>
                  </div>
                  <TextInput
                    name="password"
                    placeholder=""
                    type="password"
                    fieldClassName="mt-2"
                    disabled={isSubmitting}
                  />
                </div>
                {status?.error ? (
                  <ErrorBanner className="text-center fade-in mt-3 mb-0 api-error">
                    {status.error}
                  </ErrorBanner>
                ) : null}
                {!isSubmitting ? (
                  <Button
                    className={`white-button w-100 align-self-center ${
                      status?.error ? "mt-3" : "mt-4"
                    }`}
                    type="submit"
                    disabled={
                      isLoading || values.email === "" || values.password === ""
                    }
                  >
                    <h4>Sign in</h4>
                  </Button>
                ) : (
                  <div className="mt-3">
                    <InPageLoadingComponent />
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

export default Login;
