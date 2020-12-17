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
        phone: {
            length: {
                maximum: 20
            }
        },
        PhoneCorp: {
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
        }
    };

    var tableParams = {
        offset: 0,
        limit: 10,
        sort: 'username',
        order: '',
        filter: ''
    };

    $(document).ready(function () {
        $table = $("#tblList");

        initTbl();
        loadTableData();

        $formEl = $("#participant-form");

        $("#add-new-participant").click(function () {
            $("#participant-modal-label").text("Add new Participant");
            $formEl.trigger("reset");
            initNewFileInput($("#photo"));
            $("#participant-modal").modal("show");
        });

        $("#btn-save-participant").click(save);
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
                            `<a class="btn btn-primary btn-sm edit" title="Edit Participant">
                              <i class="fa fa-edit" aria-hidden="true"></i> 
                            </a>
                            <a class="btn btn-danger btn-sm ml-2 remove" href="javascript:" title="Delete Participant">
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
                            showBootboxConfirm("Delete Participant", "Are you sure you want to delete this?", function (yes) {
                                if (yes) {
                                    axios.delete(`/api/participants/${row.id}`)
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
                    field: "username",
                    title: "Username",
                    width: 100
                },
                {
                    field: "password",
                    title: "Password",
                    width: 100
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "email",
                    title: "Email",
                    width: 100
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
                {
                    sortable: true,
                    searchable: false,
                    field: "active",
                    title: "Active",
                    width: 100,
                    formatter: function (value, row, index, field) {
                        return value ? "Yes" : "No";
                    }
                },
                {
                    sortable: true,
                    searchable: false,
                    field: "verified",
                    title: "Verified",
                    width: 100,
                    formatter: function (value, row, index, field) {
                        return value ? "Yes" : "No";
                    }
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
        $table.bootstrapTable('showLoading');
        var queryParams = $.param(tableParams);
        var url = `/api/participants?${queryParams}`;
        axios.get(url)
            .then(function (response) {
                $table.bootstrapTable('load', response.data);
                $table.bootstrapTable('hideLoading');
            })
            .catch(function (err) {
                $table.bootstrapTable('hideLoading');
                showResponseError(err);
            })
    }

    function resetTableParams() {
        tableParams.offset = 0;
        tableParams.limit = 10;
        tableParams.filter = '';
        tableParams.sort = 'username';
        tableParams.order = '';
    }

    function getDetails(id) {
        axios.get(`/api/participants/${id}`)
            .then(function (response) {
                setFormData($formEl, response.data);
                previewFileInput(response.data.id, response.data.photoUrl, $("#photo"));
                $("#participant-modal-label").text("Edit Participant");
                $("#participant-modal").modal("show");
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

        debugger;
        var formData = getFormData($formEl);
        var id = parseInt($formEl.find("#Id").val());
  
        var files = $("#photo")[0].files;
        if (files.length > 0) formData.append("photo", files[0]);

        var email = $("input[name='primaryEmail']:checked").val() === 'work' ? $formEl.find("#emailCorp") : $formEl.find("#emailPersonal");
        formData.append("email", email);

        var config = {
            headers: {
                'Content-Type': 'multipart/form-data',
                'Authorization': "Bearer " + localStorage.getItem("token")
            }
        }

        if (isNaN(id) || id <= 0) {
            axios.post('/api/participants', formData, config)
                .then(function () {
                    toastr.success("Participant added successfully!");
                    resetLoadingButton(thisBtn, originalText);
                    $("#participant-modal").modal("hide");
                    loadTableData();
                })
                .catch(function (err) {
                    resetLoadingButton(thisBtn, originalText);
                    showResponseError(err);
                });
        }
        else {
            axios.put('/api/participants', formData, config)
                .then(function () {
                    toastr.success("Participant updated successfully!");
                    resetLoadingButton(thisBtn, originalText);
                    $("#participant-modal").modal("hide");
                    loadTableData();
                })
                .catch(function (err) {
                    resetLoadingButton(thisBtn, originalText);
                    showResponseError(err);
                });
        }
    }
})();

