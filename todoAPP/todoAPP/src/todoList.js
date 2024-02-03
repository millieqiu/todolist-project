import Vue from "vue";
import $ from "jquery";
import "./css/todoList.scss";

const appTodo = new Vue({
  el: "#appTodo",
  data() {
    //Todo: 要渲染的資料會放在data裡面 (不要用refs)
    return {
      todoText: "",

      todos: [], //傳遞API讀取的資料
      numOfPages: 0, //資料型態為數字，給定預設值0 (因為沒有任何items前不應該有頁碼)
      currentPage: 0,

      sortBy: 0,
      sortOptions: [
        { value: "createAt", order: "ASC", label: "依照建立時間（由遠到近）" },
        { value: "createAt", order: "DESC", label: "依照建立時間（由近到遠）" },
      ],
    };
  },
  computed: {
    computedText() {
      return this.todoText.length;
    },
  },
  async mounted() {
    //由於是設定頁數被點擊後才能觸發getTodoList函式，登入後尚未點擊頁數前須給定預設值為1
    await this.getTodoList(1); //todo: 改將預設值寫在函式裡
  },
  methods: {
    // get todoList
    getTodoList(page) {
      var self = this; //在這邊給定範圍內的this都是self

      fetch("/api/TodoList?page=" + page, {
        method: "get",
        headers: {
          "Content-Type": "application/json; charset=utf-8",
        },
      })
        .then((res) => {
          return res.json();
        })
        .then((json) => {
          self.numOfPages = json.numOfPages;
          self.currentPage = json.currentPage;
          self.todos = json.list;
        });
    },

    // Create
    onClickCreateTodoItem() {
      var self = this;

      let todoItem = {
        Text: this.todoText,
      };

      fetch("/api/TodoList", {
        method: "post",
        headers: {
          "Content-Type": "application/json; charset=utf-8",
        },
        body: JSON.stringify(todoItem),
      }).then((res) => {
        self.getTodoList(self.currentPage);
      });
    },

    // Delete todoItem
    onClickDeleteTodoItem(id) {
      var self = this;

      let todoItem = {
        id: id,
      };

      fetch("/api/TodoList", {
        method: "delete",
        headers: {
          "Content-Type": "application/json; charset=utf-8",
        },
        body: JSON.stringify(todoItem),
      }).then((res) => {
        self.getTodoList(self.currentPage);
      });
    },

    // Change status
    onChangeStatus(id) {
      var self = this;

      let todoItem = {
        id: id,
      };

      fetch("/api/TodoList/Status", {
        method: "patch",
        headers: {
          "Content-Type": "application/json; charset=utf-8",
        },
        body: JSON.stringify(todoItem),
      }).then((res) => {
        self.getTodoList(self.currentPage);
      });
    },
    sortList(columnName, order) {
      this.todos = this.todos.sort(function (a, b) {
        if (order == "ASC") {
          return a[columnName] < b[columnName] ? 1 : -1;
        } else {
          return a[columnName] > b[columnName] ? 1 : -1;
        }
      });
    },
  },
});
