// 建立 Vue
new Vue({
    el: '#app',
    data: {
        message: 'Hello World',

    },
    methods: {
        onClickLogin() {
            //const loginEmail = document.getElementById('loginEmail').value;
            //const loginPassword = document.getElementById('loginPassword').value;
            login(this.$refs.loginEmail.value, this.$refs.loginPassword.value);
        },
        onClickLogout() {
            logout();
        },
        onClickRegister() {
            register(this.$refs.registerEmail.value, this.$refs.registerPassword.value, this.$refs.confirmPassword.value, this.$refs.registerName.value);
        },
        onClickCreateTodoItem() {
            createTodoItem(this.$refs.todoText.value);
        },
    },
});



// site.js

//function onClickLogin(event) {
    // event.preventDefault();
    //const loginEmail = document.getElementById('loginEmail').value;
    //const loginPassword = document.getElementById('loginPassword').value;
    //login(loginEmail, loginPassword);
//}

//function onClickLogout() {
    //logout();
//}

//function onClickRegister(event) {
    //event.preventDefault();
    //const registerEmail = document.getElementById('registerEmail').value;
    //const registerName = document.getElementById('registerName').value;
    //const registerPassword = document.getElementById('registerPassword').value;
    //const registerConfirmPassword = document.getElementById('confirm-password').value;
    //register(registerEmail, registerPassword, registerConfirmPassword, registerName);
//}

function login(username, password) {
    let userInfo = JSON.stringify({
        "username": username,
        "password": password,
    });

    $.ajax({
        url: "/api/User/Login",
        method: "post",
        contentType: "application/json; charset=utf-8",
        data: userInfo,
        success: function (res) {
            window.location.assign("/TodoPage");
        },
        error: function (req, status) {
            if (req.responseJSON.service == "Login" && req.responseJSON.status == 1) {
                alert("此帳號不存在");
            }
            else if (req.responseJSON.service == "Login" && req.responseJSON.status == 2) {
                alert("帳號或密碼錯誤");
            }
            else {
                alert("無法登入，請聯絡系統管理員");
            }
        }
    })
}

function logout() {
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
}

function register(username, password, confirmPassword, nickname) {
    let userInfo = JSON.stringify({
        "username": username,
        "password": password,
        "confirmPassword": confirmPassword,
        "nickname": nickname,
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

}

//todoList.js

window.onload = function () {
    getTodoList();
}

function onClickCreateTodoItem(event) {
    event.preventDefault();
    const todoText = document.getElementById('todoText').value;
    createTodoItem(todoText);
}

function onClickDeleteTodoItem(event, todoID) {
    event.preventDefault();

    let todoItem = JSON.stringify({
        "id": todoID,
    });

    $.ajax({
        url: "/api/TodoList/Delete?id=" + todoID,
        type: "DELETE",
        contentType: "application/json; charset=utf-8",
        data: todoItem,
        success: function (res) {
            setTodoList(res.pagination);
        }
    })
}

function getTodoList(page) {
    //有可能是null或其他
    if (!page) {
        page = 1;
    }
    $.ajax({
        url: "/api/TodoList/ListPagination?page=" + page,
        method: "get",
        success: function (res) {
            setTodoList(res);
        }
    })
}

function createTodoItem(text) {
    let todoItem = JSON.stringify({
        "Text": text,
    });

    $.ajax({
        url: "/api/TodoList/Create",
        method: "post",
        contentType: "application/json; charset=utf-8",
        data: todoItem,
        success: function (res) {
            setTodoList(res.pagination);
        }
    })
}

function onChangeStatus(todoID) {
    let todoItem = JSON.stringify({
        "id": todoID,
    });

    $.ajax({
        url: "/api/TodoList/ChangeStatus",
        method: "put",
        contentType: "application/json; charset=utf-8",
        data: todoItem,
        success: function (res) {
            setTodoList(res.pagination);
        }
    })
}

function setTodoList(res) {
    $('#todolist').html("");
    $('#pagination').html("");
    for (let i = 1; i <= res.numOfPages; i++) {
        $('#pagination').append(`<li onClick="getTodoList(${i})">${i}</li>`)
    }
    $.each(res.list, function (key, value) {
        let status = "";
        if (value.status == 1) {
            status = "checked";
        }
        $('#todolist').append(`<div class="task">
                                            <label class="task-content">
                                                <input type="checkbox" name="" id="checkbox_item_${value.id}" class="checkbox" onclick="onChangeStatus(${value.id})" ${status}>
                                                <span class="gray-01 paragraph1" for="">${value.text}</span>
                                            </label>
                                            <div class="icon" id="delete_item_${value.id}" onClick="onClickDeleteTodoItem(event,${value.id})">
                                                <i class="fa-solid fa-trash gray-04"></i>
                                            </div>
                                        </div>`).html();
    });
}