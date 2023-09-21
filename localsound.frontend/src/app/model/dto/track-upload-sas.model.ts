export interface TrackUploadSASModel {
  accountName: string;
  accountUrl: string;
  containerName: string;
  containerUri: string;
  uploadLocation: string;
  trackId: string;
  sasUri: string;
  sasToken: string;
  sasPermission: string;
  sasExpire: string;
}
