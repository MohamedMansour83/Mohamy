// js of Booking.cshtml
function LawyerBookingViewModel() {
    var self = this;
	self.VM = new CommonViewModel();
	self.baseUrl = self.VM.baseURL();
	self.lawyerName = ko.observable("");
    self.lawyerNameEn = ko.observable("");
    self.amount = ko.observable("");
	self.dateTxt = ko.observable("");
	self.timeFrom = ko.observable("");
	self.timeTo = ko.observable("");
	self.discount = ko.observable("");
	self.total = ko.observable("");
    GetBookingDetails();
    self.VM.getUserNotifications();
	function GetBookingDetails() {
        var pathArray = window.location.pathname.split('/');
		var url = self.baseUrl + "/api/LawyerApi/GetAppointmentDetails/" + pathArray[pathArray.length-1];
        $.ajax({
            type: "GET",
            url: url,
            contentType: "application/json",
			success: function (data) {
                
				self.lawyerName(data.lawyerName);
                self.lawyerNameEn(data.lawyerNameEn);
				self.timeFrom(data.timeFrom);
				self.timeTo(data.timeTo);
				self.dateTxt(data.dateTxt);
				self.amount(data.amount + " EGP");
				self.discount(data.discount + " EGP");
				var result = parseFloat(data.amount) - parseFloat(data.discount);
				self.total(result);
            },
            error: function (res) {
				self.VM.handleErrorResponse(res);
            },
			beforeSend: self.VM.setHeader
        });
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
    //self.Book = function () {
    //    //BookAppointment();
    //}
    self.PayWithCard = function () {
        
        localStorage.setItem('returnURL', window.location.pathname);
        var pathArray = window.location.pathname.split('/');
        var AppointmentId = pathArray[pathArray.length - 1];
        window.location.href = self.baseUrl + "/Lawyer/PayWithCard/" + AppointmentId;
    }
    //self.alertBooking = function () {
    //    //self.VM.confirm($("#alertBooking").val(), self.Book);
    //}
};
ko.applyBindings(new LawyerBookingViewModel(), document.getElementById('bdy'));