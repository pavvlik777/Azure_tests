<template>
  <div class="form">
    <h1 class="form__header">{{ headerText }}</h1>
    <fieldset>
      <slot />
      <span v-if="errorText.length" class="form__error-text">
        {{ errorText }}
      </span>
    </fieldset>
    <div class="form__footer">
      <AppButton :disabled="isModelInvalid" @click="onSubmitClick">
        {{ footerText }}
      </AppButton>
    </div>
  </div>
</template>

<script>
import { AppButton } from "@/components/AppButton"

export default {
  name: "AppForm",
  components: {
    AppButton,
  },
  props: {
    headerText: {
      type: String,
      required: true,
    },
    footerText: {
      type: String,
      required: true,
    },
    errorText: {
      type: String,
      default: '',
    },
    isModelInvalid: {
      type: Boolean,
      required: true,
    }
  },
  methods: {
    onSubmitClick() {
      if (!this.disabled) {
        this.$emit("form:click")
      }
    },
  },
}
</script>

<style lang="scss" src="./AppForm.scss"></style>
