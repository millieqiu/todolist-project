import Vue from 'vue';
import $ from "jquery";
import './css/style.scss';

const app2 = new Vue({
    el: '#app2',
    data() {
        //Todo: �n��V����Ʒ|��bdata�̭� (���n��refs)
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
            let userInfo = JSON.stringify({
                username: this.registerEmail,
                password: this.registerPassword,
                confirmPassword: this.confirmPassword,
                nickname: this.registerName,
            });
            $.ajax({
                url: "/api/User/Register",
                method: "post",
                contentType: "application/json; charset=utf-8",
                data: userInfo,
                success: function (res) {
                    alert("���U���\�I");
                    window.location.assign("/Index");
                },
                error: function (req, status) {
                    if (req.responseJSON.service && req.responseJSON.service == "Register" && req.responseJSON.status == 1) {
                        alert("�b���w�s�b");
                    }
                    else if (req.responseJSON.errors.Username && req.responseJSON.errors.Username.includes("The Username field is not a valid e-mail address.")) {
                        alert("Email�榡���~");
                    }
                    else if (req.responseJSON.errors.ConfirmPassword) {
                        alert("�K�X�P�T�{�K�X���ŦX");
                    }
                    else {
                        alert("�L�k���U�b���A���p���t�κ޲z��");
                    }
                }
            })
        },
    }
})