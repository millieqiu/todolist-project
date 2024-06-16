<template>
  <dialog ref="modalEl" class="modal">
    <div class="modal-box w-11/12 max-w-5xl p-0">
      <h3 class="font-bold text-lg p-4">設定</h3>
      <form method="dialog">
        <button class="btn btn-sm btn-circle btn-ghost absolute right-2 top-2">✕</button>
      </form>
      <div class="modal-body border-t-2 border-base-200">
        <ul class="menu bg-base-200 w-56">
          <li><a>個人資料</a></li>
        </ul>
        <div class="p-2">
          <div class="flex flex-col gap-2">
            <div>
              <h4 class="font-medium text-sm p-1">電子信箱</h4>
              <span class="text-sm">aaabbb@gmail.com</span>
            </div>
            <div>
              <h4 class="font-medium text-sm p-1">密碼</h4>
              <span class="text-sm">aaabbb@gmail.com</span>
            </div>
            <div>
              <h4 class="font-medium text-sm p-1">密碼</h4>
              <span class="text-sm">aaabbb@gmail.com</span>
            </div>
          </div>
        </div>
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

    function openModal() {
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

<style scoped>
.modal-body {
  min-height: 500px;
  display: flex;
  flex-direction: row;
  padding: 0;
}
</style>