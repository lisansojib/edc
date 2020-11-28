(function () {
    var $table, $formEl;

    var validationConstraints = {
        companyName: {
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
        contactPerson: {
            presence: true,
            length: {
                maximum: 100
            }
        },
        contactPersonPhone: {
            presence: true,
            length: {
                maximum: 20
            }
        },
        contactPersonEmail: {
            presence: true,
            email: true
        },
        website: {
            length: {
                maximum: 250
            }
        }
    };

    var tableParams = {
        offset: 0,
        limit: 10,
        sort: 'companyName',
        order: '',
        filter: ''
    };

    $(document).ready(function () {
        $table = $("#tblList");

        initTbl();
        loadTableData();

        $formEl = $("#sponsor-form");

        $("#add-new-sponsor").click(function () {
            $("#sponsor-modal-label").text("Add new Sponsor");
            $formEl.trigger("reset");
            initNewFileInput($("#logo"));
            $("#sponsor-modal").modal("show");
        });

        $("#btn-save-sponsor").click(save);
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
                            `<a class="btn btn-primary btn-sm edit"  title="Edit Sponsor">
                              <i class="fa fa-edit" aria-hidden="true"></i> 
                            </a>
                            <a class="btn btn-danger btn-sm ml-2 remove" href="javascript:" title="Delete Sponsor">
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
                            showBootboxConfirm("Delete Sponsor", "Are you sure you want to delete this?", function (yes) {
                                if (yes) {
                                    axios.delete(`/api/sponsors/${row.id}`)
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
                    field: "companyName",
                    title: "Company Name",
                    width: 100
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "contactPerson",
                    title: "Contact Person",
                    width: 100
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "contactPersonEmail",
                    title: "Contact Person Email",
                    width: 100
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "contactPersonPhone",
                    title: "Contact Person Phone",
                    width: 100
                },
                {
                    sortable: true,
                    field: "description",
                    title: "Description",
                    width: 300
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
        var url = `/api/sponsors?${queryParams}`;
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
        tableParams.sort = 'companyName';
        tableParams.order = '';
    }

    function getDetails(id) {
        axios.get(`/api/sponsors/${id}`)
            .then(function (response) {
                setFormData($formEl, response.data);
                previewFileInput(response.data.id, response.data.logoUrl, $("#logo"));
                $("#sponsor-modal-label").text("Edit Sponsor");
                $("#sponsor-modal").modal("show");
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

        var data = getFormData($formEl);        
        var files = $("#logo")[0].files;
        if (files.length > 0) data.append("logo", files[0]);

        var id = parseInt($formEl.find("#id"));
        if (isNaN(id) || id <= 0) {
            axios.post('/api/sponsors', data)
                .then(function () {
                    toastr.success("Sponsor updated successfully!");
                    resetLoadingButton(thisBtn, originalText);
                    $("#sponsor-modal").modal("hide");
                    loadTableData();
                })
                .catch(function (err) {
                    resetLoadingButton(thisBtn, originalText);
                    resetLoadingButton(thisBtn, originalText);
                    showResponseError(err);
                });
        }
        else {
            axios.put('/api/sponsors', data)
                .then(function () {
                    toastr.success("Sponsor updated successfully!");
                    $("#sponsor-modal").modal("hide");
                    loadTableData();
                })
                .catch(function (err) {
                    resetLoadingButton(thisBtn, originalText);
                    showResponseError(err);
                });
        }
    }    
})();

