import Vue from 'vue';
import $ from "jquery";
import './css/style.scss';

const setting = new Vue({
    el: '#setting',
    data() {
        return {
            url: null,
            file: {},
        }
    },
    mounted() {
        this.getAvatar();
    },
    methods: {
        //Ū���j�Y��
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

        //�w���j�Y��
        onFileChange(e) {
            var self = this;

            this.file = e.target.files[0];
            let reader = new FileReader();
            reader.readAsDataURL(self.file);

            //�i�ױ�
            reader.addEventListener("progress", (event) => {
                console.log('progress');
            });

            reader.addEventListener("load", (event) => {
                this.url = event.target.result;
            });

            console.log(this.file);
            console.log(this.url);
        },

        //�󴫤j�Y��
        onClickUploadImage() {
            let avatar = this.file;
            var formData = new FormData();
            formData.append('avatar', avatar); // �]�w�W�Ǫ��ɮ�

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