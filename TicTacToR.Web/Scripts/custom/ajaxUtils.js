/// <reference path="../jquery-1.7.1.js" />

$(function ()
{
    $.ajaxSetup({ cache: false });
});

function ajaxAdd(url, dataToSave, callback)
{
    ajaxModify(url, dataToSave, "POST", "Added.", callback);
}

function ajaxUpdate(url, dataToSave, successCallback)
{
    ajaxModify(url, dataToSave, "PUT", "Updated.", successCallback);
}

function ajaxDelete(url)
{
    ajaxModify(url, null, "DELETE", "Deleted.");
}

function ajaxModify(url, dataToSave, httpVerb, successMessage, callback)
{
    $.ajax(url, {
        data: dataToSave,
        type: httpVerb,
        dataType: 'json',
        contentType: 'application/json',
        success: function (data)
        {
            if (callback !== undefined)
            {
                callback(data);
            }
        },
        error: function (data)
        {
            console.log(arguments[1] + arguments[2]);
        }
    });
}