import { EquipmentModel } from "./equipment.model";

export interface ArtistPackageModel {
  artistPackageId?: string;
  artistPackageName: string;
  artistPackageDescription: string;
  artistPackagePrice: string;

  equipment: EquipmentModel[];
  photos: ArtistPackagePhotoModel[];
}

export interface ArtistPackagePhotoModel {
  artistPackagePhotoId: string;
  artistPackagePhotoUrl: string;
}
