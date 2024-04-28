<template>
  <dialog ref="modalEl" class="modal">
    <div class="modal-box">
      <h3 class="font-bold text-lg">Hello!</h3>
      <p class="py-4">Press ESC key or click the button below to close</p>
      <div class="modal-action">
        <form method="dialog">
          <!-- if there is a button in form, it will close the modal -->
          <button class="btn">Close</button>
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

    const modalEl = ref(null);

    function openModal(item) {
      modalEl.value.showModal();
    }

    function closeModal() {
      modalEl.value.close();
    }

    return {
      modalEl,
      openModal,
      closeModal,
    }
  }
}
</script>