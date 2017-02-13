<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test_table.aspx.cs" Inherits="service_mass_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <meta name="viewport" content="width=device-width, initial-scale=1"/>
       <script src='<%=Page.ResolveClientUrl("~/resources/js/common/jquery.js")%>' type="text/javascript"></script>
       <link rel="stylesheet" type="text/css" href="<%=Page.ResolveClientUrl("~/resources/js/common/jquery-ui.css")%>"/>
       <script src='<%=Page.ResolveClientUrl("~/resources/js/common/jquery-ui.js")%>' type="text/javascript"></script>
        <script src='<%=Page.ResolveClientUrl("~/resources/datatables/js/jquery.dataTables.js")%>' type="text/javascript"></script>
        <script src='<%=Page.ResolveClientUrl("~/resources/datatables/js/dataTables.bootstrap.js")%>' type="text/javascript"></script>

       <script type="text/javascript">
               var $j = jQuery.noConflict(true);
       </script>
       <script src='<%=Page.ResolveClientUrl("~/resources/js/common/utils.js")%>' type="text/javascript"></script>
       <script src='<%=Page.ResolveClientUrl("~/resources/js/common/utilsDataTables.js")%>' type="text/javascript"></script>


        <link rel="stylesheet" type="text/css" href="<%=Page.ResolveClientUrl("~/resources/megamenu-js-master/css/style.css")%>"/>
        <link rel="stylesheet" type="text/css" href="<%=Page.ResolveClientUrl("~/resources/megamenu-js-master/css/ionicons.min.css")%>"/>
        <link rel="stylesheet" type="text/css" href="<%=Page.ResolveClientUrl("~/resources/datatables/css/jquery.dataTables.css")%>"/>
        <link rel="stylesheet" type="text/css" href="<%=Page.ResolveClientUrl("~/resources/datatables/css/jquery.dataTables_themeroller.css")%>"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      
         <input id="Button2" type="button" value="button" onclick="openPopup();return false;" />
         <input id="Button3" type="button" value="dataTable" onclick="makeTable();return false;" />
         <div id="result"></div>
    </div>
    </form>
   

      

        <script type="text/javascript">
                function openPopup() {
                    utils.dialog({
                        title: 'Save success!',
                        text: 'Do you want to return to the screen of my list ?',
                        onOk: function () {
                            
                        }
                    });
                }

            


              var createTable= function (dataList) {
                    $j('#result').html('');
                    $j('#result').html('<table id="table" class="hover cell-border display" cellspacing="0" width="100%"></table>');
                    var table = $j('#table').dataTable({
                        aaData: dataList || '',
                        bFilter: true,
                        bPaginate: false,
                        // "lengthMenu": [[25, 50, -1], [25, 50, "All"]],
                        aoColumnDefs: [

	                                {
	                                    "mRender": function (data, type, row) {
	                                        if (data == null || data == undefined || data == 'null') {
	                                            return '';
	                                        } else {
	                                            if (row.Status == 'Y') {
	                                                return data;
	                                            }
	                                            else { 
	                                                return '<span style="color:#ff0000;">' + data + '</span>';
	                                            }

	                                        }
	                                    },
	                                    "aTargets": [0, 3]
	                                },
                                      {
                                          "mRender": function (data, type, row) {
                                              if (data == 'Y') {
                                                  return '<input type="checkbox" name="' + row.Code + '" checked = "checked" />';
                                              } else {
                                                  return '<input type="checkbox" name="' + row.Code + '"/>';
                                              }

                                          },
                                          "aTargets": [1]
                                      }
                                      , {
                                          "mRender": function (data, type, row) {
                                              if (data == null || data == undefined || data == 'null') {
                                                  return '<input type="text" class="number" value="0" name="' + row.Code + '"/>';
                                              } else {
                                                  return '<input type="text" class="number" value="' + data + '" name="' + row.Code + '"/>';
                                              }

                                          },
                                          "aTargets": [2]
                                      }

	                            ],
                        "aoColumns": [
	                                {
	                                    "mData": "Code", //0
	                                    "sTitle": "Code",
	                                    "sClass": ""
	                                },
	                                {
	                                    "mData": "Status", //2
	                                    "sTitle": 'Status',
	                                    "sClass": "col_50px"
	                                },
	                                {
	                                    "mData": "VisibleRule", //3
	                                    "sTitle": 'Size',
	                                    "sClass": "col_100px"
	                                },
                                    {
                                        "mData": "Title", //1
                                        "sTitle": 'Title  (Description)',
                                        "sClass": ""
                                    }
	                            ]
                        //              ,
                        //            "rowCallback": function (row, data) {
                        //                $j('td', row).unbind('click').bind('click', function (evt) {
                        //                    evt.preventDefault();
                        //                    cmsContentAction.initDetailPage(data);
                        //                });
                        //            }
                    });
                            }


                            function makeTable() {
                                var data = [
                                                {
                                                    Code: 1,
                                                    Status: "test",
                                                    VisibleRule: 500,
                                                    Title: 'test'
                                                },
                                                  {
                                                      Code: 2,
                                                      Status: "test2",
                                                      VisibleRule: 400,
                                                      Title: 'test2'
                                                  }
                                            ];
                                  createTable(data);
                            }
        </script>
</body>
</html>
