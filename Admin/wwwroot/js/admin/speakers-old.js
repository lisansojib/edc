(function () {
    var $table, $formEl;

    var validationConstraints = {
        firstName: {
            presence: true,
            length: {
                maximum: 100
            }
        },
        lastName: {
            presence: true,
            length: {
                maximum: 100
            }
        },
        email: {
            presence: true,
            length: {
                maximum: 100
            }
        },
        title: {
            presence: true,
            length: {
                maximum: 100
            }
        },
        linkedInUrl: {
            //url: true,
            length: {
                maximum: 500,
            }
        },
        phone: {
            presence: true,
            length: {
                maximum: 20
            }
        }
    };

    var tableParams = {
        offset: 0,
        limit: 10,
        sort: 'firstName',
        order: '',
        filter: ''
    };

    $(document).ready(function () {
        $table = $("#tblList");

        initTbl();
        //loadTableData();

        $formEl = $("#speaker-form");

        $("#add-new-speaker").click(function () {
            $("#speaker-modal-label").text("Add new Speaker");
            $formEl.trigger("reset");
            $("#speaker-modal").modal("show");
        });

        $("#btn-save-speaker").click(save);
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
                    title: 'Actions',
                    align: 'center',
                    width: 125,
                    formatter: function (value, row, index, field) {
                        var template =
                            `<a class="btn btn-primary btn-sm edit" title="Edit Speaker">
                              <i class="fa fa-edit" aria-hidden="true"></i> 
                            </a>
                            <a class="btn btn-danger btn-sm ml-2 remove" href="javascript:" title="Delete Speaker">
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
                            showBootboxConfirm("Delete Speaker", "Are you sure you want to delete this?", function (yes) {
                                if (yes) {
                                    axios.delete(`/api/speakers/${row.id}`)
                                        .then(function () {
                                            toastr.success(appConstants.ITEM_DELETED_SUCCESSFULLY);
                                            $table.bootstrapTable('refresh');
                                        })
                                        .catch(showResponseError)
                                }
                            })
                        }
                    }
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "firstName",
                    title: "FirstName",
                    width: 100
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "lastName",
                    title: "LastName",
                    width: 100
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
                    field: "companyName",
                    title: "Company Name",
                    width: 100
                }
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
        axios.get("/api/speakers")
            .then(function (res) {
                console.log(res);
            })
            .catch(function (err) {
                debugger;
                console.error(err);
            })

        //$table.bootstrapTable('showLoading');
        //var queryParams = $.param(tableParams);
        //var url = `/api/speakers?${queryParams}`;
        //axios.get(url)
        //    .then(function (response) {
        //        $table.bootstrapTable('load', response.data);
        //        $table.bootstrapTable('hideLoading');
        //    })
        //    .catch(function (err) {
        //        debugger;
        //        showResponseError(err);
        //    });
    }

    function resetTableParams() {
        tableParams.offset = 0;
        tableParams.limit = 10;
        tableParams.filter = '';
        tableParams.sort = 'firstName';
        tableParams.order = '';
    }

    function getDetails(id) {
        axios.get(`/api/speakers/${id}`)
            .then(function (response) {
                setFormData($formEl, response.data);
                $("#speaker-modal-label").text("Edit Speaker");
                $("#speaker-modal").modal("show");
            })
            .catch(showResponseError);
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

        if (isNaN(data.id) || data.id <= 0) {
            axios.post('/api/speakers', data)
                .then(function () {
                    toastr.success("Speaker updated successfully!");
                    resetLoadingButton(thisBtn, originalText);
                    $("#speaker-modal").modal("hide");
                    loadTableData();
                })
                .catch(function (err) {
                    resetLoadingButton(thisBtn, originalText);
                    showResponseError(err);
                });
        }
        else {
            axios.put('/api/speakers', data)
                .then(function () {
                    toastr.success("Speaker updated successfully!");
                    resetLoadingButton(thisBtn, originalText);
                    $("#speaker-modal").modal("hide");
                    loadTableData();
                })
                .catch(function (err) {
                    resetLoadingButton(thisBtn, originalText);
                    showResponseError(err);
                });
        }
    }
})();

