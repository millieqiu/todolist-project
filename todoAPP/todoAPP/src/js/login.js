// # Style
import "../css/login.scss";

// # Vue
import { createApp, ref } from "vue";

const app = createApp({
  setup() {
    const tab = ref(0);
    return {
      tab,
    };
  },
});

app.mount("#app");
