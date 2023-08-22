import Vue from 'vue';
import $ from "jquery";
import './css/style.scss';

const app2 = new Vue({
    el: '#app2',
    data() {
        return {
            //Register info
            registerEmail: '',
            registerPassword: '',
            confirmPassword: '',
            registerName: '',

        }
    },
    methods: {
        onClickRegister() {
            let userInfo = {
                username: this.registerEmail,
                password: this.registerPassword,
                confirmPassword: this.confirmPassword,
                nickname: this.registerName,
            };

            fetch('/api/User/Register', {
                method: 'POST',
                headers: {
                    'Content-Type': "application/json; charset=utf-8",
                },
                body: JSON.stringify(userInfo)
            })
                .then(res => {
                    alert('註冊成功AAA！');
                    window.location.href = "/Index";
                })


            //$.ajax({
            //    url: "/api/User/Register",
            //    method: "post",
            //    contentType: "application/json; charset=utf-8",
            //    data: userInfo,
            //    success: function (res) {
            //        alert("註冊成功！");
            //        window.location.assign("/Index");
            //    },
            //    error: function (req, status) {
            //        if (req.responseJSON.service && req.responseJSON.service == "Register" && req.responseJSON.status == 1) {
            //            alert("帳號已存在");
            //        }
            //        else if (req.responseJSON.errors.Username && req.responseJSON.errors.Username.includes("The Username field is not a valid e-mail address.")) {
            //            alert("Email格式錯誤");
            //        }
            //        else if (req.responseJSON.errors.ConfirmPassword) {
            //            alert("密碼與確認密碼不符合");
            //        }
            //        else {
            //            alert("無法註冊帳號，請聯絡系統管理員");
            //        }
            //    }
            //})
        },
    }
})