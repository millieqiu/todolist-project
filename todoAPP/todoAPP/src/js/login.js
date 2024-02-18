// # Style
import "../css/login.scss";

// # Vue
import { createApp, ref } from "vue";

const app = createApp({
  components: {},
  setup() {
    const msg = ref("YA!!");
    return {
      msg,
    };
  },
});

app.mount("#app");
