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

/***/ "./src/header.js":
/*!***********************!*\
  !*** ./src/header.js ***!
  \***********************/
/***/ (() => {

eval("const header = new Vue({\r\n    el: '#header',\r\n    methods: {\r\n        //Logout\r\n        onClickLogout() {\r\n            $.ajax({\r\n                url: \"/api/User/Logout\",\r\n                method: \"post\",\r\n                success: function (res) {\r\n                    window.location.assign(\"/Index\");\r\n                },\r\n                error: function (req, status) {\r\n                    alert(\"�n�X���ѡA�Э��s���յn�X\");\r\n                }\r\n            })\r\n        },\r\n    }\r\n})\n\n//# sourceURL=webpack://todoapp/./src/header.js?");

/***/ })

/******/ 	});
/************************************************************************/
/******/ 	
/******/ 	// startup
/******/ 	// Load entry module and return exports
/******/ 	// This entry module can't be inlined because the eval devtool is used.
/******/ 	var __webpack_exports__ = {};
/******/ 	__webpack_modules__["./src/header.js"]();
/******/ 	
/******/ })()
;