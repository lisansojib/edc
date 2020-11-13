(function () {
    var $table, $formEl;

    var validationConstraints = {
        name: {
            presence: true,
            length: {
                maximum: 100
            }
        },
        participants: {
            presence: true
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

        $formEl = $("#team-form");

        $("#add-new-team").click(getNew);

        $("#btn-save-team").click(save);
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
                            `<a class="btn btn-primary btn-sm edit"  title="Edit Team">
                              <i class="fa fa-edit" aria-hidden="true"></i> 
                            </a>
                            <a class="btn btn-danger btn-sm ml-2 remove" href="javascript:" title="Delete Team">
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
                            showBootboxConfirm("Delete Team", "Are you sure you want to delete this?", function (yes) {
                                if (yes) {
                                    axios.delete(`/api/teams/${row.id}`)
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
                    field: "description",
                    title: "Description",
                    width: 100
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "participants",
                    title: "Participants",
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
        var url = `/api/teams?${queryParams}`;
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

    function getNew() {
        axios.get(`/api/select-options/participants`)
            .then(function (response) {
                initSelect2($("#participants"), response.data);
                $("#team-modal-label").text("Add new Team");
                $formEl.trigger("reset");
                $("#team-modal").modal("show");
            })
            .catch(function (err) {
                toastr.error(err.response.data);
            });
    }

    function getDetails(id) {
        axios.get(`/api/teams/${id}`)
            .then(function (response) {
                setFormData($formEl, response.data, true);
                $("#team-modal-label").text("Edit Team");
                $("#team-modal").modal("show");
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
        else hideValidationErrors($formEl);

        debugger;
        var data = formDataToJson($formEl);
        data.id = parseInt(data.id);
        if (isNaN(data.id)) data.id = 0; 

        var participants = $("#participants").val();
        data.participants = participants.map(function (el) { return parseInt(el) });

        // Check for participants (presence doesn't work)

        if (!data.participants.length) {
            resetLoadingButton(thisBtn, originalText);
            toastr.error("Please add participants");
            return;
        }

        if (data.id <= 0) {
            axios.post('/api/teams', data)
                .then(function () {
                    resetLoadingButton(thisBtn, originalText);
                    $("#team-modal").modal("hide");
                    loadTableData();
                    toastr.success("Team created successfully!");
                })
                .catch(function (err) {
                    resetLoadingButton(thisBtn, originalText);
                    $("#team-modal").modal("hide");
                    toastr.error(JSON.stringify(err.response.data.errors));
                });
        }
        else {
            axios.put('/api/teams', data)
                .then(function () {
                    resetLoadingButton(thisBtn, originalText);
                    $("#team-modal").modal("hide");
                    loadTableData();
                    toastr.success("Team updated successfully!");
                })
                .catch(function (err) {
                    resetLoadingButton(thisBtn, originalText);
                    $("#team-modal").modal("hide");
                    toastr.error(JSON.stringify(err.response.data.errors));
                });
        }
    }
})();

