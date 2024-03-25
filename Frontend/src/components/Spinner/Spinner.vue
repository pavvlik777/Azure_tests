<template>
  <div :class="this.spinnerClassesInternal" v-if="this.isEnabled">
    <div class="spinner-sector spinner-sector-red" />
    <div class="spinner-sector spinner-sector-blue" />
    <div class="spinner-sector spinner-sector-green" />
  </div>
  <template v-else>
    <slot />
  </template>
</template>

<script>
export default {
  name: "Spinner",
  props: {
    isEnabled: {
      type: Boolean,
      required: true,
    },
    spinnerClasses: {
      type: Object,
      default: () => {},
    },
  },
  computed: {
    spinnerClassesInternal() {
      return {
        "spinner": true,
        ...this.spinnerClasses,
      };
    },
  },
};
</script>

<style lang="scss" src="./Spinner.scss" />
