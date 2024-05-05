<template>
  <VueDatePicker v-model="date" cancel-text="取消" select-text="選擇" :month-change-on-scroll="false" disable-year-select format="yyyy/MM/dd HH:mm"></VueDatePicker>
</template>

<script>
import { ref } from 'vue';

// > Vue Datepicker 套件：https://vue3datepicker.com/
import VueDatePicker from '@vuepic/vue-datepicker';
import '@vuepic/vue-datepicker/dist/main.css';

export default {
  components: {
    VueDatePicker
  },
  setup(props, { emit }) {
    const date = ref(null);
    return {
      date
    }
  }
}
</script>

<style lang="scss">
  .dp__input {
    border: 0;
  }

  .dp__input_icons {
    display: none;
  }

  .dp__input_icon_pad {
    padding: 0;
  }
</style>