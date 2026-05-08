$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("inprocess")) {
        loadDataTable("inprocess");
    } else {
        if (url.includes("completed")) {
            loadDataTable("completed");
        } else {
            if (url.includes("pending")) {
                loadDataTable("pending");
            } else {
                if (url.includes("approved")) {
                    loadDataTable("approved");
                } else {
                    loadDataTable("all");
                }
            }
        }
    }
});

function loadDataTable(status) {
    $("#tblData").DataTable({
        ajax: {
            url: "/Admin/Order/GetAll?status=" + status,
        },
        columns: [
            { data: "id", width: "5%" },
            { data: "name", width: "20%" },
            { data: "orderStatus", width: "20%" },
            { data: "orderTotal", width: "15%" },
            {
                data: "id",
                render: function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                        <a href="/Admin/Order/Details?orderId=${data}"
                        class="btn mx-2"> <i class="fa fa-pencil-square-o"></i></a>                    
					</div>
                        `;
                },
                width: "5%",
            },
        ],
    });
}
