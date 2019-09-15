function HomeViewModel() {
    var self = this;

	self.VM = new CommonViewModel();

	self.baseUrl = self.VM.baseURL();

    self.Spetializations = ko.observableArray([]);

    self.searchModel = { Name: "", Specialization: 0 }

    self.questionModel = { question: ko.observable(""), categoryId: ko.observable(0), anonymousEmail: ko.observable(""), anonymousName: ko.observable("") }

    self.categories = ko.observableArray([]);
    self.serviceCategories = ko.observableArray([]);
    self.serviceCategoriesMore = ko.observableArray([]);
    self.selectedCategoryService = ko.observable("");
    self.selectedService = ko.observable("");
    
    self.isEmailVisible = ko.observable(localStorage.getItem('token') == null || localStorage.getItem('token') == "");




    GetSpetializations();
    getQuestionCategories();
    getServiceCategories();
    self.VM.getUserNotifications();

   
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

    function getQuestionCategories() {

        $.ajax({
            type: "GET",
            url: self.baseUrl + "/api/HomeApi/GetQuestionCategories",
            contentType: "application/json",
            success: function (data) {
                self.categories(data);
            },
            error: function (res) {
                self.VM.handleErrorResponse(res);
            },
            beforeSend: self.VM.setHeader
        });
    }


    function getServiceCategories() {

        $.ajax({
            type: "GET",
            url: self.baseUrl + "/api/HomeApi/GetServiceCategories",
            contentType: "application/json",
            success: function (data) {
                console.log(data);
                let array = data.slice(0, 5);
                let arrayMore = data.slice(5, data.length);


                self.serviceCategories(array);
                self.serviceCategoriesMore(arrayMore);
            },
            error: function (res) {
                self.VM.handleErrorResponse(res);
            },
            beforeSend: self.VM.setHeader
        });
    }


    self.showServices = function (data) {
        self.selectedCategoryService(data);
    }

    self.submitService = function () {
        //alert(self.selectedService());
        window.location.href = self.baseUrl + "/Lawyer/SelectLawyer/" + self.selectedService();
    }

    

   
	self.Search = function () {
		if (self.searchModel.Name != "" && self.searchModel.Name != undefined)
			localStorage.setItem('name', self.searchModel.Name);
		if (self.searchModel.Specialization != 0 && self.searchModel.Specialization != undefined)
			localStorage.setItem('specialization', self.searchModel.Specialization);

		window.location.href = self.baseUrl + "/Lawyer/Index";
		return true;
    }

    self.sendQuestion = function (el,e) {

        if (self.isEmailVisible() && (self.questionModel.anonymousEmail == "" || self.questionModel.anonymousName == "")) {
            return;
        }

        self.VM.showloading(e);
        
        var url = self.baseUrl + "/api/HomeApi/SendQuestion";
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json",
            data: ko.toJSON(self.questionModel),
            success: function (data) {
                $('#myModal').modal('hide');
                self.questionModel.question("");
                self.questionModel.anonymousEmail("");
                self.questionModel.anonymousName("");
                self.questionModel.categoryId(null);
                self.VM.hideloading(e);
                $('#characters').text("200")
            },
            error: function (res) {
                self.VM.hideloading(e);
                self.VM.handleErrorResponse(res);
            },
            beforeSend: self.VM.setHeader
        });
    }

    self.submit = function (el,e) {
        if (self.questionModel.categoryId() == 0 || self.questionModel.question().length == 0)
            return;

        self.sendQuestion(el,e);
    }

    self.submitAnonimous = function () {
        $('#myModal').modal('show');
    }

    self.goToLawyers = function () {
        window.location = self.baseUrl + "/Lawyer/Index";
    }

};
ko.applyBindings(new HomeViewModel(), document.getElementById('bdy'));