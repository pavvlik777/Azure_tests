<template>
  <div class="form__param">
    <div class="form__param__row">
      <div class="form__param__row label">
        <slot name="label" />
      </div>
      <div class="form__param__row input">
        <slot name="input" />
      </div>
    </div>
    <span v-if="errorText.length" class="form__error-text">
      {{ errorText }}
    </span>
  </div>
</template>

<script>
export default {
  name: "AppFormParam",
  props: {
    errorText: {
      type: String,
      default: '',
    },
  },
}
</script>

<style lang="scss" src="./AppForm.scss"></style>
