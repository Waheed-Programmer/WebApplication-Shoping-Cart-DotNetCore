var dTable;

$(document).ready(function () {
    dTable = $('#myTable').DataTable({
        "ajax": {
            "url": "/Admin/Product/AllProduct",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "productName" },
            { "data": "description" },
            { "data": "price" },
            { "data": "category.categoryName" },
            {
                "data": "productId",
                "render": function (data) {
                    return `                    
                    <a href="/Admin/Product/CreateUpdate?id=${data}"><i class="bi bi-pencil-square"></i></a>
                    <a onclick=DeleteProduct("/Admin/Product/Delete"${data})><i class="bi bi-trash3-fill"></i></a>
                       
                    `
                }
            },
        
        ]
    });
});

function Delete(url) {


    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dTable.ajax.reload();

                        //alertify.success(data.message);
                        Swal.fire(
                            'Deleted!',
                            data.message
                        )
                    } else {
                        alertify.warning(data.message);

                    }
                }
            })
        }
    })
}