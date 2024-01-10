import { ArtistTrackUploadModel } from "../../../app/model/dto/artist-track-upload.model";
import TrackContainer from "./TrackContainer";
import PlayButton from "./PlayButton";
import Label from "../Label/Label";
import { useDispatch, useSelector } from "react-redux";
import {
  handlePauseSong,
  handlePlaySong,
  handleSetPlayerSong,
  handleSetTrackList,
} from "../../../app/redux/actions/playerSlice";
import { State } from "../../../app/model/redux/state";
import { UserModel } from "../../../app/model/dto/user.model";
import { Icon, Image as ImageComponent, Placeholder } from "semantic-ui-react";
import { useEffect, useState } from "react";
import {
  SingletonClass,
  SingletonFactory,
} from "../../appSingleton/appSingleton";
import WaveForm from "../../../features/MusicPlayer/Waveform";
import { Button } from "react-bootstrap";
import { handleToggleModal } from "../../../app/redux/actions/modalSlice";
import EditTrackForm from "../../../features/UserProfile/Artist/Uploads/EditTrackForm";
import DeleteTrackConfirmation from "../../../features/UserProfile/Artist/Uploads/DeleteTrackConfirmation";
import { AccountImageTypes } from "../../../app/model/enums/accountImageTypes";
import PlaceholderImg from "../../../assets/placeholder.png";

interface Props {
  track: ArtistTrackUploadModel;
  artistDetails: UserModel;
  tracks: ArtistTrackUploadModel[];
  setTracks: (tracks: ArtistTrackUploadModel[]) => void;
  canLoadMore: boolean;
  page: number;
}

const Track = ({
  track,
  artistDetails,
  tracks,
  setTracks,
  canLoadMore,
  page,
}: Props) => {
  const player = useSelector((state: State) => state.player);
  const loggedInUser = useSelector((state: State) => state.user.userDetails);
  const [singleton, setSingleton] = useState<SingletonClass>(
    SingletonFactory.getInstance()
  );
  const [analyzerData, setAnalyzerData] = useState<any>(null);
  const [trackImageLoaded, setTrackImageLoaded] = useState(false);
  const [trackImage, setTrackImage] = useState<string | null>(null);
  const dispatch = useDispatch();

  useEffect(() => {
    setTrackImage(null);
    if (track.trackImageUrl) {
      const IMAGES = [track.trackImageUrl];
      Promise.all(IMAGES.map((image) => loadImage(image)))
        .then(() => {
          setTrackImage(track.trackImageUrl);
        })
        .catch((err) => console.log("Failed to load images", err))
        .finally(() => {
          setTrackImageLoaded(true);
        });
    } else {
      var profileImg = artistDetails.images.find(
        (x) => x.accountImageTypeId == AccountImageTypes.ProfileImage
      );
      if (profileImg) {
        setTrackImage(profileImg.accountImageUrl);
      } else {
        setTrackImage(PlaceholderImg);
      }
    }
  }, [track.artistTrackUploadId, track.trackImageUrl]);

  const loadImage = (image: string) => {
    if (trackImageLoaded) setTrackImageLoaded(false);
    return new Promise((resolve, reject) => {
      const loadImg = new Image();
      loadImg.src = image;
      loadImg.onload = () => resolve(image);
      loadImg.onerror = (err) => reject(err);
    });
  };

  useEffect(() => {
    if (player.trackId === track.artistTrackUploadId && player.playing) {
      if (!analyzerData) {
        setAnalyzerData(singleton.analyzerData);
      }
    } else {
      if (analyzerData !== null) {
        setAnalyzerData(null);
      }
    }
  }, [player.trackId, player.trackName, player.playing]);

  const playSong = () => {
    if (player.playing && player.trackId === track.artistTrackUploadId) {
      dispatch(handlePauseSong());
    } else if (
      !player.playing &&
      player.trackId === track.artistTrackUploadId
    ) {
      dispatch(handlePlaySong());
    } else {
      dispatch(
        handleSetPlayerSong({
          trackId: track.artistTrackUploadId,
          trackUrl: track.trackUrl,
          artistProfile: track.artistProfile,
          trackName: track.trackName,
          artistName: track.artistName,
          trackImage: trackImage,
          duration: track.duration,
        })
      );
      dispatch(
        handleSetTrackList({
          trackList: tracks,
          canLoadMore: canLoadMore,
          page: page,
        })
      );
    }
  };

  const openEditTrackModal = () => {
    dispatch(
      handleToggleModal({
        open: true,
        body: (
          <EditTrackForm
            userDetails={loggedInUser!}
            trackDetails={track}
            tracks={tracks}
            setTracks={setTracks}
          />
        ),
        size: "large",
      })
    );
  };

  const openDeleteModal = () => {
    dispatch(
      handleToggleModal({
        open: true,
        body: (
          <DeleteTrackConfirmation
            loggedInUser={loggedInUser!}
            track={track}
            tracks={tracks}
            setTracks={setTracks}
          />
        ),
        size: "tiny",
      })
    );
  };

  return (
    <div id="track" className="mt-3">
      <div className="d-flex flex-row w-100">
        {!trackImage ? (
          <div className="mr-3 fade-in">
            <Placeholder className="track-image">
              <Placeholder.Image />
            </Placeholder>
          </div>
        ) : (
          <ImageComponent
            size="small"
            src={trackImage}
            className="mr-3 track-image fade-in"
          />
        )}
        <div className="d-flex flex-column w-100 fade-in">
          <div className="d-flex flex-row justify-content-between">
            <div className="d-flex flex-row">
              <TrackContainer>
                <PlayButton
                  handlePlay={playSong}
                  playing={
                    track.artistTrackUploadId === player.trackId &&
                    player.playing
                  }
                />
              </TrackContainer>
              <div className="d-flex flex-column ml-2">
                <p className="artist-name mb-0">{artistDetails.name}</p>
                <p className="mb-0 track-name">{track.trackName}</p>
              </div>
            </div>

            <div className="my-1 action-row">
              {artistDetails.memberId === loggedInUser?.memberId ? (
                <Button
                  className="white-button track-button mr-1"
                  onClick={() => openEditTrackModal()}
                >
                  <h4>
                    <Icon name="pencil" size="small" className="mr-0" />
                  </h4>
                </Button>
              ) : null}
              {artistDetails.memberId === loggedInUser?.memberId ? (
                <Button
                  className="white-button track-button bin-button"
                  onClick={async () => await openDeleteModal()}
                >
                  <h4>
                    <Icon name="trash" size="small" className="mr-0" />
                  </h4>
                </Button>
              ) : null}
            </div>
          </div>

          <div className="w-100 h-100 d-flex flex-column align-items-center">
            <div className="w-100 h-100 position-relative">
              {track.artistTrackUploadId === player.trackId && analyzerData ? (
                <div>
                  <WaveForm
                    trackId={track.artistTrackUploadId}
                    analyzerData={analyzerData}
                  />
                </div>
              ) : null}
            </div>
          </div>
        </div>
      </div>

      <div className="d-flex flex-row justify-content-end fade-in">
        <div className="track-genre-list">
          {track.genres.map((genre, index) => (
            <Label key={index} id={genre.genreId} label={genre.genreName} />
          ))}
        </div>
      </div>
    </div>
  );
};

export default Track;
