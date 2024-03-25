<template>
    <div class="time-zone">
        <div class="time-zone__content">
            <div class="time-zone__content__display-name">
                {{ this.displayName }}
            </div>
            <div class="time-zone__content__current-time">
                {{ this.currentTime }}
            </div>
            <div class="time-zone__content__time-zone-diff">
                <AppButton @buttonClick="onTimeZoneDiffClick" size="small">
                    Diff
                </AppButton>
            </div>
            <div class="time-zone__content__update">
                <AppButton @buttonClick="onUpdateClick" size="small">
                    Update
                </AppButton>
            </div>
            <div class="time-zone__content__delete">
                <AppButton @buttonClick="onDeleteClick" size="small">
                    Delete
                </AppButton>
            </div>
            <div class="time-zone__content__image">
                <ImageBox :image="this.timeZoneImage" />
                <AppDropbox @dropbox:load="this.onLoadPicture($event)" :formats="['image/*']">
                </AppDropbox>
            </div>
        </div>
    </div>
</template>

<script>
import default_image from "@/assets/default_image.png";

import { routerHelper } from "@/utils";
import { timeZonesService } from "@/dependencies";

import { ImageBox, imageBoxImageCreator } from "@/components/ImageBox";
import { AppButton } from "@/components/AppButton";
import { AppDropbox } from "@/components/AppDropbox";

export default {
    name: "TimeZone",
    components: {
        ImageBox,
        AppButton,
        AppDropbox,
    },
    props: {
        timeZone: {
            type: Object,
            required: true,
        },
    },
    data() {
        return {
            currentTimeInterval: null,
            currentTime: null,
        }
    },
    beforeUnmount() {
        clearInterval(this.currentTimeInterval)
    },
    created() {
        this.default_image = default_image;
        this.currentTimeInterval = setInterval(() => {
            const date = new Date();
            const utc = date.getTime() + (date.getTimezoneOffset() * 60000);
            const utcOffsetMinutes = this.timeZone.utcOffsetMinutes;
            const current = new Date(utc + utcOffsetMinutes * 60000);

            this.currentTime = current.toLocaleString();
        }, 300)
    },
    computed: {
        displayName() {
            return this.timeZone.displayName;
        },
        timeZoneImage() {
            console.log('update');

            return this.createPortraitImage(
                this.timeZone.imageId,
                this.default_image
            );
        }
    },
    methods: {
        onUpdateClick() {
            this.$router
                .push({
                    name: routerHelper.EditTimeZonePage,
                    params: { zoneId: this.timeZone.zoneId },
                })
                .catch(() => { });
        },
        onDeleteClick() {
            this.$emit("time-zone:delete", this.timeZone.zoneId);
        },
        async onTimeZoneDiffClick() {
            const response = await timeZonesService.calculateTimeZonesDiffAsync(this.timeZone.zoneId, 'minsk');
            const result = response.data;

            this.$emit("time-zone:calculate-diff", result);
        },
        async onLoadPicture({ data }) {
            if (data) {
                const response = await timeZonesService.updateImageAsync(this.timeZone.zoneId, data);
                const result = response.data;

                this.$emit("time-zone:update-image", this.timeZone.zoneId, result);
            }
        },
        createPortraitImage: imageBoxImageCreator.createFrom,
    }
}
</script>

<style lang="scss" scoped>
.time-zone {
    background-color: $secondary-color;
    border-radius: 5px;
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    margin: 10px 10px 10px 10px;
    min-width: 400px;
    min-height: 140px;
    overflow: hidden;
    position: relative;

    &__content {
        display: grid;
        align-items: center;
        justify-items: center;
        grid-template-rows: 1fr 1fr 1fr;
        grid-template-columns: 1fr 2fr 1fr;
        grid-template-areas:
            "display-name time-zone-diff time-zone-image"
            "current-time current-time time-zone-image"
            "update delete  time-zone-image";
        height: 100%;
        padding: 10px;

        &__display-name {
            grid-area: display-name;
        }

        &__current-time {
            grid-area: current-time;
        }

        &__time-zone-diff {
            grid-area: time-zone-diff;
        }

        &__update {
            grid-area: update;
        }

        &__delete {
            grid-area: delete;
        }

        &__image {
            grid-area: time-zone-image;
        }
    }
}
</style>