'use-strict';
function getSelectedPosta() {
    var selectedVal = $('#ddlallpostacodes').find(":selected").attr('value');
    var pp = document.getElementById('txtCity');
    $.ajax({
        type: "POST",
        url: "/Home/SelectedPosta",
        data: "postcode=" + selectedVal,
        success: function (status) {
            if (status != "notfound") {
                pp.style = "background-color: #d7f4d7";
                pp.setAttribute('value', status);
               // alert('Selected City: ' + status);
            }
            else {
                $('#txtCity').val('');
                pp.style = "background-color: #f2bfb6";
                alert('Please Select valid Postal code!');
                $("#loginMsg").css("display", "block");
                $("#loginMsg").css("color", "red");
                $("#loginMsg").html("Please choose a valid post code!");
            }
        }
    });
}

function IsValidEmail(email) {
   var expr = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/ig;
     return expr.test(email);
};

const validateEmail = (() => {
    var myemadd = $("#txtEmailAdd").val();
    var reg = /^([\w-\.]+@(?!gmail.com)(?!yahoo.com)(?!hotmail.com)(?!yahoo.co.in)(?!aol.com)(?!abc.com)(?!xyz.com)(?!pqr.com)(?!rediffmail.com)(?!live.com)(?!outlook.com)(?!me.com)(?!msn.com)(?!ymail.com)([\w-]+\.)+[\w-]{2,4})?$/ig;
    if (reg.test(myemadd)) {
        $("#loginMsg").css("display", "block");
        $("#loginMsg").css("color", "green");
        $('#loginMsg').attr("class", "alert alert-success");
        $("#loginMsg").html("Email acceptable!");
        return 0;
    }
    else {
        Swal.fire
        ({
            title: "Email Validation Error!",
            text: "Only Business Email Address is allowed!",
            type: "error",
            showCancelButton: true,
            closeOnConfirm: false,
            confirmButtonText: "Provide nice email!",
            confirmButtonClass: "btn-danger",
            confirmButtonColor: "#ec6c62",
            position: "center"
        }).then(() => {
            $("#loginMsg").css("display", "block");
            $("#loginMsg").css("color", "red");
            $('#loginMsg').attr("class", "alert alert-danger");
            $("#loginMsg").html("Only Business Email Address is allowed!");
        });
        return false;
    }
});



