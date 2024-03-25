<template>
  <AppHeader />
  <div class="content-container">
    <main>
      <div class="main-content-container">
        <router-view />
      </div>
    </main>
  </div>
</template>

<script>
import { AppHeader } from "@/components/AppHeader";

export default {
  name: "MainLayout",
  components: {
    AppHeader,
  },
};
</script>

<style lang="scss">
.content-container {
  margin-top: 64px;
  display: flex;
  flex-direction: column;

  & main > .main-content-container {
    margin: 0 auto;
    max-width: 1200px;
    padding: 15px;
  }
}
</style>
