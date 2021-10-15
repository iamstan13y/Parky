﻿var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/trails/GetAllTrail",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "nationalPark.name", "width": "25px" },
            { "data": "name", "width": "20px" },
            { "data": "distance", "width": "15px" },
            { "data": "elevation", "width": "15px" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="trails/Upsert/${data}" class='btn btn-success text-white'
                                    stlye='cursor:pointer;'> <i class='fa fa-edit'></i></a>
                                    &nbsp;
                                <a onclick=Delete("/trails/Delete/${data}") class='btn btn-danger text-white'
                                    stlye='cursor:pointer;'> <i class='fa fa-trash-alt'></i></a>
                                </div>
                                `;
                }, "width": "30%"
            }
        ]
    });
}

function Delete(url) {
    swal({
        title: "Are you sure you want to delete?",
        text: "You will not be able to recover data.",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: 'DELETE',
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}