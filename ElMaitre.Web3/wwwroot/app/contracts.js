function ContractsViewModel() {
    var self = this;

	self.VM = new CommonViewModel();

	self.baseUrl = self.VM.baseURL();

    self.filterModel = { categoryId: ko.observable(""), name: ko.observable("") }

    self.categories = ko.observableArray([]);
    self.contracts = ko.observableArray([]);


    getCategories();
    GetContracts();
    self.VM.getUserNotifications();
   
    function GetContracts() {

        if (self.filterModel.categoryId() == "") {
            setSelectedCategoryId();
        }

        $.ajax({
            type: "POST",
            url: self.baseUrl + "/api/ContractApi/Get",
            contentType: "application/json",
            data: ko.toJSON(self.filterModel),
            success: function (data) {
                self.contracts(data);
            },
			error: function (res) {
				self.VM.handleErrorResponse(res);
            },
			beforeSend: self.VM.setHeader
        });
    }


    function getCategories() {

        $.ajax({
            type: "GET",
            url: self.baseUrl + "/api/ContractApi/GetCategories",
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

    
    self.gotoDetails = function () {
        window.location.href = self.baseUrl + "/Contract/details/" + id;
    }

    self.search = function (data) {

        GetContracts();
    }

    function setSelectedCategoryId() {
        var pathArray = window.location.pathname.split('/');
        var catId = pathArray[pathArray.length - 1]
        if (parseInt(catId) > 0)
            self.filterModel.categoryId(catId);
    }

};
ko.applyBindings(new ContractsViewModel(), document.getElementById('bdy'));