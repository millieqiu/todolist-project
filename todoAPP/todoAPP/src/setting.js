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

            isNicknameModal: false,
        }
    },
    mounted() {
        this.getAvatar();
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
    },
})