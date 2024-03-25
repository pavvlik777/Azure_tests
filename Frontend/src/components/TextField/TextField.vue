<template>
  <input
    v-bind="options"
    :type="type"
    :placeholder="placeholder"
    :id="id"
    :value="modelValue"
    :class="{ invalid: this.invalid }"
    class="text-input"
    @input="onInput"
    @blur="onBlur"
  />
</template>

<script>
export default {
  name: "TextField",
  inheritAttrs: false,
  props: {
    type: {
      type: String,
      default: "text",
    },
    placeholder: {
      type: [String],
      default: "",
    },
    id: {
      type: String,
      default: "",
    },
    modelValue: {
      type: [String, Number],
      default: "",
    },
    invalid: {
      type: Boolean,
      default: false,
    },
    options: {
      type: Object,
      default: () => {},
    },
  },
  methods: {
    onInput(event) {
      this.$emit("update:modelValue", event.target.value);
    },
    onBlur(event) {
      this.$emit("blur", event);
    },
  },
};
</script>

<style lang="scss" src="./TextField.scss"></style>
