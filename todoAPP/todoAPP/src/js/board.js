// # Style
import "../css/board.scss";

// # Vue
import { createApp, ref } from "vue";

// # Custom Components
import BaseSidebar from "./components/BaseSidebar.vue";
import BasePageTitle from "./components/BasePageTitle.vue";
import ModalEditTodoItem from "./components/ModalEditTodoItem.vue";
import ModalShowTodoNote from "./components/ModalShowTodoNote.vue";

const app = createApp({
  components: {
    BaseSidebar,
    BasePageTitle,
    ModalEditTodoItem,
    ModalShowTodoNote,
  },
  setup() {

    const fakeTodoList = ref([
      {
        title: "THIS IS A TODO!", // 限制 50 字
        time: "2024-05-31 16:17",
        note: "THIS IS A TODO!THIS IS A TODO!THIS IS A TODO!THIS IS A TODO!THIS IS A TODO!", // 限制 250 字
        isComplete: false,
      },
      {
        title: "記得幫小花澆水記得幫小花澆水記得幫小花澆水記得幫小花澆水", // 限制 50 字
        time: "2024-05-31 16:17",
        note: "THIS IS A TODO!THIS IS A TODO!THIS IS A TODO!THIS IS A TODO!THIS IS A TODO!", // 限制 250 字
        isComplete: false,
      },
      {
        title: "買衛生紙", // 限制 50 字
        time: "2024-05-31 16:17",
        note: "", // 限制 250 字
        isComplete: true,
      },
    ])

    const modalEditTodoItem = ref(null);
    function openModalEditTodoItem(item) {
      modalEditTodoItem.value.openModal(item);
    }

    const modalShowTodoNote = ref(null);
    function openModalShowTodoNote(val) {
      modalShowTodoNote.value.openModal(val);
    }

    return {
      fakeTodoList,
      modalEditTodoItem,
      openModalEditTodoItem,
      modalShowTodoNote,
      openModalShowTodoNote
    };
  },
});

app.mount("#app");
