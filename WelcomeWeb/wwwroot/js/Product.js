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
            { "data": "price" },
            { "data": "category.categoryName" },
            {
                "data": "productId",
                "render": function (data) {
                    return `                    
                    <a href="/Admin/Product/CreateUpdate?productId=${data}"><i class="bi bi-pencil-square"></i></a>
                      
                    <a onclick=Delete('/JqueryCrud/DeleteByDataApiJson?productId=' + ${ data })><i class="bi bi-trash3-fill"></i></a >
                       
                       
                    `
                }
            },
        
        ]


    });
});