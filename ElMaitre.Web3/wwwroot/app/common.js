
function CommonViewModel() {
    var self = this;
    self.hasNotification = ko.observable(false);
    self.isLawyer = ko.observable(false);
    self.notificationTtime = ko.observable("");
    self.notification = ko.observable("");
    self.contractCategories = ko.observableArray([]);
    self.QACategories = ko.observableArray([]);
    self.handleErrorResponse = function (res) {
        debugger
        if (res.status == 401)
            window.location.href = self.baseURL() + "/Account/Login";
        else
            if (res.status == 400) {
                self.alert(res.responseJSON[0]);
            } else
                if (res.status == 403)
                    window.location.href = self.baseURL() + "/Account/Login";
    }

	self.setHeader= function setHeader(xhr) {
		xhr.setRequestHeader('Authorization', 'bearer ' + localStorage.getItem('token'));
	}

    self.baseURL = function () {
        //return location.protocol + "//" + location.hostname +
        //    (location.port && ":" + location.port);
        return "http:" + "//" + location.hostname +
            (location.port && ":" + location.port);
    };

	self.setCurrency = function (fees) {
		return fees;
    }

    self.isLawyer2 = function () {
        return self.isLawyer();
    }

    self.alert = function (message) {
        $(".modal-body #message").text(message);
        $('#alert').modal('show');
    }

    self.confirm = function (message,callback) {
        $(".modal-body #message-confirm").text(message);
        $('#alert-confirm').modal('show');
        $("#btn-confirm-ok").click(function () {
            callback();
            $('#alert-confirm').modal('hide');
        });
    }
    self.renderCountDown = function (Rtime , lang) {
        if (lang == "en") {
            $("#countdown").countdown(new Date(Rtime), function (event) {
                $(this).html(
                    event.strftime(
                        '<div class="timer-wrapper"><div class="time">%D</div><span class="text">days</span></div><div class="timer-wrapper"><div class="time">%H</div><span class="text">hrs</span></div><div class="timer-wrapper"><div class="time">%M</div><span class="text">mins</span></div><div class="timer-wrapper"><div class="time">%S</div><span class="text">sec</span></div>'
                    )
                );
            });
            $("#countdown2").countdown(new Date(Rtime), function (event) {
                $(this).html(
                    event.strftime(
                        '<div class="timer-wrapper"><div class="time">%D</div><span class="text">days</span></div><div class="timer-wrapper"><div class="time">%H</div><span class="text">hrs</span></div><div class="timer-wrapper"><div class="time">%M</div><span class="text">mins</span></div><div class="timer-wrapper"><div class="time">%S</div><span class="text">sec</span></div>'
                    )
                );
            });
        }
        else {
            $("#countdown").countdown(new Date(Rtime), function (event) {
                $(this).html(
                    event.strftime(
                        '<div class="timer-wrapper"><div class="time">%D</div><span class="text">يوم</span></div><div class="timer-wrapper"><div class="time">%H</div><span class="text">ساعه</span></div><div class="timer-wrapper"><div class="time">%M</div><span class="text">دقيقه</span></div><div class="timer-wrapper"><div class="time">%S</div><span class="text">ثانيه</span></div>'
                    )
                );
            });
            $("#countdown2").countdown(new Date(Rtime), function (event) {
                $(this).html(
                    event.strftime(
                        '<div class="timer-wrapper"><div class="time">%D</div><span class="text">يوم</span></div><div class="timer-wrapper"><div class="time">%H</div><span class="text">ساعه</span></div><div class="timer-wrapper"><div class="time">%M</div><span class="text">دقيقه</span></div><div class="timer-wrapper"><div class="time">%S</div><span class="text">ثانيه</span></div>'
                    )
                );
            });

        }
       
    }
    self.showloading =function (e) {
        $(e.target).attr("disabled", true);
        $(e.target.children[0]).removeClass("d-none");
    }
    self.hideloading =function (e) {
        $(e.target).attr("disabled", false);
        $(e.target.children[0]).addClass("d-none");
    }
    //self.getUserNotifications();
    //getContractCategories();
    //getQACategories();

    self.getUserNotifications = function () {
        debugger;
        if (localStorage.getItem('token') !== null) {
            if (localStorage.getItem('isLawyer') === null || localStorage.getItem('isLawyer') === "false") {
                self.isLawyer(false);
                $.ajax({
                    type: "GET",
                    url: self.baseURL() + "/api/UserApi/GetPendingLiveSesstions",
                    contentType: "application/json",
                    success: function (data) {
                         // console.log(data);
                        if (data != "") {
                            self.notification(data);
                            self.hasNotification(true);
                            self.renderCountDown(self.notification().sessionTime, self.notification().lang);
                        }
                    },
                    error: function (res) {
                        //self.handleErrorResponse(res);
                    },
                    beforeSend: self.setHeader
                });
            }
            else {
                self.isLawyer(true);
                $.ajax({
                    type: "GET",
                    url: self.baseURL() + "/api/LawyerApi/GetLawyerPendingLiveSesstions",
                    contentType: "application/json",
                    success: function (data) {
                         // console.log(data);
                        if (data != "") {
                            self.notification(data);
                            self.hasNotification(true);
                            self.renderCountDown(self.notification().sessionTime, self.notification().lang);
                        }
                    },
                    error: function (res) {
                        //self.handleErrorResponse(res);
                    },
                    beforeSend: self.setHeader
                });
            }
        }
    }

    function getContractCategories(){

        $.ajax({
            type: "GET",
            url: self.baseURL() + "/api/HomeApi/GetContractCategories",
            contentType: "application/json",
            success: function (data) {
                self.contractCategories(data);
            },
            error: function (res) {
                self.handleErrorResponse(res);
            },
            beforeSend: self.setHeader
        });
    }

    function getQACategories() {

        if (!localStorage.getItem('isLawyer') === "true")
            return;


        $.ajax({
            type: "GET",
            url: self.baseURL() + "/api/HomeApi/GetQuestionCategories",
            contentType: "application/json",
            success: function (data) {
                self.QACategories(data);
            },
            error: function (res) {
                self.handleErrorResponse(res);
            },
            beforeSend: self.setHeader
        });
    }

    self.openSession = function () {
        
        ////window.location = self.baseURL() + "/Session/Index/" + self.notification().sessionId;
        window.location = "http://videomohamy.com/?id=" + self.notification().sessionId;
        //window.location = "http://localhost:4200/?id=" + self.notification().sessionId;

    }


    function secondsTimeSpanToHMS(s) {
        var h = Math.floor(s / 3600); //Get whole hours
        s -= h * 3600;
        var m = Math.floor(s / 60); //Get remaining minutes
        s -= m * 60;
        return h + ":" + (m < 10 ? '0' + m : m) + ":" + (s < 10 ? '0' + s : s); //zero padding on minutes and seconds
    }

};


ko.applyBindings(new CommonViewModel(), document.getElementById('hdr'));