window.onload = function () {
    getUserInfo();
    getTodoList();
}

function onClickCreateTodoItem(event) {
    event.preventDefault();
    const todoText = document.getElementById('todoText').value;
    createTodoItem(todoText);
}

function onClickDeleteTodoItem(event,todoID) {
    event.preventDefault();

    let todoItem = JSON.stringify({
        "id": todoID,
    });

    $.ajax({
        url: "/api/TodoList/Delete?id=" + todoID,
        type: "DELETE",
        contentType: "application/json; charset=utf-8",
        data: todoItem,
        success: function (res) {
            setTodoList(res.pagination);
        } 
    })
}

function getUserInfo() {
    $.ajax({
        url: "/api/User/GetUserInfo",
        method: "get",
        success: function (res) {
            $('#user_name').html(res.nickname);
        }
    })
}

function getTodoList(page) {
    //有可能是null或其他
    if (!page) {
        page = 1;
    }
    $.ajax({
        url: "/api/TodoList/ListPagination?page="+page,
        method: "get",
        success: function (res) {
            setTodoList(res);
        }
    })
}

function createTodoItem(text) {
    let todoItem = JSON.stringify({
        "Text": text,
    });

    $.ajax({
        url: "/api/TodoList/Create",
        method: "post",
        contentType: "application/json; charset=utf-8",
        data: todoItem,
        success: function (res) {
            setTodoList(res.pagination);
        }
    })
}

function onChangeStatus(todoID) {
    let todoItem = JSON.stringify({
        "id": todoID,
    });

    $.ajax({
        url: "/api/TodoList/ChangeStatus",
        method: "put",
        contentType: "application/json; charset=utf-8",
        data: todoItem,
        success: function (res) {
            setTodoList(res.pagination);
        }
    })
}

function setTodoList(res) {
    $('#todolist').html("");
    $('#pagination').html("");
    for (let i = 1; i <= res.numOfPages; i++) {
        $('#pagination').append(`<li onClick="getTodoList(${i})">${i}</li>`)
    }
    $.each(res.list, function (key, value) {
        let status = "";
        if (value.status == 1) {
            status = "checked";
        }
        $('#todolist').append(`<div class="task">
                                            <label class="task-content">
                                                <input type="checkbox" name="" id="checkbox_item_${value.id}" class="checkbox" onclick="onChangeStatus(${value.id})" ${status}>
                                                <span class="gray-01 paragraph1" for="">${value.text}</span>
                                            </label>
                                            <div class="icon" id="delete_item_${value.id}" onClick="onClickDeleteTodoItem(event,${value.id})">
                                                <i class="fa-solid fa-trash gray-04"></i>
                                            </div>
                                        </div>`).html();
    });
}