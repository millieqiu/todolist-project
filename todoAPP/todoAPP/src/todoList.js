import Vue from 'vue';
import $ from "jquery";
import './css/style.scss';


const appTodo = new Vue({
    el: '#appTodo',
    data() {
        //Todo: 要渲染的資料會放在data裡面 (不要用refs)
        return {

            todoText: '',

            todos: [], //傳遞API讀取的資料
            numOfPages: 0, //資料型態為數字，給定預設值0 (因為沒有任何items前不應該有頁碼)
            currentPage: 0,

            isLoading: false,

        }
    },
    mounted() {
        //由於是設定頁數被點擊後才能觸發getTodoList函式，登入後尚未點擊頁數前須給定預設值為1
        this.getTodoList(1); //todo: 改將預設值寫在函式裡
    },
    methods: {
        // get todoList
        getTodoList(page) {
            this.isLoading = true;
            var self = this; //在這邊給定範圍內的this都是self

            fetch(('/api/TodoList/ListPagination?page=' + page), {
                method: 'get',
                headers: {
                    'Content-Type': "application/json; charset=utf-8",
                }
            })
                .then(res => {
                    return res.json();
                })
                .then(json => {
                    self.numOfPages = json.numOfPages;
                    self.currentPage = json.currentPage;
                    self.todos = json.list;
                    self.isLoading = false;
                    console.log(json);
                })

        },

        // Create
        onClickCreateTodoItem() {
            var self = this;


            let todoItem = {
                Text: this.todoText,
                page: this.currentPage,
            };


            fetch('/api/TodoList/Create', {
                method: 'post',
                headers: {
                    'Content-Type': "application/json; charset=utf-8",
                },
                body: JSON.stringify(todoItem)
            })
                .then(res => {
                    self.getTodoList(self.currentPage);
                })

        },


        // Delete todoItem
        onClickDeleteTodoItem(id) {
            var self = this;

            let todoItem = {
                id: id,
                page: this.currentPage,
            };

            fetch('/api/TodoList/Delete', {
                method: 'delete',
                headers: {
                    'Content-Type': "application/json; charset=utf-8",
                },
                body: JSON.stringify(todoItem)
            })
                .then(res => {
                    self.getTodoList(self.currentPage);
                })

        },

        // Change status
        onChangeStatus(id) {
            let todoItem = {
                id: id,
                body: this.currentPage,
            };

            fetch('/api/TodoList/ChangeStatus', {
                method: 'put',
                headers: {
                    'Content-Type': "application/json; charset=utf-8",
                },
                data: JSON.stringify(todoItem)
            })
                .then(res => {
                    self.getTodoList(self.currentPage);
                })
        },
    }
})