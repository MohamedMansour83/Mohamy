
function SuccessBookingViewModel() {
    var self = this;
    self.VM = new CommonViewModel();
	self.baseUrl = self.VM.baseURL();
    self.Note = ko.observable("");
    localStorage.removeItem("returnURL");
    //$('#alert').modal('show');
    function sendNote() {
        var pathArray = window.location.pathname.split('/');
        var url = self.baseUrl + "/api/LawyerApi/SendNote";
        var sID = parseInt($("#sessionId").html());
        var model = {
            "SessionId": sID,
            "Note": self.Note()
        }

        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json",
            data: JSON.stringify(model),
			success: function (data) {
                self.VM.alert(data.message);
                self.Note("");
                window.location.href = self.baseUrl + "/Home/index";
            },
            error: function (res) {
				self.VM.handleErrorResponse(res);
            },
			beforeSend: self.VM.setHeader
        });
	}
    self.SendNote = function () {
        sendNote();
    }
};
ko.applyBindings(new SuccessBookingViewModel(), document.getElementById('bdy'));