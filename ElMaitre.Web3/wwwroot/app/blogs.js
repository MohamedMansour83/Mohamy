function BlogsViewModel() {
    var self = this;

	self.VM = new CommonViewModel();

	self.baseUrl = self.VM.baseURL();

    self.blogs = ko.observableArray([]);
    self.contract = ko.observable("");


    GetBlogs();
    self.VM.getUserNotifications();

    function GetBlogs() {


        $.ajax({
            type: "GET",
            url: self.baseUrl + "/api/BlogApi/Get",
            contentType: "application/json",
            success: function (data) {
                self.blogs(data);
            },
			error: function (res) {
				self.VM.handleErrorResponse(res);
            },
			beforeSend: self.VM.setHeader
        });
    }


    
    self.gotoDetails = function () {
        window.location.href = self.baseUrl + "/Blog/details/" + id;
    }

    self.search = function (data) {

        GetBlogs();
    }

};
ko.applyBindings(new BlogsViewModel(), document.getElementById('bdy'));