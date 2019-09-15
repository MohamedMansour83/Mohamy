function QuestionsViewModel() {
    var self = this;

	self.VM = new CommonViewModel();

	self.baseUrl = self.VM.baseURL();

    self.filterModel = { categoryId: ko.observable(""), unAnswerd: false, lawyerQuestions: false }

    self.categories = ko.observableArray([]);
    self.questions = ko.observableArray([]);


    getQuestionCategories();
    GetQuestions();

    $(document).ready(function () {
        document.getElementById("contact-tab").addEventListener('click', function () {
            self.unAnswerdQuestions();
        });
        document.getElementById("profile-tab").addEventListener('click', function () {
            self.lawyerAnswers();
        });

        document.getElementById("home-tab").addEventListener('click', function () {
            self.search();
        });
    })
    

   
    function GetQuestions() {

        if (self.filterModel.categoryId() == "") {
            setSelectedCategoryId();
        }

        $.ajax({
			type: "POST",
			url: self.baseUrl + "/api/QuestionApi/Get",
            contentType: "application/json",
            data: ko.toJSON(self.filterModel),
            success: function (data) {
                self.questions(data);
            },
			error: function (res) {
				self.VM.handleErrorResponse(res);
            },
			beforeSend: self.VM.setHeader
        });
    }

    function setSelectedCategoryId() {
        var pathArray = window.location.pathname.split('/');
        var catId = pathArray[pathArray.length - 1]
        if (parseInt(catId) > 0)
            self.filterModel.categoryId(catId);
    }

    function getQuestionCategories() {

        $.ajax({
            type: "GET",
            url: self.baseUrl + "/api/HomeApi/GetQuestionCategories",
            contentType: "application/json",
            success: function (data) {
                self.categories(data);
                setSelectedCategoryId();
            },
            error: function (res) {
                self.VM.handleErrorResponse(res);
            },
            beforeSend: self.VM.setHeader
        });
    }

    
    self.sendQuestion = function () {
       
    }

    self.search = function (data) {
        self.filterModel = { categoryId: null, unAnswerd: false, lawyerQuestions: false }
        GetQuestions();
    }

    self.lawyerAnswers = function (data) {
        self.filterModel = { categoryId: null, unAnswerd: false, lawyerQuestions: true }
        GetQuestions();
    }

    self.unAnswerdQuestions = function (data) {
        self.filterModel = { categoryId: null, unAnswerd: true, lawyerQuestions: false }
        GetQuestions();
    }


    self.reply = function (data,e) {
        self.VM.showloading(e);

        $.ajax({
            type: "POST",
            url: self.baseUrl + "/api/QuestionApi/Reply",
            contentType: "application/json",
            data: ko.toJSON({ questionId: data.id, answer: data.answer, unAnswerQuestions: self.filterModel.unAnswerd }),
            success: function (data) {
                self.questions(data);
                self.VM.hideloading(e);
            },
            error: function (res) {
                self.VM.hideloading(e);
                self.VM.handleErrorResponse(res);
            },
            beforeSend: self.VM.setHeader
        });

    }
    

};
ko.applyBindings(new QuestionsViewModel(), document.getElementById('bdy'));