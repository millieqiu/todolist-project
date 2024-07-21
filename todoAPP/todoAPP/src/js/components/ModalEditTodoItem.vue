<template>
  <dialog ref="modalEl" class="modal">
    <div class="modal-box">
      <h3 class="font-bold text-lg">{{ isEdit ? '編輯' : '新增' }}待辦事項</h3>
      <div class="modal-body">
        <label class="input input-bordered flex items-center gap-2">
          <input type="text" class="grow" placeholder="待辦事項名稱..." v-model="todoForm.title" />
        </label>
        <label class="input input-bordered flex items-center gap-2">
          <base-date-picker class="grow" v-model="todoForm.executeAt" placeholder="時間及日期"></base-date-picker>
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
            stroke="currentColor" class="w-4 h-4 opacity-70 flex-none">
            <path stroke-linecap="round" stroke-linejoin="round"
              d="M6.75 3v2.25M17.25 3v2.25M3 18.75V7.5a2.25 2.25 0 0 1 2.25-2.25h13.5A2.25 2.25 0 0 1 21 7.5v11.25m-18 0A2.25 2.25 0 0 0 5.25 21h13.5A2.25 2.25 0 0 0 21 18.75m-18 0v-7.5A2.25 2.25 0 0 1 5.25 9h13.5A2.25 2.25 0 0 1 21 11.25v7.5" />
          </svg>
        </label>
        <select class="select select-bordered w-full">
          <option disabled selected>待辦事項標籤</option>
          <option>Normal Apple</option>
          <option>Normal Orange</option>
          <option>Normal Tomato</option>
        </select>
        <textarea class="textarea textarea-bordered" rows="5" placeholder="備註及描述"
          v-model="todoForm.description"></textarea>
      </div>
      <div class="modal-action">
        <button class="btn" @click="closeModal">關閉</button>
        <button class="btn btn-primary" v-if="isEdit" @click="editTodoItem(todoForm.uid)">儲存變更</button>
        <button class="btn btn-primary" v-else @click="createTodoItem">添加任務</button>
      </div>
    </div>
  </dialog>
</template>

<script>
import { onMounted, reactive, ref, inject } from 'vue';

import { formatDateTime } from "../common/format"

import BaseDatePicker from './BaseDatePicker.vue';

import axios from "axios";

export default {
  components: {
    BaseDatePicker
  },
  setup(props, { emit }) {

    const isEdit = ref(false);

    const defaultTodoForm = {
      title: "",
      executeAt: null,
      description: "",
      isComplete: false
    };
    const todoForm = reactive({ ...defaultTodoForm });

    const updateList = inject("update");

    const modalEl = ref(null);

    function openModal(item) {
      isEdit.value = item ? true : false;
      Object.assign(todoForm, item);
      modalEl.value.showModal();
    }

    function closeModal() {
      Object.assign(todoForm, defaultTodoForm);
      isEdit.value = false;
      modalEl.value.close();
    }

    function createTodoItem() {
      let params = {
        title: todoForm.title,
        description: todoForm.description,
        executeAt: todoForm.executeAt
      }
      axios.post('/api/Todo', params)
        .then(function (response) {
          console.log(response);
          updateList();
        })
        .catch(function (error) {
          console.log(error);
        });
        closeModal();
    }

    function editTodoItem(id) {
      let params = {
        title: todoForm.title,
        description: todoForm.description,
        executeAt: todoForm.executeAt
      }
      axios.patch(`/api/Todo/${id}/Info`, params)
        .then(function (response) {
          console.log(response);
          updateList();
        })
        .catch(function (error) {
          console.log(error);
        });
        closeModal();
    }

    return {
      modalEl,
      openModal,
      closeModal,

      isEdit,
      todoForm,
      formatDateTime,

      createTodoItem,
      editTodoItem,
    }
  }
}
</script>

<style lang="scss" scoped>
.modal-box {
  overflow-y: visible;
}
</style>