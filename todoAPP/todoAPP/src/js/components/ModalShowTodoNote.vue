<template>
  <dialog ref="modalEl" class="modal">
    <div class="modal-box">
      <h3 class="font-bold text-lg">查看備註</h3>
      <p class="py-4">{{ note }}</p>
      <div class="modal-action">
        <form method="dialog">
          <button class="btn">關閉</button>
        </form>
      </div>
    </div>
  </dialog>
</template>

<script>
// # Vue
import { onMounted, reactive, ref } from 'vue';

export default {
  setup(props, { emit }) {

    const note = ref("");

    const modalEl = ref(null);

    function openModal(val) {
      note.value = val;
      modalEl.value.showModal();
    }

    function closeModal() {
      note.value = "";
      modalEl.value.close();
    }

    return {
      modalEl,
      openModal,
      closeModal,
      note
    }
  }
}
</script>