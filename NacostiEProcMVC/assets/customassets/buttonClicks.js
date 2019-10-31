'use-strict';
//Login Ajax
$(document).ready(function () {

    //Ajax Start- This is to show ajax loading
    $(document).ajaxStart(function () {
        $("#gifLoading").css("display", "block");
    });

    $(document).ajaxStop(function() {
        $("#gifLoading").css("display", "none");
    });

    $(document).ajaxComplete(function () {
        $("#gifLoading").css("display", "none");
    });

    $('#btnLogin').click(function (e) {
        //To prevent form submit after ajax call
        e.preventDefault();
        //reset to empty
        $("#loginMsg").html("");
        //Set data to be sent
        var data = {
            "myUsername": $("#vendor_email").val(),
            "myPassword": $("#vendor_password").val()
        }
        Swal.fire({
            title: "Are you sure?",
            text: "Proceed to login?",
            type: "warning",
            showCancelButton: true,
            closeOnConfirm: true,
            confirmButtonText: "Yes, Log me in!",
            confirmButtonClass: "btn-success",
            confirmButtonColor: "#008000",
            position: "center"
        }).then((result) => {
            if (result.value) {
                $.ajax({
                    url: "/Home/CheckLogin",
                    type: "POST",
                    data: JSON.stringify(data),
                    dataType: "json",
                    contentType: "application/json"
                }).done(function (status) {
                    switch (status) {
                    case "InvalidLogin":
                        Swal.fire
                        ({
                            title: "Snaap!!",
                            text: "Invalid account details!",
                            type: "error"
                        }).then(() => {
                            $("#loginMsg").css("display", "block");
                            $("#loginMsg").css("color", "red");
                            $('#loginMsg').attr('class', 'alert alert-danger');
                            $("#loginMsg").html("Invalid account details!");
                            document.getElementById("btnLogin").disabled = false;
                        });
                        break;

                    case "UsernameEmpty":
                        Swal.fire
                        ({
                            title: "Snaap!!",
                            text: "Username field is empty, fill it first!",
                            type: "error"
                        }).then(() => {
                            $("#loginMsg").css("display", "block");
                            $("#loginMsg").css("color", "red");
                            $('#loginMsg').attr('class', 'alert alert-danger');
                            $("#loginMsg").html("Username field is empty, fill it first!");
                            document.getElementById("btnLogin").disabled = false;
                        });
                        break;
                    case "PasswordEmpty":
                        Swal.fire
                        ({
                            title: "Snaap!!",
                            text: "Password field is empty, fill it first!",
                            type: "error"
                        }).then(() => {
                            $("#loginMsg").css("display", "block");
                            $("#loginMsg").css("color", "red");
                            $('#loginMsg').attr('class', 'alert alert-danger');
                            $("#loginMsg").html("Password field is empty, fill it first!");
                            document.getElementById("btnLogin").disabled = false;
                        });
                        break;

                    case "PasswordMismatched":
                        Swal.fire
                        ({
                            title: "Snaap!!",
                            text: "Wrong Password!",
                            type: "error"
                        }).then(() => {
                            $("#loginMsg").css("display", "block");
                            $("#loginMsg").css("color", "red");
                            $('#loginMsg').attr('class', 'alert alert-danger');
                            $("#loginMsg").html("Wrong Password!");
                            document.getElementById("btnLogin").disabled = false;
                        });
                        break;

                    case "Loginadmin":
                        //now login admin
                        document.getElementById("btnLogin").disabled = true;
                        window.location.href = '/Home/AdminIndex';
                        break;

                    case "Logincustomer":
                        //now login customer
                        document.getElementById("btnLogin").disabled = true;
                        window.location.href = '/Home/Index';
                        break;
                    case "Logincontact":
                        //now login contact
                        document.getElementById("btnLogin").disabled = true;
                        window.location.href = '/Home/ContactsIndex';
                        break;

                    default :
                        Swal.fire
                        ({
                            title: "Snaap!!",
                            text: "Error Encountered!",
                            type: "error"
                        }).then(() => {
                            $("#loginMsg").css("display", "block");
                            $("#loginMsg").css("color", "red");
                            $('#loginMsg').attr('class', 'alert alert-danger');
                            $("#loginMsg").html(status);
                            document.getElementById("btnLogin").disabled = false;
                        });
                        break;
                    }

                });
            } else if (result.dismiss === Swal.DismissReason.cancel) {
                Swal.fire(
                    'Cancelled',
                    'You cancelled login process!',
                    'error'
                );
            }
        });
           
    });

    $('#btnRegister').click(function (e) {
        //To prevent form submit after ajax call
        e.preventDefault();

        //reset to empty
        $("#loginMsg").html("");
        var vendormodel = {};
        //Set data to be sent
        vendormodel.VendorName = $("#txtVendorName").val();
        vendormodel.Country = $("#ddlallcountries").find(":selected").attr('value');
        vendormodel.Address = $("#txtAddress").val();
        vendormodel.PostalCode = $('#ddlallpostacodes').find(":selected").attr('value');
        vendormodel.City = $("#txtCity").val();
        vendormodel.Phonenumber = $("#txtPhonenumber").val();
        vendormodel.Taxpin = $("#txtTaxcomplpin").val();
        vendormodel.KraPin = $("#txtKRAPin").val();
        vendormodel.Email = $("#txtEmailAdd").val();
        vendormodel.Password1 = $("#txtPass1").val();
        vendormodel.Password2 = $("#txtPass2").val();

        Swal.fire({
            title: "Are you sure?",
            text: "Are you sure you'd like to proceed with account creation?",
            type: "warning",
            showCancelButton: true,
            closeOnConfirm: true,
            confirmButtonText: "Yes, Create Account!",
            confirmButtonClass: "btn-success",
            confirmButtonColor: "#008000",
            position: "center"
        }).then((result) => {
            if (result.value) {
                $.ajax({
                    url: "/Home/RegisterVendor",
                    type: "POST",
                    data: '{vendormodel: ' + JSON.stringify(vendormodel) + '}',
                    // data: JSON.stringify(data),
                    dataType: "json",
                    contentType: "application/json"
                }).done(function(status) {
                    switch (status) {
                    case "Your account created successfully!":
                        Swal.fire
                        ({
                            title: "Account Created!",
                            text: status,
                            type: "success"
                        }).then(() => {
                            $("#loginMsg").css("display", "block");
                            $("#loginMsg").css("color", "green");
                            $('#loginMsg').addClass("alert alert-success");
                            $("#loginMsg").html(status);
                        });
                        break;

                    case "VendorEmpty":
                        Swal.fire
                        ({
                            title: "Snaap!!",
                            text: "Vendor name cannot be empty!",
                            type: "error"
                        }).then(() => {
                            $("#loginMsg").css("display", "block");
                            $("#loginMsg").css("color", "red");
                            $('#loginMsg').attr('class', 'alert alert-danger');
                            $("#loginMsg").html("Vendor name cannot be empty!");
                        });
                        break;
                        case "PasswordMismatched":
                        Swal.fire
                        ({
                            title: "Snaap!!",
                            text: "Password mismatched, type again!",
                            type: "error"
                        }).then(() => {
                            $("#loginMsg").css("display", "block");
                            $("#loginMsg").css("color", "red");
                            $('#loginMsg').attr('class', 'alert alert-danger');
                            $("#loginMsg").html("Password mismatched, type again!");
                            $("#txtPass2").html('');
                            $("#txtPass1").html('');
                        });
                        break;
                    case "EmailEmpty":
                        Swal.fire
                        ({
                            title: "Snaap!!",
                            text: "Email Address cannot be empty!",
                            type: "error"
                        }).then(() => {
                            $("#loginMsg").css("display", "block");
                            $("#loginMsg").css("color", "red");
                            $('#loginMsg').attr('class', 'alert alert-danger');
                            $("#loginMsg").html("Email Address cannot be empty!");
                        });
                        break;
                    case "KRAEmpty":
                        Swal.fire
                        ({
                            title: "Snaap!!",
                            text: "KRA Pin cannot be empty!",
                            type: "error"
                        }).then(() => {
                            $("#loginMsg").css("display", "block");
                            $("#loginMsg").css("color", "red");
                            $('#loginMsg').addClass('alert alert-danger');
                            $("#loginMsg").html("KRA Pin cannot be empty!");
                        });
                        break;
                    case "PasswordEmpty":
                        Swal.fire
                        ({
                            title: "Snaap!!",
                            text: "Password field cannot empty!",
                            type: "error"
                        }).then(() => {
                            $("#loginMsg").css("display", "block");
                            $("#loginMsg").css("color", "red");
                            $('#loginMsg').attr('class', 'alert alert-danger');
                            $("#loginMsg").html("Password field cannot empty!");
                            $("#txtPass2").html('');
                            $("#txtPass1").html('');
                        });
                        break;
                    case "Password2Empty":
                        Swal.fire
                        ({
                            title: "Snaap!!",
                            text: "Password confirmation field cannot empty!",
                            type: "error"
                        }).then(() => {
                            $("#loginMsg").css("display", "block");
                            $("#loginMsg").css("color", "red");
                            $('#loginMsg').attr('class', 'alert alert-danger');
                            $("#loginMsg").html("Password confirmation field cannot empty!");
                            $("#txtPass2").html('');
                            $("#txtPass1").html('');
                        });
                        break;
                    default:
                        Swal.fire
                        ({
                            title: "Snaap!!",
                            text: status,
                            type: "error"
                        }).then(() => {
                            $("#loginMsg").css("display", "block");
                            $("#loginMsg").css("color", "red");
                            $('#loginMsg').addClass('alert alert-danger');
                            $("#loginMsg").html(status);
                        });
                        break;
                       }
                    }
                );
            } else if (result.dismiss === Swal.DismissReason.cancel) {
                Swal.fire(
                    'Cancelled',
                    'You cancelled your account creation!',
                    'error'
                );
            }
        });
    });
    
    $('#btnApplyPreQ').click(function (e) {
        //To prevent form submit after ajax call
        e.preventDefault();

        //reset to empty
        $("#feedbackMsg").html("");
       
        //Set data to be sent
        var data = {
            "selectedcategory": $("#ddlallsuppliercats").find(":selected").attr('value'),
            "selectedfyear": $("#ddlfiscalyear").find(":selected").attr('value')
        }

        Swal.fire({
            title: "Are you sure?",
            text: "Are you sure you'd like to proceed with submission?",
            type: "warning",
            showCancelButton: true,
            closeOnConfirm: true,
            confirmButtonText: "Yes, submit!",
            confirmButtonClass: "btn-success",
            confirmButtonColor: "#008000",
            position: "center"
        }).then((result) => {
            if (result.value) {
                $.ajax({
                    url: "/Home/ApplyforPreQualifc",
                    type: "POST",
                    data: JSON.stringify(data),
                    dataType: "json",
                    contentType: "application/json"
                }).done(function(status) {
                    switch (status) {   
                        case "Your registration for prequalification has been received!":
                            Swal.fire
                            ({
                                title: "Submitted!",
                                text: status,
                                type: "success"
                            }).then(() => {
                                $("#feedbackMsg").css("display", "block");
                                $("#feedbackMsg").css("color", "green");
                                $('#feedbackMsg').attr("class","alert alert-success");
                                $("#feedbackMsg").html(status);
                            });

                            break;
                        default:
                            Swal.fire
                            ({
                                title: "Snaap!!",
                                text: status,
                                type: "error"
                            }).then(() => {
                                $("#feedbackMsg").css("display", "block");
                                $("#feedbackMsg").css("color", "red");
                                $('#feedbackMsg').addClass('alert alert-danger');
                                $("#feedbackMsg").html(status);
                            });

                            break;
                        }
                    }
                );
            } else if (result.dismiss === Swal.DismissReason.cancel) {
                Swal.fire(
                    'Cancelled',
                    'You cancelled your submission!',
                    'error'
                );
            }
        });
    });

    $('#btnApplyPulicTender').click(function (e) {
        //To prevent form submit after ajax call
        e.preventDefault();

        //reset to empty
        $("#feedbackMsg").html("");

        //Set data to be sent
        var data = {
            "myBidamount": $("#txtBidamount").val(),
            "myTendorNo": $("#txtTendorNo").val()
        }

        Swal.fire({
            title: "Are you sure?",
            text: "Are you sure you'd like to proceed with submission?",
            type: "warning",
            showCancelButton: true,
            closeOnConfirm: true,
            confirmButtonText: "Yes, submit!",
            confirmButtonClass: "btn-success",
            confirmButtonColor: "#008000",
            position: "center"
        }).then((result) => {
            if (result.value) {
                $.ajax({
                    url: "/Home/SubmitTenderAppPublic",
                    type: "POST",
                    data: JSON.stringify(data),
                    dataType: "json",
                    contentType: "application/json"
                }).done(function (status) {
                        switch (status) {
                        case "submitted success":
                            Swal.fire
                            ({
                                title: "Submitted!",
                                text: "Your bid submitted successfully, Kindly upload documents below!",
                                type: "success"
                            }).then(() => {
                                $("#feedbackMsg").css("display", "block");
                                $("#feedbackMsg").css("color", "green");
                                $('#feedbackMsg').attr("class", "alert alert-success");
                                $("#feedbackMsg").html("Your bid submitted successfully, Kindly upload documents below!");
                                $("#uploadFilesDiv").show();
                            });

                            break;
                        case "BidamountEmpty":
                            Swal.fire
                            ({
                                title: "Snaap!!",
                                text: "Bid Amount Field is empty!",
                                type: "error"
                            }).then(() => {
                                $("#feedbackMsg").css("display", "block");
                                $("#feedbackMsg").css("color", "red");
                                $('#feedbackMsg').addClass('alert alert-danger');
                                $("#feedbackMsg").html("Bid amount cannot be empty");
                            });
                            break;
                        default:
                            Swal.fire
                            ({
                                title: "Snaap!!",
                                text: status,
                                type: "error"
                            }).then(() => {
                                $("#feedbackMsg").css("display", "block");
                                $("#feedbackMsg").css("color", "red");
                                $('#feedbackMsg').addClass('alert alert-danger');
                                $("#feedbackMsg").html(status);
                            });

                            break;
                        }
                    }
                );
            } else if (result.dismiss === Swal.DismissReason.cancel) {
                Swal.fire(
                    'Cancelled',
                    'You cancelled your submission!',
                    'error'
                );
            }
        });
    });

    $('#btnApplyTender').click(function (e) {
        //To prevent form submit after ajax call
        e.preventDefault();

        //reset to empty
        $("#feedbackMsg").html("");

        //Set data to be sent
        var data = {
            "myBidamount": $("#txtBidamount").val(),
            "myTendorNo": $("#txtTendorNo").val()
        }
        
        Swal.fire({
            title: "Are you sure?",
            text: "Are you sure you'd like to proceed with submission?",
            type: "warning",
            showCancelButton: true,
            closeOnConfirm: true,
            confirmButtonText: "Yes, submit!",
            confirmButtonClass: "btn-success",
            confirmButtonColor: "#008000",
            position: "center"
        }).then((result) => {
            if (result.value) {
                $.ajax({
                    url: "/Home/SubmitTenderApp",
                    type: "POST",
                    data: JSON.stringify(data),
                    dataType: "json",
                    contentType: "application/json"
                }).done(function (status) {
                        switch (status) {
                        case "submitted success":
                            Swal.fire
                            ({
                                title: "Submitted!",
                                text: "Your bid submitted successfully, Kindly upload documents below!",
                                type: "success"
                            }).then(() => {
                                $("#feedbackMsg").css("display", "block");
                                $("#feedbackMsg").css("color", "green");
                                $('#feedbackMsg').attr("class", "alert alert-success");
                                $("#feedbackMsg").html("Your bid submitted successfully, Kindly upload documents below!");
                                $("#uploadFilesDiv").show();
                            });

                            break;
                        case "BidamountEmpty":
                            Swal.fire
                            ({
                                title: "Snaap!!",
                                text: "Bid Amount Field is empty!",
                                type: "error"
                            }).then(() => {
                                $("#feedbackMsg").css("display", "block");
                                $("#feedbackMsg").css("color", "red");
                                $('#feedbackMsg').addClass('alert alert-danger');
                                $("#feedbackMsg").html("Bid amount cannot be empty");
                            });
                            break;
                        default:
                            Swal.fire
                            ({
                                title: "Snaap!!",
                                text: status,
                                type: "error"
                            }).then(() => {
                                $("#feedbackMsg").css("display", "block");
                                $("#feedbackMsg").css("color", "red");
                                $('#feedbackMsg').addClass('alert alert-danger');
                                $("#feedbackMsg").html(status);
                            });

                            break;
                        }
                    }
                );
            } else if (result.dismiss === Swal.DismissReason.cancel) {
                Swal.fire(
                    'Cancelled',
                    'You cancelled your submission!',
                    'error'
                );
            }
        });
    });

    //delete table row
    $("body").on("click", "#tbluploads .delete", function () {
        //reset to empty
        $("#feedbackMsg").html("");
        var row = $(this).closest("tr");
        var data = { "filepath": row.find("span").html() };

        Swal.fire({  
            title: "Are you sure?",  
            text: "Are you sure that you want to delete this file?",  
            type: "warning",  
            showCancelButton: true,  
            closeOnConfirm: false,  
            confirmButtonText: "Yes, delete it!",
            confirmButtonClass: "btn-success",
            confirmButtonColor: "#008000",
            position:"center"
        }).then((result) => {
                if (result.value) {
                    $.ajax({
                        type: "POST",
                        url: "/Home/DeleteFile",
                        data: JSON.stringify(data),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json"
                    }).done(function(status) {
                            switch (status) {
                            case "File Deleted":
                                Swal.fire
                                ({
                                    title: "Deleted!",
                                    text: "Your file was successfully deleted!",
                                    type: "success"
                                }).then(() => {
                                    $("#feedbackMsg").css("display", "block");
                                    $("#feedbackMsg").css("color", "green");
                                    $('#feedbackMsg').addClass("alert alert-success");
                                    $("#feedbackMsg").html(status + " successfully!");
                                    row.remove();
                                });
                                break;
                            default:
                                Swal.fire
                                ({
                                    title: "Error!",
                                    text: status,
                                    type: "error"
                                }).then(() => {
                                    $("#feedbackMsg").css("display", "block");
                                    $("#feedbackMsg").css("color", "red");
                                    $('#feedbackMsg').addClass('alert alert-danger');
                                    $("#feedbackMsg").html(status);
                                });
                                break;
                            }
                        }
                    );

                } else if (result.dismiss === Swal.DismissReason.cancel) {
                    Swal.fire(
                        'Cancelled',
                        'You cancelled deletion, your file is safe for now, not deleted!',
                        'error'
                    );
                }
            }
        );
    });

    
});