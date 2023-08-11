const header = new Vue({
    el: '#header',
    methods: {
        //Logout
        onClickLogout() {
            $.ajax({
                url: "/api/User/Logout",
                method: "post",
                success: function (res) {
                    window.location.assign("/Index");
                },
                error: function (req, status) {
                    alert("�n�X���ѡA�Э��s���յn�X");
                }
            })
        },
    }
})