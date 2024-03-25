<template>
  <input v-bind="options" :type="type" :placeholder="placeholder" :value="modelValue" :class="appInputClass"
    class="app-input" @input="onInput">
</template>

<script>
export default {
  name: 'AppInput',
  props: {
    type: {
      type: String,
      default: 'text'
    },
    placeholder: {
      type: [String],
      default: ''
    },
    modelValue: {
      type: [String, Number],
      default: ''
    },
    invalid: {
      type: Boolean,
      default: false
    },
    options: {
      type: Object,
      default: () => { }
    }
  },
  computed: {
    appInputClass() {
      const classes = []

      if (this.invalid) {
        classes.push('app-input--invalid')
      }

      return classes.join` `
    }
  },
  methods: {
    onInput(event) {
      this.$emit("update:modelValue", event.target.value);
    },
  }
}
</script>

<style lang="scss" src="./AppInput.scss"></style>