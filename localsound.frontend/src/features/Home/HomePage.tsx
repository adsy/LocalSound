import { useSelector } from "react-redux";
import { State } from "../../app/model/redux/state";
import { useEffect } from "react";
import { useHistory } from "react-router-dom";

const HomePage = () => {
  const userDetails = useSelector((state: State) => state.user.userDetails);
  const history = useHistory();

  useEffect(() => {
    if (!userDetails) {
      history.push("/");
    }
  }, [userDetails]);

  return (
    <div id="home-page">
      <div className="container">test</div>
      <div className="container">test</div>
      <div className="container">test</div>
      <div className="container">test</div>
      <div className="container">test</div>
      <div className="container">test</div>
      <div className="container">test</div>
      <div className="container">test</div>
      <div className="container">test</div>
      <div className="container">test</div>
    </div>
  );
};

export default HomePage;
