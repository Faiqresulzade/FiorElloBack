$(function () {
    $(document).on('click', '#loadMore', function () {
        $.ajax({
            method: "GET",
            url: "Pricing/LoadMore",
            success: function (result) {
                $('#ourpricings').append(result)
                console.log("heelo")
            }
        })
    })
})