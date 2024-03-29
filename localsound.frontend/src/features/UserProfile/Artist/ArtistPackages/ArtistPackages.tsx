import { useLayoutEffect, useState } from "react";
import { ProfileTabs } from "../../../../app/model/enums/ProfileTabTypes";
import agent from "../../../../api/agent";
import { UserModel } from "../../../../app/model/dto/user.model";
import { ArtistPackageModel } from "../../../../app/model/dto/artist-package.model";
import ErrorBanner from "../../../../common/banner/ErrorBanner";
import InfoBanner from "../../../../common/banner/InfoBanner";
import { Icon } from "semantic-ui-react";
import InPageLoadingComponent from "../../../../app/layout/InPageLoadingComponent";
import ArtistPackageDisplay from "./ArtistPackageDisplay";

interface Props {
  currentTab: ProfileTabs;
  profileDetails: UserModel;
  viewingOwnProfile: boolean;
  packages: ArtistPackageModel[];
  setPackages: (packages: ArtistPackageModel[]) => void;
}

const ArtistPackages = ({
  currentTab,
  profileDetails,
  viewingOwnProfile,
  packages,
  setPackages,
}: Props) => {
  const [loading, setLoading] = useState(true);
  const [loadPackagesError, setLoadPackagesError] = useState<string | null>();
  const [selectedPackageId, setSelectedPackageId] = useState<string | null>(
    null
  );

  useLayoutEffect(() => {
    (async () => {
      if (currentTab === ProfileTabs.Packages) {
        try {
          setLoading(true);
          var result = await agent.Packages.getPackages(
            profileDetails!.memberId
          );
          setPackages([...result]);
        } catch (err: any) {
          setLoadPackagesError(err);
        }
        setLoading(false);
      }
    })();

    return () => {
      setLoadPackagesError(null);
      setPackages([]);
      setSelectedPackageId(null);
    };
  }, [currentTab]);

  return (
    <div id="artist-packages">
      {loadPackagesError ? <ErrorBanner children={loadPackagesError} /> : null}
      {packages?.length < 1 && !loading && loadPackagesError === null ? (
        <InfoBanner className="fade-in mb-2">
          <div className="d-flex flex-row justify-content-center align-items-center">
            <Icon
              name="box"
              size="small"
              className="box-icon d-flex align-items-center justify-content-center"
            />
            <div className="ml-2">
              <p className="mb-0">
                {viewingOwnProfile
                  ? "You havent created any packages yet. Click on the box icon within your profile banner to start creating a package."
                  : "This artist hasn't created any packages yet."}
              </p>
            </div>
          </div>
        </InfoBanner>
      ) : (
        <div className="fade-in p-2 package-container d-flex flex-row flex-wrap justify-content-around">
          {packages.map((artistPackage, index) => (
            <div
              key={index}
              id="artist-package-display"
              className={`col-12 col-lg-4 px-2 ${
                selectedPackageId === null
                  ? ""
                  : selectedPackageId === artistPackage.artistPackageId
                  ? "selected-package"
                  : "d-none"
              }`}
            >
              <ArtistPackageDisplay
                artistPackage={artistPackage}
                packages={packages}
                setPackages={setPackages}
                profileDetails={profileDetails}
                selectedPackageId={selectedPackageId}
                setSelectedPackageId={setSelectedPackageId}
              />
            </div>
          ))}
        </div>
      )}
      {loading ? (
        <div className="h-100 d-flex justify-content-center align-self-center">
          <InPageLoadingComponent height={80} width={80} />
        </div>
      ) : null}
    </div>
  );
};

export default ArtistPackages;
