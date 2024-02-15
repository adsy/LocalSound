export interface CurrentSongState {
  trackId: string | null;
  playing: boolean;
  trackUrl: string | null;
  trackName: string | null;
  trackImage: string | null;
  artistName: string | null;
  currentSongArtistProfile: string | null;
  duration: number | null;
}
