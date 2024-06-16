// # Style
import "../css/label.scss";

// # Vue
import { createApp, ref } from "vue";

// # Custom Components
import BaseSidebar from "./components/BaseSidebar.vue";
import BasePageTitle from "./components/BasePageTitle.vue";

// # Enums & Functions
import { tagColorsEnum } from "./common/enum";

const app = createApp({
  components: {
    BaseSidebar,
    BasePageTitle,
  },
  setup() {

    const isEdit = ref(false);

    const fakeLabelList = ref([
      {
        name: "工作",
        color: "fill-rose-400"
      },
      {
        name: "待辦事項",
        color: "fill-amber-400"
      },
      {
        name: "約會",
        color: "fill-amber-400"
      },
      {
        name: "紀念日",
        color: "fill-amber-400"
      },
      {
        name: "出去玩",
        color: "fill-amber-400"
      },
      {
        name: "還沒想好名稱",
        color: "fill-amber-400"
      },
      {
        name: "還沒想好名稱",
        color: "fill-amber-400"
      },
      {
        name: "超長的標籤名稱最多25個字超長的標籤名稱最多25個字超長的標籤名稱最多25個字",
        color: "fill-amber-400"
      },
    ])

    function editTags() {
      isEdit.value = true;
      // apis
    }

    function updateTags() {
      isEdit.value = false;
      // apis
    }

    function cancelEdit() {
      isEdit.value = false;
      // 資料復原
    }

    return {
      fakeLabelList,
      tagColorsEnum,
      isEdit,
      editTags,
      updateTags,
      cancelEdit
    };
  },
});

app.mount("#app");
