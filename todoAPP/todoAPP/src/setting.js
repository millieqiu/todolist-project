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
        }
    },
    mounted() {
        this.getAvatar();
    },
    methods: {
        //���o�j�Y��
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

        //��s�j�Y��
        onFileChange(e) {
            var self = this;

            this.file = e.target.files[0];
            let reader = new FileReader();
            reader.readAsDataURL(self.file);

            //�i�ױ�
            reader.addEventListener("progress", (event) => {
                this.percent = parseInt(Math.round((event.loaded / event.total) * 100));
                console.log([this.percent, event.lengthComputable, event.loaded, event.total]);
            });

            reader.addEventListener("load", (event) => {
                self.url = event.target.result;
                console.log(self.url);
            });
        },

        //�󴫤j�Y��
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