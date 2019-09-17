function SelectLawyerViewModel() {
    var self = this;
	self.VM = new CommonViewModel();
	self.baseUrl = self.VM.baseURL();
    self.Lawyers = ko.observableArray([]);
    self.Spetializations = ko.observableArray([]);
    self.searchModel = { ServiceId: 0, Name: "", Specialization: 0, Gender: -1, IsOnline: -1 }
    GetLawyers();
    GetSpetializations();
    self.VM.getUserNotifications();
    function GetLawyers() {
         
		var url = self.baseUrl + "/api/LawyerApi/Get";
		self.searchModel.Name = localStorage.getItem('name') !== null ? localStorage.getItem('name') : self.searchModel.Name;
		self.searchModel.Specialization=localStorage.getItem('specialization') !== null ? localStorage.getItem('specialization') : self.searchModel.Specialization;
        var pathArray = window.location.pathname.split('/');
        self.searchModel.ServiceId = pathArray[pathArray.length - 1]
        $.ajax({
            type: "POST",
            url: url,
			contentType: "application/json",
			data: ko.toJSON(self.searchModel),
            success: function (data) {
				self.Lawyers(data);
				localStorage.removeItem('name');
				if (localStorage.getItem('specialization') !== null) {
					$(document.getElementById("specializations")).val(localStorage.getItem('specialization'));
					localStorage.removeItem('specialization');
				}

            },
            error: function (res) {
				self.VM.handleErrorResponse(res);
            },
            beforeSend: self.VM.setHeader
        });
    }
    function GetSpetializations() {

        $.ajax({
			type: "GET",
			url: self.baseUrl + "/api/LawyerApi/GetSpetializations",
            contentType: "application/json",
            success: function (data) {
                self.Spetializations(data);
            },
			error: function (res) {
				self.VM.handleErrorResponse(res);
            },
			beforeSend: self.VM.setHeader
        });
    }
    self.setCurrency= function(fees) {
		return self.VM.setCurrency(fees);
    }
    self.Search = function () {
		GetLawyers();

		return true;
	}
    self.goToDetails = function (data) {
		window.location = self.baseUrl + "/Lawyer/Details/" + data.id;
    }
    self.selectLawyer = function (data) {
        window.location.href = self.baseUrl + "/Service/Index/" + self.searchModel.ServiceId + "/" + data.id;
	}
	function remove(arr, item) {
		for (var i = arr.length; i--;) {
			if (arr[i] === item) {
				arr.splice(i, 1);
			}
		}
	}

};
ko.applyBindings(new SelectLawyerViewModel(), document.getElementById('bdy'));