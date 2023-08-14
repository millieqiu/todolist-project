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

/***/ "./src/site.js":
/*!*********************!*\
  !*** ./src/site.js ***!
  \*********************/
/***/ (() => {

eval("﻿// 建立 Vue\r\nconst app = new Vue({\r\n    el: '#app',\r\n    data() {\r\n        //Todo: 要渲染的資料會放在data裡面 (不要用refs)\r\n        return {\r\n            //Login info\r\n            loginEmail: '',\r\n            loginPassword: '',\r\n\r\n            message: 'Hello World', //測試用資料\r\n\r\n        }\r\n    },\r\n\r\n    //執行Vue實體後先載入getTodoList()函式讀取資料\r\n    //在登陸頁會有無法讀取api的錯誤訊息\r\n    //拆成兩支js\r\n    //mounted() {\r\n    //由於是設定頁數被點擊後才能觸發getTodoList函式，登入後尚未點擊頁數前須給定預設值為1\r\n        //this.getTodoList(1); //todo: 改將預設值寫在函式裡\r\n    //},\r\n\r\n    methods: {\r\n        //Login\r\n        onClickLogin() {\r\n            var self = this;\r\n\r\n            //refs - 用來控制Component(子項目)\r\n            let userInfo = {\r\n                username: this.loginEmail,\r\n                password: this.loginPassword,\r\n            };\r\n\r\n            //改用fetch\r\n            fetch('/api/User/Login', {\r\n                method: \"post\",\r\n                headers: {\r\n                    'Content-Type': 'application/json'\r\n                },\r\n                body: JSON.stringify(userInfo),\r\n            }).then((response) => {\r\n                if (response.ok) {\r\n                    console.log(response.json());  \r\n                    window.location.href = \"/TodoPage\";\r\n                    //self.getTodoList(1); //在logIn呼叫的函式不會被帶到下一頁\r\n                }\r\n                return Promise.reject(response);\r\n            }).catch((response, status) => {\r\n                response.json().then((json) => {\r\n                    if (json.service == \"Login\" && json.status == 1) {\r\n                        alert(\"此帳號不存在\");\r\n                    }\r\n                    else if (json.service == \"Login\" && json.status == 2) {\r\n                        alert(\"帳號或密碼錯誤\");\r\n                    }\r\n                    else {\r\n                        alert(\"無法登入，請聯絡系統管理員\");\r\n                    }\r\n                })\r\n                \r\n            });\r\n\r\n        },\r\n \r\n    },\r\n});\r\n\r\n\r\n\r\n//todoList.js\n\n//# sourceURL=webpack://todoapp/./src/site.js?");

/***/ })

/******/ 	});
/************************************************************************/
/******/ 	
/******/ 	// startup
/******/ 	// Load entry module and return exports
/******/ 	// This entry module can't be inlined because the eval devtool is used.
/******/ 	var __webpack_exports__ = {};
/******/ 	__webpack_modules__["./src/site.js"]();
/******/ 	
/******/ })()
;