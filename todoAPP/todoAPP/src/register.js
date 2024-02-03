import Vue from "vue";
import $ from "jquery";

const app2 = new Vue({
  el: "#app2",
  data() {
    return {
      //Register info
      registerEmail: "",
      registerPassword: "",
      confirmPassword: "",
      registerName: "",
      msg: [],
    };
  },
  computed: {
    validateEmail() {
      return /^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z]+$/.test(
        this.registerEmail
      ) || this.registerEmail == ""
        ? ""
        : "請輸入正確 Email 格式";
    },
    validatePassword() {
      if (
        /[A-Za-z0-9]/.test(this.registerPassword) &&
        this.registerPassword.length >= 8
      ) {
        return "";
      } else if (this.registerPassword == "") {
        return "";
      } else {
        return "請使用英數且需達八字以上";
      }
    },
    validateConfirmPassword() {
      return this.registerPassword == this.confirmPassword ? "" : "密碼不一致";
    },
  },
  methods: {
    onClickRegister() {
      let userInfo = {
        username: this.registerEmail,
        password: this.registerPassword,
        confirmPassword: this.confirmPassword,
        nickname: this.registerName,
      };

      fetch("/Register", {
        method: "POST",
        headers: {
          "Content-Type": "application/json; charset=utf-8",
        },
        body: JSON.stringify(userInfo),
      })
        .then((response) => {
          if (response.ok) {
            alert("註冊成功！");
            if (response.redirected) {
              window.location.href = response.url;
            }
          }
          return Promise.reject(response);
        })
        .catch((response, status) => {
          response.json().then((json) => {
            if (json.service == "Register" && json.status == 1) {
              alert("帳號已存在");
            } else {
              alert("無法註冊帳號，請聯絡系統管理員");
            }
          });
        });
    },
  },
});
