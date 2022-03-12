var dTable;

$(document).ready(function () {
    dTable = $('#myTable').DataTable({
        "ajax": {
            "url": "/Admin/Product/AllProduct",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "productId" },
            { "data": "productName" },
            { "data": "description" },
            { "data": "price" }
        
        ]


    });
});