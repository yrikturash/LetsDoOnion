/*--------------------------- Add new issues & save to db----------------------------------------*/
$("body").on("click", "#add_btn", function () {

    var text = $('#issue_input').val();
    $('#issue_input').val("");
    console.log(text);

    $.ajax({
        url: '/api/issue/' + activeCategoriId,
        data: JSON.stringify(text),
        type: 'POST',
        contentType: 'application/json',
        complete: function () {


        },
        success: function (data) {

            var d = new Date();
            var day = d.getDay();
            var month = d.getMonth();
            var time = (day < 10 ? '0' : '') + day + "." + (month < 10 ? '0' : '') + month + "." + d.getFullYear() + " " + d.getHours() + ":" + d.getMinutes();
            var categoryName = $('ul.side-menu.side-lists li.active').text().trim();

            $('<li/>', {
                'class': 'row list-group-item',
                'data-id': data
            }).prependTo('#issue_list').prepend("<span class='glyphicon glyphicon-option-vertical pull-left move'' aria-hidden='true'></span><span class='col-md-6'><input type='checkbox'/> " + text + "</span><span class='col-md-3'>" + time + "</span><span class='col-md-2'>" + categoryName + "</span><span class='badge pull-right'>0</span>");

            $('#issue_list li:first').click();

        },
        error: function (error) {
        }
    });
});
/*---------------------- Some init ----------------------------------------------*/
$(function () {
    $('#issue_list li').first().addClass('active');
    $(".sortable").sortable();
    $(".sortable").disableSelection();
    NProgress.configure({ showSpinner: true });
});


$(document).ajaxStart(function () {
    NProgress.start();
});
$(document).ajaxComplete(function () {
    NProgress.done();
});

/*--------------------------- Add new under issues & save to db----------------------------------------*/
$("body").on("click", "#add_uissue_btn", function () {

    if ($('#issue_list li.active').length === 0) {
        alert("Select or create the task first!");
        return;
    }

    var text = $('#uissue_input').val();
    $('#uissue_input').val("");
    var id = $('#issue_list li.active').first().data('id');

    //console.log('id ' + id);

    $.ajax({
        url: '/api/UnderIssue/' + id,
        data: JSON.stringify(text),
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        complete: function () {


        },
        success: function (data) {

            $('<li/>', {
                'class': 'list-group-item row',
                'data-id': data.Id
            }).appendTo('#uIssue_list').append("<span class='glyphicon glyphicon-option-vertical pull-left move' aria-hidden='true'></span><label class='col-md-10'><input type='checkbox' /> " + data.Text + "</label>");
        },
        error: function (error) {
        }
    });
});


/*--------------------------- Implement selecting main issue -  Make li active & loading underIssues----------------------------------------*/
$("body").on("click", "#issue_list li", function (e) {

    //if click on checkbox then return
    if (e.target === $(this).find('input:first')[0])
        return;


    var li = $(this);
    $('#issue_list li').removeClass('active');
    li.addClass('active');

    var id = li.data('id');
    console.log(id);

    $.ajax({
        cache: false,
        type: "GET",
        url: '/api/underissue/'+id,
        //data: { 'id': id },
        contentType: 'application/json; charset=utf-8',
        complete: function () {


        },
        success: function (data) {
            $('#uIssue_list').empty();

            for (var i = 0; i < data.length; i++) {
                if (!data[i].IsFinished) {

                    $('<li/>', {
                        'class': 'list-group-item row',
                        'data-id': data[i].Id
                    }).appendTo('#uIssue_list').append("<span class='glyphicon glyphicon-option-vertical pull-left move' aria-hidden='true'></span><label class='col-md-10'><input type='checkbox' /> " + data[i].Text + "</label>");
                }
            }
            for (var i = 0; i < data.length; i++) {
                if (data[i].IsFinished) {

                    $('<li/>', {
                        'class': 'list-group-item row disabled',
                        'data-id': data[i].Id
                    }).appendTo('#uIssue_list').append("<span class='glyphicon glyphicon-option-vertical pull-left move' aria-hidden='true'></span><label class='col-md-10'><input type='checkbox' checked /> " + data[i].Text + "</label>");
                }
            }


        },
        error: function (error) {
        }
    });
});
/*--------------------------- Implement Finish/Unfinish Issue ----------------------------------------*/
$("body").on("click", "#issue_list li input", function () {

    var li = $(this).closest('li');
    var id = li.data('id');
    //--------------- if not disabled ------------------
    if (!li.hasClass("disabled")) {
        li.removeClass('active');
        li.addClass('disabled');


        /*--------- Animate finish action ----------------*/
        var $this = li,
        callback = function () {
            $this.insertAfter($this.siblings(':last-child'));
        };
        li.slideUp(500, callback).slideDown(500);


        /*------------ Do changes in database --------------*/
        $.ajax({
            cache: false,
            type: "PUT",
            url: '/api/issue/' + id,
            //data: JSON.stringify({ 'id': id }),
            contentType: 'application/json; charset=utf-8',
            complete: function () {
                $('#uIssue_list').empty();


            },
            success: function (data) {

            },
            error: function (error) {
            }
        });
    } else {
        li.removeClass('active');
        li.removeClass('disabled');


        /*--------- Animate finish action ----------------*/
        var $this = li,
        callback = function () {
            $this.insertAfter($this.siblings(':first-child'));
        };
        li.slideUp(500, callback).slideDown(500);


        /*------------ Do changes in database --------------*/
        $.ajax({
            cache: false,
            type: "DELETE",
            url: '/api/issue/'+id,
            //data: JSON.stringify({ 'id': id }),
            contentType: 'application/json; charset=utf-8',
            complete: function () {
                li.click();


            },
            success: function (data) {

            },
            error: function (error) {
            }
        });
    }


});
/*--------------------------- Implement Finish/Unfinish Under Issue ----------------------------------------*/
$("body").on("click", "#uIssue_list li input", function () {

    var li = $(this).closest('li');
    var id = li.data('id');


    //if we finish all then finish main task/issue
    var finishedUIssues = $('#uIssue_list li input:checked').length;
    var unfinishedUIssues = $('#uIssue_list li input:not(:checked)').length;
    if (unfinishedUIssues === 0) {
        var selectedIssueId = $('#issue_list li.active').data('id');
        $('#issue_list li[data-id=' + selectedIssueId + '] input').click();
    }

    //if we unfinish one underisuue when all was finished, then unfinish parent isuue
    if (finishedUIssues + 1 === finishedUIssues + unfinishedUIssues) {
        var issueId = $('#issue_list li.active').data('id');

        var isuueLi = $('#issue_list li[data-id=' + issueId + ']');
        $('#issue_list li[data-id=' + issueId + '] input').prop("checked", "");
        isuueLi.removeClass('disabled');
        /*--------- Animate finish action ----------------*/
        var $this = isuueLi,
        callback = function () {
            $this.insertAfter($this.siblings(':first-child'));
        };
        isuueLi.slideUp(500, callback).slideDown(500);
    }

    //--------------- if not disabled ------------------
    if (!li.hasClass("disabled")) {
        li.removeClass('active');
        li.addClass('disabled');


        /*--------- Animate finish action ----------------*/
        var $this = li,
        callback = function () {
            $this.insertAfter($this.siblings(':last-child'));
        };
        li.slideUp(500, callback).slideDown(500);


        /*------------ Do changes in database --------------*/
        $.ajax({
            type: "PUT",
            url: '/api/underissue/' + id,
            //data: JSON.stringify({ 'id': id }),
            contentType: 'application/json; charset=utf-8',
            complete: function () {


            },
            success: function (data) {

            },
            error: function (error) {
            }
        });
    } else {
        li.removeClass('active');
        li.removeClass('disabled');


        /*--------- Animate finish action ----------------*/
        var $this = li,
        callback = function () {
            $this.insertAfter($this.siblings(':first-child'));
        };
        li.slideUp(500, callback).slideDown(500);


        /*------------ Do changes in database --------------*/
        $.ajax({
            cache: false,
            type: "DELETE",
            url: '/api/underissue/' + id,
            //data: JSON.stringify({ 'id': id }),
            contentType: 'application/json; charset=utf-8',
            complete: function () {


            },
            success: function (data) {

            },
            error: function (error) {
            }
        });

    }
});



/*--------------------------- RemoveCompletedIssues & save to db----------------------------------------*/
$("body").on("click", "#remove_completed", function () {
    var finishedIssues = $('#issue_list li input:checked').parents('li');

    var ids = "";
    finishedIssues.each(function () {
        ids += $(this).attr('data-id') + ",";
    });

    $.ajax({
        url: '/api/issue?ids=' + ids,
        //data: JSON.stringify({ 'ids': ids }),
        type: 'DELETE',
        contentType: 'application/json; charset=utf-8',
        complete: function () {

        },
        success: function (data) {
            finishedIssues.remove();

        },
        error: function (error) {
        }
    });
});
/*--------------------------- RemoveCompletedUIssues & save to db----------------------------------------*/
$("body").on("click", "#remove_completed2", function () {
    var finishedUIssues = $('#uIssue_list li input:checked').parents('li');

    var ids = "";
    finishedUIssues.each(function () {
        ids += $(this).attr('data-id') + ",";
    });

    $.ajax({
        url: '/api/underissue?ids='+ids,
        //data: JSON.stringify(ids),
        type: 'DELETE',
        contentType: 'application/json; charset=utf-8',
        complete: function () {

        },
        success: function (data) {
            finishedUIssues.remove();

        },
        error: function (error) {
        }
    });
});

/*--------------------------- Add new category btn click ----------------------------------------*/
$("body").on("click", "#add_category_btn", function () {
    var categoryName = $('#category_input').val();

    $.ajax({
        url: '/api/category?categoryName',
        data: JSON.stringify(categoryName),
        type: 'POST',
        contentType: 'application/json',
        complete: function () {

        },
        success: function (data) {
            $('#list-all').after("<li class='accepts-issues ui-droppable'><input type='radio' name='list_id' value='all' checked='checked'>" +
                "<a href='/Home/Index?categoryId=" + data.Id + "'>" +
                "<i class='icon-list'></i> " + categoryName +
                "<span class='glyphicon glyphicon-remove pull-right remove_category' data-id='" + data.Id + "'style='margin-top: 10px' aria-hidden='true'></span>" +
                "</a></li>");
        },
        error: function (error) {
        }
    });
});
/*--------------------------- Remove category btn click ----------------------------------------*/
$("body").on("click", ".remove_category", function (e) {

    var id = $(this).data('id');
    console.log('remove');

    $.ajax({
        url: '/api/category/' + id,
        //data: JSON.stringify({ 'id': id }),
        type: 'DELETE',
        contentType: 'application/json; charset=utf-8',
        complete: function () {

        },
        success: function (data) {
            window.location.href = "/Home/Index/";

        },
        error: function (error) {
        }
    });
});

/*--------------------------- Remove category btn click ----------------------------------------*/
$("body").on("click", "ul.side-men.side-lists li a", function (e) {
    console.log(e.target.is("a"));
    if (e.target.is("a")) {
        return;
    }
});