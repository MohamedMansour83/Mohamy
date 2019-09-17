
function SessionViewModel() {
    var self = this;
	self.VM = new CommonViewModel();
	self.baseUrl = self.VM.baseURL();
    self.name = ko.observable("");
    self.email = ko.observable("");
	self.age = ko.observable("");
	self.sessions = ko.observableArray([]);
	self.txtMessage = ko.observable("");
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
                    self.VM.alert($("#SessionFinished").val());
                    self.txtMessage($("#SessionFinished").val());
					return;
				}
				if (!data.isStarted) {
                    self.VM.alert($("#SessionNotStarted").val());
                    self.txtMessage($("#SessionNotStarted").val());
					return;
				}

                var isConfirm = confirm($("#StartTheSessionAlert").val());
                 // //console.log('alo1');
				if (isConfirm) {
					startSession(data.sessionId);
				}
            },
            error: function (res) {
                localStorage.setItem('returnURL', window.location.pathname);
				self.VM.handleErrorResponse(res);
            },
			beforeSend: self.VM.setHeader   
        });
	}


	function startSession(id) {
		var url = self.baseUrl + "/api/SessionApi/StartSession/" + id;
		$.ajax({
			type: "GET",
			url: url,
			contentType: "application/json",
			success: function (data) {
				if (data.isStarted) {
					window.location.href = self.baseUrl + "/Session/Call/" + id;
				} else {
                    self.VM.alert("Something wrong.");
				}
			},
			error: function (res) {
				self.VM.handleErrorResponse(res);
			},
			beforeSend: self.VM.setHeader
		});
	}

	self.openSession = function (data) {
		alert(data.id);
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
ko.applyBindings(new SessionViewModel(), document.getElementById('bdy'));