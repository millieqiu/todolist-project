/*
 * ATTENTION: The "eval" devtool has been used (maybe by default in mode: "development").
 * This devtool is neither made for production nor for readable output files.
 * It uses "eval()" calls to create a separate source file in the browser devtools.
 * If you are trying to read the output file, select a different devtool (https://webpack.js.org/configuration/devtool/)
 * or disable the default devtool with "devtool: false".
 * If you are looking for production-ready output files, see mode: "production" (https://webpack.js.org/configuration/mode/).
 */
/******/ (() => { // webpackBootstrap
/******/ 	var __webpack_modules__ = ({

/***/ "./src/todoList.js":
/*!*************************!*\
  !*** ./src/todoList.js ***!
  \*************************/
/***/ (() => {

eval("﻿const appTodo = new Vue({\r\n    el: '#appTodo',\r\n    data() {\r\n        //Todo: 要渲染的資料會放在data裡面 (不要用refs)\r\n        return {\r\n\r\n            todoText: '',\r\n\r\n            message: 'Hello World', //測試用資料\r\n\r\n            todos: [], //傳遞API讀取的資料\r\n            numOfPages: 0, //資料型態為數字，給定預設值0 (因為沒有任何items前不應該有頁碼)\r\n            currentPage: 0,\r\n\r\n        }\r\n    },\r\n    mounted() {\r\n        //由於是設定頁數被點擊後才能觸發getTodoList函式，登入後尚未點擊頁數前須給定預設值為1\r\n        this.getTodoList(1); //todo: 改將預設值寫在函式裡\r\n    },\r\n    methods: {\r\n        getTodoList(page) {\r\n            var self = this; //在這邊給定範圍內的this都是self\r\n\r\n            $.ajax({\r\n                url: \"/api/TodoList/ListPagination?page=\" + page,\r\n                method: \"get\",\r\n                contentType: \"application/json; charset=utf-8\",\r\n\r\n                success: function (res) {\r\n                    //在這裡寫this的話會指向ajax\r\n                    self.numOfPages = res.numOfPages;\r\n                    self.currentPage = res.currentPage;\r\n                    self.todos = res.list;\r\n                }\r\n            })\r\n\r\n\r\n        },\r\n\r\n        onClickCreateTodoItem() {\r\n            var self = this;\r\n\r\n\r\n            let todoItem = JSON.stringify({\r\n                Text: this.todoText,\r\n                page: this.currentPage,\r\n            });\r\n\r\n            $.ajax({\r\n                url: \"/api/TodoList/Create\",\r\n                method: \"post\",\r\n                contentType: \"application/json; charset=utf-8\",\r\n                data: todoItem,\r\n                success: function (res) {\r\n                    self.getTodoList(self.currentPage);\r\n                }\r\n            })\r\n        },\r\n        onClickDeleteTodoItem(id) {\r\n            //todo: delete功能\r\n            var self = this;\r\n\r\n            let todoItem = JSON.stringify({\r\n                id: id,\r\n                page: this.currentPage,\r\n            });\r\n\r\n            $.ajax({\r\n                url: \"/api/TodoList/Delete\",\r\n                method: \"delete\",\r\n                contentType: \"application/json; charset=utf-8\",\r\n                data: todoItem,\r\n                success: function (res) {\r\n                    self.getTodoList(self.currentPage);\r\n                }\r\n            })\r\n        },\r\n        onChangeStatus(id) {\r\n            //todo: check功能\r\n            let todoItem = JSON.stringify({\r\n                id: id,\r\n                page: this.currentPage,\r\n            });\r\n\r\n            $.ajax({\r\n                url: \"/api/TodoList/ChangeStatus\",\r\n                method: \"put\",\r\n                contentType: \"application/json; charset=utf-8\",\r\n                data: todoItem,\r\n                success: function (res) {\r\n                    getTodoList(page);\r\n                }\r\n            })\r\n        },\r\n    }\r\n})\n\n//# sourceURL=webpack://todoapp/./src/todoList.js?");

/***/ })

/******/ 	});
/************************************************************************/
/******/ 	
/******/ 	// startup
/******/ 	// Load entry module and return exports
/******/ 	// This entry module can't be inlined because the eval devtool is used.
/******/ 	var __webpack_exports__ = {};
/******/ 	__webpack_modules__["./src/todoList.js"]();
/******/ 	
/******/ })()
;