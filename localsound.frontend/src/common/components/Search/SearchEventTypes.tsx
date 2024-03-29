import { useEffect, useMemo, useRef, useState } from "react";
import { EventTypeModel } from "../../../app/model/dto/eventType.model";
import agent from "../../../api/agent";
import lodash from "lodash";
import ErrorBanner from "../../banner/ErrorBanner";
import Label from "../Label/Label";

interface Props {
  eventTypes: EventTypeModel[];
  setEventTypes: (eventTypes: EventTypeModel[]) => void;
  title: string;
  description: string;
}

const SearchEventTypes = ({
  eventTypes,
  setEventTypes,
  title,
  description,
}: Props) => {
  const [eventType, setEventType] = useState("");
  const [eventTypeList, setEventTypeList] = useState<EventTypeModel[]>([]);
  const [error, setError] = useState<string | null>(null);
  const ref = useRef<() => void>();

  const onChange = async () => {
    try {
      setEventTypeList([]);
      if (eventType.length > 0) {
        setError(null);
        var result = await agent.EventType.searchEventType(eventType);
        var eventTypesFilter = result.filter(
          (x) => !eventTypes.find((z) => x.eventTypeName === z.eventTypeName)
        );
        setEventTypeList(eventTypesFilter.slice(0, 5));
      }
    } catch (e) {
      setError(
        "There was an error while searching, please refresh your page and try again."
      );
    }
  };

  const deleteSelectedEventType = (id: string) => {
    var eventTypeList = [...eventTypes];
    setEventTypes(eventTypeList.filter((x) => x.eventTypeId != id));
  };

  const addEventType = (eventType: EventTypeModel) => {
    setEventType("");
    setEventTypeList([]);
    setEventTypes([...eventTypes, eventType]);
  };

  useEffect(() => {
    ref.current = onChange;
  }, [onChange]);

  const doCallbackWithDebounce = useMemo(() => {
    const callback = () => ref.current?.();
    return lodash.debounce(callback, 500);
  }, []);

  return (
    <div className="mb-3">
      <div className="d-flex mb-1">
        <p className="form-label">{title}</p>
      </div>
      <p className="text-justify mb-0">{description}</p>
      <div id="search-label-component">
        <div className="box d-flex flex-column justify-content-between">
          <div className="container">
            {eventTypes.map((eventType, index) => (
              <span key={index} className="badge-container">
                <Label
                  label={eventType.eventTypeName}
                  id={eventType.eventTypeId}
                  deleteLabelItem={deleteSelectedEventType}
                  showDeleteButton={true}
                />
              </span>
            ))}
          </div>

          <input
            className="input"
            placeholder="Add some event types to your profile so you can be found"
            value={eventType}
            onChange={(e) => {
              doCallbackWithDebounce();
              setEventType(e.target.value);
            }}
          />
        </div>
        <div className="positive-relative">
          {eventTypeList.length > 0 ? (
            <div className="dropdown">
              {eventTypeList.map((eventType, index) => {
                return (
                  <div
                    key={index}
                    onClick={() => addEventType(eventType)}
                    className="dropdown-option"
                  >
                    {eventType.eventTypeName}
                  </div>
                );
              })}
            </div>
          ) : null}
        </div>
        {error ? (
          <ErrorBanner className="mt-2 text-center">{error}</ErrorBanner>
        ) : null}
      </div>
    </div>
  );
};

export default SearchEventTypes;
