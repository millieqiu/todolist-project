﻿window.onload = function () {
    getTodoList();
}

function onClickCreateTodoItem() {
    const todoText = document.getElementById('todoText').value;
    createTodoItem(todoText);
}

function getTodoList() {
    $.ajax({
        url: "/api/TodoList/ListAll",
        method: "get",
        success: function (res) {
            $.each(res, function (key, value) {
                $('#todolist').append(`<div class="task">
                                            <label class="task-content">
                                                <input type="checkbox" name="" id="${value.id}" class="checkbox" onclick="onChangeStatus(${value.id})">
                                                <span class="gray-01 paragraph1" for="">${value.text}</span>
                                            </label>
                                            <div class="icon">
                                                <i class="fa-solid fa-trash gray-04"></i>
                                            </div>
                                        </div>`).html();
            });
        }
    })
}

function createTodoItem(text) {
    $.ajax({
        url: "/api/TodoList/Create",
        method: "post",
        data: {
            Text: text,
        },
        success: function (res) {
            console.log(res);
        }
    })
}

function onChangeStatus(todoID) {

    $.ajax({
        url: "/api/TodoList/ChangeStatus",
        method: "put",
        data: {
            id: todoID,
        },
        success: function (res) {
            //console.log(res);
        }
    })
}