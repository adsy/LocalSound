import { useCallback, useEffect, useMemo } from "react";

interface Props {
  hasMoreItems: boolean;
  fetchMoreItems: () => void;
}

const useFixMissingScroll = ({ hasMoreItems, fetchMoreItems }: Props) => {
  const mainElement = useMemo(() => document.querySelector("main"), []);
  const bodyElement = useMemo(() => document.querySelector("body"), []);

  const fetchCb = useCallback(() => {
    fetchMoreItems();
  }, [fetchMoreItems]);

  useEffect(() => {
    const hasScroll =
      mainElement && bodyElement
        ? mainElement.scrollHeight > bodyElement.clientHeight
        : false;
    if (!hasScroll && hasMoreItems) {
      setTimeout(() => {
        fetchCb();
      }, 100);
    }
  }, [hasMoreItems, mainElement, bodyElement]);
};

export default useFixMissingScroll;
