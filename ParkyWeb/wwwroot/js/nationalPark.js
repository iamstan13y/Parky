var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/nationalParks/GetAllNationalPark",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "name", "width": "50px" },
            { "data": "state", "width": "20px" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="nationalParks/Upsert/${data}" class='btn btn-success text-white'
                                    stlye='cursor:pointer;'> <i class='fa fa-edit'></i></a>
                                    &nbsp;
                                <a onclick="Delete(nationalParks/Delete/${data})" class='btn btn-danger text-white'
                                    stlye='cursor:pointer;'> <i class='fa fa-trash-alt'></i></a>
                                </div>
                                `;
                }, "width": "30%"
            }
        ]
    });
}