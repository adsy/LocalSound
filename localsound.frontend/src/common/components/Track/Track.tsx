import { ArtistTrackModel } from "../../../app/model/dto/artist-track-upload.model";
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
import { Icon, Image as ImageComponent } from "semantic-ui-react";
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
import agent from "../../../api/agent";
import Login from "../../../features/Authentication/Login/Login";
import ErrorBanner from "../../banner/ErrorBanner";
import { PlaylistTypes } from "../../../app/model/enums/playlistTypes";
import signalHub from "../../../api/signalR";
import { CustomerTypes } from "../../../app/model/enums/customerTypes";
import PlaceHolderImg from "../../../assets/placeholder.png";

interface Props {
  track: ArtistTrackModel;
  tracks: ArtistTrackModel[];
  setTracks: (tracks: ArtistTrackModel[]) => void;
  canLoadMore: boolean;
  artistName: string;
  artistMemberId: string;
  playlistType: PlaylistTypes;
  listeningProfileMemberId: string;
  viewingOwnProfile: boolean;
}

const Track = ({
  track,
  tracks,
  setTracks,
  canLoadMore,
  artistName,
  artistMemberId,
  playlistType,
  listeningProfileMemberId,
  viewingOwnProfile,
}: Props) => {
  const userDetails = useSelector((state: State) => state.user.userDetails);
  const player = useSelector((state: State) => state.player);
  const loggedInUser = useSelector((state: State) => state.user.userDetails);
  const [singleton] = useState<SingletonClass>(SingletonFactory.getInstance());
  const [analyzerData, setAnalyzerData] = useState<any>(null);
  const [trackImageLoaded, setTrackImageLoaded] = useState(false);
  const [trackImage, setTrackImage] = useState<string | null>(null);
  const [trackLikeError, setTrackLikeError] = useState<string | null>(null);
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
      setTrackImage(PlaceHolderImg);
    }
  }, [track.artistTrackId, track.trackImageUrl]);

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
    if (
      player.currentSong?.trackId === track.artistTrackId &&
      player.currentSong?.playing
    ) {
      if (!analyzerData) {
        setAnalyzerData(singleton.analyzerData);
      }
    } else {
      if (analyzerData !== null) {
        setAnalyzerData(null);
      }
    }
  }, [
    player.currentSong?.trackId,
    player.currentSong?.trackName,
    player.currentSong?.playing,
  ]);

  const playSong = () => {
    if (
      player.currentSong?.playing &&
      player.currentSong?.trackId === track.artistTrackId
    ) {
      dispatch(handlePauseSong());
    } else if (
      !player.currentSong?.playing &&
      player.currentSong?.trackId === track.artistTrackId
    ) {
      dispatch(handlePlaySong());
    } else {
      dispatch(
        handleSetPlayerSong({
          trackId: track.artistTrackId,
          trackUrl: track.trackUrl,
          currentSongArtistProfile: track.artistProfile,
          listeningMemberId: track.artistMemberId,
          trackName: track.trackName,
          artistName: track.artistName,
          trackImage: track.trackImageUrl,
          duration: track.duration,
          playlistType: playlistType,
          uploadDate: track.uploadDate,
          songLikeId: track.songLikeId,
        })
      );
      if (
        player.listeningProfileMemberId !== listeningProfileMemberId ||
        player.playlistType !== playlistType
      ) {
        dispatch(
          handleSetTrackList({
            trackList: tracks,
            canLoadMore: canLoadMore,
            listeningProfileMemberId: listeningProfileMemberId,
          })
        );
      }
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

  const likeSong = async () => {
    setTrackLikeError(null);
    if (loggedInUser?.memberId) {
      try {
        var trackClone = { ...track };
        var clone = [...tracks];
        var trackIndex = clone.findIndex(
          (x) => x.artistTrackId === track.artistTrackId
        );
        if (track.songLiked && track.songLikeId) {
          await agent.Tracks.unlikeSong(
            loggedInUser?.memberId,
            track.songLikeId
          );

          if (playlistType === PlaylistTypes.Favourites && viewingOwnProfile) {
            clone.splice(trackIndex, 1);
            setTracks(clone);
          } else {
            trackClone.songLiked = false;
            trackClone.likeCount--;
            clone[trackIndex] = trackClone;
            setTracks(clone);
          }
        } else {
          var trackData = {
            artistId: track.artistMemberId,
            trackId: track.artistTrackId,
          };
          await agent.Tracks.likeSong(loggedInUser?.memberId, trackData);

          trackClone.songLiked = true;
          trackClone.likeCount++;

          await signalHub.createNotification({
            receiverMemberId: track.artistMemberId,
            message: `${
              userDetails?.customerType === CustomerTypes.Artist
                ? userDetails.name
                : userDetails?.firstName + " " + userDetails?.lastName
            } just liked your track ${track.trackName}.`,
            redirectUrl: "",
          });

          clone[trackIndex] = trackClone;
          setTracks(clone);
        }
      } catch (err: any) {
        console.log(err);
        setTrackLikeError(err);
      }
    } else {
      dispatch(
        handleToggleModal({
          open: true,
          body: <Login />,
          size: "tiny",
        })
      );
    }
  };

  return (
    <div id="track" className="mt-3 d-flex flex-column">
      {trackLikeError ? <ErrorBanner children={trackLikeError} /> : null}
      <div className="d-flex flex-row w-100">
        <ImageComponent
          size="small"
          src={trackImage}
          className={`track-image ${
            track.artistTrackId === player.currentSong?.trackId &&
            player.currentSong.playing
              ? "playing"
              : ""
          } fade-in`}
        />
        <div className="d-flex flex-column w-100 fade-in">
          <div className="d-flex flex-row justify-content-between">
            <div className="d-flex flex-row">
              <TrackContainer>
                <PlayButton
                  handlePlay={playSong}
                  playing={
                    track.artistTrackId === player.currentSong?.trackId &&
                    player.currentSong?.playing
                  }
                />
              </TrackContainer>
              <div className="d-flex flex-column ml-2">
                <p className="artist-name mb-0">{artistName}</p>
                <p className="mb-0 track-name">{track.trackName}</p>
              </div>
            </div>

            <div className="my-1 action-row">
              <Button
                className={`track-button mr-1 ${
                  !track.songLiked ? "white-button" : "purple-button"
                }`}
                onClick={async () => await likeSong()}
              >
                <h4 className="d-flex flex-row align-items-center">
                  <Icon name="heart" size="small" className="pr-1" />
                  <span className="ml-1">{track.likeCount}</span>
                </h4>
              </Button>
              {artistMemberId === loggedInUser?.memberId &&
              viewingOwnProfile ? (
                <Button
                  className="white-button track-button mr-1"
                  onClick={() => openEditTrackModal()}
                >
                  <h4>
                    <Icon name="pencil" size="small" className="mr-0" />
                  </h4>
                </Button>
              ) : null}
              {artistMemberId === loggedInUser?.memberId &&
              viewingOwnProfile ? (
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
              {track.artistTrackId === player.currentSong?.trackId &&
              analyzerData ? (
                <>
                  <WaveForm analyzerData={analyzerData} />
                  {/* <ThreeDAnalyzer /> */}
                </>
              ) : null}
            </div>
          </div>
        </div>
      </div>

      <div className="d-flex flex-row justify-content-end fade-in">
        <div className="track-genre-list d-flex flex-wrap-reverse justify-content-end">
          {track.genres.map((genre, index) => (
            <Label key={index} id={genre.genreId} label={genre.genreName} />
          ))}
        </div>
      </div>
    </div>
  );
};

export default Track;
