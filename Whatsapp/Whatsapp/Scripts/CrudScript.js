var CrudScript = {

    makeAjaxRequest: function (methodType,url,params,FormData) {
        return new Promise(function (resolve, reject) {
            if (methodType == "GET") {
                if (params == null) {
                    $.get(url).done(function (response) {
                        resolve(response)
                    }).fail(function (response) {
                        reject(response);
                    })
                } else {
                    $.get(url, params).done(function (response) {
                        resolve(response)
                    }).fail(function (response) {
                        reject(response);
                    })
                }
            } else {
                if (params == null) {
                    $.post(url).done(function (response) {
                        resolve(response)
                    }).fail(function (response) {
                        reject(response);
                    });
                }
                else {
                    debugger
                    $.post(url, params).done(function (response) {
                        resolve(response)
                    }).fail(function (response) {
                        reject(response);
                    });
                }
            }
            })
            }
    ,
    JqueryDataTable: function (elem, data, columns) {
        debugger
        $(elem).DataTable({
            data: data,
            buttons: [
       {
           extend: 'pdf',
           text: 'Save current page',
           exportOptions: {
               modifier: {
                   page: 'current'
               }
           }
       }
            ],
            //"aaData": data,
            "columns": columns,
            "columnDefs": [
            {
                "targets": [0],
                "visible": false,
                "searchable": false,
                className: "hide_column"
            },
            ],
            "sPaginationType": "simple_numbers", //
            "aaSorting": [],
            "bFilter": true, //
            "bInfo": false, //
            

            // success: function (data) {
            //     var html = '';
            //     $.each(data, function (key, item) {
            //         html = "<tr>"
            //         + "<td>" + item.RoleID + "</td>"
            //         + "<td>" + item.RoleName + "</td>"
            //         +"<td><button type='button' class='btn btn-primary edit mr-2' value='Edit' id='" + item.RoleID + "' '><i class='fa fa-edit'></i></button><button type='button' class='btn btn-danger delete' value='Delete' id='" + item.RoleID + "'  ><i class='fa fa-trash'></i></button></td>"
            //         "</tr>";
            //         $("#body").append(html);

            //     })



            // },
            // error: function (e) {
            //     //alert(e.statusText);
            // }
        });
    },

    JqueryDataTableforAll: function (elem, data, columns) {
        debugger
        $(elem).DataTable({
            data: data,
            buttons: [
       {
           extend: 'pdf',
           text: 'Save current page',
           exportOptions: {
               modifier: {
                   page: 'current'
               }
           }
       }
       
            ],
            autoWidth: false,
            //"aaData": data,
            "columns": columns,
            "columnDefs": [
            {
                //"targets": [0],
                //"visible": false,
                //"searchable": false,
                className: "hide_column"
            },
            ],
            dom: 'Bfrtip',
            buttons: [
              
                'csvHtml5',
                'pdfHtml5'
            ],
            "sPaginationType": "simple_numbers", //
            "aaSorting": [],
            "bFilter": true, //
            "bInfo": false, //

        });
    },
    Response:function (response) {
        var x = document.getElementById("snackbar");
        x.textContent = '' + response.Msg;

        if (response.MsgType == "1") {
            
            $('#snackbar').css("background-color", "mediumseagreen");
        }
        else {
            $('#snackbar').css("background-color", "salmon");
        }

        x.className = "show";
        setTimeout(function () { x.className = x.className.replace("show", ""); }, 3000);
    }
}