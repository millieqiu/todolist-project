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

/***/ "./src/register.js":
/*!*************************!*\
  !*** ./src/register.js ***!
  \*************************/
/***/ (() => {

eval("const app2 = new Vue({\r\n    el: '#app2',\r\n    data() {\r\n        //Todo: �n��V����Ʒ|��bdata�̭� (���n��refs)\r\n        return {\r\n            //Register info\r\n            registerEmail: '',\r\n            registerPassword: '',\r\n            confirmPassword: '',\r\n            registerName: '',\r\n\r\n        }\r\n    },\r\n    methods: {\r\n        onClickRegister() {\r\n            let userInfo = JSON.stringify({\r\n                username: this.registerEmail,\r\n                password: this.registerPassword,\r\n                confirmPassword: this.confirmPassword,\r\n                nickname: this.registerName,\r\n            });\r\n            $.ajax({\r\n                url: \"/api/User/Register\",\r\n                method: \"post\",\r\n                contentType: \"application/json; charset=utf-8\",\r\n                data: userInfo,\r\n                success: function (res) {\r\n                    alert(\"���U���\\�I\");\r\n                    window.location.assign(\"/Index\");\r\n                },\r\n                error: function (req, status) {\r\n                    if (req.responseJSON.service && req.responseJSON.service == \"Register\" && req.responseJSON.status == 1) {\r\n                        alert(\"�b���w�s�b\");\r\n                    }\r\n                    else if (req.responseJSON.errors.Username && req.responseJSON.errors.Username.includes(\"The Username field is not a valid e-mail address.\")) {\r\n                        alert(\"Email�榡���~\");\r\n                    }\r\n                    else if (req.responseJSON.errors.ConfirmPassword) {\r\n                        alert(\"�K�X�P�T�{�K�X���ŦX\");\r\n                    }\r\n                    else {\r\n                        alert(\"�L�k���U�b���A���p���t�κ޲z��\");\r\n                    }\r\n                }\r\n            })\r\n        },\r\n    }\r\n})\n\n//# sourceURL=webpack://todoapp/./src/register.js?");

/***/ })

/******/ 	});
/************************************************************************/
/******/ 	
/******/ 	// startup
/******/ 	// Load entry module and return exports
/******/ 	// This entry module can't be inlined because the eval devtool is used.
/******/ 	var __webpack_exports__ = {};
/******/ 	__webpack_modules__["./src/register.js"]();
/******/ 	
/******/ })()
;