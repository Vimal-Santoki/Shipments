$(document).ready(function () {
    $("[data-toggle='tooltip']").tooltip();

    $(".Qty,.PerUnitCost").blur(function () {
        DoCalculation();
    });

    repos = new Bloodhound({
        datumTokenizer: function (d) {
            return Bloodhound.tokenizers.whitespace(d.sku);
        },
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        remote: '/Shipments/GetSKUs?Query=%QUERY'
    });

    $('#SKU').bind('typeahead:selected', function (obj, selected, name) {
        $(".PerUnitCost:first").val(selected.price.replace("$", ""));
        $("#BrandID").val(selected.brandId);
        $(".PerUnitCost:first").blur();
    });

    repos.initialize();

    $('.typeahead').typeahead(null, {
        name: 'repos',
        displayKey: "sku",
        highlight: true,
        source: repos.ttAdapter(),
        templates: {
            suggestion: Handlebars.compile([
        '<p class="repo-language">{{price}}</p>',
        '<p class="repo-name">{{sku}}</p>',
        '<p class="repo-description">{{brand}}</p>',
        '<p style="display:none">{{brandId}}</p>'
        ].join(''))
        }
    });
});
function DoCalculation()
{
    var Qty=parseFloat($(".Qty:first").val());
    var ShipCost=parseFloat($(".PerUnitCost:first").val());
    Qty=isNaN(Qty)?0:Qty;
    ShipCost=isNaN(ShipCost)?0:ShipCost;
    $(".TotalCost:first").val(Qty*ShipCost);
}
function Edit(id,U,RU) {
    $.ajax({
        url: U,
        data: JSON.stringify({ "ID": id }),
        type: "POST",
        contentType: "application/json",
        success: function (data) {
            if (data == 1) {
                location.href = RU;
            }
        }
    });
}
function Delete(id, U, RU, C) {
    if (confirm("Are you sure you want to delete this " + C + "?")) {
        $.ajax({
            url: U,
            data: JSON.stringify({ "ID": id }),
            type: "POST",
            contentType: "application/json",
            success: function (data) {
                if (data == 1) {
                    location.href = RU;
                }
            }

        });
    }
}
function SubmitMe(Action,ID) {
    $("form#"+ID).attr("action",Action).submit();
}

function OpenUploadModel(Path) {
    $.ajax({
        url: Path,
        type: "GET",
        contentType: "application/json",
        success: function (data) {
            var UploadModel = $(data);
            UploadModel.modal();
        }

    });
}
  

var myApp;
myApp = myApp || (function () {
    var pleaseWaitDiv = $('<div id="myModal" class="modal fade" data-backdrop="static" data-keyboard="false">        <div class="modal-dialog model-dialog-search">            <div class="modal-content">                <div class="modal-header model-header-search">                    <h5>                        Processing...</h5>                </div>                <div class="modal-body" style="text-align:center">                    <img src="../../../../Images/loading.gif" height="50px" width="50px" />                </div>            </div>        </div>    </div>');
    return {
        showPleaseWait: function() {
            pleaseWaitDiv.modal();
        },
        hidePleaseWait: function () {
            pleaseWaitDiv.modal('hide');
        }
    };
})();

var myDownloadApp;
myDownloadApp = myDownloadApp || (function () {
    var pleaseWaitDiv = $('<div id="myModal" class="modal fade" data-backdrop="static" data-keyboard="false">        <div class="modal-dialog model-dialog-search">            <div class="modal-content">                <div class="modal-header model-header-search">                    <h5>                        Processing...</h5>                </div>                <div class="modal-body" style="text-align:center">    <img src="../../../../Images/loading.gif" height="50px" width="50px" />                  <iframe src="" id="frmdwFile" style="display:none"></iframe>                </div>            </div>        </div>    </div>');
    return {
        showPleaseWait: function() {
            
            pleaseWaitDiv.modal();
        },
        hidePleaseWait: function (url) {
        $(pleaseWaitDiv).find("#frmdwFile").attr("src",url);
            pleaseWaitDiv.modal('hide');
        }
    };
})();