export interface PlayerState {
  trackId: string | null;
  playing: boolean;
  trackUrl: string | null;
  trackName: string | null;
  trackImage: string | null;
  artistName: string | null;
  artistProfile: string | null;
  duration: number | null;
}
