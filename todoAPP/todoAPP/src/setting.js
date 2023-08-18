import Vue from 'vue';
import $ from "jquery";
import './css/style.scss';

const setting = new Vue({
    el: '#setting',
    data() {
        return {
            url: null,
            file: {},
            percent: 0,
        }
    },
    mounted() {
        this.getAvatar();
    },
    methods: {
        //取得大頭照
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

        //更新大頭照
        onFileChange(e) {
            var self = this;

            this.file = e.target.files[0];
            let reader = new FileReader();
            reader.readAsDataURL(self.file);

            //進度條
            reader.addEventListener("progress", (event) => {
                this.percent = parseInt(Math.round((event.loaded / event.total) * 100));
            });

            reader.addEventListener("load", (event) => {
                self.url = event.target.result;
                console.log(self.url);
            });
        },

        //嚙踟換大嚙磐嚙踝蕭
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
    },
})