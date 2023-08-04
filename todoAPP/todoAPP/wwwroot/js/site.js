function onClickLogin() {
    const loginEmail = document.getElementById('loginEmail').value;
    const loginPassword = document.getElementById('loginPassword').value;
    login(loginEmail, loginPassword);
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
        }
    })
}