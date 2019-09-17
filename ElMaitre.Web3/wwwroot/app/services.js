function ServicesViewModel() {
    var self = this;

	self.VM = new CommonViewModel();

	self.baseUrl = self.VM.baseURL();

    self.service = ko.observable("");
    self.paperWork = ko.observableArray([]);


    GetService();
    self.VM.getUserNotifications();

    function GetService() {
        var pathArray = window.location.pathname.split('/');

        var serviceId = pathArray[pathArray.length - 2];
        var lawyerId = pathArray[pathArray.length - 1];
        
        $.ajax({
            type: "GET",
            url: self.baseUrl + "/api/ServiceApi/Get/" + serviceId + "/" + lawyerId,
            contentType: "application/json",
            success: function (data) {
                self.service(data);
                 
                if (self.service().CategoryId == 9) {
                    $('#PriceType1').html('رأس مال اكثر من 100000');
                    $('#PriceType2').html('رأس مال اقل من 100000');
                    $('#PriceLawyerbtn').removeAttr('hidden');
                    $('#PriceLevel2Lawyerbtn').removeAttr('hidden');
                    $('.pricing-price hr').removeAttr('hidden');
                }
                else if (self.service().CategoryId == 4) {
                    $('#PriceType2').html('السعر');
                    $('#PriceLawyer').hide();
                    $('#PriceLawyerbtn').hide();
                    $('.pricing-price hr').remove();
                    $('#PriceLevel2Lawyerbtn').removeAttr('hidden');
                }
                else {
                    $('#PriceType1').html('سعر ابتدائى');
                    $('#PriceType2').html('سعر استئناف');
                    $('#PriceLawyerbtn').removeAttr('hidden');
                    $('#PriceLevel2Lawyerbtn').removeAttr('hidden');
                    $('.pricing-price hr').removeAttr('hidden');
                }
                
                //console.log(data);
                self.paperWork(data.PaperWork.split(','));
            },
            error: function (res) {
                self.VM.handleErrorResponse(res);
            },
            beforeSend: self.VM.setHeader
        });
    }


    
    self.pay = function () {
        //window.location.href = self.baseUrl + "/Blog/details/" + id;
    }

};
ko.applyBindings(new ServicesViewModel(), document.getElementById('bdy'));