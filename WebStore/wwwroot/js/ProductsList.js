function addAdminContent() {

    document.getElementById("addProducts").style.visibility = "visible";
    document.getElementById("editOrder").style.visibility = "visible";
}

function addToCart(data) {
    var prodName = $(data).data('assigned-id');
    $.ajax({
        type: "GET",
        url: "Home/AddToCart?prodName=" + prodName,
        success: function (data) {
            alert("Successfuly added to cart")
        }
    });
}

function removeProd(data) {
    var prodName = $(data).data('assigned-id');
    $.ajax({
        type: "DELETE",
        url: "Home/RemoveProduct?prodName=" + prodName,
        success: function (data) {
            alert("Successfuly removed")
        }
    });
}


function getCookie(name) {
    var matches = document.cookie.match(new RegExp(
        "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}

function getAdminVisibility() {
    if (getCookie('IsAdmin') === true) {
        return 'visible'
    } else {
        return "hiddent"
    }
}