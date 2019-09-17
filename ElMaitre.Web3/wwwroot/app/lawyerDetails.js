

function LawyerDetailsViewModel() {

    var self = this;
    self.VM = new CommonViewModel();

    self.baseUrl = self.VM.baseURL();


    self.Spetializations = ko.observableArray([]);
    self.appointments = ko.observableArray([]);

    self.searchModel = { Name: "", Spetialization: 0 }
    self.name = ko.observable("");
    self.profileImg = ko.observable("");
    self.nameEn = ko.observable("");
    self.specialization = ko.observable("");
    self.specializationEn = ko.observable("");
    self.rates = ko.observableArray([]);
    self.certificates = ko.observable("");
    self.certificatesEn = ko.observable("");
    self.description = ko.observable("");
    self.descriptionEn = ko.observable("");
    self.fees = ko.observable("");
    self.fees60 = ko.observable("");

    self.times = ko.observableArray([]);
    self.selectedTime = { id: 0, time: "" }

    self.reviews = ko.observableArray([]);
    self.services = ko.observableArray([]);
    self.video = ko.observable("");
    self.videoUrl = ko.observable("");
    self.embed = ko.computed(function () {
        var EmbedCodeNew = "";
         // //console.log(self.videoUrl());
        if (self.videoUrl() === null)
            return "";
        if (self.videoUrl().includes("youtube.com")) {
            if (self.videoUrl().includes("/v/")) {
                EmbedCodeNew = self.videoUrl();
            }
            else if (self.videoUrl().includes("/watch?v")) {
                EmbedCodeNew = self.videoUrl().replace("/watch?v=", "/v/");
            }
            else if (self.videoUrl().includes("/watch?feature=player_embedded&v=")) {
                EmbedCodeNew = self.videoUrl().replace("/watch?feature=player_embedded&v=", "/v/");
            }
        }
        EmbedCodeNew = EmbedCodeNew.replace("m.youtube.com", "www.youtube.com");
         // //console.log(EmbedCodeNew);

        var str = '<object width="100%" height="334">';
        str += '<param name="movie" value="' + EmbedCodeNew + '"></param>';
        str += '<param name="allowFullScreen" value="true"></param>';
        str += '<param name="allowscriptaccess" value="always"></param>';
        str += '<param value="transparent" name="wmode"/>';
        str += '<embed src="' + EmbedCodeNew + '" type="application/x-shockwave-flash" width="500" height="334" allowscriptaccess="always" allowfullscreen="true" wmode="transparent"></embed>';
        str += '</object>';

        return str;
    });



    GetSpetializations();
    GetLawyer();
    self.VM.getUserNotifications();


    function GetLawyer() {
		
        var pathArray = window.location.pathname.split('/');


        var url = self.baseUrl + "/api/LawyerApi/GetDetails/" + pathArray[pathArray.length - 1];

        $.ajax({
            type: "GET",
            url: url,
            contentType: "application/json",
            success: function (data) {
                 // //console.log(data);
                self.profileImg(data.profileImg);
                self.name(data.name);
                self.specialization(data.specialization);
                self.certificates(data.certificates);
                self.description(data.description);
                self.nameEn(data.nameEn);
                self.specializationEn(data.specializationEn);
                self.certificatesEn(data.certificatesEn);
                self.descriptionEn(data.descriptionEn);
                self.reviews(data.reviews);
                self.services(data.services);
                self.appointments(data.appointments);
                self.fees(data.fees);
                self.fees60(parseFloat(data.fees * 2));
                self.video(data.video);
                self.videoUrl(data.videoURL);
                 // //console.log(data);
                self.embed = ko.computed(function () {
                    var EmbedCodeNew = "";
                     // //console.log(self.videoUrl());
                    if (self.videoUrl() === null)
                        return "";
                    if (self.videoUrl().includes("youtube.com")) {
                        if (self.videoUrl().includes("/v/")) {
                            EmbedCodeNew = self.videoUrl();
                        }
                        else if (self.videoUrl().includes("/watch?v")) {
                            EmbedCodeNew = self.videoUrl().replace("/watch?v=", "/v/");
                        }
                        else if (self.videoUrl().includes("/watch?feature=player_embedded&v=")) {
                            EmbedCodeNew = self.videoUrl().replace("/watch?feature=player_embedded&v=", "/v/");
                        }
                    }
                     // //console.log(EmbedCodeNew);

                    var str = '<object width="100%" height="334">';
                    str += '<param name="movie" value="' + EmbedCodeNew + '"></param>';
                    str += '<param name="allowFullScreen" value="true"></param>';
                    str += '<param name="allowscriptaccess" value="always"></param>';
                    str += '<param value="transparent" name="wmode"/>';
                    str += '<embed src="' + EmbedCodeNew + '" type="application/x-shockwave-flash" width="500" height="334" allowscriptaccess="always" allowfullscreen="true" wmode="transparent"></embed>';
                    str += '</object>';

                    return str;
                });
                 // //console.log(data.video);

                for (var i = 0; i < 5; i++) {
                    if (data.rate > i)
                        self.rates.push({ name: 'active' });
                    else
                        self.rates.push({ name: '' });
                }

				
				
				
				GetAppointments(data.id, function (d) {
    var evnts = "{";
    var lst = d;
    for (var i = 0; i < lst.length; i++)
    {
        if (i > 0)
            evnts += ",";
        var strdate = getFormatedDate(lst[i].date);
        var row = "\"" + strdate + "\": { \"number\": 2,\"id\":" + lst[i].id + " }";
        evnts += row;
    }
    evnts += "}";
	$(".responsive-calendar").responsiveCalendar({

    onDayClick: function(events) {
            //var thisDayEvent, key;
            //debugger
            //var isConfirmed = confirm('Are you sure, Do you want to reserve?');
            //if (isConfirmed) {
            //	key = $(this).data('year') + '-' + addLeadingZero($(this).data('month')) + '-' + addLeadingZero($(this).data('day'));
            //	thisDayEvent = events[key];
            //	//alert(thisDayEvent.id);
            //	window.location.href = self.baseUrl + "Lawyer/Booking/" + pathArray[pathArray.length - 1] + "/" + thisDayEvent.id;
            //}

            var val = $(this).data('year') + '-' + addLeadingZero($(this).data('month')) + '-' + addLeadingZero($(this).data('day'));

            var show = false;
            var t = [];
            for (var i = 0; i < lst.length; i++)
            {
                var d = getFormatedDate(lst[i].date);
                if (d == val)
                {
                    show = true;
                    t.push({ time: lst[i].timeFrom, id: lst[i].id });
        }
    }
    if (show)
    {
        self.times(t);
				$('#myModal').modal('show');
    }



}

                });



            });
                //GetAppointments(data.id,function (d) {
                //	var evnts = "{";
                //	var lst = d;
                //	for (var i = 0; i < lst.length; i++) {
                //		if (i > 0)
                //			evnts += ",";
                //		var strdate = getFormatedDate(lst[i].date);
                //		var row = "\"" + strdate + "\": { \"number\": 2,\"id\":" + lst[i].id+" }";
                //		evnts += row;
                //	}
                //	evnts += "}";
                //	$(".responsive-calendar").responsiveCalendar({

                //		onDayClick: function (events) {
                //			//var thisDayEvent, key;
                //			//debugger
                //			//var isConfirmed = confirm('Are you sure, Do you want to reserve?');
                //			//if (isConfirmed) {
                //			//	key = $(this).data('year') + '-' + addLeadingZero($(this).data('month')) + '-' + addLeadingZero($(this).data('day'));
                //			//	thisDayEvent = events[key];
                //			//	//alert(thisDayEvent.id);
                //			//	window.location.href = self.baseUrl + "Lawyer/Booking/" + pathArray[pathArray.length - 1] + "/" + thisDayEvent.id;
                //			//}

                //			var val = $(this).data('year') + '-' + addLeadingZero($(this).data('month')) + '-' + addLeadingZero($(this).data('day'));

                //			var show = false;
                //			var t = [];
                //			for (var i = 0; i < lst.length; i++) {
                //				var d = getFormatedDate(lst[i].date);
                //				if (d == val) {
                //					show = true;
                //					t.push({ time: lst[i].timeFrom, id: lst[i].id });
                //				}
                //			}
                //			if (show) {
                //				self.times(t);
                //				$('#myModal').modal('show');
                //			}



                //		}

                //                });



                //            });


            },
            error: function (res) {
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


    function GetAppointments(id, callback) {
        var url = self.baseUrl + "/api/LawyerApi/GetAppointments/" + id;

        $.ajax({
            type: "GET",
            url: url,
            contentType: "application/json",
            success: callback,
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


    self.Search = function () {
        GetLawyers(self.searchModel.Name, self.searchModel.Spetialization);
    }


    self.bookService = function (data) {
        var pathArray = window.location.pathname.split('/');
        var lawyerId = pathArray[pathArray.length - 1]
        window.location.href = self.baseUrl + "/service/index/" + data.Id + "/" + lawyerId;
    }

    self.book = function (parent, data) {
        var pathArray = window.location.pathname.split('/');
        var id=pathArray[pathArray.length - 1]
        window.location.href = self.baseUrl + "/Lawyer/Booking/" + id + "/" + data.id;
    }

	self.Reserve = function () {
		if (self.selectedTime.id != undefined) {
			var pathArray = window.location.pathname.split('/');
			window.location.href = self.baseUrl + "/Lawyer/Booking/" + pathArray[pathArray.length - 1] + "/" + self.selectedTime.id;
		}
    }


       function waitForElement(elementId, callBack) {
        window.setTimeout(function () {
            var element = document.getElementsByClassName(elementId);
            if (element) {
                callBack(elementId, element);
            } else {
                waitForElement(elementId, callBack);
            }
        }, 500)
    }
};
ko.applyBindings(new LawyerDetailsViewModel(), document.getElementById('bdy'));