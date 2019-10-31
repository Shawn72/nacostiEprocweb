'use-strict';
$(function () {
   // var allDatatables = document.querySelectorAll(".dataTableX");

    $("#tbluploads").DataTable({
        "paging": true,
        "lengthChange": false,
        "searching": false,
        "ordering": true,
        "info": true,
        "autoWidth": false,
        "responsive": true,
        'destroy': true
    });
    
});

