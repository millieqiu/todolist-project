// # Style
import "../css/index.scss";

// # Vue
import { createApp, ref } from "vue";

// # Custom Components
import BaseSidebar from "./components/BaseSidebar.vue"

const app = createApp({
  components: {
    BaseSidebar
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
        note: "THIS IS A TODO!THIS IS A TODO!THIS IS A TODO!THIS IS A TODO!THIS IS A TODO!", // 限制 250 字
        isComplete: true,
      },
    ])

    return {
      fakeTodoList,
    };
  },
});

app.mount("#app");
