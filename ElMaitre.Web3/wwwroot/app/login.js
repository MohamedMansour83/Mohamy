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

function isEmpty(str) {
    return (!str || 0 === str.length);
}

function AccountViewModel() {
    var self = this;
    self1 = self;

    self.baseUrl = getBaseURL();
    let PaswordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;
    let EmailRegex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    let usernameRegex = /[ !#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/;

    self.loginModel = { UserName: "", Password: "" };
    self.registerModel = {
        UserName: "", Email: "", Password: "", ConfirmPassword: "",
        Gender: false, DateOfBirth: "", AcceptTerms: false, IsLawyer: false,
        PhoneNumber: "", FbId: ""
    };
    // console.log(self.registerModel);

    function Token(e) {
        //$.post(baseUrl + "/api/Auth/Token", function (data) {
        //    self.List(data);
        //})

        localStorage.removeItem("token");
        localStorage.removeItem("UsrName");
        localStorage.removeItem("isLawyer");

        // console.log(ko.toJSON(self.loginModel));
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
        debugger;
        if (isEmpty(self.registerModel.Email)) {
            $(".modal-body #message").text("من فضلك ادخل البريد الالكترزنى");
            $('#alert').modal('show');
            return;
        }
        else if (isEmpty(self.registerModel.Password)) {
            $(".modal-body #message").text("من فضلك ادخل كلمة السر");
            $('#alert').modal('show');
            return;
        }
        else if (!self.registerModel.AcceptTerms) {
            $(".modal-body #message").text("من فضلك اقبل الشروط و الاحكام");
            $('#alert').modal('show');
            return;
        }
        else if (!EmailRegex.test(self.registerModel.Email)) {
            $('.reg-Email').css('display', '');
            $('.reg-password').css('display', 'none');
        }
        else if (!PaswordRegex.test(self.registerModel.Password)) {
            $('.reg-Email').css('display', 'none');
            $('.reg-password').css('display', '');
        }

        else {
            showloading(e);
            localStorage.removeItem("token");
            localStorage.removeItem("UsrName");
            localStorage.removeItem("isLawyer");
            localStorage.removeItem("returnURL");
            var fbIdChk = self.registerModel.FbId;
            debugger;
            let name = "";
            let str = self.registerModel.Email;
            for (var i = 0; i < str.length; i++) {
                if (usernameRegex.test(str[i])) {
                    str = str.replace(str[i], '');
                }
            }
           
            localStorage.setItem('p2', str.substring(0,str.indexOf("@")));
            localStorage.setItem('p1', fbIdChk);
            localStorage.setItem('p3', self.registerModel.Email);
            localStorage.setItem('p4', self.registerModel.Password);
            window.location = self1.baseUrl + "/Account/RegisterStep2";
        }

        //     $.ajax({
        //         type: "POST",
        //         url: self.baseUrl + "/api/Auth/Register",
        //         data: ko.toJSON(self.registerModel),
        //         contentType: "application/json",
        //success: function (data) {
        //             $(".modal-body #message").text(data.message);
        //             $('#alert').modal('show');
        //             $('#ok_btn').click(function () {
        //                 if (fbIdChk !== '') {
        //                     localStorage.setItem('token', data.token);
        //                     localStorage.setItem('UsrName', self.registerModel.UserName);
        //                     localStorage.setItem('isLawyer', data.isLawyer);

        //                      // console.log(localStorage);
        //                      // console.log(data);

        //                     window.location = self.baseUrl + "/";
        //                 }
        //             });
        //             //window.location = self.baseUrl+"Account/Login";
        //             $("#home").addClass("active");
        //             $("#register").removeClass("active");
        //             $("#h").addClass("active show");
        //             $("#reg").removeClass("active show");

        //             hideloading(e);

        //         },
        //         error: function (res) {
        //             $(".modal-body #message").text(getErrorMessage(res));
        //             $('#alert').modal('show');
        //             hideloading(e);
        //         }
        //     });
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