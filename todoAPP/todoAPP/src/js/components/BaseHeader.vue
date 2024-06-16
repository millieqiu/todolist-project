<template>
  <div class="navbar bg-base-100 border-b">
    <div class="flex-1">
      <a class="btn btn-ghost text-xl"><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"
          class="text-primary w-6 h-6">
          <path fill-rule="evenodd"
            d="M6.75 2.25A.75.75 0 0 1 7.5 3v1.5h9V3A.75.75 0 0 1 18 3v1.5h.75a3 3 0 0 1 3 3v11.25a3 3 0 0 1-3 3H5.25a3 3 0 0 1-3-3V7.5a3 3 0 0 1 3-3H6V3a.75.75 0 0 1 .75-.75Zm13.5 9a1.5 1.5 0 0 0-1.5-1.5H5.25a1.5 1.5 0 0 0-1.5 1.5v7.5a1.5 1.5 0 0 0 1.5 1.5h13.5a1.5 1.5 0 0 0 1.5-1.5v-7.5Z"
            clip-rule="evenodd" />
        </svg>
        EveryTodo</a>
    </div>
    <div class="flex-none gap-2">
      <button class="btn btn-ghost btn-circle">
        <div class="indicator">
          <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
              d="M15 17h5l-1.405-1.405A2.032 2.032 0 0118 14.158V11a6.002 6.002 0 00-4-5.659V5a2 2 0 10-4 0v.341C7.67 6.165 6 8.388 6 11v3.159c0 .538-.214 1.055-.595 1.436L4 17h5m6 0v1a3 3 0 11-6 0v-1m6 0H9" />
          </svg>
          <span class="badge badge-xs badge-accent indicator-item"></span>
        </div>
      </button>

      <div class="dropdown dropdown-end">
        <div tabindex="0" role="button" class="btn btn-ghost btn-circle avatar">
          <div class="w-10 rounded-full">
            <img alt="User Avatar"
              src="https://images.unsplash.com/photo-1607746882042-944635dfe10e?q=80&w=3540&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D" />
          </div>
        </div>
        <ul tabindex="0" class="mt-3 z-[1] p-2 shadow menu menu-sm dropdown-content bg-base-100 rounded-box w-52">
          <li><a @click="openModalSetting">設定</a></li>
          <li><a>登出</a></li>
        </ul>
      </div>
    </div>
  </div>
  <modal-setting ref="modalSetting"></modal-setting>
</template>

<script>
// # Vue
import { ref } from 'vue';

// # Custom Components
import ModalSetting from "./ModalSetting.vue"

export default {
  components: {
    ModalSetting
  },
  setup() {

    const modalSetting = ref(null);
    function openModalSetting() {
      modalSetting.value.openModal();
    }

    return {
      modalSetting,
      openModalSetting,
    }
  }
}
</script>