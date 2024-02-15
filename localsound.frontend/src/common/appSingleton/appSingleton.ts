class SingletonClass {
  audioElementRef: React.RefObject<HTMLAudioElement> | null = null;
  analyzerData: any = null;
  refreshingToken: boolean = false;
  threeAudio: THREE.Audio<GainNode> | null = null;
}

var SingletonFactory = (function () {
  var instance: SingletonClass | null;

  return {
    getInstance: function () {
      if (instance == null) {
        instance = new SingletonClass();
      }
      return instance;
    },
    setRefreshingTokenTrue: function () {
      if (instance) {
        instance.refreshingToken = true;
      }
    },
    setRefreshingTokenFalse: function () {
      if (instance) {
        instance.refreshingToken = false;
      }
    },
  };
})();

export { SingletonFactory, SingletonClass };
