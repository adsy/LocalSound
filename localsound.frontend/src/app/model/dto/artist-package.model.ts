import { EquipmentModel } from "./equipment.model";

export interface ArtistPackageModel {
  artistPackageId?: string;
  packageName: string;
  packageDescription: string;
  packagePrice: string;

  equipment: EquipmentModel[];
  photos?: ArtistPackagePhotoModel[];
}

export interface ArtistPackagePhotoModel {
  artistPackagePhotoId: string;
  artistPackageId: string;
  artistPackagePhotoUrl: string;
}
