<template>
  <div class="flex items-center mb-3">
    <h1 class="text-xl font-semibold mr-auto">{{ title }}</h1>
    <slot></slot>
  </div>
</template>

<script>
// # Vue
import { ref } from 'vue';

export default {
  props: {
    title: {
      type: String,
      required: true
    }
  },
  setup(props) {

    return {
      // 內容
    }
  }
}
</script>