class SingletonClass {
  audioElementRef: React.RefObject<HTMLAudioElement> | null = null;
  analyzerData: any = null;
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
  };
})();

export { SingletonFactory, SingletonClass };
