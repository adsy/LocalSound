import { useState } from "react";
import { Tab, Tabs } from "react-bootstrap";
import EditArtistDetails from "./EditArtistDetails/EditArtistDetails";
import { UserModel } from "../../../../app/model/dto/user.model";

interface Props {
  userDetails: UserModel;
  setSubmittingRequest: (submittingRequest: boolean) => void;
}

const EditArtist = ({ userDetails, setSubmittingRequest }: Props) => {
  const [key, setKey] = useState<string | null>("details");

  return (
    <div id="edit-artist">
      <Tabs
        id="controlled-tab-example"
        activeKey={key!}
        onSelect={(k) => setKey(k)}
        className="mt-2"
      >
        <Tab eventKey="details" title="Account details" className="px-3">
          <EditArtistDetails
            userDetails={userDetails}
            setSubmittingRequest={setSubmittingRequest}
          />
        </Tab>
      </Tabs>
    </div>
  );
};

export default EditArtist;
