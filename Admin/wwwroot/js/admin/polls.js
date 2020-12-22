(function () {
    var $table, $tblPollDataPoint, $formEl, poll, chart;

    var validationConstraints = {
        name: {
            presence: true,
            length: {
                maximum: 100
            }
        },
        graphTypeId: {
            presence: true,
        },
        eventId: {
            presence: true,
        }
    };

    var tableParams = {
        offset: 0,
        limit: 10,
        sort: 'name',
        order: '',
        filter: ''
    };

    $(document).ready(function () {
        $table = $("#tblList");
        $tblPollDataPoint = $("#tblPollDataPoints");

        initTbl();
        loadTableData();

        $formEl = $("#poll-form");

        $("#add-new-poll").click(getNew);

        $("#btn-save-poll").click(save);

        $("#add-data-point").click(addNewDataPoint);

        $("#generate-graph").click(generateGraph);
    });

    function initTbl() {
        $table.bootstrapTable('destroy');
        $table.bootstrapTable({
            toolbar: "#tblToolbar",
            search: true,
            searchOnEnterKey: true,
            showSearchClearButton: true,
            showExport: true,
            showColumns: true,
            exportTypes: "['csv', 'excel']",
            pagination: true,
            sidePagination: "server",
            pageList: "[10, 20, 50, 100, 200]",
            cache: false,
            sortable: true,
            columns: [
                {
                    sortable: false,
                    searchable: false,
                    title: 'Actions',
                    align: 'center',
                    width: 125,
                    formatter: function (value, row, index, field) {
                        var template =
                            `<a class="btn btn-primary btn-sm edit"  title="Edit Poll">
                              <i class="fa fa-edit" aria-hidden="true"></i> 
                            </a>
                            <a class="btn btn-danger btn-sm ml-2 remove" href="javascript:" title="Delete Poll">
                              <i class="fa fa-trash" aria-hidden="true"></i>
                            </a>`;
                        return template;
                    },
                    events: {
                        'click .edit': function (e, value, row, index) {
                            e.preventDefault();
                            getDetails(row.id);
                        },
                        'click .remove': function (e, value, row, index) {
                            e.preventDefault();
                            showBootboxConfirm("Delete Poll", "Are you sure you want to delete this?", function (yes) {
                                if (yes) {
                                    axios.delete(`/api/polls/${row.id}`)
                                        .then(function () {
                                            toastr.success(appConstants.ITEM_DELETED_SUCCESSFULLY);
                                            $table.bootstrapTable('refresh');
                                        })
                                        .catch(function (err) {
                                            toastr.error(err.response.data.message);
                                        })
                                }
                            })
                        }
                    }
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "name",
                    title: "Name",
                    width: 100
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "graphType",
                    title: "Graph Type",
                    width: 100
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "event",
                    title: "Event",
                    width: 100
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "cohort",
                    title: "Cohort",
                    width: 100
                }],
            onPageChange: function (number, size) {
                var newOffset = (number - 1) * size;
                var newLimit = size;
                if (tableParams.offset == newOffset && tableParams.limit == newLimit)
                    return;

                tableParams.offset = newOffset;
                tableParams.limit = newLimit;

                loadTableData();
            },
            onSort: function (name, order) {
                tableParams.sort = name;
                tableParams.order = order;
                tableParams.offset = 0;

                loadTableData();
            },
            onRefresh: function () {
                resetTableParams();
                loadTableData();
            },
            onSearch: function (text) {
                tableParams.filter = text;
                loadTableData();
            }
        });
    }

    function initPollDataPointTbl(data) {
        $tblPollDataPoint.bootstrapTable('destroy');
        $tblPollDataPoint.bootstrapTable({
            toolbar: "#tblToolbarPollDataPoints", 
            uniqueId: "id",
            columns: [
                {
                    title: 'Actions',
                    align: 'center',
                    width: 50,
                    formatter: function (value, row, index, field) {
                        var template =
                            `<a class="btn btn-danger btn-sm ml-2 remove" href="javascript:" title="Delete Poll Data">
                              <i class="fa fa-trash" aria-hidden="true"></i>
                            </a>`;
                        return template;
                    },
                    events: {
                        'click .remove': function (e, value, row, index) {
                            e.preventDefault();
                            $tblPollDataPoint.bootstrapTable("removeByUniqueId", row.id);
                        }
                    }
                },
                {
                    field: "name",
                    title: "Name",
                    width: 100,
                    editable: {
                        type: "text",
                        showButtons: false,
                    }
                },
                {
                    field: "value",
                    title: "Value",
                    width: 100,
                    editable: {
                        type: "text",
                        showButtons: false,
                        tpl: '<input type="number" class="form-control input-sm" min="0" style="padding-right: 24px;">',
                        validate: function (value) {
                            if (!value || !value.trim() || isNaN(parseInt(value)) || parseInt(value) < 0) {
                                return 'Must be a positive integer.';
                            }
                        }
                    }
                }],
            data: data
        });
        $tblPollDataPoint.bootstrapTable("hideLoading")
    }

    function loadTableData() {
        $table.bootstrapTable('showLoading');
        var queryParams = $.param(tableParams);
        var url = `/api/polls?${queryParams}`;
        axios.get(url)
            .then(function (response) {
                $table.bootstrapTable('load', response.data);
                $table.bootstrapTable('hideLoading');
            })
            .catch(function (err) {
                toastr.error(err.response.data.Message);
            })
    }

    function resetTableParams() {
        tableParams.offset = 0;
        tableParams.limit = 10;
        tableParams.filter = '';
        tableParams.sort = 'name';
        tableParams.order = '';
    }

    function getDetails(id) {
        axios.get(`/api/polls/${id}`)
            .then(function (response) {
                poll = response.data;
                setFormData($formEl, poll);
                initPollDataPointTbl(poll.dataPoints);
                $("#poll-modal-label").text("Edit Poll");
                $("#poll-modal").modal("show");
            })
            .catch(function (err) {
                showResponseError(err)
            });
    }

    function getNew() {      
        axios.get(`/api/polls/new`)
            .then(function (response) {
                poll = response.data;
                setFormData($formEl, poll);
                initPollDataPointTbl(poll.dataPoints);
                $("#poll-modal").modal("show");
            })
            .catch(function (err) {
                showResponseError(err)
            });
    }

    function addNewDataPoint(e) {
        e.preventDefault();
        var maxId = getMaxIdForArray(poll.dataPoints, "id");
        poll.dataPoints.push({ "id": maxId, "pollId": poll.id, "name": "", "value": 0 });
        $tblPollDataPoint.bootstrapTable("load", poll.dataPoints);
    }

    function generateGraph(e) {
        e.preventDefault();
        if (chart) chart.destroy();

        try {
            var graphType = $("#graphTypeId").select2("data")[0].text;

            var series = poll.dataPoints.map(function (el) { return parseInt(el.value) });
            var labels = poll.dataPoints.map(function (el) { return el.name });

            if (graphType == graphTypes.PIE_CHART) {
                var options = {
                    series: series,
                    chart: {
                        height: 300,
                        type: 'pie',
                    },
                    labels: labels,
                    legend: {
                        position: 'top',
                        horizontalAlign: 'center'
                    }
                };

                chart = new ApexCharts(document.querySelector("#chart"), options);
                chart.render();
            }
            else if (graphType == graphTypes.BAR_CHART) {
                var options = {
                    series: [{
                        data: series
                    }],
                    chart: {
                        type: 'bar',
                        height: 350
                    },
                    plotOptions: {
                        bar: {
                            horizontal: true,
                        }
                    },
                    dataLabels: {
                        enabled: false
                    },
                    xaxis: {
                        categories: labels,
                    }
                };

                chart = new ApexCharts(document.querySelector("#chart"), options);
                chart.render();
            }
        } catch (e) {
            console.error(e);
            toastr.error("Can not generate graph.");
        }
        
    }

    function save(e) {
        e.preventDefault();

        var thisBtn = $(this);
        var originalText = thisBtn.html();
        setLoadingButton(thisBtn);

        initializeValidation($formEl, validationConstraints);

        var errorObj = isValidForm($formEl, validationConstraints);
        if (errorObj) {
            showValidationToast(errorObj);
            resetLoadingButton(thisBtn, originalText);
            return;
        }
        else resetValidationState($formEl);

        var data = formDataToJson($formEl);
        data.id = parseInt(data.id);
        data.graphTypeId = parseInt(data.graphTypeId);
        data.eventId = parseInt(data.eventId);
        data.dataPoints = poll.dataPoints;
        data.dataPoints.forEach(function (item) {
            item.value = parseFloat(item.value);
        })

        if (data.id <= 0) {
            axios.post('/api/polls', data)
                .then(function () {
                    toastr.success("Poll created successfully!");
                    resetLoadingButton(thisBtn, originalText);
                    $("#poll-modal").modal("hide");
                    loadTableData();
                })
                .catch(function (err) {
                    resetLoadingButton(thisBtn, originalText);
                    showResponseError(err);
                });
        }
        else {
            axios.put('/api/polls', data)
                .then(function () {
                    toastr.success("Poll updated successfully!");
                    resetLoadingButton(thisBtn, originalText);
                    $("#poll-modal").modal("hide");
                    loadTableData();
                })
                .catch(function (err) {
                    resetLoadingButton(thisBtn, originalText);
                    showResponseError(err);
                });
        }
    }
})();
