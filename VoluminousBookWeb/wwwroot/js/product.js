var dataTable;

$(document).ready(function () {
    LoadDataTable();
});

function LoadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url":"/Admin/Product/GetAll"
        },
        "columns": [
            { "data": "title", "Width": "15%" },
            { "data": "isbn", "Width": "15%" },
            { "data": "price", "Width": "15%" },
            { "data": "author", "Width": "15%" },
            { "data": "category.name", "Width": "15%" },
            
        ]
    });
}