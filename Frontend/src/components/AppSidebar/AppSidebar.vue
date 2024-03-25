<template>
  <aside class="app-sidebar">
    <div class="app-sidebar__menu">
      <router-link class="app-sidebar__menu__item" :to="alignmentsPageRoute"> Alignments </router-link>
      <router-link class="app-sidebar__menu__item" :to="racesPageRoute"> Races </router-link>
    </div>
  </aside>
</template>

<script>
import { routerHelper } from "@/utils";

export default {
  name: "AppSidebar",
  computed: {
    alignmentsPageRoute() {
      return { name: routerHelper.AlignmentsPage };
    },
    racesPageRoute() {
      return { name: routerHelper.RacesPage };
    },
  },
};
</script>

<style lang="scss" src="./AppSidebar.scss" />
