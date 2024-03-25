import { createRouter, createWebHistory } from "vue-router";
import { routerHelper } from "@/utils";

const MainLayout = () => import("@/layouts/MainLayout");

const TimeZonesPage = () => import("@/views/TimeZones/TimeZonesPage");
const CreateTimeZonePage = () => import("@/views/TimeZones/CreateTimeZonePage");
const EditTimeZonePage = () => import("@/views/TimeZones/EditTimeZonePage");

const Error404 = () => import("@/views/Errors/Error404");

const routes = [
  {
    path: "/",
    component: MainLayout,
    redirect: { name: routerHelper.TimeZonesPage },
    children: [
      {
        path: "time-zones",
        component: TimeZonesPage,
        name: routerHelper.TimeZonesPage,
      },
      {
        path: "time-zones/create",
        component: CreateTimeZonePage,
        name: routerHelper.CreateTimeZonePage,
      },
      {
        path: "time-zones/:zoneId/edit",
        component: EditTimeZonePage,
        name: routerHelper.EditTimeZonePage,
      },

      {
        path: "404",
        component: Error404,
        name: routerHelper.Error404,
      },
    ],
  },
  {
    path: "/:pathMatch(.*)*",
    redirect: { name: routerHelper.Error404 },
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
