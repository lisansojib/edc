﻿(function () {
    var $table, $formEl;

    var validationConstraints = {
        topic: {
            presence: true
        },
        startTime: {
            presence: true,
        },
        agenda: {
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

        $formEl = $("#zoom-meeting-form");

        $("#add-new").click(function (e) {
            e.preventDefault();
            $formEl.trigger("reset");
            $("#zoom-meeting-modal").modal("show");
        });

        $("#btn-save-zoom-meeting").click(save);
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
                    width: 100,
                    formatter: function (value, row, index, field) {
                        var template =
                            `<a class="btn btn-primary btn-sm view"  title="View Meeting Details">
                              <i class="fa fa-eye" aria-hidden="true"></i> 
                            </a>
                            <a class="btn btn-danger btn-sm ml-2 remove" href="javascript:" title="Delete Guest">
                              <i class="fa fa-trash" aria-hidden="true"></i>
                            </a>`;
                        return template;
                    },
                    events: {
                        'click .view': function (e, value, row, index) {
                            e.preventDefault();
                            getDetails(row.id);
                        },
                        'click .remove': function (e, value, row, index) {
                            e.preventDefault();
                            showBootboxConfirm("Delete Meeting", "Are you sure you want to delete this?", function (yes) {
                                if (yes) {
                                    axios.delete(`/api/zoom/meetings/${row.id}`)
                                        .then(function () {
                                            toastr.success(appConstants.ITEM_DELETED_SUCCESSFULLY);
                                            $table.bootstrapTable('refresh');
                                        })
                                        .catch(showResponseError)
                                }
                            })
                        },
                    }
                },
                {
                    field: "id",
                    title: "Meeting Id",
                    width: 100
                },
                {
                    field: "joinUrl",
                    title: "Join Url",
                    width: 100
                },
                {
                    field: "hostId",
                    title: "Host Id",
                    width: 100
                },
                {
                    field: "topic",
                    title: "Topic",
                    width: 100
                },
                {
                    field: "type",
                    title: "Type",
                    width: 100
                },
                {
                    field: "startTime",
                    title: "Start Time",
                    formatter: function (value, row, index, field) {
                        return formatDateToDDMMYYYY(value);
                    },
                    width: 100
                },
                {
                    field: "duration",
                    title: "Duration",
                    width: 100
                },
                {
                    field: "timezone",
                    title: "Timezone",
                    width: 100
                },
            ],
            onPageChange: function (number, size) {
                var newOffset = (number - 1) * size;
                var newLimit = size;
                if (tableParams.offset == newOffset && tableParams.limit == newLimit)
                    return;

                tableParams.offset = newOffset;
                tableParams.limit = newLimit;

                loadTableData();
            },
            onRefresh: function () {
                resetTableParams();
                loadTableData();
            }
        });
    }

    function loadTableData() {
        $table.bootstrapTable('showLoading');
        var queryParams = $.param(tableParams);
        var url = `/api/zoom/meetings?${queryParams}`;
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
        axios.get(`/api/zoom/meetings/${id}`)
            .then(function (response) {
                var data = response.data;
                setFormData($("#view-zoom-meeting-form"), data);
                $("#view-zoom-meeting-modal").modal("show");
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
        else resetValidationState($formEl);

        var data = formDataToJson($formEl);
        data.duration = parseInt(data.duration);

        axios.post('/api/zoom/meetings', data)
            .then(function () {
                toastr.success("Meeting created successfully!");
                resetLoadingButton(thisBtn, originalText);
                $("#zoom-meeting-modal").modal("hide");
                loadTableData();
            })
            .catch(function (err) {
                resetLoadingButton(thisBtn, originalText);
                showResponseError(err);
            });
    }
})();
