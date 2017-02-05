var utilsDataTable = {};

utilsDataTable.dateToyyyyMMdd = function (d) {
    var e = "";
    if (d != null && d != "") {
        if (d.length > 10) {
            var c = d.split(" ");
            if (c != null && c.length == 2) {
                var f = c[0].split("/");
                if (f.length == 3) {
                    e = f[2] + f[1] + f[0]
                }
                f = c[1].split(":");
                if (f.length == 3) {
                    e += f[0] + f[1] + f[2]
                } else {
                    if (f.length == 2) {
                        e += f[0] + f[1]
                    }
                }
            }
        } else {
            var f = d.split("/");
            if (f.length == 3) {
                e = f[2] + f[1] + f[0]
            }
        }
    }
    return e
}

jQuery.fn.dataTableExt.oSort["mydate-asc"] = function (e, d) {
    var c = utilsDataTable.dateToyyyyMMdd(e);
    var f = utilsDataTable.dateToyyyyMMdd(d);
    return c - f
};
jQuery.fn.dataTableExt.oSort["mydate-desc"] = function (e, d) {
    var c = utilsDataTable.dateToyyyyMMdd(e);
    var f = utilsDataTable.dateToyyyyMMdd(d);
    return f - c
};