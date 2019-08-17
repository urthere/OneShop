// Write your JavaScript code.
//var chart = Highcharts.chart('container', {
//    title: {
//        text: '月'
//    },
//    credits: {
//        enabled: false
//    },
//    xAxis: {
//        categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
//    },
//    series: [{
//        data: [29.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6, 54.4]
//    }]
//});
var path = "http://" + document.location.host + '/OrderDetails/JsonIndex'; 
$.ajax({
    url: path,
    type: 'get',
    success: function (da) {
        var chart = Highcharts.chart('container', {
            title: {
                text: '2010 ~ 2016 年太阳能行业就业人员发展情况'
            },
            subtitle: {
                text: '数据来源：thesolarfoundation.com'
            },
            yAxis: {
                title: {
                    text: '就业人数'
                }
            },
            legend: {
                layout: 'vertical',
                align: 'right',
                verticalAlign: 'middle'
            },
            plotOptions: {
                series: {
                    label: {
                        connectorAllowed: false
                    },
                    pointStart: 2010
                }
            },
            series: [{
                name: '安装，实施人员',
                data: [43934, 52503, 57177, 69658, 97031, 119931, 137133]
            }],
            responsive: {
                rules: [{
                    condition: {
                        maxWidth: 500
                    },
                    chartOptions: {
                        legend: {
                            layout: 'horizontal',
                            align: 'center',
                            verticalAlign: 'bottom'
                        }
                    }
                }]
            }
        });
    },
    error: function (txt) {
        $('#container').html(path);
    }
});