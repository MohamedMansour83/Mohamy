function BlogDetailsViewModel() {
    var self = this;

	self.VM = new CommonViewModel();

	self.baseUrl = self.VM.baseURL();


    self.blog = ko.observable("");


    GetBlogDetails();
    self.VM.getUserNotifications();

    function GetBlogDetails() {

        var pathArray = window.location.pathname.split('/');
        var id = pathArray[pathArray.length - 1];

        $.ajax({
            type: "GET",
            url: self.baseUrl + "/api/BlogApi/GetById?id=" + id,
            contentType: "application/json",
            success: function (data) {
                self.blog(data);
            },
            error: function (res) {
                self.VM.handleErrorResponse(res);
            },
            beforeSend: self.VM.setHeader
        });
    }

    
   

    

};
ko.applyBindings(new BlogDetailsViewModel(), document.getElementById('bdy'));