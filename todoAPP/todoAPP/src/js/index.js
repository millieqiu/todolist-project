// # Style
import "../css/index.scss";

// # Vue
import { createApp, onMounted, ref, provide, computed, reactive } from "vue";

// # Custom Components
import BaseHeader from "./components/BaseHeader.vue";
import BaseSidebar from "./components/BaseSidebar.vue";
import BasePageTitle from "./components/BasePageTitle.vue";
import ModalEditTodoItem from "./components/ModalEditTodoItem.vue";
import ModalShowTodoNote from "./components/ModalShowTodoNote.vue";

import axios from "axios";

import { formatDateTime } from "./common/format";

const app = createApp({
  components: {
    BaseHeader,
    BaseSidebar,
    BasePageTitle,
    ModalEditTodoItem,
    ModalShowTodoNote,
  },
  setup() {
    const todoList = ref([]);

    const isHideDoneTodo = ref(false);

    const todoItemObj = reactive({
      title: "",
      description: "",
      executeAt: null,
    });

    const modalEditTodoItem = ref(null);
    function openModalEditTodoItem(item) {
      modalEditTodoItem.value.openModal(item);
    }

    const modalShowTodoNote = ref(null);
    function openModalShowTodoNote(val) {
      modalShowTodoNote.value.openModal(val);
    }

    function toggleHideDoneTodo() {
      isHideDoneTodo.value = isHideDoneTodo.value ? false : true;
    }

    async function getTodoList() {
      await axios
        .get("/api/Todo/List")
        .then((res) => {
          console.log(res);
          todoList.value = res.data;
        })
        .catch((err) => {
          console.log(err);
        });
    }

    async function changeTodoStatus(id) {
      await axios
        .patch(`/api/Todo/${id}/Status`)
        .then((res) => {
          console.log(res);
          getTodoList();
        })
        .catch((err) => {
          console.log(err);
        });
    }

    async function deleteTodoItem(id) {
      console.log("D");
      await axios
        .delete(`/api/Todo/${id}`)
        .then(function (response) {
          console.log(response);
          getTodoList();
        })
        .catch(function (error) {
          console.log(error);
        });
    }

    async function deleteAllDoneTodoItem() {
      await axios
        .delete("/api/Todo/Done")
        .then(function (response) {
          console.log(response);
          getTodoList();
        })
        .catch(function (error) {
          console.log(error);
        });
    }

    // NOTE: 對 <li> 標籤加 @click.prevent，且 <a> 標籤加 href="/" 就可以，但不曉得為什麼，待研究
    function copyTodoItem(item) {
      let params = {
        title: item.title,
        description: item.description,
        executeAt: item.executeAt,
      };
      axios
        .post("/api/Todo", params)
        .then(function (response) {
          console.log(response);
          getTodoList();
        })
        .catch(function (error) {
          console.log(error);
        });
    }

    // ** provide 重整頁面的 function 到孫組件
    provide("update", getTodoList);

    onMounted(async () => {
      await getTodoList();
    });

    return {
      modalEditTodoItem,
      openModalEditTodoItem,
      modalShowTodoNote,
      openModalShowTodoNote,
      todoList,
      formatDateTime,
      changeTodoStatus,
      deleteTodoItem,
      deleteAllDoneTodoItem,
      isHideDoneTodo,
      toggleHideDoneTodo,
      copyTodoItem
    };
  },
});

app.mount("#app");
