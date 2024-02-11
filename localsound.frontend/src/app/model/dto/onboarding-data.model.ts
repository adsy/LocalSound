import { EquipmentModel } from "./equipment.model";
import { EventTypeModel } from "./eventType.model";
import { GenreModel } from "./genre.model";

export interface OnboardingDataModel {
  aboutSection: string;
  genres: GenreModel[];
  eventTypes?: EventTypeModel[];
  equipment?: EquipmentModel[];
}
