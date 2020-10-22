(function () {
    var $table, $formEl;
    var resoureCount = 0;

    var validationConstraints = {
        title: {
            presence: true,
            length: {
                maximum: 100
            }
        },
        cohortId: {
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

        $("#eventTypeId").on("select2:select select2:unselecting", handleEventTypeSelection);

        $("#eventDate").on("change", handleEventDateChange);

        $("#addMoreResource").click(addMoreResource);

        $("#btn-save-event").click(save);
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
                        return formatDateToMMDDYYYY(value);
                    },
                    width: 100
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "description",
                    title: "Description",
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
                setFormData($formEl, response.data);
                $("#event-modal-label").text("Add new Event");
                $formEl.trigger("reset");
                $("#fg-session").addClass("d-none");
                $("#fg-speaker").addClass("d-none");
                $("#fg-sponsor").addClass("d-none");
                $("#fg-presenter").addClass("d-none");
                $("#fg-cto").addClass("d-none");
                removeResources();
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

    function handleEventTypeSelection(e) {
        if (e.params.name === "unselect") return;
        
        if (e.params.data.text == eventTypeConstants.EDC_PANEL) {
            $("#fg-speaker").removeClass("d-none");
            $("#fg-sponsor").addClass("d-none");
            $("#fg-presenter").addClass("d-none");
            $("#fg-cto").addClass("d-none");
            resoureCount = 0;
            addResource();
            $("#resourceDiv").removeClass("d-none");
        }
        else if (e.params.data.text == eventTypeConstants.EDC_TEAM_SESSION) {
            $("#fg-speaker").addClass("d-none");
            $("#fg-sponsor").addClass("d-none");
            $("#fg-presenter").removeClass("d-none");
            $("#fg-cto").removeClass("d-none");
            resoureCount = 0;
            addResource();
            $("#resourceDiv").removeClass("d-none");
        }
        else if (e.params.data.text == eventTypeConstants.EDC_NETWORKING) {
            $("#fg-speaker").addClass("d-none");
            $("#fg-sponsor").addClass("d-none");
            $("#fg-presenter").addClass("d-none");
            $("#fg-cto").addClass("d-none");
            resoureCount = 0;
            removeResources();           
        }
        else { // EDC Post-Panel
            $("#fg-speaker").addClass("d-none");
            $("#fg-sponsor").removeClass("d-none");
            $("#fg-presenter").addClass("d-none");
            $("#fg-cto").addClass("d-none");
            resoureCount = 0;
            addResource();
            $("#resourceDiv").removeClass("d-none");
        }
    }

    function handleEventDateChange(e) {
        axios.get(`/api/select-options/related-events?date=${e.currentTarget.value}`)
            .then(function (response) {
                initSelect2($("#sessionId"), response.data);
                $("#fg-session").removeClass("d-none");
            })
            .catch(showResponseError);
    }

    function addResource() {
        if (resoureCount.length === 10) return toastr.warning("You can only add up to 10 files.");
        resoureCount++;
        var template =
            `<h6>Resource ${resoureCount}</h6>
            <div class="form-group row">
                <div class="col">
                    <input type="text" id="resourceTitle-${resoureCount}" name="resourceTitle-${resoureCount}" class="form-control" placeholder="Resource Title">
                </div>
                <div class="col">
                    <input type="text" id="resourceDescription-${resoureCount}" name="resourceDescription-${resoureCount}" class="form-control" placeholder="Resource Description">
                </div>
            </div>
            <div class="form-group row">
                <div class="col">
                    <input type="file" accept="image/jpeg,image/gif,image/png,application/pdf" id="resourceFile-${resoureCount}" name="resourceFile-${resoureCount}" class="form-control file">
                </div>
            </div>`;

        $("#resource-container").append(template);
        $(".file").fileinput({
            showPreview: false,
            showUpload: false,
            theme: "fa"
        });
    }

    function addMoreResource(e) {
        if (e) e.preventDefault();
        addResource();
    }

    function removeResources() {
        resoureCount = 0;
        $("#resource-container").empty();
        $("#resourceDiv").addClass("d-none");
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
        if (isNaN(data.id)) data.id = 0;
        data.eventTypeId = parseInt(data.eventTypeId);

        data.cohortId = parseInt(data.cohortId);

        var presenterId = parseInt(data.presenterId);
        if (isNaN(presenterId)) data.presenterId = 0;

        var ctoId = parseInt(data.ctoId);
        if (isNaN(ctoId)) data.ctoId = 0;

        var speakers = $("#speakers").select2("data");
        if (speakers) data.speakers = speakers.map(function (el) { return { id: el.id, text: el.text } });

        var sponsors = $("#sponsors").select2("data");
        if (sponsors) data.sponsors = sponsors.map(function (el) { return { id: el.id, text: el.text } });

        data.resources = [];
        for (var i = 1; i <= resoureCount; i++) {
            var isValid = true;
            var errors = '';
            var title = $(`#resourceTitle-${i}`).val();
            if (!title || !title.trim()) {
                errors += `Title can't be empty for resource ${i}<br>`;
                isValid = false;
            }

            var description = $(`#resourceDescription-${i}`).val();
            //if (!description || !description.trim()) {
            //    errors += `Description can't be empty for resource ${i}<br>`;
            //    false;
            //}

            var file = $(`#resourceFile-${i}`)[0].files[0];
            if (!file) {
                errors += `You must upload a file for resource ${i}`;
                isValid = false;
            }
            if (!isValid) {
                toastr.error(errors);
                continue;
            }

            var newResource = {
                title: title,
                description: description,
                file: file
            }

            data.resources.push(newResource);
        }

        var formData = new FormData();
        buildFormData(formData, data);

        const config = {
            headers: {
                'Content-Type': 'multipart/form-data',
                'Authorization': "Bearer " + localStorage.getItem("token")
            }
        }

        if (data.id <= 0) {
            axios.post('/api/events', formData, config)
                .then(function () {
                    resetLoadingButton(thisBtn, originalText);
                    $("#event-modal").modal("hide");
                    loadTableData();
                    toastr.success("Event created successfully!");
                })
                .catch(function (err) {
                    resetLoadingButton(thisBtn, originalText);
                    toastr.error(JSON.stringify(err.response.data.errors));
                });
        }
        else {
            axios.put('/api/events', formData, config)
                .then(function () {
                    resetLoadingButton(thisBtn, originalText);
                    $("#event-modal").modal("hide");
                    loadTableData();
                    toastr.success("Event updated successfully!");
                })
                .catch(function (err) {
                    resetLoadingButton(thisBtn, originalText);
                    toastr.error(JSON.stringify(err.response.data.errors));
                });
        }
    }

    function buildFormData(formData, data, parentKey) {
        if (data && typeof data === 'object' && !(data instanceof Date) && !(data instanceof File) && !(data instanceof Blob)) {
            Object.keys(data).forEach(key => {
                buildFormData(formData, data[key], parentKey ? `${parentKey}[${key}]` : key);
            });
        } else {
            const value = data == null ? '' : data;

            if (data instanceof File) formData.append("files", data);
            else formData.append(parentKey, value);
        }
    }

})();

