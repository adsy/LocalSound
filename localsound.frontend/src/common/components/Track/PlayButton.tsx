import { Button } from "react-bootstrap";
import { Icon } from "semantic-ui-react";

interface Props {
  handlePlay: () => void;
  playing: boolean;
}

const PlayButton = ({ handlePlay, playing }: Props) => {
  return (
    <Button className="play-button" onClick={() => handlePlay()}>
      {!playing ? (
        <Icon name="play" size="small" color="grey" />
      ) : (
        <Icon name="pause" size="small" color="grey" />
      )}
    </Button>
  );
};

export default PlayButton;
