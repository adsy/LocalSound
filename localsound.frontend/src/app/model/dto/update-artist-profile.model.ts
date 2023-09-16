import { EquipmentModel } from "./equipment.model";
import { EventTypeModel } from "./eventType.model";
import { GenreModel } from "./genre.model";

export interface UpdateArtistProfileDetailsModel {
  genres: GenreModel[];
  eventTypes: EventTypeModel[];
  equipment: EquipmentModel[];
}
