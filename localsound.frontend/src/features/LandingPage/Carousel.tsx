import { useState } from "react";
import banner1 from "../../assets/landing-page-banner/banner1.jpg";
import banner2 from "../../assets/landing-page-banner/banner2.jpg";
import banner3 from "../../assets/landing-page-banner/banner3.jpg";
import banner4 from "../../assets/landing-page-banner/banner4.jpg";
import banner5 from "../../assets/landing-page-banner/banner5.jpg";
import banner6 from "../../assets/landing-page-banner/banner6.jpg";
import banner7 from "../../assets/landing-page-banner/banner7.jpg";

const Carousel = () => {
  const images = [
    banner1,
    banner2,
    banner3,
    banner4,
    banner5,
    banner6,
    banner7,
  ];

  setTimeout(() => {
    if (imageIndex < images.length - 1) {
      setSelectedImage(images[imageIndex + 1]);
      setImageIndex(imageIndex + 1);
    } else {
      setSelectedImage(images[0]);
      setImageIndex(0);
    }
  }, 5000);

  const [imageIndex, setImageIndex] = useState(0);
  const [selectedImage, setSelectedImage] = useState(images[0]);

  return <img className="carousel" src={selectedImage} />;
};

export default Carousel;
