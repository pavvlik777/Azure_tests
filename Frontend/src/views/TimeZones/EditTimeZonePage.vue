<template>
    <Spinner :isEnabled="this.isLoading">
        <AppForm :header-text="'Edit timezone'" :footer-text="'Save'" :is-model-invalid="this.isInvalid"
            :error-text="this.errorText" @form:click="onEditClick">
            <AppFormParam :error-text="displayNameErrorText">
                <template v-slot:label>
                    <label for="displayName"> Display Name </label>
                </template>
                <template v-slot:input>
                    <AppInput id="displayName" v-model="this.model.displayName"
                        :invalid="displayNameErrorText.length > 0" />
                </template>
            </AppFormParam>
            <AppFormParam :error-text="utcOffsetMinutesErrorText">
                <template v-slot:label>
                    <label for="utcOffsetMinutes"> UTC Offset Minutes </label>
                </template>
                <template v-slot:input>
                    <AppInput id="utcOffsetMinutes" type="number" v-model="this.model.utcOffsetMinutes"
                        :invalid="utcOffsetMinutesErrorText.length > 0" />
                </template>
            </AppFormParam>
        </AppForm>
    </Spinner>
</template>

<script>
import { routerHelper } from "@/utils"
import { timeZonesService } from "@/dependencies";

import { AppInput } from "@/components/AppInput"
import { AppForm, AppFormParam } from "@/components/AppForm"
import { Spinner } from "@/components/Spinner";

export default {
    name: "EditTimeZonePage",
    components: {
        AppInput,
        AppForm,
        AppFormParam,
        Spinner,
    },
    data() {
        return {
            model: {
                displayName: "",
                utcOffsetMinutes: null,
            },
            isLoading: true,
            errorText: "",
        }
    },
    computed: {
        zoneId() {
            return this.$route.params.zoneId
        },
        displayNameErrorText() {
            if (this.model.displayName == null || this.model.displayName == "") {
                return "Field is required";
            }

            return "";
        },
        utcOffsetMinutesErrorText() {
            if (this.model.utcOffsetMinutes == null || this.model.utcOffsetMinutes == "") {
                return "Field is required";
            }

            return "";
        },
        isInvalid() {
            return this.displayNameErrorText !== "" || this.utcOffsetMinutesErrorText !== "";
        }
    },
    async created() {
        await this.initialLoadAsync();
    },
    methods: {
        async initialLoadAsync() {
            this.isLoading = true;
            this.timeZones = await this.loadTimeZoneAsync();
            this.isLoading = false;
        },
        async loadTimeZoneAsync() {
            const response = await timeZonesService.getTimeZoneByIdAsync(this.$route.params.zoneId);
            this.model.displayName = response.data.displayName;
            this.model.utcOffsetMinutes = response.data.utcOffsetMinutes;
        },
        async onEditClick() {
            try {
                await timeZonesService.updateTimezoneAsync(this.$route.params.zoneId, this.model.displayName, this.model.utcOffsetMinutes);
                this.$router
                    .push({
                        name: routerHelper.TimeZonesPage,
                    })
                    .catch(() => { });
            } catch (r) {
                if (r.status === 400) {
                    if (r.data.errorText) {
                        this.errorText = r.data.errorText
                    } else {
                        this.errorText = "Service error"
                    }
                }
            }
        },
        onInput(callback) {
            this.errorText = ""
            callback()
        },
    }
}
</script>

<style lang="scss">
.edit-time-zones-page {
    display: flex;
    flex-direction: column;
}
</style>