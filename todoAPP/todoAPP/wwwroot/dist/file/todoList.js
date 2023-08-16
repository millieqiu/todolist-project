import Vue from 'vue';
import $ from "jquery";

const appTodo = new Vue({
    el: '#appTodo',
    data() {
        //Todo: 要渲染的資料會放在data裡面 (不要用refs)
        return {

            todoText: '',

            message: 'Hello World', //測試用資料

            todos: [], //傳遞API讀取的資料
            numOfPages: 0, //資料型態為數字，給定預設值0 (因為沒有任何items前不應該有頁碼)
            currentPage: 0,

        }
    },
    mounted() {
        //由於是設定頁數被點擊後才能觸發getTodoList函式，登入後尚未點擊頁數前須給定預設值為1
        this.getTodoList(1); //todo: 改將預設值寫在函式裡
    },
    methods: {
        getTodoList(page) {
            var self = this; //在這邊給定範圍內的this都是self

            $.ajax({
                url: "/api/TodoList/ListPagination?page=" + page,
                method: "get",
                contentType: "application/json; charset=utf-8",

                success: function (res) {
                    //在這裡寫this的話會指向ajax
                    self.numOfPages = res.numOfPages;
                    self.currentPage = res.currentPage;
                    self.todos = res.list;
                }
            })


        },

        onClickCreateTodoItem() {
            var self = this;


            let todoItem = JSON.stringify({
                Text: this.todoText,
                page: this.currentPage,
            });

            $.ajax({
                url: "/api/TodoList/Create",
                method: "post",
                contentType: "application/json; charset=utf-8",
                data: todoItem,
                success: function (res) {
                    self.getTodoList(self.currentPage);
                }
            })
        },
        onClickDeleteTodoItem(id) {
            //todo: delete功能
            var self = this;

            let todoItem = JSON.stringify({
                id: id,
                page: this.currentPage,
            });

            $.ajax({
                url: "/api/TodoList/Delete",
                method: "delete",
                contentType: "application/json; charset=utf-8",
                data: todoItem,
                success: function (res) {
                    self.getTodoList(self.currentPage);
                }
            })
        },
        onChangeStatus(id) {
            //todo: check功能
            let todoItem = JSON.stringify({
                id: id,
                page: this.currentPage,
            });

            $.ajax({
                url: "/api/TodoList/ChangeStatus",
                method: "put",
                contentType: "application/json; charset=utf-8",
                data: todoItem,
                success: function (res) {
                    getTodoList(page);
                }
            })
        },
    }
})