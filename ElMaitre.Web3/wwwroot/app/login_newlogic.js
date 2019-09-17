var self1;
function getQuerystring(key, default_) {
    if (default_ === null) default_ = "";
    key = key.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regex = new RegExp("[\\?&]" + key + "=([^&#]*)");
    var qs = regex.exec(window.location.href);
    if (qs === null)
        return default_;
    else
        return qs[1];
}

function AccountViewModel() {
    let mobRegex = /^([0|\+[0-9]{1,5})?([7-9][0-9]{9})$/;
    var self = this;
    self1 = self;

    self.baseUrl = getBaseURL();
    self.loginModel = { UserName: "", Password: "" };
    self.registerModel = {
        UserName: localStorage.getItem('p2'), Email: localStorage.getItem('p3'),
        Password: localStorage.getItem('p4'), ConfirmPassword: "", Gender: false,
        AcceptTerms: true, IsLawyer: false,
        PhoneNumber: "", FbId: localStorage.getItem('p1')
    };
    // //console.log(self.registerModel);

    function Token(e) {
        //$.post(baseUrl + "/api/Auth/Token", function (data) {
        //    self.List(data);
        //})

        localStorage.removeItem("token");
        localStorage.removeItem("UsrName");
        localStorage.removeItem("isLawyer");

        // //console.log(ko.toJSON(self.loginModel));
        $.ajax({
            type: "POST",
            url: self.baseUrl + "/Account/Login",
            data: ko.toJSON(self.loginModel),
            contentType: "application/json",
            success: function (data) {
                localStorage.setItem('token', data.token);
                localStorage.setItem('UsrName', self.loginModel.UserName);
                localStorage.setItem('isLawyer', data.isLawyer);
                if (localStorage.getItem("returnURL") !== null) {
                    window.location = self.baseUrl + localStorage.getItem("returnURL");
                    localStorage.removeItem("returnURL");
                } else
                    window.location = self.baseUrl + "/";

            },
            error: function (res) {
                if (res.status == 401) {
                    $(".modal-body #message").text(res.responseText);
                    $('#alert').modal('show');
                } else
                    if (res.status == 400) {
                        $(".modal-body #message").text(res.responseJSON[0]);
                        $('#alert').modal('show');
                    }
                hideloading(e);
            }
        });
    }

    self.Login = function (el, e) {
        showloading(e);
        Token(e);
    };

    self.Register = function (el, e) {
         
        $('.reg-mobileempty').css('display', 'none');
        $('.reg-mobile').css('display', 'none');
        $('.reg-CPasswordempty').css('display', 'none');
        $('.reg-Date').css('display', 'none');
        if (!self.registerModel.PhoneNumber)
        {
            $('.reg-mobileempty').css('display', '');
        }

        else if (self.registerModel.PhoneNumber[0] != '0' && self.registerModel.PhoneNumber[0] != '1') {
            $('.reg-mobileempty').css('display', 'none');
            $('.reg-mobile').css('display', '');
        }
        else if (!self.registerModel.ConfirmPassword) {
            $('.reg-CPasswordempty').css('display', '');
        }
        else if (self.registerModel.ConfirmPassword != self.registerModel.Password) {
            $(".modal-body #message").text("تأكيد كلمة المرور لا تطابق مع كلمة المرور");
            $('#alert').modal('show');
            return;
        }
        //else if (!self.registerModel.DateOfBirth) {
        //    $('.reg-Date').css('display', '');
        //}

        else if (!self.registerModel.AcceptTerms) {
            $(".modal-body #message").text("Please accept terms");
            $('#alert').modal('show');
            return;
        }
        else {

            showloading(e);
            localStorage.removeItem("token");
            localStorage.removeItem("UsrName");
            localStorage.removeItem("isLawyer");
            localStorage.removeItem("returnURL");

            // // //console.log(ko.toJSON(self.registerModel));

            var fbIdChk = self.registerModel.FbId;
            $.ajax({
                type: "POST",
                url: self.baseUrl + "/api/Auth/Register",
                data: ko.toJSON(self.registerModel),
                contentType: "application/json",
                success: function (data) {
                    self.datamodel = { UserName: self.registerModel.Email, Password: self.registerModel.Password, FbId: self.registerModel.FbId };
                    $(".modal-body #message").text(data.message);
                    $('#alert').modal('show');
                    $('#ok_btn').click(function () {
                        hideloading(e);
                        $.ajax({
                            type: "POST",
                            url: self.baseUrl + "/Account/login",
                            data: ko.toJSON(self.datamodel),
                            contentType: "application/json",
                            success: function (data) {
                                window.location = self.baseUrl + "/Home/index";
                            },
                            error: function (res) {

                            }
                        });
                    });

                    //    if (fbIdChk !== '') {
                    //        localStorage.setItem('token', data.token);
                    //        localStorage.setItem('UsrName', self.registerModel.UserName);
                    //        localStorage.setItem('isLawyer', data.isLawyer);
                    //        window.location = self.baseUrl + "/";
                    //    }
                    //});
                    //window.location = self.baseUrl+"Account/Login";
                    //$("#home").addClass("active");
                    //$("#register").removeClass("active");
                    //$("#h").addClass("active show");
                    //$("#reg").removeClass("active show");
                  
                },
                error: function (res) {
                    $(".modal-body #message").text(res.responseText);
                    $('#alert').modal('show');
                    hideloading(e);
                }
            });
        }

    };

    self.ForgotPassword = function () {
        $.ajax({
            type: "POST",
            url: self.baseUrl + "/api/Auth/ForgotPassword",
            data: ko.toJSON(self.loginModel),
            contentType: "application/json",
            success: function (data) {
                $(".modal-body #message").text(data.message);
                $('#alert').modal('show');
                //window.location = self.baseUrl + "/Account/Login";
            },
            error: function (res) {
                alert(getErrorMessage(res));
            }
        });
    }

    function getBaseURL() {
        return location.protocol + "//" + location.hostname +
            (location.port && ":" + location.port);
    }

    function getErrorMessage(res) {
        return res.responseJSON[0];
    }

    function showloading(e) {
        $(e.target).attr("disabled", true);
        $(e.target.children[0]).removeClass("d-none");
    }
    function hideloading(e) {
        $(e.target).attr("disabled", false);
        $(e.target.children[0]).addClass("d-none");
    }
};
//ko.applyBindings(new AccountViewModel());
ko.applyBindings(new AccountViewModel(), document.getElementById('bdy'));