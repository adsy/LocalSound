import { useEffect, useRef } from "react";
import * as THREE from "three";
import { SingletonFactory } from "../../common/appSingleton/appSingleton";
import { RenderPass } from "three/examples/jsm/postprocessing/RenderPass.js";
import { UnrealBloomPass } from "three/examples/jsm/postprocessing/UnrealBloomPass.js";
import { EffectComposer, OutputPass } from "three/examples/jsm/Addons.js";

const ThreeDAnalyzer = () => {
  const ref = useRef<HTMLDivElement>(null);
  var singleton = SingletonFactory.getInstance();

  useEffect(() => {
    if (ref.current) {
      const renderer = new THREE.WebGLRenderer({
        antialias: true,
        alpha: true,
      });
      renderer.setSize(ref.current.clientWidth, ref.current.clientHeight);
      ref.current.appendChild(renderer.domElement);

      const scene = new THREE.Scene();
      const camera = new THREE.PerspectiveCamera(
        45,
        ref.current!.clientWidth / ref.current!.clientHeight,
        0.1,
        1000
      );

      const params = {
        red: 1.0,
        green: 1.0,
        blue: 1.0,
        threshold: 1,
        strength: 1,
        radius: 0,
      };

      const renderScene = new RenderPass(scene, camera);
      renderScene.clear = false;

      const bloomPass = new UnrealBloomPass(
        new THREE.Vector2(window.innerWidth, window.innerHeight),
        params.strength,
        params.radius,
        params.threshold
      );

      var parameters = {
        minFilter: THREE.LinearFilter,
        magFilter: THREE.LinearFilter,
        format: THREE.RGBAFormat,
        stencilBuffer: false,
      };

      var renderTarget = new THREE.WebGLRenderTarget(
        ref.current.clientWidth,
        ref.current.clientHeight,
        parameters
      );

      const bloomComposer = new EffectComposer(renderer, renderTarget);
      bloomComposer.addPass(renderScene);
      bloomComposer.addPass(bloomPass);

      const outputPass = new OutputPass();
      bloomComposer.addPass(outputPass);

      camera.position.set(0, -2, 14);
      camera.lookAt(0, 0, 0);

      const uniforms = {
        u_time: { type: "f", value: 0.0 },
        u_frequency: { type: "f", value: 0.0 },
        u_red: { type: "f", value: 0.0 },
        u_green: { type: "f", value: 0.0 },
        u_blue: { type: "f", value: 0.0 },
      };

      const mat = new THREE.ShaderMaterial({
        uniforms,
        vertexShader: document.getElementById("vertexshader")!.textContent!,
        fragmentShader: document.getElementById("fragmentshader")!.textContent!,
      });

      const geo = new THREE.IcosahedronGeometry(4, 30);
      const mesh = new THREE.Mesh(geo, mat);
      scene.add(mesh);
      mesh.material.wireframe = true;

      const listener = new THREE.AudioListener();
      let analyser: THREE.AudioAnalyser | null = null;

      if (singleton.threeAudio === null) {
        const sound = new THREE.Audio(listener);

        if (singleton.audioElementRef?.current)
          sound.setMediaElementSource(singleton.audioElementRef.current);
        analyser = new THREE.AudioAnalyser(sound, 32);
        singleton.threeAudio = sound;
      } else {
        analyser = new THREE.AudioAnalyser(singleton.threeAudio, 32);
      }

      const clock = new THREE.Clock();
      function animate() {
        uniforms.u_time.value = clock.getElapsedTime();
        uniforms.u_frequency.value = analyser!.getAverageFrequency();
        renderer.render(scene, camera);
        requestAnimationFrame(animate);
      }
      animate();
      window.addEventListener("resize", function () {
        camera.aspect = ref.current!.clientWidth / ref.current!.clientHeight;
        camera.updateProjectionMatrix();
        renderer.setSize(ref.current!.clientWidth, ref.current!.clientHeight);
        bloomComposer.setSize(
          ref.current!.clientWidth,
          ref.current!.clientHeight
        );
      });
    }
  }, [ref]);

  return (
    <>
      <div
        id="3d"
        style={{
          position: "absolute",
          top: "0",
          left: "0",
          zIndex: "1",
          width: "100%",
          height: "100%",
        }}
        ref={ref}
      ></div>
    </>
  );
};

export default ThreeDAnalyzer;
