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
            window.location.replace("/TodoPage");
        },
        error: function (req, status) {
            if (req.responseJSON.service == "Login" && req.responseJSON.status == 1) {
                window.location.replace("/Register");
            }
        }
    })
}

function logout() {
    $.ajax({
        url: "/api/User/Logout",
        method: "post",
        success: function (res) {
            window.location.replace("/Index");
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
            window.location.replace("/Index");
        },
        error: function (req, status) {
            if (req.responseJSON.service == "Register" && req.responseJSON.status == 2) {
                alert("密碼與確認密碼不符合")
            }
        }
    })

}
