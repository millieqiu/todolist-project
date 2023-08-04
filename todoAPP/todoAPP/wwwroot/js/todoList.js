window.onload = function () {
    getUserInfo();
    getTodoList();
}

function onClickCreateTodoItem() {
    const todoText = document.getElementById('todoText').value;
    createTodoItem(todoText);
}

function onClickDeleteTodoItem(todoID) {
    $.ajax({
        url: "/api/TodoList/Delete?id=" + todoID,
        type: "DELETE",
        success: function () {
            getTodoList();
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
    if (page == undefined) {
        page = 1;
    }
    $.ajax({
        url: "/api/TodoList/Pagination?page="+page,
        method: "get",
        success: function (res) {
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
                                            <div class="icon" id="delete_item_${value.id}" onClick="onClickDeleteTodoItem(${value.id})">
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
            getTodoList();
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