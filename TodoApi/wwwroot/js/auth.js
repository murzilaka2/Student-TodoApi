document.addEventListener('DOMContentLoaded', function () {
    // Проверяем наличие авторизационного токена в localStorage
    var token = getCookie("accessToken");

    if (!token) {
        // Токен отсутствует, выполните необходимые действия, например, перенаправление на страницу входа
        window.location.href = '/login.html';
    } else {
        checkAuthorization(token)
            .then(authorized => {
                if (authorized) {
                    // Пользователь авторизован, продолжите загрузку страницы
                    // Дополнительные действия или загрузка остального контента страницы
                } else {
                    // Пользователь не авторизован, выполните необходимые действия, например, перенаправление на страницу входа
                    window.location.href = '/login.html';
                }
            });
    }
});

function checkAuthorization(token) {
    return fetch('/api/CheckAuthorization', {
        method: 'GET',
        headers: {
            "Accept": "application/json",
            "Authorization": "Bearer " + token  // передача токена в заголовке
        }
    })
        .then(response => response.ok)
        .catch(error => false);
}

function logOut() {
    deleteCookie('accessToken');
    window.location.href = '/login.html';
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

function deleteCookie(name) {
    document.cookie = name + '=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
}