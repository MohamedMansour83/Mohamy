
function LawyerProfileViewModel() {
     
    var self = this;
	self.VM = new CommonViewModel();
	self.baseUrl = self.VM.baseURL();
    self.SessionNotes = ko.observableArray([]);
    self.timesModel = ko.observableArray([]);
	self.selectedTimes = ko.observableArray([]);
	self.selectedTime = { Id: 0, Time: "" };
	self.ChangePasswordModel = { OldPassword: "", Password: "", ConfirmPassword: "" };
	self.times = [];
    initData();
    self.VM.getUserNotifications();
    function initData() {
         
        var isltr = $("#IsLtr").val() == "True";
        var date = $('#myModal').data("date");
        //var quarterHours = ["00", "45"];
        var times = [];
        var am = isltr ? " AM" : " ص";
        var pm = isltr ? " PM" : " م";

        times.push(new time(0, date, "08:00" + am));
        times.push(new time(1, date, "08:30" + am));
        times.push(new time(2, date, "09:00" + am));
        times.push(new time(3, date, "09:30" + am));
        times.push(new time(4, date, "10:00" + am));
        times.push(new time(5, date, "10:30" + am));
        times.push(new time(6, date, "11:00" + am));
        times.push(new time(7, date, "11:30" + am));
        times.push(new time(8, date, "12:00" + pm));
        times.push(new time(9, date, "12:30" + pm));
        times.push(new time(10, date, "01:00" + pm));
        times.push(new time(11, date, "01:30" + pm));
        times.push(new time(12, date, "02:00" + pm));
        times.push(new time(13, date, "02:30" + pm));
        times.push(new time(14, date, "03:00" + pm));
        times.push(new time(15, date, "03:30" + pm));
        times.push(new time(16, date, "04:00" + pm));
        times.push(new time(17, date, "04:30" + pm));
        times.push(new time(18, date, "05:00" + pm));
        times.push(new time(19, date, "05:30" + pm));
        times.push(new time(20, date, "06:00" + pm));
        times.push(new time(21, date, "06:30" + pm));
        times.push(new time(22, date, "07:00" + pm));
        times.push(new time(23, date, "07:30" + pm));
        times.push(new time(24, date, "08:00" + pm));
        times.push(new time(25, date, "08:30" + pm));
        times.push(new time(26, date, "09:00" + pm));
        times.push(new time(27, date, "09:30" + pm));
        times.push(new time(28, date, "10:00" + pm));
        times.push(new time(29, date, "10:30" + pm));
        times.push(new time(30, date, "11:00" + pm));
        times.push(new time(31, date, "11:30" + pm));
        times.push(new time(32, date, "12:00" + am));

        //times.push(new time(1, date, "12:30" + (isltr ? " AM" : " ص")));
        //var id = 2;
        //for (var i = 2; i < 24; i++) {
        //    for (var j = 0; j < 2; j++) {
        //        if (i > 11) {
        //            var num = i;
        //            if (num != 12)
        //                num = num - 12;
        //            times.push(new time(id, date, addLeadingZero(num) + ":" + quarterHours[j] + (isltr ? " PM" : " م")));
        //        } else
        //            times.push(new time(id, date, addLeadingZero(i) + ":" + quarterHours[j] + (isltr ? " AM" : " ص")));
        //        id++;
        //    }
        //}

        self.timesModel(times);

		$(".responsive-calendar").responsiveCalendar({
			onDayClick: function (events) {
				self.selectedTimes([]);
				var t = new time(0, null, "");

				var val = $(this).data('year') + '-' + addLeadingZero($(this).data('month')) + '-' + addLeadingZero($(this).data('day'));

                var url = self.baseUrl + "/api/LawyerApi/GetAppointments/" + self.LawyerId() + "/" + val;
				var d1 = new Date(val);
				var today = new Date();
				today.setHours(0,0,0,0);
				d1.setHours(0, 0, 0, 0);
				var s = +d1 >= +today;
				if (s) {
					$('#myModal').modal('show');
					$('#myModal').data("date", val);
                    $("#result-message").html("");
					$.ajax({
						type: "GET",
						url: url,
						contentType: "application/json",
						success: function (data) {
							if (data.length > 0) {
								var tms = [];
								for (var i = 0; i < data.length; i++) {
									t.Time = data[i].timeFrom;
									var time = self.timesModel.find("Time", t);
									tms.push(time);
								}
								self.selectedTimes(tms);
							}
						},
						error: function (res) {
							self.VM.handleErrorResponse(res);
						},
						beforeSend: self.VM.setHeader
					});
				}
			}

		});
    }
    function setUserData(data) {
        self.lawyerId = data.lawyerId;
        self.name(data.name);
        self.nameEn(data.nameEn);
        self.email(data.email);
        self.age(data.age);
        self.sessions(data.sessions);
        self.userName(data.userName);
        self.dateOfBirth(data.dateOfBirth);
        var g = data.gender == 1;
        self.gender(g);
        self.Spetializations(data.spetializations);
        self.spetializationId(data.spetializationId);
        self.description(data.description);
        self.certificates(data.certificates);
        self.descriptionEn(data.descriptionEn);
        self.certificatesEn(data.certificatesEn);
        self.price(data.price);
        self.rating(data.rating);
        self.reviews(data.rating.reviews);
        self.provinces(data.provinces);
        self.provinceId(data.provinceId);
        self.experiences(data.experiences);
        self.experienceId(data.experienceId);
        self.phoneNumber(data.phoneNumber);
         // //console.log(self.reviews());
        self.lawyerRate(data.rating.lawyerRate);
        self.rate1(data.rating.rate1);
        self.rate2(data.rating.rate2);
        self.rate3(data.rating.rate3);
        self.rate4(data.rating.rate4);
        self.rate5(data.rating.rate5);
        self.totalRating(data.rating.totalRating);

    }
	self.addTime= function() {
		//var $checkBox = $(elem.currentTarget);
		//var isChecked = $checkBox.is(':checked');
		////If it is checked and not in the array, add it
		//if (isChecked && self.selectedTimes.indexOf(time) < 0) {
		//	self.selectedTimes.push(time);
		//}
		////If it is in the array and not checked remove it                
		//else if (!isChecked && self.selectedTimes.indexOf(time) >= 0) {
		//	remove(self.selectedTimes, time);
		//}
		//return true;
		var time = self.timesModel.find("Id", self.selectedTime);
		var isExists = self.selectedTimes.find("Id", time) != null;
		if (!isExists) {
			var tms = [];
			tms = ko.toJS(self.selectedTimes);
			tms.push(time);
			self.selectedTimes(tms);
		}
	}
	self.removeTime = function (time, elem) {
		var tms = ko.toJS(self.selectedTimes);
		remove(tms, time);
		self.selectedTimes(tms);
	}
	function time(id,date, time) {
		this.Id = id;
		this.Date = date;
		this.Time = time;
	};
	ko.observableArray.fn.find = function (prop, data) {
		var valueToMatch = data[prop];
		return ko.utils.arrayFirst(this(), function (item) {
			return item[prop] === valueToMatch;
		});
	};

    self.openSession = function (data) {
        window.location = "http://localhost:4200/?id=" + '2FE8483B-9010-4038-B165-03A51B8166A2';
		//window.location.href = self.baseUrl + "/Session/Index/"+ data.SessionId();
    }
    self.openSessioNotes = function (data) {
       self.SessionNotes(data.Notes());
        $('#session-notes').modal('show');

    }

	self.Save = function (data,e) {
         
		var url = self.baseUrl + "/api/LawyerApi/SetAppointments";

		var date = $('#myModal').data("date");
        var time = self.timesModel.find("Id", self.selectedTime);
        if (self.selectedTimes.length == 0) {
            var time = self.timesModel.find("Id", self.selectedTime);
            var isExists = self.selectedTimes.find("Id", time) != null;
            if (!isExists) {
                var tms = [];
                tms = ko.toJS(self.selectedTimes);
                tms.push(time);
                self.selectedTimes(tms);
            }
        }
        var tt = ko.toJS(self.selectedTimes)

		for (var i = 0; i < tt.length; i++) {
			tt[i].Date = date;
		}

        var model = {
            Date: date,
			Appointments: tt
		};

        self.VM.showloading(e);
		$.ajax({
			type: "Post",
			url: url,
			data: ko.toJSON(model),
			contentType: "application/json",
			success: function (data) {
				if (!data.isSuccess) {
                    self.VM.alert(data.errorMessage);
				} else {
                    //self.VM.alert("Appointment added successfully.");
                    $("#result-message").html(data.message)
                }
                self.VM.hideloading(e);
			},
			error: function (res) {
                self.VM.handleErrorResponse(res);
                self.VM.hideloading(e);
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

	self.isLawyer = function () {
		return lawyerId > 0;
	}

    self.SaveSetting = function (data) {
         
        $('#cover-spin').show(0);
        var url = self.baseUrl + "/api/UserApi/SaveInfo";
        var model = {
            Name: self.Name(),
            NameEn: self.NameEn(),
            DateOfBirth: self.DateOfBirth(),
            Gender: self.Gender(),
            SpetializationId: self.SpetializationId(),
            Description: self.Description(),
            Certificates: self.Certificates(),
            DescriptionEn: self.DescriptionEn(),
            CertificatesEn: self.CertificatesEn(),
            VideoURL: self.VideoURL(),
            Email: "",
            Price: self.Price(),
            ProvinceId: self.ProvinceId,
            PhoneNumber: self.PhoneNumber,
            ExperienceId: self.ExperienceId,
        }

        $.ajax({
            type: "Post",
            url: url,
            data: ko.toJSON(model),
            contentType: "application/json",
            success: function (data) {
                $('#cover-spin').hide(0);
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
    };

    self.calculateRate = function (rate) {
        return (rate() / self.Rating.TotalRating()) * 100;
    };

    self.experienceChanged = function (obj) {
         // //console.log(obj.ExperienceId());
        var exps = ko.toJS(self.Experiences());
        for (var i = 0; i < exps.length; i++) {
            if (exps[i].Id == obj.ExperienceId()) {
                 // //console.log(exps[i]);
                self.Price(exps[i].Price);
                break;
            }
        }
    };

    self.serviceChecked = function (data, el) {
        if ($(el.currentTarget).prop("checked") === true) {
            //alert('hi');
            //var url = self.baseUrl + "/api/LawyerApi/AddServiceToLawyer/" + data.Id();

            //$.ajax({
            //    type: "GET",
            //    url: url,
            //    contentType: "application/json",
            //    success: function (data) {
            //         // //console.log(data);
            //        self.Services(data);
            //    },
            //    error: function (res) {
            //        self.VM.handleErrorResponse(res);
            //    },
            //    beforeSend: self.VM.setHeader
            //});


            var s1 = self.Services();
            s1.push(data);
            self.Services(ko.toJS(s1));

        } else {
            var s1 = ko.toJS(self.Services());
             // //console.log(s1);
            var ind = 0;
            for (var i = 0; i < s1.length; i++) {
                 // //console.log(s1[i].Id);
                 // //console.log(data.Id());
                if (s1[i].Id === data.Id()) {
                    ind = i;
                    break;
                }
                ind++;
            }
             // //console.log(ind);
            s1.splice(ind, 1);
             // //console.log(s1);
            self.Services(ko.toJS(s1));

            var url = self.baseUrl + "/api/LawyerApi/RemoveServiceLawyer/" + data.Id();

            $.ajax({
                type: "GET",
                url: url,
                contentType: "application/json",
                success: function (data) {

                },
                error: function (res) {
                    self.VM.handleErrorResponse(res);
                },
                beforeSend: self.VM.setHeader
            });

        }

        return true;


    };

    self.serviceClicked = function (data, el) {
         
        $('#cover-spin').show(0);

        var url = self.baseUrl + "/api/LawyerApi/AddServiceToLawyer/" + data.Id() + "/" + data.PriceLawyer() + "/" + data.PriceLevel2Lawyer();
        $.ajax({
            type: "GET",
            url: url,
            contentType: "application/json",
            success: function (data) {
                $('#cover-spin').hide(0);
                 // //console.log(data);
                if (data.responseStatus) {
                     // //console.log(data);
                    self.Services(data.data);
                }
                else {
                    alert(data.arabicMessage);
                }
            },
            error: function (res) {
                $('#cover-spin').hide(0);
                self.VM.handleErrorResponse(res);
            },
            beforeSend: self.VM.setHeader
        });
        return true;


    };

    self.isHasService = function (id) {
        return ko.computed(function () {



            var match = ko.utils.arrayFirst(ko.toJS(self.Services()), function (item) {
                return item.Id === id;
            });


            return match;
        }, this);
    };

    self.checkCategory = function (catId) {
        return catId === 9;
    };

    self.test = function (data) {
         // //console.log(data);
        return data;
    };

    self.reply = function (data) {

        var url = self.baseUrl + "/api/LawyerApi/ReviewReply";

        var model = {
            Reply: data.ReplyMessage,
            ReviewId: data.Id,
            UserId: data.UserId
        }

        $.ajax({
            type: "Post",
            url: url,
            data: ko.toJSON(model),
            contentType: "application/json",
            success: function (data) {
                //setUserData(data.value);
                self.Rating.Reviews(data.value.rating.reviews);
            },
            error: function (res) {
                self.VM.handleErrorResponse(res);
            },
            beforeSend: self.VM.setHeader
        });
    }

    self.uploadImageProfile = function (file) {
         
        $('#cover-spin').show(0);
         // //console.log(self.baseUrl + "/api/UserApi/UploadProfileImg");
        var data = new FormData();
        data.append("file", file);


        $.ajax({
            type: "POST",
            url: self.baseUrl + "/api/UserApi/UploadProfileImg",
            contentType: false,
            processData: false,
            data: data,
            success: function (data) {
                $('#cover-spin').hide(0);
                self.ProfileImg(data);
            },
            error: function (res) {
                $('#cover-spin').hide(0);
                self.VM.handleErrorResponse(res);
            },
            beforeSend: self.VM.setHeader
        });
    }

	function addLeadingZero(num) {
		if (num < 10) {
			return "0" + num;
		} else {
			return "" + num;
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
	function remove(arr, item) {
		for (var i = arr.length; i--;) {
			if (arr[i].Id === item.Id) {
				arr.splice(i, 1);
			}
		}
	}

};
LawyerProfileViewModel.prototype.init = function (data) {
    var self = this;
     // //console.log(data);
    ko.mapping.fromJS(data, {}, self);
     // //console.log(ko.toJS(self.Services));
}
//ko.applyBindings(new LawyerProfileViewModel());