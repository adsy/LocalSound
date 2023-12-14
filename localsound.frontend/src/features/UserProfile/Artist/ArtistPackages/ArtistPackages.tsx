import { useLayoutEffect, useState } from "react";
import { ArtistProfileTabs } from "../../../../app/model/enums/artistProfileTabTypes";
import agent from "../../../../api/agent";
import { UserModel } from "../../../../app/model/dto/user.model";
import { ArtistPackageModel } from "../../../../app/model/dto/artist-package.model";
import ErrorBanner from "../../../../common/banner/ErrorBanner";
import InfoBanner from "../../../../common/banner/InfoBanner";
import { Icon } from "semantic-ui-react";

interface Props {
  currentTab: ArtistProfileTabs;
  artistDetails: UserModel;
  viewingOwnProfile: boolean;
}

const ArtistPackages = ({
  currentTab,
  artistDetails,
  viewingOwnProfile,
}: Props) => {
  const [loading, setLoading] = useState(false);
  const [packages, setPackages] = useState<ArtistPackageModel[]>([]);
  const [loadPackagesError, setLoadPackagesError] = useState<string | null>();

  useLayoutEffect(() => {
    (async () => {
      if (currentTab === ArtistProfileTabs.Uploads) {
        try {
          setLoading(true);
          var result = await agent.Packages.getPackages(
            artistDetails!.memberId
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
    };
  }, [currentTab]);

  return (
    <div id="artist-packages">
      {loadPackagesError ? <ErrorBanner children={loadPackagesError} /> : null}
      {packages?.length < 1 ? (
        <InfoBanner className="fade-in mb-2 mx-3">
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
      ) : null}
    </div>
  );
};

export default ArtistPackages;
