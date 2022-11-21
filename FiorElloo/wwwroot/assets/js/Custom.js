jQuery(function ($) {
    $(document).on('click', '#addToCart', function () {
        var id = $(this).data('id');
        $.ajax({
            method: "POST",
            url: "/basket/add",
            data: {
                id: id
            },
            succes: function () {
                console.log("ok");
            }
        })
    })
})