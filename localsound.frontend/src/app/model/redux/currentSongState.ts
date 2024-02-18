export interface CurrentSongState {
  trackId: number | null;
  playing: boolean;
  trackUrl: string | null;
  trackName: string | null;
  trackImage: string | null;
  artistName: string | null;
  currentSongArtistProfile: string | null;
  duration: number | null;
  uploadDate: Date;
}
