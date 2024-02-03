import Vue from "vue";
import $ from "jquery";
import "./css/header.scss";

const header = new Vue({
  el: "#header",
  methods: {
    //Logout
    onClickLogout() {
      fetch("/Logout", {
        method: "GET",
      }).then((res) => {
        if (res.redirected) {
          window.location.href = res.url;
        }
      });

      //$.ajax({
      //    url: "/api/User/Logout",
      //    method: "post",
      //    success: function (res) {
      //        window.location.assign("/Index");
      //    },
      //    error: function (req, status) {
      //        alert("登出失敗，請重新嘗試登出");
      //    }
      //})
    },
  },
});
