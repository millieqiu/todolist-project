// # Style
import "../css/label.scss";

// # Vue
import { createApp, ref } from "vue";

// # Custom Components
import BaseSidebar from "./components/BaseSidebar.vue";
import BasePageTitle from "./components/BasePageTitle.vue";

const app = createApp({
  components: {
    BaseSidebar,
    BasePageTitle,
  },
  setup() {

    const msg = ref("TEST!");

    return {
      msg
    };
  },
});

app.mount("#app");
