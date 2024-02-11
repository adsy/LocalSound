import { useState } from "react";
import Label from "../Label/Label";
import { EquipmentModel } from "../../../app/model/dto/equipment.model";
import { v4 as uuidv4 } from "uuid";

interface Props {
  equipment: EquipmentModel[];
  setEquipment: (equipment: EquipmentModel[]) => void;
  title: string;
  description: string;
}

const EquipmentEntry = ({
  equipment,
  setEquipment,
  title,
  description,
}: Props) => {
  const [newEquipment, setNewEquipment] = useState("");

  const deleteSelectedEquipment = (id: string) => {
    var equipmentsList = [...equipment];
    setEquipment(equipmentsList.filter((x) => x.equipmentId != id));
  };

  const addEquipment = () => {
    setEquipment([
      ...equipment,
      { equipmentId: uuidv4(), equipmentName: newEquipment },
    ]);
    setNewEquipment("");
  };

  return (
    <div className="mb-3">
      <div className="d-flex mb-1">
        <p className="form-label">{title}</p>
      </div>
      <p className="text-justify mb-0">{description}</p>
      <div id="search-label-component">
        <div className="box d-flex flex-column justify-content-between">
          <div className="container">
            {equipment.map((equipment, index) => (
              <span key={index} className="badge-container">
                <Label
                  label={equipment.equipmentName}
                  id={equipment.equipmentId}
                  deleteLabelItem={deleteSelectedEquipment}
                  showDeleteButton={true}
                />
              </span>
            ))}
          </div>

          <input
            className="input"
            placeholder="Show how prepared you are by adding your equipment to your profile"
            value={newEquipment}
            onChange={(e) => {
              setNewEquipment(e.target.value);
            }}
            onKeyDown={(e) => {
              if (e.key === "Enter" && newEquipment.length > 0) {
                addEquipment();
              }
            }}
          />
        </div>
      </div>
    </div>
  );
};

export default EquipmentEntry;
