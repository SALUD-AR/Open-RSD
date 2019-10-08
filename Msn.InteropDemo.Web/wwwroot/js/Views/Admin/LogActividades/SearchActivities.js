

function seachActivities() {
    var loc = window.rootUrl + "Admin/LogActividades/SeachActivities";
    var tableActivitiesContainer = $('#tableActivitiesContainer');
    var btnSearch = $('#btnSearch');
    var preHtml = btnSearch.html();

    btnSearch.html('<span class="fas fa-spinner fa-spin"></span><span> Buscar</span>');
    btnSearch.addClass('disabled');

    var dataToPost = {
        dateFrom: $('#dateFromInput').val(),
        dateTo: $('#dateToInput').val(),
        __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val()
    };

    $.ajax({
        url: loc,
        type: 'POST',
        dataType: 'json',
        data: dataToPost,
        cache: false,
        success: function (data) {
            tableActivitiesContainer.html(data.table);
            tableActivitiesContainer.fadeIn('slow');

            btnSearch.html(preHtml);
            btnSearch.removeClass('disabled');
        },
        error: function (request, status, error) {
            console.log(request.responseText);
            alert(request.responseText);
        }
    });
}
