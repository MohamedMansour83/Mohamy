function QuestionDetailsViewModel() {
    var self = this;

	self.VM = new CommonViewModel();

	self.baseUrl = self.VM.baseURL();


    self.question = ko.observable("");


    GetQuestion();

   
    function GetQuestion() {

        var pathArray = window.location.pathname.split('/');

        $.ajax({
            type: "GET",
            url: self.baseUrl + "/api/QuestionApi/GetDetails/" + pathArray[pathArray.length - 1],
            contentType: "application/json",
            data: ko.toJSON(self.filterModel),
            success: function (data) {
                self.question(data);
            },
            error: function (res) {
                self.VM.handleErrorResponse(res);
            },
            beforeSend: self.VM.setHeader
        });
    }

};
ko.applyBindings(new QuestionDetailsViewModel(), document.getElementById('bdy'));