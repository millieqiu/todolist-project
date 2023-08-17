import Vue from 'vue';
import $ from "jquery";
import './css/style.scss';

const setting = new Vue({
    el: '#setting',
    data() {
        return {
            message: 'Testtttttt', //測試用資料
            url: null,
            file: {},
        }
    },
    mounted() {
        this.getAvatar();
    },
    methods: {
        //讀取大頭照
        getAvatar() {
            //fetch('/api/User/Avatar', {
            //    method: 'get',
            //})
            //    .then(res => {
            //        this.file = res.avatar;
            //        this.url = URL.createObjectURL(this.file);
            //        console.log(res);
            //    })
            this.url = '/api/User/Avatar'
        },

        //預覽大頭照
        onFileChange(e) {
            this.file = e.target.files[0];
            this.url = URL.createObjectURL(this.file);
            console.log(this.file);
            console.log(this.url);
        },

        //更換大頭照
        onClickUploadImage() {
            let avatar = this.file;
            var formData = new FormData();
            formData.append('avatar', avatar); // 設定上傳的檔案

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
    },
})