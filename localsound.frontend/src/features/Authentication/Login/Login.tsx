import { useEffect, useState } from "react";
import { Button } from "react-bootstrap";
import { CustomerTypes } from "../../../app/model/enums/customerTypes";
import { Formik } from "formik";
import { Divider, Form } from "semantic-ui-react";
import MyTextInput from "../../../common/form/MyTextInput";
import { UserLoginModel } from "../../../app/model/dto/user-login.model";
import agent from "../../../api/agent";
import InPageLoadingComponent from "../../../app/layout/InPageLoadingComponent";
import { useDispatch } from "react-redux";
import { handleSetUserDetails } from "../../../app/redux/actions/userSlice";

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

      dispatch(handleSetUserDetails(result));

      // TODO: redirect once logged in
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
    <div id="auth-modal" className="fade-in">
      <div className="d-flex flex-row header">
        <h2 className="header-title mt-1 align-self-center">
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
                  <MyTextInput
                    name="email"
                    placeholder=""
                    fieldClassName="mt-2"
                    disabled={isSubmitting}
                  />
                  <div className="d-flex">
                    <p className="form-label">PASSWORD</p>
                  </div>
                  <MyTextInput
                    name="password"
                    placeholder=""
                    type="password"
                    fieldClassName="mb-1"
                    disabled={isSubmitting}
                  />
                </div>
                {status?.error ? (
                  <p className="text-center fade-in mb-0 api-error">
                    {status.error}
                  </p>
                ) : null}
                {!isSubmitting ? (
                  <Button
                    className={`purple-button w-100 align-self-center ${
                      status?.error ? "mt-3" : "mt-5"
                    }`}
                    type="submit"
                    disabled={
                      isLoading || values.email === "" || values.password === ""
                    }
                  >
                    <strong>LOGIN</strong>
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
