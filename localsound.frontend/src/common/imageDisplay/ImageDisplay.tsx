import { Image } from "semantic-ui-react";

interface Props {
  imageUrl: string;
}
const ImageDisplay = ({ imageUrl }: Props) => {
  return (
    <div id="modal-popup" className="mt-3 px-4 py-4">
      <Image size="huge" src={imageUrl} />
    </div>
  );
};

export default ImageDisplay;
