<template>
  <label class="app-toggle" :for="id">
    <input type="checkbox" :id="id" :checked="modelValue" @input="onInput" @change="onChange" />
    <div class="app-toggle__slider round" />
  </label>
</template>

<script>
export default {
  name: "AppToggle",
  props: {
    id: {
      type: [String],
      default: "checkbox"
    },
    modelValue: {
      type: [Boolean],
      default: false,
    },
  },
  emits: ['change'],
  methods: {
    onInput(event) {
      this.$emit("update:modelValue", event.target.checked);
    },
    onChange(event) {
      this.$emit("change", event)
    }
  },
}
</script>

<style lang="scss" src="./AppToggle.scss"></style>
