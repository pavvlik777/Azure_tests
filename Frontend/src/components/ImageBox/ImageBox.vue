<template>
  <img :class="this.htmlClassesInternal" :src="this.imgInternal" :alt="this.imageAlt"
    :width="this.imgSizeInternal.width" :height="this.imgSizeInternal.height" />
</template>

<script>
export default {
  name: "ImageBox",
  props: {
    image: {
      type: Object,
      required: true,
    },
    imageAlt: {
      type: String,
      default: "",
    },
    htmlClasses: {
      type: Object,
      default: () => { },
    },
  },
  computed: {
    htmlClassesInternal() {
      return {
        imagebox: true,
        ...this.htmlClasses,
      };
    },
    imgInternal() {
      const image = this.imageInternal && this.imageInternal.image;

      return image || this.image.defaultImage;
    },
    imgSizeInternal() {
      if (!this.imageInternal) {
        return {
          width: 90,
          height: 90,
        };
      }

      const widthFit = this.imageInternal.sizeX <= this.imageInternal.sizeY;

      return {
        width: widthFit ? 90 : null,
        height: !widthFit ? 90 : null,
      };
    },
  },
  data() {
    return {
      imageInternal: null,
      isLoading: false,
    };
  },
  watch: {
    image: async function () {
      await this.setImageAsync();
    },
  },
  async created() {
    await this.setImageAsync();
  },
  methods: {
    async setImageAsync() {
      const imageUrl = this.image.imageId
        ? this.getImageUrl(this.image.imageId)
        : this.image.defaultImage;
      this.isLoading = !!this.image.imageId;

      try {
        const response = await fetch(imageUrl);
        if (response.status === 200) {
          const imageBlob = await response.blob();
          const imageObjectUrl = URL.createObjectURL(imageBlob);

          const setPortraitData = this.setPortraitData;

          const image = new Image();
          image.onload = function () {
            const width = this.width;
            const height = this.height;
            setPortraitData(imageUrl, imageBlob, width, height);
          };
          image.src = imageObjectUrl;
        }
      } catch {
        //
      }
    },
    setPortraitData(imageContent, data, width, height) {
      this.imageInternal = {
        image: imageContent,
        data: data,
        sizeX: width,
        sizeY: height,
      };
      this.isLoading = false;
    },
    getImageUrl(imageId) {
      return `https://azuretestdev9e8a.blob.core.windows.net/images/${imageId}`;
    }
  },
};
</script>
