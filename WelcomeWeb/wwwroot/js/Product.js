var dTable;

$(document).ready(function () {
    dTable = $('#myTable').DataTable({
        "ajax": {
            url:"Admin/Product/AllProduct"
        },
        "columns": {
            "data": ""
        }


    });
});