/// <reference path="../plugins/jQuery/jquery-1.12.4.js" />
/// <reference path="bootstrap/js/bootstrap.js" />
/// <reference path="app.js" />
/// <reference path="../plugins/datatables/jquery.dataTables.js" />
/// <reference path="../plugins/Highcharts/highcharts.src.js" />

var MES = function () {

    var handleInitDataTable = function () {
        if (!$().DataTable) {
            return;
        }
        $.extend($.fn.DataTable.defaults, {
            "language": {
                "sProcessing": "处理中...",
                "sLengthMenu": "显示 _MENU_ 项结果",
                "sZeroRecords": "没有匹配结果",
                "sInfo": "显示第 _START_ 至 _END_ 项结果，共 _TOTAL_ 项",
                "sInfoEmpty": "显示第 0 至 0 项结果，共 0 项",
                "sInfoFiltered": "(由 _MAX_ 项结果过滤)",
                "sInfoPostFix": "",
                "sSearch": "搜索:",
                "sUrl": "",
                "sEmptyTable": "表中数据为空",
                "sLoadingRecords": "载入中...",
                "sInfoThousands": ",",
                "oPaginate": {
                    "sFirst": "首页",
                    "sPrevious": "上页",
                    "sNext": "下页",
                    "sLast": "末页"
                },
                "oAria": {
                    "sSortAscending": ": 以升序排列此列",
                    "sSortDescending": ": 以降序排列此列"
                }
            },
            "searching": false,
            "lengthChange": false,
            "ordering": false
        });
    }

    var handleInitbootbox = function () {
        if (!$().bootbox) {
            return;
        }
        bootbox.setLocale("zh_CN");
    }

    return {
        init: function () {
            handleInitDataTable();
            handleInitbootbox();
        },

        blockUI: function (options) {
            options = $.extend(true, {}, options);
            var html = '';
            if (options.animate) {
                html = '<div class="loading-message ' + (options.boxed ? 'loading-message-boxed' : '') + '">' + '<div class="block-spinner-bar"><div class="bounce1"></div><div class="bounce2"></div><div class="bounce3"></div></div>' + '</div>';
            } else if (options.iconOnly) {
                html = '<div class="loading-message ' + (options.boxed ? 'loading-message-boxed' : '') + '"><i class="fa fa-refresh fa-spin"></i></div>';
            } else if (options.textOnly) {
                html = '<div class="loading-message ' + (options.boxed ? 'loading-message-boxed' : '') + '"><span>&nbsp;&nbsp;' + (options.message ? options.message : 'LOADING...') + '</span></div>';
            } else {
                html = '<div class="loading-message ' + (options.boxed ? 'loading-message-boxed' : '') + '"><i class="fa fa-refresh fa-spin"></i><span>&nbsp;&nbsp;' + (options.message ? options.message : 'LOADING...') + '</span></div>';
            }

            if (options.target) { // element blocking
                var el = $(options.target);
                if (el.height() <= ($(window).height())) {
                    options.cenrerY = true;
                }
                el.block({
                    message: html,
                    baseZ: options.zIndex ? options.zIndex : 1000,
                    centerY: options.cenrerY !== undefined ? options.cenrerY : false,
                    css: {
                        top: '10%',
                        border: '0',
                        padding: '0',
                        backgroundColor: 'none'
                    },
                    overlayCSS: {
                        backgroundColor: options.overlayColor ? options.overlayColor : '#555',
                        opacity: options.boxed ? 0.05 : 0.1,
                        cursor: 'wait'
                    }
                });
            } else { // page blocking
                $.blockUI({
                    message: html,
                    baseZ: options.zIndex ? options.zIndex : 1000,
                    css: {
                        border: '0',
                        padding: '0',
                        backgroundColor: 'none'
                    },
                    overlayCSS: {
                        backgroundColor: options.overlayColor ? options.overlayColor : '#555',
                        opacity: options.boxed ? 0.05 : 0.1,
                        cursor: 'wait'
                    }
                });
            }
        },

        unblockUI: function (target) {
            if (target) {
                $(target).unblock({
                    onUnblock: function () {
                        $(target).css('position', '');
                        $(target).css('zoom', '');
                    }
                });
            } else {
                $.unblockUI();
            }
        },
        /// <summary>
        ///      
        /// </summary>
        /// <param name="container" type="String">
        ///     
        /// </param>
        /// <param name="type" type="String">
        ///     折线图line; 曲线图spline; 柱状图column
        /// </param>
        /// <param name="title" type="String">
        ///     
        /// </param>
        /// <param name="series" type="Array">
        ///     {name: value,data:[]}
        /// </param>
        createChartOpt: function (container, options) {
            if (!$().highcharts) {
                return;
            }
            Highcharts.setOptions({
                global: {
                    useUTC: false
                }
            });
            MainChart = $(container).highcharts(options).highcharts();
            return MainChart;
        },

        createOptions: function (type, title, series, opposite, categories) {
            var yAxis = {
                title: {
                    text: null
                },
                lineWidth: 1,
                labels: {
                    format: '{value:.1f}'
                }
            };
            if (opposite) {
                yAxis = [{
                    title: {
                        text: null
                    },
                    lineWidth: 1,
                    labels: {
                        format: '{value:.1f}'
                    }
                }, {
                    opposite: true,
                    title: {
                        text: null
                    },
                    lineWidth: 1,
                    labels: {
                        format: '{value:.1f}'
                    }
                }]
            }
            var options = {
                chart: {
                    type: type
                },
                title: {
                    text: title
                },
                subtitle: {
                },
                xAxis: {
                    categories: categories || null,
                    title: {
                        text: null
                    },
                    allowDecimals: false
                },
                yAxis: yAxis,
                tooltip: {
                    shared: true,
                    useHTML: true,
                    formatter: function () {
                        var sHtml = this.x + '<br/>';
                        $.each(this.points, function (index) {
                            sHtml += this.series.name + ':' + this.y.toFixed(1) + '<br/>';
                        })
                        return sHtml;
                    }
                },
                plotOptions: {

                },
                legend: {

                },
                credits: {
                    enabled: false,
                    text: ''
                },
                exporting: {
                    enabled: false
                },
                series: series
            };
            return options;
        },

        createOptions_f3: function (type, title, series, opposite, categories) {
            var yAxis = {
                title: {
                    text: null
                },
                lineWidth: 1,
                labels: {
                    format: '{value:.3f}'
                }
            };
            if (opposite) {
                yAxis = [{
                    title: {
                        text: null
                    },
                    lineWidth: 1,
                    labels: {
                        format: '{value:.3f}'
                    }
                }, {
                    opposite: true,
                    title: {
                        text: null
                    },
                    lineWidth: 1,
                    labels: {
                        format: '{value:.3f}'
                    }
                }]
            }
            var options = {
                chart: {
                    type: type
                },
                title: {
                    text: title
                },
                subtitle: {
                },
                xAxis: {
                    categories: categories || null,
                    title: {
                        text: null
                    },
                    allowDecimals: false
                },
                yAxis: yAxis,
                tooltip: {
                    shared: true,
                    useHTML: true,
                    formatter: function () {
                        var sHtml = this.x + '<br/>';
                        $.each(this.points, function (index) {
                            sHtml += this.series.name + ':' + this.y.toFixed(3) + '<br/>';
                        })
                        return sHtml;
                    }
                },
                plotOptions: {

                },
                legend: {

                },
                credits: {
                    enabled: false,
                    text: ''
                },
                exporting: {
                    enabled: false
                },
                series: series
            };
            return options;
        },

        createChart: function (container, type, title, series, opposite, categories) {
            var options = MES.createOptions(type, title, series, opposite, categories);
            return MES.createChartOpt(container, options);
        },
        createChart_f3: function (container, type, title, series, opposite, categories) {
            var options = MES.createOptions_f3(type, title, series, opposite, categories);
            return MES.createChartOpt(container, options);
        }


    }

} ();

var tipDialog = function (msg, delay, type) {
    if (!$.bootstrapGrowl) {
        return;
    }
    msg = msg || '保存成功';
    delay = delay || 2000;
    type = type || 'info';
    $.bootstrapGrowl(msg, {
        ele: 'body',
        type: type,
        offset: {
            from: 'top',
            amount: 100
        },
        align: 'center',
        delay: delay
    });
}

var draw = function (oDataTable) {
    oDataTable.DataTable().draw();
}

var blockUI = function (target) {
    MES.blockUI(target);
}

var unblockUI = function (target) {
    MES.unblockUI(target);
}

var hideModal = function (oJquery) {
    oJquery = oJquery || $('.modal:visible');
    oJquery.modal('hide');
}

var doDelete = function (url, params, msg, oDataTable, callback) {
    oDataTable = oDataTable || $('.dataTable');
    msg = msg || '确认删除吗?';
    params = params || {};
    bootbox.confirm(msg, function (result) {
        if (!result) {
            return;
        }
        $.post(url, params, function (data) {
            if (typeof (callBack) == "function") callBack(data);
            tipDialog(data.message);
            draw(oDataTable);
        });
    });
}

var doSubmit = function (formId, callback) {
    var sId = '#' + formId;
    if ($(sId).valid()) {
        blockUI();
        if (typeof (callBack) == "function") callBack(formId);
        $(sId).submit();
    }
}

var doFailure = function (msg) {
    msg = msg || '系统错误';
    hideModal();
    unblockUI();
    tipDialog(msg);
}

var doSuccess = function (oDataTable, msg) {
    oDataTable = oDataTable || $('.dataTable');
    msg = msg || '保存成功';
    hideModal();
    unblockUI();
    tipDialog(msg);
    draw(oDataTable);
}