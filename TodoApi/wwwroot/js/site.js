const uri = 'api/todo';
let todos = [];


function getItems() {
    // получаем токен из cookie
    const token = getCookie("accessToken");;
    if (token === "") {
        window.location.href = '/login.html';
    }
    fetch(uri, {
        method: "GET",
        headers: {
            "Accept": "application/json",
            "Authorization": "Bearer " + token  // передача токена в заголовке
        }
    })
        .then(response => {
            if (response.status === 401) {
                // Если не авторизован, выполняем редирект
                console.log('Ошибка сервера!')
                //window.location.href = '/login.html';
            }
            else if (response.status === 200) {
                // Если данные успешно получили передаем их дальше
                return response.json();
            }
        })
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

function addItem() {
    const addNameTextbox = document.getElementById('add-name');

    const item = {
        isComplete: false,
        name: addNameTextbox.value.trim()
    };
    //получение токена из cookie
    const token = getCookie("accessToken");;
    if (token === "") {
        window.location.href = '/login.html';
    }
    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            "Authorization": "Bearer " + token  // передача токена в заголовке
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            getItems();
            addNameTextbox.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id) {
    //получение токена из cookie
    const token = getCookie("accessToken");;
    if (token === "") {
        window.location.href = '/login.html';
    }
    fetch(`${uri}/${id}`, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            "Authorization": "Bearer " + token  // передача токена в заголовке
        },
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const item = todos.find(item => item.id === id);

    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-isComplete').checked = item.isComplete;
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        id: parseInt(itemId, 10),
        isComplete: document.getElementById('edit-isComplete').checked,
        name: document.getElementById('edit-name').value.trim()
    };
    //получение токена из cookie
    const token = getCookie("accessToken");;
    if (token === "") {
        window.location.href = '/login.html';
    }
    fetch(`${uri}/${itemId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            "Authorization": "Bearer " + token  // передача токена в заголовке
        },
        body: JSON.stringify(item)
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to update item.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'to-do' : 'to-dos';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayItems(data) {
    const tBody = document.getElementById('todos');
    tBody.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {
        let baseDiv = document.createElement("div");
        baseDiv.className = 'row px-3 align-items-center todo-item rounded';
        let funcDiv = document.createElement("div");
        funcDiv.className = 'col-auto m-1 p-0 d-flex align-items-center';
        baseDiv.appendChild(funcDiv);
        //h2 araea
        let h2 = document.createElement("h2");
        h2.className = 'm-0 p-0';

        let i1 = document.createElement("i");
        i1.className = 'fa fa-square-o text-primary btn m-0 p-0 d-none';
        i1.setAttribute('data-toggle', 'tooltip');
        i1.setAttribute('data-placement', 'bottom');
        i1.setAttribute('title', 'Mark as complete');

        //Checked: fa fa-check-square-o text-primary btn m-0 p-0
        //Not checked: fa fa-square-o text-primary btn m-0 p-0
        let i2 = document.createElement("i");
        i2.id = 'edit-isComplete';
        if (item.isComplete == true) {
            i2.className = 'fa fa-check-square-o text-primary btn m-0 p-0';
        } else {
            i2.className = 'fa fa-square-o text-primary btn m-0 p-0';
        }
        i2.setAttribute('data-toggle', 'tooltip');
        i2.setAttribute('data-placement', 'bottom');
        i2.setAttribute('title', 'Mark as complete');

        h2.appendChild(i1);
        h2.appendChild(i2);
        //h2 area and
        funcDiv.appendChild(h2);

        //div text area
        let textDiv = document.createElement("div");
        textDiv.className = 'col px-1 m-1 d-flex align-items-center';

        let inputText = document.createElement("input");
        inputText.className = 'form-control form-control-lg border-0 edit-todo-input bg-transparent rounded px-3';
        inputText.setAttribute('value', item.name);
        inputText.readOnly = true;
        inputText.setAttribute('title', item.name);

        let inputCheckBox = document.createElement("input");
        inputCheckBox.id = 'edit-name';
        inputCheckBox.className = 'form-control form-control-lg border-0 edit-todo-input rounded px-3 d-none';
        inputCheckBox.setAttribute('value', item.name);
        //div text area end
        textDiv.appendChild(inputText);
        textDiv.appendChild(inputCheckBox);
        baseDiv.appendChild(textDiv);


        //empty div
        let emptyDiv = document.createElement("div");
        emptyDiv.className = 'col-auto m-1 p-0 px-3 d-none';
        //empty div end

        baseDiv.appendChild(emptyDiv);

        //edit delete area
        let buttonsDiv = document.createElement("div");
        buttonsDiv.className = 'col-auto m-1 p-0';

        let innerButtonsDiv = document.createElement("div");
        innerButtonsDiv.className = 'row d-flex align-items-center justify-content-end';

        let h51 = document.createElement("h5");
        h51.className = 'm-0 p-0 px-2';

        let i12 = document.createElement("i");
        i12.className = 'fa fa-pencil text-info btn m-0 p-0';
        i12.setAttribute('data-toggle', 'tooltip');
        i12.setAttribute('data-placement', 'bottom');
        i12.setAttribute('title', 'Edit todo');
        i12.setAttribute('onclick', `displayEditForm(${item.id})`);

        h51.appendChild(i12);

        let h52 = document.createElement("h5");
        h52.className = 'm-0 p-0 px-2';

        let i22 = document.createElement("i");
        i22.className = 'fa fa-trash-o text-danger btn m-0 p-0';
        i22.setAttribute('data-toggle', 'tooltip');
        i22.setAttribute('data-placement', 'bottom');
        i22.setAttribute('title', 'Delete todo');
        i22.setAttribute('onclick', `deleteItem(${item.id})`);

        h52.appendChild(i22)

        innerButtonsDiv.appendChild(h51);
        innerButtonsDiv.appendChild(h52);

        buttonsDiv.appendChild(innerButtonsDiv);
        //edit delete area end
        baseDiv.appendChild(buttonsDiv);

        tBody.appendChild(baseDiv);
    });

    todos = data;
}

function getCookie(cookieName) {
    var name = cookieName + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var cookieArray = decodedCookie.split(';');

    for (var i = 0; i < cookieArray.length; i++) {
        var cookie = cookieArray[i];
        while (cookie.charAt(0) == ' ') {
            cookie = cookie.substring(1);
        }
        if (cookie.indexOf(name) == 0) {
            return cookie.substring(name.length, cookie.length);
        }
    }

    return "";
}


