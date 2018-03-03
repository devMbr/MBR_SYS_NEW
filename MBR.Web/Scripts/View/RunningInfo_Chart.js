/// <reference path="../jquery-1.7.2-vsdoc.js" />
/// <reference path="../jquery.validate-vsdoc.js" />
/// <reference path="../jquery.jqGrid.src.js" />
/// <reference path="../Highcharts/highcharts.src.js" />
/// <reference path="../framework.js" />

$(function () {
    var MainChart;
    $(document).ready(function () {
        if (true) {
            var url = paramFromView.AllDataUrl;
            var data = {
                beginDate: $('#beginDate').val(),
                endDate: $('#endDate').val() 
            };
            $.ajax({
                url: url,
                data: data,
                success: function (data) {
                    if (data != null) {
                        var seriesData = [];
                        var yAxisData = [];
                        $.each(paramData, function (idx, val) {
                            var dataValue = [];
                            $.each(data, function (index, value) {
                                
                                dataValue[index] = [value['UpdateTime'], value[val['ParamName']]];
                            });
                            seriesData[idx] = {
                                id: val['ParamName'],
                                name: val['Title'],
                                yAxis: idx,
                                data: dataValue
                            };
                            yAxisData[idx] = {
                                lineWidth: 0,
                                gridLineWidth: 0,
                                startOnTick: false,
                                endOnTick: false,
                                labels: {
                                    enabled: false,
                                    style: {
                                        color: '#FFFFFF'
                                    }
                                },
                                title: {
                                    text: '',
                                    style: {
                                        color: '#FFFFFF'
                                    }
                                }
                            };
                        });
                        Highcharts.setOptions({
                            global: {
                                useUTC: false
                            }
                        });
                        MainChart = $('#container').highcharts({
                            chart: {
                                type: 'spline',
                                animation: Highcharts.svg,
                                marginRight: 10,
                                events: {
                                    load: function () {

                                    }
                                },
                                backgroundColor: '#2e2e2e'
                            },
                            colors: ['#7cb5ec', '#90ed7d', '#f7a35c', '#8085e9', '#f15c80', '#e4d354', '#8085e8', '#8d4653', '#91e8e1'],
                            title: {
                                text: '',
                                style: {
                                    color: '#FFFFFF'
                                }
                            },
                            xAxis: [{
                                lineWidth: 1,
                                type: 'datetime',
                                tickPixelInterval: 150,
                                title: {
                                    style: {
                                        color: '#FFFFFF'
                                    }
                                },
                                labels: {
                                    style: {
                                        color: '#FFFFFF'
                                    }
                                }
                            }],
                            yAxis: yAxisData,
                            tooltip: {
                                shared: true,
                                useHTML: true,
                                formatter: function () {
                                    var sHtml = Highcharts.dateFormat('%Y-%m-%d %H:%M:%S', this.x) + '<br/>';
                                    $.each(this.points, function (index) {
                                        sHtml += this.series.name + ':' + Highcharts.numberFormat(this.y, 2) + '<br/>';
                                    })
                                    return sHtml;
                                }
                            },
                            legend: {
                                enabled: true,
                                align: 'left',
                                verticalAlign: 'top',
                                backgroundColor: '#FFFFFF'
                            },
                            credits: {
                                enabled: false,
                                text: '*'
                            },
                            exporting: {
                                enabled: false
                            },
                            series: seriesData,
                            plotOptions: {
                                series: {
                                    visible: false
                                },
                                spline: {
                                    marker: {
                                        enabled: false
                                    },
                                    events: {
                                        legendItemClick: function () {
                                            return false;
                                        }
                                    }
                                }
                            }
                        }).highcharts();

                        var len = Highcharts.charts[0].options.colors.length;
                        $('input[type="hidden"][id*="color"]').each(function (index) {
                            var idx = index % len;
                            $(this).css('background', Highcharts.charts[0].options.colors[idx]);
                            $(this).val(Highcharts.charts[0].options.colors[idx]);
                            $(this).prev().css('background', Highcharts.charts[0].options.colors[idx]);
                        });

                    }
                    else {
                        $('#container').html('数据加载失败');
                    }
                },
                dataType: 'json',
                async: false
            });
        }
        
    });



    $('input[id="key"]').click(function () {
        var oRow = $(this).parents('tr');
        if ($(this)[0].checked) {
            UpdateChartVisible(oRow, true);
            if (selectedRow().length == 0)
                UpdateChartSelected(oRow, true);
            oRow.find('input[id*="select"]').removeAttr('disabled');
        }
        else {
            UpdateChartVisible(oRow, false);
            UpdateChartSelected(oRow, false);
            oRow.find('input[id*="select"]').attr('disabled', true);
        }
    });

    $('input[id*="select"]').click(function () {
        var lastselectedRow = selectedRow();
        var oRow = $(this).parents('tr');
        if (lastselectedRow.attr('index') == oRow.attr('index')) return;
        if (lastselectedRow.length == 1) UpdateChartSelected(lastselectedRow, false);
        UpdateChartSelected(oRow, true);
    });
    $('input[id*="range"]').click(function () {
        var oRow = $(this).parents('tr');
        var oData = paramData[oRow.attr('param')];
        if (!oData) return false;
        if (oRow.find('input[id="key"]:checked').length == 0) return;
        $('#vRangeTable').attr('index', oRow.attr('index'));
        $('#SetRangeDiv').modal('show');
    });
    $('#btnOK').on("click", function () {
        var oRow = $('tr[index="' + $('#vRangeTable').attr('index') + '"]');
        if (oRow.length == 1) {
            

            var idx = parseInt(oRow.attr('index'));
            $('#container').highcharts().yAxis[idx].update({});
        }
        $('#SetRangeDiv').modal('hide');
    });

    $('input[name="Scale"]').each(function () {
        if ($(this).val() == vScale) {
            $(this).attr('checked', true);
            return false;
        }
    });

});

var selectedRow = function () {
    return $('tr[current="true"]');
};

function UpdateChartVisible(row, visible) {
    var oRow = row;
    if (oRow.length != 1) return;
    var idx = parseInt(oRow.attr('index'));
    if (visible) {
        $('#container').highcharts().series[idx].show();
        oRow.find('input[id*="range"]').removeAttr('disabled');
    }
    else {
        $('#container').highcharts().series[idx].hide();
        oRow.find('input[id*="range"]').attr('disabled', true);
    }
}

function UpdateChartSelected(row, selected) {
    var oRow = row;
    if (oRow.length != 1) return;
    var idx = parseInt(oRow.attr('index'));
    var param = oRow.attr('param');
    if (selected) {
        if (oRow.find('input[id="key"]:checked').length != 1) return;
        $('#container').highcharts().yAxis[idx].update({
            labels: {
                enabled: true
            },
            title: {
                text: paramData[idx]['Title']
            }
        });
        oRow.find('input[id*="select"]').css('backgroundColor', 'blue');
        oRow.attr('current', 'true');
    }
    else {
        $('#container').highcharts().yAxis[idx].update({
            labels: {
                enabled: false
            },
            title: {
                text: ''
            },
            //max: null,
            //min: null,
            plotLines: []
        });
        oRow.find('input[id*="select"]').css('backgroundColor', '');
        oRow.attr('current', 'false');
    }
}

function UpdateChartColor(row, color) {
    var oRow = row;
    if (oRow.length != 1) return;
    var idx = parseInt(oRow.attr('index'));
    var param = oRow.attr('param');
    var oData = paramData[param];
    if (color) {
        if (oRow.find('input[id="key"]:checked').length != 1) return;
        $('#container').highcharts().series[idx].update({
            color: color
        });
        if (selectedRow().attr('index') == oRow.attr('index')) {
            $('#container').highcharts().yAxis[idx].update({});
        }
    }
}

function colorPickerSuccess(id, color) {
    var oRow = $('#' + id).parents('tr');
    oRow.find('#btncolor').css('background', color);
    UpdateChartColor(oRow, color);
}