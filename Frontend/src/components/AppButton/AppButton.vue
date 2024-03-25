<template>
  <button :class="buttonClasses" :disabled="isDisabled" @click="onButtonClick">
    <slot />
  </button>
</template>

<script>
export default {
  name: "AppButton",
  props: {
    disabled: {
      type: Boolean,
      default: false,
    },
    color: {
      type: String,
      default: "primary",
    },
    size: {
      type: String,
      default: "normal",
    },
    text: {
      type: Boolean,
      default: false,
    },
    htmlClasses: {
      type: Object,
      default: () => {},
    },
  },
  computed: {
    isDisabled() {
      return this.disabled;
    },
    buttonClasses() {
      const classes = {
        button: true,
        enabled: !this.disabled,
        "button--text": this.text,
        "button--with-bg": !this.text,
        ...this.htmlClasses,
      };

      if (this.color) {
        classes[`button--${this.color}`] = true;
      }

      if (this.size) {
        classes[`button--${this.size}`] = true;
      }

      return classes;
    },
  },
  methods: {
    onButtonClick() {
      if (!this.disabled) {
        this.$emit("buttonClick");
      }
    },
  },
};
</script>

<style lang="scss" src="./AppButton.scss"></style>
