// 建立 Vue
const app = new Vue({
    el: '#app',
    data() {
        //Todo: 要渲染的資料會放在data裡面 (不要用refs)
        return {
            //Login info
            loginEmail: '',
            loginPassword: '',

            //Register info
            registerEmail: '',
            registerPassword: '',
            confirmPassword: '',
            registerName: '',

            todoText: '',

            message: 'Hello World', //測試用資料

            todos: [], //傳遞API讀取的資料
            numOfPages: 0, //資料型態為數字，給定預設值0 (因為沒有任何items前不應該有頁碼)
            currentPage: 0,

        }
    },

    //執行Vue實體後先載入getTodoList()函式讀取資料
    //在登陸頁會有無法讀取api的錯誤訊息
    //拆成兩支js
    mounted() {
    //由於是設定頁數被點擊後才能觸發getTodoList函式，登入後尚未點擊頁數前須給定預設值為1
        this.getTodoList(1); //todo: 改將預設值寫在函式裡
    },

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
        //Logout
        onClickLogout() {
            $.ajax({
                url: "/api/User/Logout",
                method: "post",
                success: function (res) {
                    window.location.assign("/Index");
                },
                error: function (req, status) {
                    alert("登出失敗，請重新嘗試登出");
                }
            })
        },
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
                    alert("註冊成功！");
                    window.location.assign("/Index");
                },
                error: function (req, status) {
                    if (req.responseJSON.service && req.responseJSON.service == "Register" && req.responseJSON.status == 1) {
                        alert("帳號已存在");
                    }
                    else if (req.responseJSON.errors.Username && req.responseJSON.errors.Username.includes("The Username field is not a valid e-mail address.")) {
                        alert("Email格式錯誤");
                    }
                    else if (req.responseJSON.errors.ConfirmPassword) {
                        alert("密碼與確認密碼不符合");
                    }
                    else {
                        alert("無法註冊帳號，請聯絡系統管理員");
                    }
                }
            })
        },
        getTodoList(page) {
            var self = this; //在這邊給定範圍內的this都是self

            $.ajax({
                url: "/api/TodoList/ListPagination?page=" + page,
                method: "get",
                contentType: "application/json; charset=utf-8",

                success: function (res) {
                    //在這裡寫this的話會指向ajax
                    self.numOfPages = res.numOfPages;
                    self.currentPage = res.currentPage;
                    self.todos = res.list;
                }
            })

            
        },

        onClickCreateTodoItem() {
            var self = this;


            let todoItem = JSON.stringify({
                Text: this.todoText,
                page: this.currentPage,
            });

            $.ajax({
                url: "/api/TodoList/Create",
                method: "post",
                contentType: "application/json; charset=utf-8",
                data: todoItem,
                success: function (res) {
                    self.getTodoList(self.currentPage);
                }
            })
        },
        onClickDeleteTodoItem(id) {
            //todo: delete功能
            var self = this;

            let todoItem = JSON.stringify({
                id: id,
                page: this.currentPage,
            });

            $.ajax({
                url: "/api/TodoList/Delete",
                method: "delete",
                contentType: "application/json; charset=utf-8",
                data: todoItem,
                success: function (res) {
                    self.getTodoList(self.currentPage);
                }
            })
        },
        onChangeStatus(id) {
            //todo: check功能
            let todoItem = JSON.stringify({
                id: id,
                page: this.currentPage,
            });

            $.ajax({
                url: "/api/TodoList/ChangeStatus",
                method: "put",
                contentType: "application/json; charset=utf-8",
                data: todoItem,
                success: function (res) {
                    getTodoList(page);
                }
            })
        },
    },
});



//todoList.js