$(function () {
    $(document).on("click", "#edit-area .img-delete", function () {
        let id = parseInt($(this).attr("data-id"));

        $.ajax({
            type: "POST",
            url: `/admin/product/deleteproductimage?id=${id}`,
            success: function () {
                $(`[data-id=${id}]`).closest(".col-3").remove();
            }
        });
    })

      $(document).on("click", "#edit-area .set-main", function () {
        let imgId = parseInt($(this).attr("data-id"));
        let productId = parseInt($(this).attr("data-productId"));

        $.ajax({
            type: "POST",
            url: `/admin/product/setmainimage?imgid=${imgId}&productid=${productId}`,
            success: function () {
                $("img").removeClass("image-main");
                $(`[data-id=${imgId}]`).closest(".col-3").find("img").addClass("image-main");
                $(".operation-area").removeClass("d-none");
                $(`[data-id=${imgId}]`).closest(".operation-area").addClass("d-none");
            }
        });
    })
})