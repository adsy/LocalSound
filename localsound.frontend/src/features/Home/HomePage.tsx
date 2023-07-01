import ProfileSummary from "../UserProfile/UserProfileSummary";

const HomePage = () => {
  console.log("home");
  return (
    <div id="home-page" className="fade-in">
      <ProfileSummary />
    </div>
  );
};

export default HomePage;
