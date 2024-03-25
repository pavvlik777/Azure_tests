<template>
  <ul v-if="messages.length" class="form__messagelist">
    <li v-for="(message, index) in messages"
      :key="index"
      :class="[ 'form__messagelist__item', message.error ? 'error' : 'success' ]">
      {{ message.msg }}
    </li>
  </ul>
</template>

<script>
export default {
  name: 'AppFormMessageList',

  props: {
    messages: {
      type: Array,
      default: () => ([]),
    }
  },
}
</script>
