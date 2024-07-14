// # Style
import "../css/index.scss";

// # Vue
import { createApp, onMounted, ref } from "vue";

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

    const modalEditTodoItem = ref(null);
    function openModalEditTodoItem(item) {
      modalEditTodoItem.value.openModal(item);
    }

    const modalShowTodoNote = ref(null);
    function openModalShowTodoNote(val) {
      modalShowTodoNote.value.openModal(val);
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
    };
  },
});

app.mount("#app");
