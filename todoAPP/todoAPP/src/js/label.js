// # Style
import "../css/label.scss";

// # Vue
import { createApp, ref, onMounted } from "vue";

// # Custom Components
import BaseHeader from "./components/BaseHeader.vue";
import BaseSidebar from "./components/BaseSidebar.vue";
import BasePageTitle from "./components/BasePageTitle.vue";

// # Enums & Functions
import axios from "axios";

const app = createApp({
  components: {
    BaseHeader,
    BaseSidebar,
    BasePageTitle,
  },
  setup() {

    const isEdit = ref(false);

    const labelList = ref([]);

    async function getLabelList() {
      await axios
        .get("/api/UserTag/List")
        .then((res) => {
          console.log(res);
          labelList.value = res.data.filter((i) => i.type);
        })
        .catch((err) => {
          console.log(err);
        });
    }

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

    onMounted(async () => {
      await getLabelList();
    });

    return {
      labelList,
      isEdit,
      editTags,
      updateTags,
      cancelEdit
    };
  },
});

app.mount("#app");
