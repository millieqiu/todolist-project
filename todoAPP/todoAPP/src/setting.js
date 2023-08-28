import Vue from 'vue';
import $ from "jquery";
import './css/setting.scss';

const setting = new Vue({
    el: '#setting',
    data() {
        return {
            url: null,
            file: {},
            percent: 0,
            email: '',

            isNicknameModal: false,
            isEmailModal: false,
            userInfo: {
                id: 0,
                nickname: "",
                username: "",
            },
        }
    },
    mounted() {
        this.getAvatar();
        this.getUserInfo();
    },
    methods: {
        getAvatar() {
            this.url = '/api/User/Avatar'
        },

        onFileChange(e) {
            var self = this;

            this.file = e.target.files[0];
            let reader = new FileReader();
            reader.readAsDataURL(self.file);

            reader.addEventListener("progress", (event) => {
                this.percent = parseInt(Math.round((event.loaded / event.total) * 100));
                console.log([this.percent, event.lengthComputable, event.loaded, event.total]);
            });

            reader.addEventListener("load", (event) => {
                self.url = event.target.result;
                console.log(self.url);
            });
        },

        onClickUploadImage() {
            let avatar = this.file;
            var formData = new FormData();
            formData.append('avatar', avatar);

            fetch('/api/User/Avatar', {
                method: 'patch',
                body: formData,
            })
                .then(res => {
                    this.getAvatar();
                    alert('Upload Successful!')
                }
            )
        },

        showNicknameModal() {
            this.isNicknameModal = true;
        },
        hideNicknameModal() {
            this.isNicknameModal = false;
        },
        showEmailModal() {
            this.isEmailModal = true;
        },
        hideEmailModal() {
            this.isEmailModal = false;
        },
        getUserInfo() {
            let self = this;
            fetch('/api/User', {
                method: 'get',
            })
                .then(response => {
                    return response.json();
                })
                .then(json => {
                    self.userInfo = json;
                })
        },
        patchEmailAddress() {
            var self = this;

            let userInfo = {
                Username: this.email,
            };

            fetch('/api/User/Username', {
                method: 'patch',
                headers: {
                    'Content-Type': "application/json; charset=utf-8",
                },
                body: JSON.stringify(userInfo)
            })
                .then(success => {
                    if (success.ok) {
                        alert('Upload Successful!');
                        self.getUserInfo();
                        self.hideEmailModal();
                        return Promise.resolve(success);
                    }
                    return Promise.reject(success);
                })
                .catch(error => {
                    alert('Upload failed!');
                    //error.json().then(errorInfo => {
                    //    console.log(errorInfo);
                    //})
                })
        },
    },
})