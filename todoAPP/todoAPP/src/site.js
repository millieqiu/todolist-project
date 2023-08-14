//嘗試過的解法
//import Vue from 'vue';
//import Vue from 'vue/dist/vue.js'

// 建立 Vue
const app = new Vue({
    el: '#app',
    data() {
        //Todo: 要渲染的資料會放在data裡面 (不要用refs)
        return {
            //Login info
            loginEmail: '',
            loginPassword: '',

            message: 'Hello World', //測試用資料

        }
    },

    //執行Vue實體後先載入getTodoList()函式讀取資料
    //在登陸頁會有無法讀取api的錯誤訊息
    //拆成兩支js
    //mounted() {
    //由於是設定頁數被點擊後才能觸發getTodoList函式，登入後尚未點擊頁數前須給定預設值為1
        //this.getTodoList(1); //todo: 改將預設值寫在函式裡
    //},

    methods: {
        //Login
        onClickLogin() {
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
                    window.location.href = "/TodoPage";
                    //self.getTodoList(1); //在logIn呼叫的函式不會被帶到下一頁
                }
                return Promise.reject(response);
            }).catch((response, status) => {
                response.json().then((json) => {
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
 
    },
});



//todoList.js