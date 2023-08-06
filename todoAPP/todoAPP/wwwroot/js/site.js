function onClickLoginOrRegister(event) {
    event.preventDefault();
    const loginEmail = document.getElementById('loginEmail').value;
    const loginPassword = document.getElementById('loginPassword').value;
    login(loginEmail, loginPassword);
}

function onClickLogout() {
    logout();
}

function onClickRegister(event) {
    event.preventDefault();
    const registerEmail = document.getElementById('registerEmail').value;
    const registerName = document.getElementById('registerName').value;
    const registerPassword = document.getElementById('registerPassword').value;
    const registerConfirmPassword = document.getElementById('confirm-password').value;
    register(registerEmail, registerPassword, registerConfirmPassword, registerName);
}

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
                alert("帳號不存在，為您跳轉至註冊頁面進行註冊");
                window.location.assign("/Register");
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
