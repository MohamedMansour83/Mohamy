
function SessionCallViewModel() {
    var self = this;
	self.VM = new CommonViewModel();
	self.baseUrl = self.VM.baseURL();
    self.userName = ko.observable("");
    self.name = ko.observable("");
    self.email = ko.observable("");
	self.age = ko.observable("");
	self.sessions = ko.observableArray([]);
	self.rate = ko.observable(0);
	self.reviewText = ko.observable("");
    self.IsLawyer = false;
	GetSession();

	function GetSession() {
		var pathArray = window.location.pathname.split('/');
		var url = self.baseUrl + "/api/SessionApi/GetSession/" + pathArray[pathArray.length - 1];
        $.ajax({
            type: "GET",
            url: url,
            contentType: "application/json",
			success: function (data) {
				if (data.isFinished) {
                    self.VM.alert("Session has been finished.");
					self.txtMessage("Session has been finished.");
					window.location.href = self.baseUrl + "/Session/Index/" + pathArray[pathArray.length - 1];
					return;
				}
				if (!data.isStarted) {
                    self.VM.alert("Session not started yet.");
					self.txtMessage("Session not started yet.");
					window.location.href = self.baseUrl + "/Session/Index/" + pathArray[pathArray.length - 1];
					return;
                }
                self.userName(data.userName);
                self.IsLawyer = !data.isGeust;
    //            setTimeout(function () {
    //                 
				//	confirm("Session has been finished.");
				//	if (data.isGeust) {
				//		$('#myModal').modal('show');
				//		$(".inner_pages").html("");
				//	} else {
				//		window.location.href = self.baseUrl + "/Account/LawyerProfile";
				//	}
				//}, 10000);
            },
            error: function (res) {
				self.VM.handleErrorResponse(res);
            },
			beforeSend: self.VM.setHeader
        });
	}


	self.changeRate = function (c, event) {
		var el = event.target;
		$(el.parentElement.children).removeClass("active");
		var r = parseInt($(el).data("value"));
		for (var i = 0; i < r; i++) {
			$(el.parentElement.children[i]).addClass("active");
		}
		self.rate(r);
		return true;
	}

	self.Save = function () {
		var pathArray = window.location.pathname.split('/');

		var url = self.baseUrl + "/api/SessionApi/SendRate";

		var model = {
			SessionId: pathArray[pathArray.length - 1],
			Rate: self.rate(),
			Review: self.reviewText()
		}

		$.ajax({
			type: "POST",
			url: url,
			data: ko.toJSON(model),
			contentType: "application/json",
			success: function (data) {
				window.location.href = self.baseUrl + "/Account/UserProfile";
			},
			error: function (res) {
				self.VM.handleErrorResponse(res);
			},
			beforeSend: self.VM.setHeader
		});
	}

    self.Close = function () {
        if (self.IsLawyer)
            window.location.href = self.baseUrl + "/Account/LawyerProfile";
        else
            window.location.href = self.baseUrl + "/Account/UserProfile";
    }

	self.EndSession = function () {
		var pathArray = window.location.pathname.split('/');

		var url = self.baseUrl + "/api/SessionApi/EndSession/" + pathArray[pathArray.length - 1];

		$.ajax({
			type: "GET",
			url: url,
			contentType: "application/json",
            success: function (data) {
                $('#notify-modal').modal('hide');

				if (data.isGeust) {
					$('#myModal').modal('show');
					$(".inner_pages").html("");
				} else {
					window.location.href = self.baseUrl + "/Account/LawyerProfile";
				}
			},
			error: function (res) {
				self.VM.handleErrorResponse(res);
			},
			beforeSend: self.VM.setHeader
		});
    }



    self.UploadFile = function (f) {
        if (jQuery("#file").val()) {
            var file = $("#file").get(0).files[0];
            var pathArray = window.location.pathname.split('/');
            var sessionId = pathArray[pathArray.length - 1];


            var data = new FormData();
            data.append("file", file);
            data.append("sessionId", sessionId);


            $.ajax({
                type: "POST",
                url: self.baseUrl + "/api/SessionApi/UploadFile",
                contentType: false,
                processData: false,
                data: data,
                success: function (data) {
                    //alert("done");
                },
                error: function (res) {
                    self.VM.handleErrorResponse(res);
                },
                beforeSend: self.VM.setHeader
            });

        }

    }
	function getFormatedDate(date) {
		var d = new Date(date);
		var day = d.getDate();
		var month = d.getMonth() + 1;
		var year = d.getFullYear();

		if (month < 10)
			month = "0" + month;

		if (day < 10)
			day = "0" + day;

		return year + "-" + month + "-" + day;
	}

	
};
ko.applyBindings(new SessionCallViewModel(), document.getElementById('bdy'));