
function UserProfileViewModel() {
    var self = this;

	self.VM = new CommonViewModel();

	self.baseUrl = self.VM.baseURL();


 //   self.name = ko.observable("");
 //   self.nameEn = ko.observable("");
 //   self.email = ko.observable("");
	//self.age = ko.observable("");
	//self.sessions = ko.observableArray([]);
 //   self.documents = ko.observableArray([]);
	//self.gender = ko.observable("");
	//self.dateOfBirth = ko.observable("");
 //   self.userName = ko.observable("");
 //   self.provinces = ko.observableArray([]);
 //   self.provinceId = ko.observable("");
 //   self.phoneNumber = ko.observable("");
	self.ChangePasswordModel = { OldPassword: "", Password: "", ConfirmPassword: "" };


    document.getElementById("sent-tab").addEventListener('click', function () {
        GetDocuments(true);
    });
    document.getElementById("received-tab").addEventListener('click', function () {
        GetDocuments(false);
    });
    self.VM.getUserNotifications();
    //GetLawyerDetails();

	//function GetLawyerDetails() {
	//	var url = self.baseUrl + "/api/UserApi/GetUserDetails";

 //       $.ajax({
 //           type: "GET",
 //           url: url,
 //           contentType: "application/json",
 //           success: function (data) {
	//			self.name(data.name);
 //               self.nameEn(data.nameEn);
	//			self.userName(data.userName);
	//			self.dateOfBirth(data.dateOfBirth);
	//			var g = data.gender == 1;
	//			self.gender(g);
	//			self.email(data.email);
	//			self.age(data.age);
 //               self.sessions(data.sessions);
 //               self.documents(data.documents);
 //           },
 //           error: function (res) {
	//			self.VM.handleErrorResponse(res);
 //           },
	//		beforeSend: self.VM.setHeader
 //       });
 //   }


    function GetDocuments(sent) {
        var url = self.baseUrl + "/api/UserApi/GetDocuments";

        $.ajax({
            type: "GET",
            url: url + "?isSent="+sent,
            contentType: "application/json",
            success: function (data) {
                self.Documents(data);
            },
            error: function (res) {
                self.VM.handleErrorResponse(res);
            },
            beforeSend: self.VM.setHeader
        });
    }


	self.openSession = function (data) {
		window.location.href = self.baseUrl + "/Session/Index/" + data.SessionId();
	}

	self.SaveSetting = function (data) {

		var url = self.baseUrl + "/api/UserApi/SaveInfo";





		var model = {
            Name: self.Name(),
            NameEn: self.NameEn(),
			DateOfBirth: self.DateOfBirth(),
			Gender: self.Gender(),
            Email: "",
            ProvinceId: self.ProvinceId,
            PhoneNumber: self.PhoneNumber
		}




		$.ajax({
			type: "Post",
			url: url,
			data: ko.toJSON(model),
			contentType: "application/json",
			success: function (data) {
				if (!data.isSuccess) {
                    self.VM.alert(data.errorMessage);
				} else {
                    self.VM.alert($("#saveMsg").val());
				}
			},
			error: function (res) {
				self.VM.handleErrorResponse(res);
			},
			beforeSend: self.VM.setHeader
		});
	}

	self.ChangePassword = function () {

		var url = self.baseUrl + "/api/UserApi/ChangePassword";

		$.ajax({
			type: "Post",
			url: url,
			data: ko.toJSON(self.ChangePasswordModel),
			contentType: "application/json",
			success: function (data) {
				if (data.succeeded) {
                    self.VM.alert("Password Changed successfully.");
				}
			},
			error: function (res) {
				self.VM.handleErrorResponse(res);
			},
			beforeSend: self.VM.setHeader
		});
    }

    //self.uploadImageProfile = function (file) {
    //    var data = new FormData();
    //    data.append("file", file);


    //    $.ajax({
    //        type: "POST",
    //        url: self.baseUrl + "/api/UserApi/UploadProfileImg",
    //        contentType: false,
    //        processData: false,
    //        data: data,
    //        success: function (data) {
    //            self.ProfileImg(data);
    //        },
    //        error: function (res) {
    //            self.VM.handleErrorResponse(res);
    //        },
    //        beforeSend: self.VM.setHeader
    //    });
    //}

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

UserProfileViewModel.prototype.init = function (data) {
    var self = this;
    ko.mapping.fromJS(data, {}, self);
}
 //ko.applyBindings(new UserProfileViewModel());