function ContractDetailsViewModel() {
    var self = this;

	self.VM = new CommonViewModel();

	self.baseUrl = self.VM.baseURL();


    self.contract = ko.observable("");
    self.pdfPath = ko.observable("");


    GetContractDetails();
    self.VM.getUserNotifications();

    function GetContractDetails() {

        var pathArray = window.location.pathname.split('/');
        var id = pathArray[pathArray.length - 1];

        $.ajax({
            type: "GET",
            url: self.baseUrl + "/api/ContractApi/Get?id=" + id,
            contentType: "application/json",
            success: function (data) {
                self.contract(data);
                self.pdfPath("/"+data.path.replace('\\','\/'));
            },
            error: function (res) {
                self.VM.handleErrorResponse(res);
            },
            beforeSend: self.VM.setHeader
        });
    }

    
   

    

};
ko.applyBindings(new ContractDetailsViewModel(), document.getElementById('bdy'));