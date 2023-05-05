import { Button, Row } from "react-bootstrap";
import { useState } from "react";

const LandingPageBanner = () => {
  return (
    <Row className="banner mb-3">
      <div className="d-flex flex-column justify-content-between">
        <div className="d-flex flex-row justify-content-between h-100">
          <div className="d-flex flex-column justify-content-end">
            <div className="d-flex flex-row">
              <span className="navbar-logo align-self-center"></span>
              <h2 className="page-title font-bold ml-1 mt-1">LocalSound</h2>
            </div>
          </div>

          <div className="mt-2">
            <Button className="login-button mr-2">Login</Button>
            <Button className="blue-button">Create account</Button>
          </div>
        </div>
      </div>
    </Row>
  );
};
export default LandingPageBanner;

{
  /* <Formik
  initialValues={{ name: "", error: null }}
  onSubmit={async (values, { setStatus }) => {
    setIsLoading(true);
    setStatus(null);
    //TODO: Actually do something here
  }}
>
  {({ handleSubmit, isSubmitting, status, values }) => {
    return (
      <Form
        className="ui form error fade-in"
        onSubmit={handleSubmit}
        autoComplete="off"
      >
        <MyTextInput
          name="name"
          placeholder="Search for a local artist"
          className="mt-2 w-100 br-5 black-border p-2"
          disabled={isSubmitting}
        />
      </Form>
    );
  }}
</Formik>; */
}
