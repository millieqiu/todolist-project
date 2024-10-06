// # Style
import "../css/login.scss";

// # Vue
import { createApp, ref } from "vue";

// TODO: 把 axios 拆出去
import axios from "./common/api";

const app = createApp({
  setup() {
    const tab = ref(0);

    const loginData = ref({
      username: "",
      password: ""
    })

    const registerData = ref({
      nickname: "",
      username: "",
      password: "",
      confirmPassword: ""
    })

    async function login() {
      const params = {
        username: loginData.value.username,
        password: loginData.value.password
      }
      await axios
      .post("/Login", params)
      .then((res) => {
        console.log(res);
        window.location.replace("/Index");
      })
      .catch((err) => {
        console.log(err);
      });
    }

    async function register() {
      const params = {
        nickname: registerData.value.nickname,
        username: registerData.value.username,
        password: registerData.value.password,
        confirmPassword: registerData.value.confirmPassword
      }
      await axios
      .post("/Register", params)
      .then((res) => {
        console.log(res);
        window.location.replace("/Index");
      })
      .catch((err) => {
        console.log(err);
      });
    }

    return {
      tab,
      loginData,
      registerData,
      login,
      register,
    };
  },
});

app.mount("#app");
