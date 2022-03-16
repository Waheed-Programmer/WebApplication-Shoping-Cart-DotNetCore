var dTable;
$(document).ready(function () {
    dTable = $('#myTable').DataTable({
        "ajax": {
            "url": "/Admin/Order/AllOrders",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "name" },
            { "data": "orderStatus" },
            { "data": "phon" },
            { "data": "orderTotal" },
            {
                "data": "OrderHeaderId",
                "render": function (data) {
                    return `                    
                    <a href="/Admin/Order/OrderDetails?id=${data}"><i class="bi bi-pencil-square"></i></a>                      
                   `
                }
            },
        
        ]
    });
});
