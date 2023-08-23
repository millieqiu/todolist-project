import Vue from 'vue';
import $ from "jquery";
import './css/style.scss';

const header = new Vue({
    el: '#header',
    methods: {
        //Logout
        onClickLogout() {

            fetch('/api/Logout', {
                method: 'POST',
            })
                .then(res => {
                    window.location.href = "/Index";
                })


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
    }
})