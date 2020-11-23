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
        emailCorp: {
            presence: true,
            email: true,
            length: {
                maximum: 500
            }
        },
        emailPersonal: {
            presence: true,
            email: true,
            length: {
                maximum: 500
            }
        },
        phoneCorp: {
            length: {
                maximum: 20
            }
        },
        title: {
            length: {
                maximum: 100
            }
        },
        linkedinUrl: {
            length: {
                maximum: 250
            }
        },
        guestTypeId: {
            presence: true
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
        loadTableData();

        $formEl = $("#guest-form");


        $("#add-new-guest").click(function () {
            $("#guest-modal-label").text("Add new Guest");
            $formEl.trigger("reset");
            initNewFileInput($("#photo"));
            $("#guest-modal").modal("show");
        });

        $("#btn-save-guest").click(save);
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
                            `<a class="btn btn-primary btn-sm edit" title="Edit Guest">
                              <i class="fa fa-edit" aria-hidden="true"></i> 
                            </a>
                            <a class="btn btn-danger btn-sm ml-2 remove" href="javascript:" title="Delete Guest">
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
                            showBootboxConfirm("Delete Guest", "Are you sure you want to delete this?", function (yes) {
                                if (yes) {
                                    axios.delete(`/api/guests/${row.id}`)
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
                    title: "First Name",
                    width: 100
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "lastName",
                    title: "Last Name",
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
                    field: "emailCorp",
                    title: "Email Corp",
                    width: 100
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "phoneCorp",
                    title: "Phone Corp",
                    width: 100
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "companyName",
                    title: "Company Name",
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
        var url = `/api/guests?${queryParams}`;
        axios.get(url)
            .then(function (response) {
                $table.bootstrapTable('load', response.data);
                $table.bootstrapTable('hideLoading');
            })
            .catch(showResponseError)
    }

    function resetTableParams() {
        tableParams.offset = 0;
        tableParams.limit = 10;
        tableParams.filter = '';
        tableParams.sort = 'name';
        tableParams.order = '';
    }

    function getDetails(id) {
        axios.get(`/api/guests/${id}`)
            .then(function (response) {
                setFormData($formEl, response.data);
                previewFileInput(response.data.id, response.data.photoUrl, $("#photo"));
                $("#guest-modal-label").text("Edit Guest");
                $("#guest-modal").modal("show");
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
        else hideValidationErrors($formEl);

        var data = getFormData($formEl);

        data = formDataToJson2(data);

        debugger;
        //data.guestTypeId = parseInt(data.guestTypeId);
        // Default
        data.guestTypeId = 2001;

        data.id = parseInt(data.id);
        debugger;

        if (isNaN(data.id) || data.id <= 0) {
            axios.post('/api/guests', data)
                .then(function () {
                    toastr.success("Guest added successfully!");
                    resetLoadingButton(thisBtn, originalText);
                    $("#guest-modal").modal("hide");
                    loadTableData();
                })
                .catch(function (err) {
                    resetLoadingButton(thisBtn, originalText);
                    showResponseError(err);
                });
        }
        else {
            axios.put('/api/guests', data)
                .then(function () {
                    toastr.success("Guest updated successfully!");
                    resetLoadingButton(thisBtn, originalText);
                    $("#guest-modal").modal("hide");
                    loadTableData();
                })
                .catch(function (err) {
                    resetLoadingButton(thisBtn, originalText);
                    showResponseError(err);
                });
        }
    }
})();

