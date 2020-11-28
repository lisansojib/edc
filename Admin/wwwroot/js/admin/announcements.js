(function () {
    var $table, $formEl;

    var validationConstraints = {
        title: {
            presence: true,
            length: {
                maximum: 100
            }
        },
        description: {
            length: {
                maximum: 1000
            }
        },
        callAction: {
            presence: true,
            length: {
                maximum: 500
            }
        },
        expiration: {
            presence: true,
        }
    };

    var tableParams = {
        offset: 0,
        limit: 10,
        sort: 'title',
        order: '',
        filter: ''
    };

    $(document).ready(function () {
        $table = $("#tblList");

        initTbl();
        loadTableData();

        $formEl = $("#announcement-form");

        $("#add-new-announcement").click(function () {
            $("#announcement-modal-label").text("Add new Announcement");
            $formEl.trigger("reset");
            $("#announcement-modal").modal("show");
        })

        $("#btn-save-announcement").click(save);
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
                            `<a class="btn btn-primary btn-sm edit"  title="Edit Announcement">
                              <i class="fa fa-edit" aria-hidden="true"></i> 
                            </a>
                            <a class="btn btn-danger btn-sm ml-2 remove" href="javascript:" title="Delete Announcement">
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
                            showBootboxConfirm("Delete Announcement", "Are you sure you want to delete this?", function (yes) {
                                if (yes) {
                                    axios.delete(`/api/announcements/${row.id}`)
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
                    field: "title",
                    title: "Title",
                    width: 100
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "description",
                    title: "Description",
                    width: 300
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "callAction",
                    title: "Call Action",
                    width: 100
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "linkUrl",
                    title: "Link Url",
                    width: 100
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "expiration",
                    title: "Expiration",
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
        var url = `/api/announcements?${queryParams}`;
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
        tableParams.sort = 'title';
        tableParams.order = '';
    }

    function getDetails(id) {
        axios.get(`/api/announcements/${id}`)
            .then(function (response) {
                setFormData($formEl, response.data);
                $("#announcement-modal-label").text("Edit Announcement");
                $("#announcement-modal").modal("show");
            })
            .catch(function (err) {
                toastr.error(err.response.data);
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
        data.id = parseInt(data.id);

        if (data.id <= 0) {
            axios.post('/api/announcements', data)
                .then(function () {
                    toastr.success("Announcement created successfully!");
                })
                .catch(function (err) {
                    toastr.error(err.response.data);
                })
                .then(function () {
                    resetLoadingButton(thisBtn, originalText);
                    $("#announcement-modal").modal("hide");
                    loadTableData();
                });
        }
        else {
            axios.put('/api/announcements', data)
                .then(function () {
                    toastr.success("Announcement updated successfully!");
                })
                .catch(function (err) {
                    toastr.error(err.response.data);
                })
                .then(function () {
                    resetLoadingButton(thisBtn, originalText);
                    $("#announcement-modal").modal("hide");
                    loadTableData();
                });
        }
    }
})();

