<template>
    <Spinner :isEnabled="this.isLoading">
        <Title title="Time zones" />
        <div class="time-zones-page">
            <div class="time-zones-diff">
                {{ this.currentDiff }}
            </div>
            <div class="time-zones-container">
                <TimeZone v-for="timeZone in this.timeZones" :key="timeZone.zoneId" :time-zone="timeZone"
                    @time-zone:update-image="this.onTimeZoneImageUpdate" @time-zone:delete="this.onTimeZoneDelete"
                    @time-zone:calculate-diff="this.onCalculateDiff" />
            </div>
            <AppButton @buttonClick="onAddNewTimeZoneClick">
                Add new time zone
            </AppButton>
        </div>
    </Spinner>
</template>

<script>
import TimeZone from "./TimeZone";

import { routerHelper } from "@/utils";
import { timeZonesService } from "@/dependencies";

import { AppButton } from "@/components/AppButton";
import { Spinner } from "@/components/Spinner";
import { Title } from "@/components/Title";

export default {
    name: "TimeZonesPage",
    components: {
        AppButton,
        Spinner,
        Title,

        TimeZone
    },
    data() {
        return {
            timeZones: [],
            isLoading: true,
            currentDiff: "N/A",
        };
    },
    async created() {
        await this.initialLoadAsync();
    },
    methods: {
        async initialLoadAsync() {
            this.isLoading = true;
            this.timeZones = await this.loadTimeZonesAsync();
            this.isLoading = false;
        },
        async loadTimeZonesAsync() {
            const response = await timeZonesService.getTimeZonesAsync();

            return response.data;
        },
        async onTimeZoneImageUpdate(zoneId, timeZone) {
            const timeZoneToUpdate = this.timeZones.find(z => z.zoneId === zoneId);
            if (timeZoneToUpdate) {
                timeZoneToUpdate.displayName = timeZone.displayName;
                timeZoneToUpdate.utcOffsetMinutes = timeZone.utcOffsetMinutes;
                timeZoneToUpdate.imageId = timeZone.imageId;
            }
        },
        async onTimeZoneDelete(zoneId) {
            await timeZonesService.deleteTimezoneAsync(zoneId);
            const timeZoneToRemove = this.timeZones.find(z => z.zoneId === zoneId);
            const index = this.timeZones.indexOf(timeZoneToRemove);
            if (index > -1) {
                this.timeZones.splice(index, 1);
            }
        },
        onCalculateDiff(diff) {
            this.currentDiff = diff / 60 + ' hours';
        },
        onAddNewTimeZoneClick() {
            this.$router
                .push({
                    name: routerHelper.CreateTimeZonePage,
                })
                .catch(() => { });
        }
    },
}
</script>

<style lang="scss">
.time-zones-page {
    display: flex;
    flex-direction: column;

    &>.time-zones-container {
        display: grid;
        grid-template-columns: 1fr 1fr 1fr;
    }
}
</style>