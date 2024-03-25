<template>
    <AppForm :header-text="'Create timezone'" :footer-text="'Save'" :is-model-invalid="this.isInvalid"
        :error-text="this.errorText" @form:click="onCreateClick">
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
        <AppFormParam :error-text="''">
            <template v-slot:label>
                <label for="ttl"> TTL </label>
            </template>
            <template v-slot:input>
                <AppToggle id="ttl" v-model="this.model.ttl" :invalid="false" />
            </template>
        </AppFormParam>
    </AppForm>
</template>

<script>
import { routerHelper } from "@/utils"
import { timeZonesService } from "@/dependencies";

import { AppInput } from "@/components/AppInput"
import { AppForm, AppFormParam } from "@/components/AppForm"
import { AppToggle } from "@/components/AppToggle"

export default {
    name: "CreateTimeZonePage",
    components: {
        AppInput,
        AppForm,
        AppFormParam,
        AppToggle,
    },
    data() {
        return {
            model: {
                displayName: "",
                utcOffsetMinutes: null,
                ttl: false,
            },
            errorText: "",
        }
    },
    computed: {
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
    methods: {
        async onCreateClick() {
            try {
                await timeZonesService.createTimezoneAsync(this.model.displayName, this.model.utcOffsetMinutes, this.model.ttl);
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