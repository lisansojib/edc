﻿(function () {
    var $table, $formEl;

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
        panelId: {
            presence: true,
        },
        originId: {
            presence: true,
        },
        pollDate: {
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

        initTbl();
        loadTableData();

        $formEl = $("#poll-form");

        $("#add-new-poll").click(getNew);

        $("#btn-save-poll").click(save);
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
                    field: "panel",
                    title: "Panel",
                    width: 100
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "origin",
                    title: "Origin",
                    width: 100
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "pollDate",
                    title: "PollDate",
                    formatter: function (value, row, index, field) {
                        return formatDateToDDMMYYYY(value);
                    },
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

    function loadTableData() {
        $table.bootstrapTable('showLoading');
        var queryParams = $.param(tableParams);
        var url = `/api/polls?${queryParams}`;
        axios.get(url)
            .then(function (response) {
                $table.bootstrapTable('load', response.data);
                console.log(response.data);
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
                var data = response.data;
                setFormData($formEl, data);

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
                var data = response.data;

                setFormData($formEl, data);

                $("#poll-modal").modal("show");
            })
            .catch(function (err) {
                showResponseError(err)
            });
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
        else hideValidationErrors($formEl);

        var data = formDataToJson($formEl);
        data.id = parseInt(data.id);
        data.graphTypeId = parseInt(data.graphTypeId);
        data.originId = parseInt(data.originId);
        data.panelId = parseInt(data.panelId);

        if (data.id <= 0) {
            axios.post('/api/polls', data)
                .then(function () {
                    toastr.success("Poll created successfully!");
                })
                .catch(function (err) {
                    showResponseError(err);
                })
                .finally(function () {
                    resetLoadingButton(thisBtn, originalText);
                    $("#poll-modal").modal("hide");
                    loadTableData();
                });
        }
        else {
            axios.put('/api/polls', data)
                .then(function () {
                    toastr.success("Poll updated successfully!");
                })
                .catch(function (err) {
                    showResponseError(err);
                })
                .finally(function () {
                    resetLoadingButton(thisBtn, originalText);
                    $("#poll-modal").modal("hide");
                    loadTableData();
                });
        }
    }

    // Temporarily here cause of reference error
    // Also in the utilities file
    function formatDateToYYYYMMDD(date) {
        if (!date) return "";
        return moment(date).format("YYYY-MM-DD");
    }
})();

