//---------------Thêm mới
function ThemMoi(action, controller) {
    $.ajax({
        type: 'GET',
        url: '/Admin/' + controller + '/' + action + '',
        success: function (data) {
            $('#modalcontent').html(data);
            $('#myModal').modal('show');
        }
    });
};
//--------------Xóa
function Xoa(action, controller, Id) {
    swal({
        title: "Bạn có chắc chắn muốn xóa không?",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "OK",
        cancelButtonText: "Cancel",
        closeOnConfirm: false,
        closeOnCancel: true
    },
        function (isConfirm) {
            if (isConfirm) {
                funcXoa(action, controller, Id);
            }
        });
};
function funcXoa(action, controller, Id) {
    $.ajax({
        type: "POST",
        url: '/Admin/' + controller + '/' + action + '',
        data: { id: Id },
        ajaxasync: true,
        success: function () {
            swal("Thành công", "Đã xóa thành công!", "success");
            $("#MyDataTable").DataTable();
            window.location.reload();
        },
        error: function () {
            swal("Error", "Có lỗi trong quá trình xóa", "error");
        }
    });
};
//---------------Xem chi tiết
function Xem(action, controller, Id) {
    $.ajax({
        type: 'GET',
        data: { id: Id },
        url: '/Admin/' + controller + '/' + action + '',
        success: function (data) {
            $('#modalcontent').html(data);
            $('#myModal').modal('show');
        },
        error: function () {
            swal("Error", "Có lỗi khi xem chi tiết", "error");
        }
    });
};

function Sua(action, controller, Id) {
    $.ajax({
        type: 'GET',
        data: { id: Id },
        url: '/Admin/' + controller + '/' + action + '',
        success: function (data) {
            $('#modalcontent').html(data);
            $('#myModal').modal('show');
        }
    });

};
