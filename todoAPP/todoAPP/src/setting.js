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
        //Åªï¿½ï¿½ï¿½jï¿½Yï¿½ï¿½
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

        //ï¿½wï¿½ï¿½ï¿½jï¿½Yï¿½ï¿½
        onFileChange(e) {
            var self = this;

            this.file = e.target.files[0];
            let reader = new FileReader();
            reader.readAsDataURL(self.file);

            //¶i«×±ø
            reader.addEventListener("progress", (event) => {
                console.log('progress');
            });

            reader.addEventListener("load", (event) => {
                this.url = event.target.result;
            });

            console.log(this.file);
            console.log(this.url);
        },

        //ï¿½ó´«¤jï¿½Yï¿½ï¿½
        onClickUploadImage() {
            let avatar = this.file;
            var formData = new FormData();
            formData.append('avatar', avatar); // ï¿½]ï¿½wï¿½Wï¿½Çªï¿½ï¿½É®ï¿½

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