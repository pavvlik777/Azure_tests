<template>
  <div class="app-dropbox">
    <input
      ref="fileInput"
      :name="uploadFileName"
      :disabled="isLoading"
      :accept="supportedFormats"
      type="file"
      class="app-dropbox__input-file"
      @change="fileChanged"
    />

    <slot />
    <p class="app-dropbox__message" v-if="showMessage">
      <template v-if="!isFileAvailable" path="to_upload_file" tag="span">
        <span class="app-dropbox__link">Click here</span>
      </template>
      <template v-else>
        {{ textValue }}
      </template>
    </p>
    <button
      v-if="isFileAvailable && showRemoveButton"
      class="app-dropbox__remove-button"
      @click.stop="onClickRemove"
    >
      X
    </button>
  </div>
</template>

<script>
const loadingStatuses = {
  Initial: 0,
  Loading: 1,
  Success: 2,
  Failure: 3,
};
const defaultValueModel = { fileName: "", data: null };

const isEmptyValueModel = (model) => !(model.fileName || model.data);

export default {
  name: "AppDropbox",
  props: {
    value: {
      type: Object,
      default: () => ({ ...defaultValueModel }),
    },
    formats: {
      type: Array,
    },
    showMessage: {
      type: Boolean,
      default: false,
    },
    showRemoveButton: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      currentStatus: loadingStatuses.Initial,
      uploadFileName: "",
    };
  },
  emits: ['interface', 'dropbox:load', 'dropbox:clear'],
  computed: {
    supportedFormats() {
      return this.formats && this.formats.length ? this.formats.join(",") : "";
    },
    isFileAvailable() {
      return !!(this.uploadFileName && this.uploadFileName.length);
    },
    isInitial() {
      return this.currentStatus === loadingStatuses.Initial;
    },
    isLoading() {
      return this.currentStatus === loadingStatuses.Loading;
    },
    isSuccess() {
      return this.currentStatus === loadingStatuses.Success;
    },
    isFailure() {
      return this.currentStatus === loadingStatuses.Failure;
    },
    textValue() {
      return this.isLoading
        ? "Uploading this.uploadFileName"
        : this.uploadFileName;
    },
  },
  watch: {
    value: {
      handler: function (newValue) {
        if (newValue) {
          this.uploadFileName = newValue.fileName;
        }

        if (this.$refs.fileInput && isEmptyValueModel(newValue)) {
          this.$refs.fileInput.value = ""; // Vue doesn't support v-model for files so it is alternative way to reset input value
          this.currentStatus = loadingStatuses.Initial;
        }
      },
      immediate: true,
    },
  },
  mounted() {
    this.emitInterface();
  },
  methods: {
    emitInterface() {
      this.$emit("interface", {
        clearValue: () => this.clearValue(),
      });
    },
    onClickRemove() {
      this.clearValue();
    },
    onFileLoad() {
      this.currentStatus = loadingStatuses.Success;
      this.$emit("dropbox:load", {
        fileName: this.uploadFileName,
        data: this.$refs.fileInput.files[0],
      });
    },
    fileChanged(e) {
      if (!e.target.files.length) {
        this.currentStatus = loadingStatuses.Initial;
        this.clearValue();

        return;
      }

      const formData = e.target.files[0];
      this.uploadFileName = formData.name;
      this.currentStatus = loadingStatuses.Loading;
      const reader = new FileReader();
      reader.onload = this.onFileLoad;
      reader.readAsDataURL(formData);
    },
    clearValue() {
      this.$refs.fileInput.value = "";
      this.uploadFileName = "";
      this.$emit("dropbox:clear", { ...defaultValueModel });
    },
  },
};
</script>

<style lang="scss" src="./AppDropbox.scss"></style>
