import Vue from "vue";
import "./image/Google.svg";
import "./css/style.scss";

// 建立 Vue
const app = new Vue({
  el: "#app",
  data() {
    //Todo: 要渲染的資料會放在data裡面 (不要用refs)
    return {
      //Login info
      loginEmail: "",
      loginPassword: "",

      isLoading: false,
    };
  },

  methods: {
    //Login
    onClickLogin() {
      this.isLoading = true;
      var self = this;

      //refs - 用來控制Component(子項目)
      let userInfo = {
        username: this.loginEmail,
        password: this.loginPassword,
      };

      //改用fetch
      fetch("/Login", {
        method: "post",
        headers: {
          "Content-Type": "application/json",
        },
        // redirect: 'follow',
        body: JSON.stringify(userInfo),
      })
        .then((response) => {
          if (response.ok) {
            this.isLoading = false;
            if (response.redirected) {
              window.location.href = response.url;
            }
          } else {
            return Promise.reject(response);
          }
        })
        .catch((response, status) => {
          this.isLoading = false;
          alert("帳號或密碼錯誤");
        });
    },
    onClickLoginGoogle() {
      fetch("/api/OAuth/ProviderEndpoint", {
        method: "get",
      })
        .then((response) => {
          response.text().then(function (text) {
            window.location.href = text;
          });
        })
        .catch((response, status) => {
          console.log(response);
        });
    },
  },
});
