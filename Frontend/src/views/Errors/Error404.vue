<template>
  <Title title="Page Not Found" />
  <section class="error-page">
    <p class="error-page__large-text">Sorry, the page not found.</p>
    <p class="error-page__text">
      The link you followed probably broken or the page has been removed.
    </p>
  </section>
</template>

<script>
import { Title } from "@/components/Title";

export default {
  name: "Error404",
  components: {
    Title,
  },
};
</script>

<style lang="scss">
.error-page {
  display: flex;
  flex-direction: column;
  align-items: center;
  margin: auto;

  &__large-text {
    font-size: 1.5em;
    font-weight: $font-weight-bold;
  }

  &__text {
    font-weight: $font-weight-medium;
  }
}
</style>