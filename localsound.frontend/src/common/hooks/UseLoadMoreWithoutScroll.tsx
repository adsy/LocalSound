import { useCallback, useEffect, useMemo } from "react";

interface Props {
  hasMoreItems: boolean;
  fetchMoreItems: () => void;
}

const useFixMissingScroll = ({ hasMoreItems, fetchMoreItems }: Props) => {
  const mainElement = useMemo(() => document.querySelector("main"), []);

  const fetchCb = useCallback(() => {
    fetchMoreItems();
  }, [fetchMoreItems]);

  useEffect(() => {
    const hasScroll = mainElement
      ? mainElement.scrollHeight > mainElement.clientHeight
      : false;
    if (!hasScroll && hasMoreItems) {
      setTimeout(() => {
        fetchCb();
      }, 100);
    }
  }, [hasMoreItems, fetchCb, mainElement]);
};

export default useFixMissingScroll;
