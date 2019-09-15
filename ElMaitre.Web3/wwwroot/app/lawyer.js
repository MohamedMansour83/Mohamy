function LawyerViewModel() {
    var self = this;

	self.VM = new CommonViewModel();

	self.baseUrl = self.VM.baseURL();

    self.Lawyers = ko.observableArray([]);
    self.Spetializations = ko.observableArray([]);
    self.searchModel = { Name: "", Specialization: 0, Gender: -1, IsOnline: -1, Rating: ["-1"], MinFees: "", MaxFees: "", Prices: [], Experiences: []};
	self.isLawyerViewVisible = ko.observable(false);
	self.isLawyerCardVisible = ko.observable(true);
	self.dateTxt = ko.observable("");
	self.viewIcon = ko.observable("fas fa-th-list");
	self.checkedRatings = ko.observableArray(["-1"]);
    self.checkedPrices = ko.observableArray([]);
    self.checkedExperiences = ko.observableArray([]);
    self.Prices = ko.observableArray([]);
    self.Experiences = ko.observableArray([]);
    GetLawyers();
    GetSpetializations();
    GetPrices();
    GetExperiences();
    self.VM.getUserNotifications();
	

    function GetLawyers() {
		
		var url = self.baseUrl + "/api/LawyerApi/Get";
		self.searchModel.Name = localStorage.getItem('name') !== null ? localStorage.getItem('name') : self.searchModel.Name;
		self.searchModel.Specialization=localStorage.getItem('specialization') !== null ? localStorage.getItem('specialization') : self.searchModel.Specialization;
		
        self.searchModel.Rating = self.checkedRatings();
        self.searchModel.Prices = self.checkedPrices();
        self.searchModel.Experiences = self.checkedExperiences();

		if (self.searchModel.Rating[0] == "-1")
            self.searchModel.Rating = null;



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

    function GetPrices() {

        $.ajax({
            type: "GET",
            url: self.baseUrl + "/api/LawyerApi/GetPrices",
            contentType: "application/json",
            success: function (data) {
                self.Prices(data);
            },
            error: function (res) {
                self.VM.handleErrorResponse(res);
            },
            beforeSend: self.VM.setHeader
        });
    }

    function GetExperiences() {

        $.ajax({
            type: "GET",
            url: self.baseUrl + "/api/LawyerApi/GetExperiences",
            contentType: "application/json",
            success: function (data) {
                self.Experiences(data);
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

	self.ratingChecked = function (d) {

		var ss = self.checkedRatings();

		if (ss.length > 1 )
			remove(ss, "-1");
		else if (ss.length == 0)
			ss.push("-1");

		self.checkedRatings(ss);

		self.Search();
		return true;
    }

    self.pricesChecked = function (d) {


        self.searchModel.MinFees = "";
        self.searchModel.MaxFees = "";

        self.Search();
        return true;
    }

    self.goToDetails = function (data) {
		window.location = self.baseUrl + "/Lawyer/Details/" + data.id;
    }

    

	self.book = function (parent,data) {
		window.location.href = self.baseUrl + "/Lawyer/Booking/" + parent.id + "/" + data.id;
	}

	self.changeView = function () {
		self.isLawyerViewVisible(!self.isLawyerViewVisible());
		self.isLawyerCardVisible(!self.isLawyerCardVisible());

		if (self.isLawyerViewVisible()) {
			self.viewIcon("fas fa-columns");
		} else {
			self.viewIcon("fas fa-th-list");
		}
	}

	function remove(arr, item) {
		for (var i = arr.length; i--;) {
			if (arr[i] === item) {
				arr.splice(i, 1);
			}
		}
	}

};
ko.applyBindings(new LawyerViewModel(), document.getElementById('bdy'));