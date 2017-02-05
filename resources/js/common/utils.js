var utils = {};
(function ($) {   
    var pDefult={
            url: '',
            type: 'POST',
            dataType: 'json',
            async:false,
            cache: false,
            success: function (data) {
                console.log(data);
            },
            error: function (xhr, status, error) {
                //$.unblockUI();
                console.log(xhr.responseText);
            }
    };

    utils.ajax = function (opt) {
        var _opt = $.extend(pDefult, opt);
        $.ajax(_opt);
    };

    utils.rp=function(selection,opt){
        var _opt = $.extend(pDefult,{
            dataType: 'text',
            success: function (data) {
            }
        }, opt);

        if( $(selection).size()==0){
          return;//skip
        }
        $.ajax({
            url: _opt.url,
            type: _opt.type,
            data:_opt.data,
            dataType: _opt.dataType,
            async :_opt.async,
            cache: _opt.cache,
            success: function(data){
                if(selection!=null && selection!=''){
//                    $j(data).find('.news_subtitle').each(function() {
//                        utils.setRow( this,3);
//                    });
                    $(selection).html(data);
                    onReady(data);
                }
                _opt.success(data);
            } ,
            error: function (xhr, status, error) {
               _opt.error(xhr, status, error);
            }
        });
    };

     utils.append=function(selection,opt){
        var _opt = $.extend(pDefult,{
            dataType: 'text',
            success: function (data) {
            }
        }, opt);

        if( $(selection).size()==0){
          return;//skip
        }
        $.ajax({
            url: _opt.url,
            type: _opt.type,
            data:_opt.data,
            dataType: _opt.dataType,
            async :_opt.async,
            cache: _opt.cache,
            success: function(data){
                if(selection!=null && selection!=''){
                    $(selection).append(data);
                    onReady(data);
                }
                _opt.success(data);
            } ,
            error: function (xhr, status, error) {
               _opt.error(xhr, status, error);
            }
        });
    };
    utils.prepend=function(selection,opt){
        var _opt = $.extend(pDefult,{
            dataType: 'text',
            success: function (data) {
            }
        }, opt);

        if( $(selection).size()==0){
          return;//skip
        }
        $.ajax({
            url: _opt.url,
            type: _opt.type,
            data:_opt.data,
            dataType: _opt.dataType,
            async :_opt.async,
            cache: _opt.cache,
            success: function(data){
                if(selection!=null && selection!=''){
                    $(selection).prepend(data);
                    onReady(data);
                }
                _opt.success(data);
            } ,
            error: function (xhr, status, error) {
               _opt.error(xhr, status, error);
            }
        });
    };
    
    utils.dialog=function(opt){
    
        if($('#dialog').size()<1){
            $('body').append('<div id="dialog"></div>')
        }
        var _opt = $.extend({
            title:'',
            text:'',
            modal:true,
            autoOpen:true,
            onOk:function(){
            },
            onclose:function(){
            },
            onCancel:function(){
            }
        }, opt);
        $('#dialog').html(_opt.text);
        $("#dialog").dialog({
            title: _opt.title,
            modal:_opt.modal,
            autoOpen:_opt.autoOpen,
            dialogClass: "no-close",
            close: function( event, ui ) {
                $('#dialog').empty();
            },
            buttons: [
                {
                    text: "OK",
                    click: function() {
                        _opt.onOk();
                        $( this ).dialog("close");
                        $( this ).dialog("destroy");
                    }
                },
                {
                    text: "Cancel",
                    click: function() {
                        _opt.onCancel();
                        $( this ).dialog("close");
                        $( this ).dialog("destroy");
                    }
                }
            ]
        });
     };
       utils.jump=function(element) {
            // var target = '#rejectreason';
            $target = $(element);
            if(($target).size()<=0){
                return;
            }
            $('html, body').stop().animate({
                'scrollTop': $target.offset().top - 40
            }, 900, 'swing', function () {
                //window.location = target;
            });
        };

       utils.datePicker=function(opt){
	        var date=new Date();
	        var _opt=$.extend({
		        defaultDate:null,
		        selection:'.date',
		        onClose:function(d){},
		        beforeShowDay:null,
		        dateFormat:'dd/mm/yy',
		        validate:null
	        },opt);

	        if(_opt.defaultDate==null || _opt.defaultDate=='' || _opt.defaultDate==undefined){
		        _opt.defaultDate=$.datepicker.formatDate(_opt.dateFormat, date);
	        }
	
	        $(_opt.selection).datepicker({
		        /* showOn: "button", */
		        showOn: "both",
		        buttonImage: "../../images/calendar.png",
		        buttonImageOnly: true,
		        dateFormat: _opt.dateFormat,
		        autoSize: true,
		        //defaultDate:_opt.defaultDate,
		        beforeShowDay:_opt.beforeShowDay,
		        beforeShow:function(d){
		        },		
		        onClose:function(d){
			        if(_opt.validate!=null && _opt.validate!=undefined){
				        _opt.validate();
			        }else{
				       // isValid();
			        }
			        _opt.onClose(d);
		        }
			
	        });
	       $(_opt.selection).datepicker("setDate",_opt.defaultDate);	
        };//end date picker

        utils.getParam=function(name){
            if(name=(new RegExp('[?&]'+encodeURIComponent(name)+'=([^&]*)')).exec(location.search)){
                return decodeURIComponent(name[1]);
            }
        };
        utils.getRow=function(selector) {
            var $j= jQuery.noConflict();
            var height = $j(selector).height();
            var line_height = $j(selector).css('line-height');
            line_height = parseFloat(line_height)
            var rows = height / line_height;
            return Math.round(rows);
        };
        utils.setRow=function(selector,row,lang){
            var $j= jQuery.noConflict();
            $j(selector).css('display','none');
            var line_height = $j(selector).css('line-height');
            line_height = parseFloat(line_height);
            var height=line_height*row;
            $j(selector).css('height',height+(line_height/2));
            $j(selector).css({'overflow' : 'hidden'});
            $j(selector).css('display','');
            utils.ellipsizeTextBox(selector,lang);
        };
        utils.ellipsizeTextBox=function(selector,lang){
            var $j= jQuery.noConflict();
            var el = $j(selector);
            var keep = el.text();
            var suffix="...more";
            if(lang==='th'||lang==='TH'){
                suffix='...มีต่อ';
            }
            while(el.prop('scrollHeight')> el.prop('offsetHeight')) {
                el.text(keep);
                el.text( el.text().substring(0, el.text().length-1));
                keep = el.text();
                el.text( el.text() + suffix);
           }  
        };
})(jQuery);


(function ($) {
    $.ajaxSetup({
        cache: false,
        timeout: 300000, //5 minite
        statusCode: {
            404: function (msg) {
                 //$.unblockUI();
            },
            440: function () {
                 //$.unblockUI(msg);
            },
            500: function (msg) {
                 //$.unblockUI();
            },
            503: function (msg) {
                utils.dialog({
                    title:'error',
                    text:msg.responseText
                });
               // $.unblockUI();
            }

        },
        beforeSend: function () {
           /* $.blockUI({
                message:'<img src="data:image/gif;base64,R0lGODlhQABAAPMAAP4EBO2usrPJ39/o8X2jyPT3+rptgP4KCjdnn/7+/v59feUwN+3b4Od0e9fBzPHHyyH/C05FVFNDQVBFMi4wAwEAAAAh/hoiQ3JlYXRlZCB3aXRoIENoaW1wbHkuY29tIgAh+QQJBQAAACwAAAAAQABAAAAE/xDISau9OOvN+yxD4Y1klhQJNQhDBYplrL0qW62trFtgLuEVgW1HlCQGg9RvaBQqPwJY0YN8AqECi4CQnXpoSx9g1Z0MCASpd9MzMwFCcQJdXm+OvqtEqN4SngAFaXYXVWFYFIIEYooEhBZIMHpkFFt1jWqPgSGHe0yKPpiaFXidcFETlh9og6MUeClXCU4/aDCirjecV5QAc1wSuGaLozS8Q34pwr4CCAiOmifKnJsioIGsfQTOxLkaqsJn3GLeGAVRuILOCAKA5RvZzOut71+D4s/k9VTbz3X7JPq1y3CuG8AbBmv043bwzoqF6/I1LARxHgt3EylA5KIvI0VUHkZDiqwnQIHJkyhTqlx58t8aBQdiypxJs6ZNmQpGlWTJs2fLkUCDCh1KtKjRo0iTKl3KtKnTp1CjSp1KtarVq1izat3KdWsEACH5BAkFAAAALAAAAABAAEAAAAT/EMhJq7046837LKAnjmMxFJWJkiypVoMwtLT3UvFa79gtJQJBIiXQ8Vo+QCFoCQqOtWRsRlkWoTTpdQKUYbMnStAYE1a3X43PSulSJQUC4ZlWh+HeibMqJxjrFj5TH2MffX+AKXcAhRJ7cIeJditdQwBllnFyiJJVd0tUbpCbRJxpN4OXQUOafm0CcnSdICsvViitZH2unRp7uRMDu2+9GUsgkUqwsZbFHcsoQH1ozh3Hl8PVSNnaLctmPUHE3YScMXII6bLkGAnn6fAIc+PsEu/x8jLN9U3wsfT8LpQxFbCgQUAOGjggqGeXw4frvjQ4QPGAQlME8GmMRyBRwoogSy+KeUgyVq8CH0FSFHnwAsqJKi0ubOkypUqWNKvYDDkzp06YNx34TLGzwdAeCYUeXcq0qdOnUKNKnUq1qtWrWLNq3cq1q9evYAFEAAAh+QQJBQAAACwAAAAAQABAAAAE/xDISau9OOvNu/9g2BWkaJ5UMRRoKw6rK3cJnFQqO+/qYA0C327WwwkEuqELlgTUgsqZjQIU3CYFZBRUE0oSRy8ge9x+ihOgmKw1d5jYY5LddGvgEnVcbqdNAVVXdH0jMF9hEoMpbYRYMYBBN4qHBAQCjSklZCyTgJWWdZh5UJNAn2KiKSuKWadXqRlHWjWfjLAZWSwDtai3Gq2Vvb4aApVWuHrDibZpR5+WyhdPz89Q0aPUvK/XEsW11twXaqHh5eYnDAcMb7Lt7rLCMgfzDdsXAgj5+vv8l1vp89TdeUcQkRuA89adk9YgYL0Ku5hFQyhwAoF9luLBoqhwF798GUrtpUrQkN6raR8RENDYh6O4YhgnBqx4oRXLRiQdilwIwCVPCz5/TshpUiiFoEZ7zlSYlOiBpEdpQp1KtarVq1izat3KtavXr1MjAAAh+QQJBQAAACwAAAAAQABAAAAE/xDISau9OOvNu/9gKI5kaZ5oqn5JkaywVQxFbAPz8N7wQPOw3E6S0AFHxd+kKBgcRTkK0/hk+YbTYbXjqxEHTe12I/yGx56kN1spCLxoShTAXoIFgri0SwdTJXd4cHpRdTh4eE56Ui51CYhnixd+L0yIYpJ7L4FvmRxuiIqeG6GYEzNNo26Dp2AErwR5o3t4sLCJswB3tq+JppkCtk2suRN+xMXJyiEFC8gWnJDSuDYLBwfPFMG83Lc3BdfYG9HTkKIx4NfZyunik7HrY+3IBAj2veeE4cQDr/b/CGL9kbcPA5N6AAMSyHdkHqCFk7b9IxDHIUIC2dxArLiv3z9Zy0kcCpjIcJbDRxPj6VMnoQBCBAJ+SXL48CO7ghNG3is5E2fLizdZUujHMxPNZTJ8IpWjdGnLpk7TLXAqwxnVq1izat3KtatXTxEAACH5BAkFAAAALAAAAABAAEAAAAT/EMhJq7046827/2AojmRpnmiqrmzrvnAsx0mRzGkyDAV+6rubjwQcCIei4hEJUjKTO2NFt3xOo0udQDCwXpyTwpZb9YIBhcG4570GJdqttF1JS+Nk+teGHvP0GnhsgBlqeRhAg2ZzdYZ+hBM1jmNyim1plJQ8ZXSTXJaQEnacoaWmHQwLDB2emZldLgsHCx0CBLe4uboCLwwHB6sbra6PLwa/pKYJvwYaan96vsAZtrhybbK0GGq61ow40sEZWt23XD7HB1UFBKAAhrm8OMsHzRQECAgCyZiw4L/i0OBDQEAepGyi2r0bSMBfNIAABvZIICAfQXdP0tWwyIadxX16VOg1K8BxwgCGDq2EQ1Nygq18CrHNEtUSTkWCdFayzGeJHcYhGmnyPCUyTM1QOnci+ClTm9ClppLuJHAqaJiYpYqeuiB16wSEXiukChi2rNmzaJ9EAAAh+QQJBQAGACwAAAAAQABAAAAE/9DISau9OOvNu/9gKI5kaZ5oqq5s675wLM90bd94ru+8mhSJnuc3GBSEnELRGERiEstB02khFo/Ui/I6zU6gy67XYDWOLVtghojl/cQS8EBAH3it9Lzg6tzq60xec4BSZxNKaoaKixcBCgEdUZKTRS8KAAqRf5t6di4BAAeRlKSeLQUAAG2MBgcAkBpzhT2XmRoCBLl1qzagorF0ucIEdXAvqKodYLjDxHszrrBoArwSSsG6M7UYzALGcbIzvhgFzASmPMi8BQRHA8PoOtEVBAjtZObUPNsG70b19wyUE+ZNxzgDAAUCbPNOWDwa6vohQGCHnb1VDYlVkzHPgICJQV4sBozTDQe/BBMFWFtYodzGGAc9TsQi8uWNiBISrryIpKNEiodY0sJEQefOkQZDUSiQcqlRHTglfETQhZ3NGj7JNB3Db8KAiQ+FxJzwtE8qXv7OZGXUlZUjaazivogAACH5BAkFAAAALAAAAABAAEAAAAT/EMhJq7046827/2AojmRpnmiqrmzrvnAsz3Rt33iu73zv/8CgcEgsGnuJQiFxBCQLg+igMEwkpdgp83fNRpfbIFSqDBef5qa6KVAIOmOvnOpSHBSdgWDP7/sHL3Z4HHFyWXQtgmsTihtSaTiNGnp9UzmSGVF+fFGQdXcfCZqbAlExmBYFAogUUH+noBkCBAQCnhKipjCoFaq0BIA8vBIFBFQDv6U7wwC/VKLJrDWoyFPOxLO0A7eBsRMECAQAxbSIer/BNIrVAAMICIDkxhTItKvqoODiAALhTPKsEmR7M0NRPwS43hEE2OteQVAF3m05SIehjkbv0umbYDFSLHAETdtl5HjN46B++yRsJFnOpAR3CDkqbPXLpYSREw6aKSbt4aBmCEI6AflDEsoKMNMt8wbTwkph3hK8k8auB6aZRTA9HYKpKhFmRdoIVREBACH5BAkFAAAALAAAAABAAEAAAAT/EMhJq7046827/2AojmRpnmiqrmzrvnAsz3Rt33iu73zv/8CgcEgsGo/IpHLJbDqdj8WjU6har9jqa3FYUAfgsHhceB0OU052fXUxzgnmo/stxHfc9GYg6IfLNglnDF98fYdgdjJzByEJBWCHiAOALVxeGwUClRSQkX5maByIdxePlG5wapIDOnOYGZplhn2cNHkbAgQEZQm0AqUyggeEFwW8x7uACazBL4wZuwQAybwTmogylxUDBK3J39Kcv7YrZ1O6AgAECNMAugRx1Zy+hy9vB3Hs0wLtEgm71FETV0HWMzrr/BVAgOBON2sDldnAldAdu1YSAl4jOGNYsX0ZahEIBPAQY0SIMaCFdNfPnQR4nTjC2LZSwgCGMb3lHBlKD0gJDE2+2xXMIAx8pX4m5AmQAE8arygobclN4kRRE5TedJl1V6BBUv39Y2jrWDkYKrOKDfk0B021XJXuiKKn5oRuQoXITZLORQQAIfkECQUAAAAsAAAAAEAAQAAABP8QyEmrvTjrzbv/YCiOZGmeaKqubOu+cCzPdG3feK7vfO//wKBwSCwaj8ikcslsOp2MBaO5OCyYjMNhqkxoG8yGNtEpmMm57NZTGLgHZ1v1ym6/7/GXmgtKmO94BSxeB2BlcBd+dm6CK2IHaBwDAgIDkRaKjSl7H22UlTdzbAKCnpWaMZwdn4IJk5SWMYSGGgWktpSNrp+xLo+XGAIEAgC4pBOmiCx7AwQDFpPPBQTOxawUr6crogQIBBLRAMLE4tRkxqi7oJtaUwgI5ALwAM3fAAnUz9a5FW2oJbPuvWskj9g0AmjqNUIH4xe9dxMKSsgXcRiyay1UdbMnbl45cvRzKEpgyELURI8d41GjMI4CSXZrJLzTlxLcSmQiR35SERAcxIgoc4ZE6PJYCocS5HGsOdHiBHxOXag6uVRi0qg2CfxTYXLCTJYo61loKVWKy3eXrN6j9s/WVhjyEFRQC0Cojo1zUX700YxmUr3hitBNEphFBAAh+QQJBQAGACwAAAAAQABAAAAE/9DISau9OOvNu/9gKI5kaZ5oqq5s675wLM90bd94ru987//AoHBILBqPyKRyyWw6nYVFgVk4ABZLhvUwTWoBXOVj20WOweXjeZFQBgDXdvIdByUK8hq9ISoMBng0eyMJf39pLoMhhW1+hnksDXABIwMCA3KOgJAokgCUAwQDHQUCeJaYEneGiCQJnpQGBAgEHQK3BoWXeYWsJgkLkxIDCAh/ohaoBqWXuaiQmqkiwMK5xbWztQbKAgQCw7dtutKqjq0c1AcPEwLFbbOjBt3fod+5t/HjnHecHFVg6yQUKPYtQbEu85YRICDHkgA5+k78U0ch2zBa7LxJWBhPXjNVz45KTGRAgZgxCe20ydO4siOzMhH7bCFZEeNGBPZW2qtXEteEmCCChaHQDkGZYh0TKlR57yNIpx+iIDKIc4JJoixlEUjjEBIjFbMQ5EmJNWe3jk3RsjCJdlZOnVazgjP1wmJNtEq3LbRw6+2KUGgHii37cyGiUudeEGMKN6PaHHYzvs27A3AFysqKUE6SmUUEACH5BAkFAAAALAAAAABAAEAAAAT/EMhJq7046827/2AojmRpnmiqrmzrvnAsz3Rt33iu73zv/8CgcEgsGo/IpHLJbDqdiUNiGT1YlQSrdXqsWgVI74HALYqlyKy2bNYewF032nhmEwVu+1BMABQIBSQJA3ondX4ICH0jA42FIwlqWxIFiQiBAwQDHQUDBYOOKHhrE5WJgQAEih0DApughCV8FKaXEgOJja4WjZsFAgKBsI8cebSWqJGrAgRwrZuZmwDPU8OQc6XIEwKnqZoSzGCZcAnA0tYotaiICGCRgODNfgRkt8Bc6CXqFKqLmYsAwsWTNm2XhHwi9k3AhUAaM4ACA36jBGwdwg8KJ/TTKC+es472awgCuNhBlS0K3E56IxjxD5yD5ijASgjvWLuF9FCCZLauoAA2g4iF6Ffmoc6XzEQOMuiCoUh6LyW+HFfhV7AXG/lN9IgzasgX0WjRYxNx5Niqnmz8q1BWokgdbaVSCNvjGVuQ0xoZiYvEbosIACH5BAkFAAEALAAAAABAAEAAAAT/MMhJq7046827/2AojmRpnmiqrmzrvnAsz3Rt33iu73zv/8CgcEgsGo/IpHLJbDqbBUfDgRxID4BsgzhoNLDZ7GEKlH7DgMPYUfAlrmDxOuGGo9VTei/6jae9VD9XaH8NA0QNYWuHFgMEjDxSbBkFBAgIBEoCl5iQRY6cAgGVbUOVoXSnmUEJm5cEpaptBQKlGwkFeiuunROyEgQEohwFA7YooJfDEr8BjrADAp7MxgEJA9UplpjHzQnBogLCEtiH0YzF0yWO080B4gR04ozno9IS19ku7pWP1sGl6rUSoCedrhXu3gUjt5DcvXe1JmA7liLhM0biltVz9nDURISWamBVADdhnsSHG6mpKxGSIjxbAE+iE7DM2seKIil8G8esoUNINLthO9gCni5HNVOmJKevRT9PGSko7aiSqAp4I/3J9EXTQjGKKmh1C3Yw5UCiuKzCQFphaTSwOKJKpbpUR92lX43UrYLNRQQAIfkECQUAAAAsAAAAAEAAQAAABP8QyEmrvTjrzbv/YCiOZGmeaKqubOu+cCzPdG3feK7vfO//wKBwSCwaj8ikcslsOmEChXRKrVqvU8FLceh6v+Cw2Kt4RbHodPbZKwgIA+TgjagjCMPEnGDvw4EDfH11BAIDCUOChIYXgXE9gQIFGW6ESgJ2f0eBhHFuk0MFigKIooVCCZiEoKYEk5+QgpoAraBvWjqcdbgStRKBrgUDoBQFxi+CksUEzKCpp3OPwq/DL44Vvr/MiAKMANG0A4802bTMcamkv4x6h+TMrhS367zg39Uy5d/n9BP20/ngEZMwj6C3b96EjTPTrAIwYt2I2WuXTxmFBPx6daNg7x4iHLc7PiLkhXCcuIHvZgHotrAjwBsFJ7DkeFAhDlgT3Kjzd7CdyB1zSJbkiI9Hx3ULX/JQirTYsSJHkzBVEQEAIfkECQUAAQAsAAAAAEAAQAAABP8wyEmrvTjrzbv/YCiOZGmeaKqubOu+cCzPdG3feK7vfO//wKBwSCzuHAZHwWgxAACHQ5IpQR6ez8NCSS04Fles1JFgJqxY6MKwZHoN4WyybSwYwOlx2XiG57d0LwIEhIWGhwIVXnhiUy4ECJGSk5QEGVZhBoKHnIaJG28OVEQFgwNUA4OQkZZCBamrk4SnPwOElAgEAgN7QbGEuxcFBIE6tgLFFKWFowGqhMlCtoWnpdE9y8Blw7pCCc/IEtzEAdY907MT420C7Tvj3eqFbanIr9cwheHy0BLfwaloFRg4Y1ixdRPqlUnV5p4OhOLanfq3xyEOiBLafSr1qdwAfC9QMJZrRy+YuI83RDpzJyGgOpQ2VBWrR4fkS5AtzFH4R2tkR484aShMaPJkUBkcewZwedOHxgo2m2LbF1FAL6BFSinFSoSp1CEWvzYLO5ZgiwgAIfkECQUAAQAsAAAAAEAAQAAABP8wyEmrvTjrzbv/YCiOZGmeaKqubOu+cCzPdG3feK7v/M0ojB5IATg0EsLOD1AMJjmMA/PgfGYSDSkAaNUUssVjNxOdVnsDgqCAKTeFBIQcoR5UsNqGfZeOz+UEBANIAQMNU2I8CX1/gIIBblRWAwJ+gBORZ0kFlXsSeGFsYxdLYYSjFZkxlAKtrq+vnqRaXC4Cjbh/AhugCi+ssMGtsmS1qKgFlMRjrIGBu12Llc7UAss6lNTVg2PTz9YXnKI8yuMWCdPQxxJ9auaj2YF24qOczgJInK3R3msS+v6SvcPRDtw/V6KU7bAnjwJAUfoKSBwoI92pAA8/DcM4gGIMeg5SEU5ghWRAx00iNYJbxK1HxpEGk1278fKgP44ea9RkZ7DQSR4pYeIb2XLhzQnoerI8RnKCTGStzAlEpZCCyZw8piI1eXEdx5nIfnp1KnGs2TERAAAh+QQJBQAAACwAAAAAQABAAAAE/xDISau9OOvNu/9gKI5kaZ5oqq5s675wjBZLIa/Lcdj3rO89n44XLBV+xOLoOFQakc7NgDCwMIFRDAGBICQB129WMuUiBJXw+DIwUynqdSUhcBPjcviWK0hI8HkTZVxVYFBOUwJiEnVcb4A9e10EAoUTBXsEf4dBU5JulX4AU5aQPQkDAp+Ob2mcWakEkppWr2sFqpauTYEcpr21vC6pAsXGx8e6F78oqrLP0NFoG0wLL8TI2cXKGDSLwOB/qdzgBbHP08CoxdGUlb3E7duiwM6ylYu430Xj++vF4dgc2xcIl7EqqOipS+ZnHbkx/4qJctiwgMIo2BRNoCiOYAyD20socAQzYMDFIgcVjhw1wONHjSLH0TNnMtxKVC1tyoRTUmeomDXV7aRQ0qWSoZdKnpSTcE7RgN16QrWA02jBnFOpWszKtavXr2BJRAAAIfkECQUAAAAsAAAAAEAAQAAABP8QyEmrvTjrzbv/YCiOZGmeaKoxC6O+2HIscE0xx/HYvJEnPFOBUKAkcoZgiYBAFCePnEspKjSdFN+BOrI2n4BCbscNebGS6AFc9pzByLb5+sTp5JchW/KWaIF4FEwIAoATfUcHSYGHg4QVfWpTjBIDgwQDFH0yNJQTCQJXRIdNBAB2ZJ58joWrT3+qFJalmRWJi2UDAgN7E6Gle5JtAgTFBAK7tr+mFVptusbRxwOAehZid3IJ0NLFyRhqvc/I0QIacbFhurUYqOkfsO8ct0G6yPf4+ewZwjbE3QCPdeDEw16+g8j2ZWAxSZ5DDNsEiHNYwOC9hwASVESIbGKgAhxidxUwJO8grwvbSFICSS1DxG8YId1rGfOQwSIaVb57uQvItpMOedL8SfMdyIRgiOqkxJONUof2imYcQHWpp5S2qEqtObUqVyNarT58+lUC2bLqtn7Nibat27dw48qdS7duhwgAIfkECQUAAAAsAAAAAEAAQAAABP8QyEmrvTjrzbv/4OcYTmiel3EYaIs6x+HO2EAMVRIndD8RCIIlVvL1BggEjqJaGH1ABG8CO0yfnwKhQEkkBRUiFgTcUgRJ7mSxGmfLakmZUnW/g3GkcqKz2j1aeD9BFGJ/HYFmAAVfEyosh4hwEmhSEnWRFFpxE4lcXoQAfVeZBKYCpIuTektsRZmqpjcVngBzADBOsBM2sksSnjZLo7t8ArKKqqGFB6/FsadTmxaPzxW9pr8XmH8DAgOcFNjJOTuHx7Lf4QnoQhiGdt7I6d/S5BVNsAne7cjfG9yKFZCXbkMfZ9YG/tvABpI1EwEffiA2ZoDFixgzWuwA78m4eSBvtWWoVlGjyYsdRiCUyBJEgoHhWloYaFGATZEyX9a0ydNizIc0efYEl0omv28+L7ws+gwmUwk6wcnkQJPo1Az7Lv68KqrqVq5en16tOkDs1KhSuWKgWcAs16Vq48qdS7eu3bt48+rdy7ev37+Ax0QAACH5BAkFAAAALAAAAABAAEAAAAT/EMhJq7046837fMvjjWQ2EAO1HEvpvgRCqOxrj4JMPfXtSwVCoVJAIBKf3u8Wm1WMKQmvtbwVEdFJM0mt2prISc4JmHo9weEkiBBQBkbuuUOoqyVwxB0AlSrnGUF2FFtabX5dgIF1QhN5WYUMBweKHIKNEoUAeRKSlJUbl2pXUQlGQwOToJaMamNqMW4JqqtAAntArQAJhVuzn7UCdQNha7p5qCi7k8WgBcLDRLqaE8y1joyYuXZpFtbXEgkndbfGdRmTWeAAz4zEubgUkwzr0u4d884DA/F4jOUaVoioNI6AgH24xJHbIFCfAGjkDhYI86wfjYGrEhTYB9GgxA4Nbuux43iPQ0iRE8TxA3kAI0oXJ1+WiHlm476bOHNarLAi0ZmcQIGy9OnFZlCgO3eEkMn0h8YCSZuGg4ozKsqnRqtaBUcVKdRmUoFoBRuWyNeyaNOqXcu2rdu3cOPKnUu3rt27ePPq3cu3r9+/byMAACH5BAkFAAEALAAAAABAAEAAAAT/MMhJq7046837FITgjWQ2EANFIET1NE8pb2s71VUDNHN/CSxVkHIAxHykAqFQKSAQidtQkgAAosgRYWtZpSS4yQNwyCa3X6kIPA3oeOYRiICVAG2BsGRhjGMKAkwTCVtrEgNPUngJRXV+FHOCdluSAU9femNljxZKlCohQmt6b5wXnksTJygfQ3p8R6ZNW6mTeIg2YYwADLKntIKeawlPTGGavn/AtlgrImE6C76AlRKoTISheUFhsL4C4AKOAdcBq0wnKSBrRbGcgOFp1su0GcjJ5uGBFNdK1RRjpOELkGCAPknCOPCBM5CcvgFYqHFo15DfwxEMyIwzU2BAgY2HfA52CCjLYDyPFRLo61DKVEd9Jz9a2zexT7IEHU3CRNmhygGQ0wboFCBPA8mKFnDy5NASKRKKTn1UuSKrgNWrWLNaHUkmqNCvYMP+u9D0ndazV0fCiMq2rdu3cOPKnUu3rt27ePPq3cu3r9+/gAMLHky4sOHDiBMrXsx4cAQAIfkECQUAAAAsAAAAAEAAQAAABP8QyEmrvTjrzfscwuCNZAaKk4AIlUCwZay5sKTWEoEQcn8NBF5qVUEQfaSCoFApBBNDXMLIpDAWDCSGZgmiADfKwGhZHBbai5Pwtb2iFJVwwjgcsmmLiwBNBeETOlJmBnlKVRIJQThAc2ETRm11B4hIApeVjZVeNkcAYwgUBXZ4aUqYcW+Bqo9gOxQGZ3kTp0u0QYhcrjWCdKSztJe2bjWNnTWRE2ZowMGoAGsoigRMjwVGfZOlzdDCVXt9nI8qoQAJdoWmA5UStUzTMFyPOkKxB31aICH47d6fuJ9CBETBZcAvQ/qGObOli8MybrXWifJ2qIM2buYSDsBXiwQhjBOKEmjsU9HDpDYgCyRk1+HcgXSmChTgN/GSxBH2aPpQOaCnTJoibY64OEtmz6NHf7a72eEhxgRGkSZlqYEoyJBRfXp8eTUDVJlD7VDtKmPUHbKzYi3QiTaG1bY+nMJFcmXb3Lt48+rdy7ev37+AAwseTLiw4cOIEytezLix48eQI0ueTLmy5cuYfUQAACH5BAkFAAYALAAAAABAAEAAAAT/0MhJq7046837HMLgjWQGipNACNVAoGWcqWy6VgRCyPzl7rbaJCfseQqCQqVAICSClAQCoaS4YMYKzdKEbT86bjhrQb60N8lXPU5NseTP6ikXrg3E6BSYTQzoEgkqWEx8d28UAlNVEgMLcB0gAoAGLpMUTVVrA1MUBVNFCQcACzGCApdQNihrinx4nROiAAeQHaepBoWMX2t5jYiBC7S2HriAm01qabAwe7LDtX2oqYJnBgmZBl+fCHSKVMLEcceNcxJd22muEt01CdHFGgkFlIHUT9asN185QDnesCkYF6PAgAH1KpSzpOREpRDmRHBCUGPgAQc86B1EqBCfuiIamPxJsIjRiMGNlI4hYbThioGBAALE0bUx4T2IMhoAiDkz0EmOgf7k3NmgZ5SNQHkEIGq0TE17HpYCUND0gsakIxyMoloVAz2oG7RO7Wp0wFayPc2SAos2htoFbNuSeBtXrge6do2oPVA3L4doff22fCS4sOHDiBMrXsy4sePHkCNLnky5suXLmDNr3sy5s+fPoEOLHk26NOcIACH5BAkFAAAALAAAAABAAEAAAAT/EMhJq7046837HMLgjWRWDAUlEEIFimWsvSpbra2sW2Au4RUCAbYbnVKUgkCQmAAnCSFyUiBMi66QZTl9SgZCCwFBwF6UgitgSfSubZQBAqE2f0PNz9IJlwh9AFEIgDsJBXkSCWxJez99AFIqc1dVdRwnA4gAaJpcjj5gZVRzgAVzoiQJA6uaNE5abz4rqJBkSXN0MqqsVJ53OV5ecghEppOFq5l6xY1eQkRjqMa5RbvKgb6KabESVQR5AsebuJYx1nk94FpPod2k7uIfB0SpyU2qsDRP+2RN01dRDgjUcW6TpyMAXNEwJuKfCoEC6ZUo6IpDtHHxEkA8QIugPYQdfSo5/LFRU7VV5SyK0wiRkBlDJo1YIQkxph07Aza6vGmHQEmeQBNulBi02saORYsIGJrUDkuBO5vGWFpTKpanB6JaHeGz6lYdOSES/UoC4jeyRXKORcu2rdu3cOPKnUu3rt27ePPq3cu3r9+/gAMLHky4sOHDiBMrXsy4cYwIACH5BAkFAAEALAAAAABAAEAAAAT/MMhJq7046837HKAnjlkxFNQgDJXKkrDmpmslEEKsX0X9+ZPbaydKFBKVhECAlMwmCQIBRSlMiRgTlbZ9OqUWKQF7SYC2TqA3cMulpGjyxDRMD9dCShQnL5+rSz91cBQ3BE0SVnE7dIhsAlReVmNzUnVWCJRyf4J2H3xBYJWZi4ycAU9ebZ8EQ5hXfYkhiT5elhNSbgGvpVhmJxJKTKg+k00DhLsEpLEVjWkoqaB7brwtrX2cM0+rbFJI1qOZfc9aqLMzkyzhEgII79jkwB1iysxf7wi6fUaOGorhML0b1mzHsitK8sEqyOjKAIV1GBJBlm+fRCzL7pVx0MDBRQ/IdCJ+aHDgAAAADT7qSMDR5EkABzqqHDGApEuYMUXOxMCS5EuYHf3txFCz5MuSDXQOTdLyZ0wHQpe2sHk0p9QNTY8GvdqhwUmkSrli4AhVrNmzaNOqXcu2rdu3cOPKnUu3rt27ePPq3cu3r9+/gAMLHky48MwIADs="/>',
                css:{
                   border:'0px solid #ffffff',
                   cursor:'wait',
                   backgroundColor:'none'
                },
                overlayCss:{
                    backgroundColor:'#ffffff',
                    opacity:'0.0',
                    cursor:'wait'
                }
            });*/
            
        },
        complete: function () {
            //$.unblockUI();
        }//,
        //error: handleAjaxError
    });
})(jQuery);



$(function () {
    console.log("utils.js start...");
    console.log("require jqueryUI, blockUI.js");
    $j=jQuery.noConflict();
});

function onReady(data){
    //console.log('default page success');
}
