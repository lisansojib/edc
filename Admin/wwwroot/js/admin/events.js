(function () {
    var $table, $formEl;

    var validationConstraints = {
        title: {
            presence: true,
            length: {
                maximum: 100
            }
        },
        speakers: {
            presence: true
        },
        sponsors: {
            presence: true
        },
        eventDate: {
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

        $formEl = $("#event-form");

        $("#add-new-event").click(getNew);

        $("#btn-save-event").click(save);
    });

    function initTbl() {
        $table.bootstrapTable('destroy');
        $table.bootstrapTable({
            toolbar: "#tblToolbar",
            search: true,
            searchOnEnterKey: true,
            showSearchClearButton: true,
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
                            `<a class="btn btn-primary btn-sm edit"  title="Edit Event">
                              <i class="fa fa-edit" aria-hidden="true"></i> 
                            </a>
                            <a class="btn btn-danger btn-sm ml-2 remove" href="javascript:" title="Delete Event">
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
                            showBootboxConfirm("Delete Event", "Are you sure you want to delete this?", function (yes) {
                                if (yes) {
                                    axios.delete(`/api/events/${row.id}`)
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
                    width: 100
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "speakers",
                    title: "Speakers",
                    width: 100
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "sponsors",
                    title: "Sponsors",
                    width: 100
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "eventDate",
                    title: "Event Date",
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
        var url = `/api/events?${queryParams}`;
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
        axios.get(`/api/events/new`)
            .then(function (response) {
                setFormData($formEl, response.data, true);
                $("#event-modal-label").text("Add new Event");
                $formEl.trigger("reset");
                $("#event-modal").modal("show");
            })
            .catch(function (err) {
                toastr.error(err.response.data);
            });
    }

    function getDetails(id) {
        axios.get(`/api/events/${id}`)
            .then(function (response) {
                setFormData($formEl, response.data, true);
                $("#event-modal-label").text("Edit Event");
                $("#event-modal").modal("show");
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

        var speakers = $("#speakers").select2("data");
        data.speakers = speakers.map(function (el) { return { id: el.id, text: el.text } });

        var sponsors = $("#sponsors").select2("data");
        data.sponsors = sponsors.map(function (el) { return { id: el.id, text: el.text } });

        if (data.id <= 0) {
            axios.post('/api/events', data)
                .then(function () {
                    resetLoadingButton(thisBtn, originalText);
                    $("#event-modal").modal("hide");
                    loadTableData();
                    toastr.success("Event created successfully!");
                })
                .catch(function (err) {
                    resetLoadingButton(thisBtn, originalText);
                    $("#event-modal").modal("hide");
                    toastr.error(JSON.stringify(err.response.data.errors));
                });
        }
        else {
            axios.put('/api/events', data)
                .then(function () {
                    resetLoadingButton(thisBtn, originalText);
                    $("#event-modal").modal("hide");
                    loadTableData();
                    toastr.success("Event updated successfully!");
                })
                .catch(function (err) {
                    resetLoadingButton(thisBtn, originalText);
                    $("#event-modal").modal("hide");
                    toastr.error(JSON.stringify(err.response.data.errors));
                });
        }
    }
})();

