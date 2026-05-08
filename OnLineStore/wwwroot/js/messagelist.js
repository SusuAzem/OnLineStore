$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    $("#messages").DataTable({
        ajax: {
            url: "/Admin/Messages/GetAll",
        },
        columns: [
            { data: "name", width: "25%" },
            { data: "email", width: "25%" },
            { data: "phoneNumber", width: "25%" },
            { data: "message", width: "25%" },
            {
                data: "timeSend",
                render: function (data, type, announcement) {
                    return announcement.timeSend.format(
                        "dddd D MMMM YYYY hh:mm:ss"
                    );
                },
            },
            {
                data: "id",
                render: function (data) {
                    return `
                            <a onclick=Delete("/Admin/Messages/Delete/${data}") class="btn" data-bs-toggle="modal" data-bs-target="#Modal">
                                <i class="fa fa-trash"></i> &nbsp;
                            </a>
                            </div>`;
                },
                width: "20%",
            },
        ],
    });
}

//function Delete(url) {
//    swal({
//        title: "Are you sure you want to Delete?",
//        text: "You will not be able to restore the data!",
//        icon: "warning",
//        buttons: true,
//        dangerMode: true
//    }).then((willDelete) => {
//        if (willDelete) {
//            $.ajax({
//                type: "DELETE",
//                url: url,
//                success: function (data) {
//                    if (data.success) {
//                        toastr.success(data.message);
//                        dataTable.ajax.reload();
//                    } else {
//                        toastr.error(data.message);
//                    }
//                }
//            });
//        }
//    });
//}
