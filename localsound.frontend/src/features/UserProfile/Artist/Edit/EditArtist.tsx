import { useState } from "react";
import { Tab, Tabs } from "react-bootstrap";
import EditArtistDetails from "./EditArtistDetails/EditArtistDetails";
import { UserModel } from "../../../../app/model/dto/user.model";
import EditArtistProfile from "./EditArtistProfile/EditArtistProfile";

interface Props {
  userDetails: UserModel;
}

const EditArtist = ({ userDetails }: Props) => {
  const [key, setKey] = useState<string | null>("profileDetails");

  return (
    <div id="edit-artist">
      <Tabs
        id="controlled-tab-example"
        activeKey={key!}
        onSelect={(k) => setKey(k)}
        className="mt-2"
      >
        <Tab
          eventKey="profileDetails"
          title="Personal details"
          className="px-3"
        >
          <EditArtistDetails userDetails={userDetails} />
        </Tab>
        <Tab eventKey="artistDetails" title="Profile details" className="px-3">
          <EditArtistProfile userDetails={userDetails} />
        </Tab>
      </Tabs>
    </div>
  );
};

export default EditArtist;
