function onClickLoginOrRegister(event) {
    event.preventDefault();
    const loginEmail = document.getElementById('loginEmail').value;
    const loginPassword = document.getElementById('loginPassword').value;
    login(loginEmail, loginPassword);
}

function onClickLogout() {
    logout();
}

function onClickRegister() {
    const registerEmail = document.getElementById('registerEmail').value;
    const registerName = document.getElementById('registerName').value;
    const registerPassword = document.getElementById('registerPassword').value;
    const registerConfirmPassword = document.getElementById('confirm-password').value;
    register(registerEmail, registerPassword, registerConfirmPassword, registerName);
}

function login(username, password) {
    $.ajax({
        url: "/api/User/Login",
        method: "post",
        data: {
            Username: username,
            Password: password,
        },
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
    $.ajax({
        url: "/api/User/Register",
        method: "post",
        data: {
            Username: username,
            Password: password,
            ConfirmPassword: confirmPassword,
            Nickname: nickname,
        }, 
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
