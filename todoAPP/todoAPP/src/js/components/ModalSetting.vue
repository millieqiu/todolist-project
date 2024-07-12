<template>
  <dialog ref="modalEl" class="modal">
    <div class="modal-box w-11/12 max-w-5xl p-0">
      <h3 class="font-bold text-lg p-4">設定</h3>
      <form method="dialog">
        <button class="btn btn-sm btn-circle btn-ghost absolute right-2 top-2">
          ✕
        </button>
      </form>
      <div class="modal-body border-t-2 border-base-200">
        <ul class="menu bg-base-200 w-56">
          <li><a>個人資料</a></li>
        </ul>
        <div class="p-2 flex-1" v-if="!isChangingPassword">
          <div class="flex flex-col gap-4">
            <div>
              <h4 class="font-medium text-sm p-1">頭像</h4>
              <div class="flex items-center">
                <div class="avatar">
                  <div class="w-24 rounded-full">
                    <img src="https://img.daisyui.com/images/stock/photo-1534528741775-53994a69daeb.jpg" />
                  </div>
                </div>
                <button class="btn ml-3">變更頭像</button>
              </div>
            </div>
            <div>
              <h4 class="font-medium text-sm p-1">電子信箱</h4>
              <span class="text-sm">aaabbb@gmail.com</span>
            </div>
            <div>
              <h4 class="font-medium text-sm p-1">密碼</h4>
              <button class="btn" @click="isChangingPassword = true">變更密碼</button>
            </div>
          </div>
        </div>
        <div class="p-2 flex-1" v-if="isChangingPassword">
          <div class="flex flex-col gap-4">
            <div>
              <h4 class="font-medium text-sm p-1">舊密碼</h4>
              <input type="text" placeholder="Type here" class="input input-bordered input-md w-full max-w-xs" />
            </div>
            <div>
              <h4 class="font-medium text-sm p-1">新密碼</h4>
              <input type="text" placeholder="Type here" class="input input-bordered input-md w-full max-w-xs" />
            </div>
            <div>
              <h4 class="font-medium text-sm p-1">確認新密碼</h4>
              <input type="text" placeholder="Type here" class="input input-bordered input-md w-full max-w-xs" />
            </div>
            <div class="mt-4">
              <button class="btn mr-3">返回</button>
              <button class="btn btn-primary">確認變更</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </dialog>
</template>

<script>
// # Vue
import { onMounted, reactive, ref } from "vue";

export default {
  setup(props, { emit }) {

    const isChangingPassword = ref(false);

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
      isChangingPassword
    };
  },
};
</script>

<style scoped>
.modal-body {
  min-height: 500px;
  display: flex;
  flex-direction: row;
  padding: 0;
}
</style>
