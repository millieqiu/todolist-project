import Vue from 'vue';
import $ from "jquery";
import './css/style.scss';

const setting = new Vue({
    el: '#setting',
    data() {
        return {
            message: 'Testtttttt', //���եθ��
            default: '~/lib/default.jpg',
            url: null,
        }
    },
    methods: {
        onFileChange(e) {
            const file = e.target.files[0];
            this.url = URL.createObjectURL(file);
        }
    },
})