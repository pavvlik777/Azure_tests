<template>
  <header class="app-header">
    <div class="items left">
      <router-link :to="mainPageRoute"> Azure Time App </router-link>
    </div>
  </header>
</template>

<script>
import { routerHelper } from "@/utils";

export default {
  name: "AppHeader",
  computed: {
    mainPageRoute() {
      return { name: routerHelper.TimeZonesPage };
    },
  },
};
</script>

<style lang="scss" src="./AppHeader.scss" />
