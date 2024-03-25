<template>
  <slot />
</template>

<script>
export default {
  name: "Title",
  props: ["title"],
  watch: {
    title: {
      immediate: true,
      handler() {
        const titleFormatter = (value) =>
          `${Array.isArray(value) ? value.join("") : value} - Equestria`;
        document.title = titleFormatter(this.title);
      },
    },
  },
}
</script>
