$(document).ready(function () {
    $(".editor").each(function () {
        CKEDITOR.replace($(this).attr("id"));
    })
});

var silinecekID;
var productID;
$(".slideDelete").click(function () {
    silinecekID = $(this).attr("rowID");
    $("#modelDelete").modal();
});
$(".brandDelete").click(function () {
    silinecekID = $(this).attr("rowID");
    $("#modelDelete").modal();
});
$(".categoryDelete").click(function () {
    silinecekID = $(this).attr("rowID");
    $("#modelDelete").modal();
});
$(".productDelete").click(function () {
    silinecekID = $(this).attr("rowID");
    $("#modelDelete").modal();
});
$(".productPictureDelete").click(function () {
    silinecekID = $(this).attr("rowID");
    productID = $(this).attr("productID");
    $("#modelDelete").modal();
});

function deleteSlide() {
    $.ajax({
        type: "POST",
        url: "/admin/slayt/sil",
        data: { id: silinecekID },
        success: function (result) {
            if (result == "OK") location.href = "/admin/slayt";
            else alert(result);
        }
    });
}

function deleteBrand() {
    $.ajax({
        type: "POST",
        url: "/admin/marka/sil",
        data: { id: silinecekID },
        success: function (result) {
            if (result == "OK") location.href = "/admin/marka";
            else alert(result);
        }
    });
}

function deleteCategory() {
    $.ajax({
        type: "POST",
        url: "/admin/kategori/sil",
        data: { id: silinecekID },
        success: function (result) {
            if (result == "OK") location.href = "/admin/kategori";
            else alert(result);
        }
    });
}

function deleteProduct() {
    $.ajax({
        type: "POST",
        url: "/admin/urun/sil",
        data: { id: silinecekID },
        success: function (result) {
            if (result == "OK") location.href = "/admin/urun";
            else alert(result);
        }
    });
}

function deleteProductPicture() {
    $.ajax({
        type: "POST",
        url: "/admin/resim/sil",
        data: { id: silinecekID },
        success: function (result) {
            if (result == "OK") location.href = "/admin/resim?productid=" + productID;
            else alert(result);
        }
    });
}

