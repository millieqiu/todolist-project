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
        <div class="p-2 flex-1" v-if="!isChangingPassword && !isChangingAvatar">
          <div class="flex flex-col gap-4">
            <div>
              <h4 class="font-medium text-sm p-1">頭像</h4>
              <div class="flex items-center">
                <div class="avatar">
                  <div class="w-24 rounded-full">
                    <img :src="userInfo.userAvatar || defaultUserAvatar" alt="User Avatar" />
                  </div>
                </div>
                <button class="btn ml-3" @click="isChangingAvatar = true">變更頭像</button>
              </div>
            </div>
            <div>
              <h4 class="font-medium text-sm p-1">電子信箱</h4>
              <span class="text-sm">{{ userInfo.account }}</span>
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
              <input type="text" placeholder="Type here" class="input input-bordered input-md w-full max-w-xs" v-model="passwordForm.oldPassword" />
            </div>
            <div>
              <h4 class="font-medium text-sm p-1">新密碼</h4>
              <input type="text" placeholder="Type here" class="input input-bordered input-md w-full max-w-xs" v-model="passwordForm.newPassword" />
            </div>
            <div>
              <h4 class="font-medium text-sm p-1">確認新密碼</h4>
              <input type="text" placeholder="Type here" class="input input-bordered input-md w-full max-w-xs" v-model="passwordForm.confirmNewPassword" />
            </div>
            <div class="mt-4">
              <button class="btn mr-3" @click="reset">返回</button>
              <button class="btn btn-primary" @click="updatePassword">確認變更</button>
            </div>
          </div>
        </div>
        <div class="p-2 flex-1" v-if="isChangingAvatar">
          <div class="flex flex-col gap-4">
            <div>
              <h4 class="font-medium text-sm p-1">檔案</h4>
              <input type="file" class="file-input file-input-bordered w-full max-w-xs" @change="onChangeFile" />
            </div>
            <div class="mt-4">
              <button class="btn mr-3" @click="reset">返回</button>
              <button class="btn btn-primary" @click="updateUserAvatar">確認變更</button>
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

import axios from "axios";

export default {
  setup(props, { emit }) {

    const isChangingPassword = ref(false);
    const isChangingAvatar = ref(false);

    const userInfo = ref({
      account: "",
      userAvatar: null
    })
    const defaultUserAvatar = ref("../dist/images/defaultAvatar.jpg");

    const fileInput = ref(null);

    const defaultPasswordForm = {
      oldPassword: "",
      newPassword: "",
      confirmNewPassword: ""
    };
    const passwordForm = reactive({...defaultPasswordForm});

    const modalEl = ref(null);

    async function openModal() {
      modalEl.value.showModal();
      await getUserAvatar();
      await getUserBasicInfo();
    }

    function closeModal() {
      modalEl.value.close();
    }

    async function getUserAvatar() {
      userInfo.value.userAvatar = "/api/User/Avatar";
    }

    async function getUserBasicInfo() {
      await axios
        .get("/api/User/Info")
        .then((res) => {
          userInfo.value.account = res.data.username;
        })
        .catch((err) => {
          console.log(err);
        });
    }

    function onChangeFile(event) {
      fileInput.value = event.target.files[0];
    }

    function updateUserAvatar() {
      const form = new FormData();
      form.append('AvatarFile', fileInput.value);

      axios
        .patch("/api/User/Avatar", form)
        .then((res) => {
          reset();
        })
        .catch((err) => {
          console.log(err);
        });
    }

    function updatePassword() {
      let params = {
        oldPassword: passwordForm.oldPassword,
        newPassword: passwordForm.newPassword,
        confirmNewPassword: passwordForm.confirmNewPassword
      }
      axios
        .patch("/api/User/Password", params)
        .then(function (response) {
          reset();
        })
        .catch(function (error) {
          console.log(error);
        });
    }

    async function reset() {
      isChangingAvatar.value = false;
      isChangingPassword.value = false;
      fileInput.value = "";
      Object.assign(passwordForm, defaultPasswordForm);
      await getUserAvatar();
      await getUserBasicInfo();
    }

    return {
      modalEl,
      openModal,
      closeModal,
      isChangingPassword,
      isChangingAvatar,
      reset,

      // 大頭照相關
      defaultUserAvatar,
      userInfo,
      onChangeFile,
      updateUserAvatar,

      // 密碼相關
      passwordForm,
      updatePassword
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
