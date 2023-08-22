﻿//嘗試過的解法
import Vue from 'vue';
import './css/style.scss';
import './image/Google.svg';

// 建立 Vue
const app = new Vue({
    el: '#app',
    data() {
        //Todo: 要渲染的資料會放在data裡面 (不要用refs)
        return {
            //Login info
            loginEmail: '',
            loginPassword: '',

            isLoading: false,
        }
    },

    methods: {
        //Login
        onClickLogin() {
            this.isLoading =  true;
            var self = this;

            //refs - 用來控制Component(子項目)
            let userInfo = {
                username: this.loginEmail,
                password: this.loginPassword,
            };

            //改用fetch
            fetch('/api/User/Login', {
                method: "post",
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(userInfo),
            }).then((response) => {
                if (response.ok) {
                    console.log(response.json()); 
                    this.isLoading = false;
                    window.location.href = "/TodoPage";
                    //self.getTodoList(1); //在logIn呼叫的函式不會被帶到下一頁
                }
                return Promise.reject(response);
            }).catch((response, status) => {
                response.json().then((json) => {
                    this.isLoading = false;
                    if (json.service == "Login" && json.status == 1) {
                        alert("此帳號不存在");
                    }
                    else if (json.service == "Login" && json.status == 2) {
                        alert("帳號或密碼錯誤");
                    }
                    else {
                        alert("無法登入，請聯絡系統管理員");
                    }
                })
                
            });

        },
        onClickLoginGoogle() {
            fetch('/api/OAuth/RedirectToServiceProvider', {
                method: "get",
            }).then((response) => {
                response.text().then(function (text) {
                    const popup = window.open(text, "popup", "popup=true");
                    const checkPopup = setInterval(() => {
                        if (!popup || !popup.closed) return;
                        clearInterval(checkPopup);
                        window.location.href = "/TodoPage";
                    }, 100);
                });
            }).catch((response, status) => {

            });
        },
 
    },
});