﻿(function () {
    var $table, $formEl;
    var initialEventForm;
    var resourceCount = 0;

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

        // Clear form on close
        // On modal close set the event form to its initial state
        // Trigger click event on selectto after setting form data for editing, this will display correct inputs
        // 
        //initialEventForm = $("#event-form").html();

        //$("#btn-close-event-modal").click(function () {
        //    //$("#event-form").trigger("reset");
        //    $("#event-form-container").html(initialEventForm);
        //})
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
                            `<a class="btn btn-primary btn-sm edit" title="Edit Event">
                              <i class="fa fa-edit" aria-hidden="true"></i> 
                            </a>
                            <a class="btn btn-primary btn-sm share" title="Share Event with Guest">
                              <i class="fa fa-share" aria-hidden="true"></i> 
                            </a>
                            <a class="btn btn-danger btn-sm remove" href="javascript:" title="Delete Event">
                              <i class="fa fa-trash" aria-hidden="true"></i>
                            </a>`;
                        return template;
                    },
                    events: {
                        'click .edit': getDetails,
                        'click .share': shareEventLink,
                        'click .remove': removeEvent
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
                    field: "meetingId",
                    title: "Meeting Id",
                    width: 100
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "meetingPassword",
                    title: "Meeting Pwd",
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
            .catch(showResponseError)
    }

    function resetTableParams() {
        tableParams.offset = 0;
        tableParams.limit = 10;
        tableParams.filter = '';
        tableParams.sort = 'title';
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
            .catch(showResponseError);
    }

    function getDetails(e, value, row, index) {
        e.preventDefault();
        $("#fg-createZoomMeeting").hide();

        axios.get(`/api/events/${row.id}`)
            .then(function (response) {
                console.log(response.data);
                setFormData($formEl, response.data, true);
                $("#event-modal-label").text("Edit Event");
                $("#event-modal").modal("show");
            })
            .catch(showResponseError);
    }

    function removeEvent(e, value, row, index) {
        e.preventDefault();
        showBootboxConfirm("Delete Event", "Are you sure you want to delete this?", function (yes) {
            if (yes) {
                axios.delete(`/api/events/${row.id}`)
                    .then(function () {
                        toastr.success(appConstants.ITEM_DELETED_SUCCESSFULLY);
                        $table.bootstrapTable('refresh');
                    })
                    .catch(showResponseError)
            }
        })
    }

    function shareEventLink(e, value, row, index) {
        e.preventDefault();
        var eventId = row.id;
        var eventTitle = row.title;
        axios.get("/api/select-options/guest").then(function (response) {
            showBootboxSelect2MultipleDialog(
                "Select one or many guest.", "GuestIds",
                "Share event link to one or more guest.",
                response.data,
                true,
                function (data) {
                    if (!data) return;

                    var toEmail = data.map(function (el) { return el.text });
                    var model = {
                        guestEmails: toEmail,
                        eventId: eventId,
                        eventTitle: eventTitle
                    };

                    axios.post("/api/events/share-link", model)
                        .then(function () {
                            toastr.success("Link to this event is shared to the selected guests.");
                        })
                        .catch(showResponseError);
                });
        }).catch(showResponseError);
    }

    function handleEventTypeSelection(e) {
        if (e.params.name === "unselect") return;
        resourceCount = 0;

        if (e.params.data.text == eventTypeConstants.EDC_NETWORKING) {
            $("#resourceDiv").addClass("d-none");
            removeResources();   
        } else {
            $("#resourceDiv").removeClass("d-none");
        }

        if (e.params.data.text == eventTypeConstants.EDC_PANEL) {
            $("#fg-speaker").removeClass("d-none");
            $("#fg-sponsor").addClass("d-none");
            $("#fg-presenter").addClass("d-none");
            $("#fg-cto").addClass("d-none");
    
            
        }
        else if (e.params.data.text == eventTypeConstants.EDC_TEAM_SESSION) {
            $("#fg-speaker").addClass("d-none");
            $("#fg-sponsor").addClass("d-none");
            $("#fg-presenter").removeClass("d-none");
            $("#fg-cto").removeClass("d-none");
  
            
        }
        else if (e.params.data.text == eventTypeConstants.EDC_NETWORKING) {
            $("#fg-speaker").addClass("d-none");
            $("#fg-sponsor").addClass("d-none");
            $("#fg-presenter").addClass("d-none");
            $("#fg-cto").addClass("d-none");        
        }
        else { // EDC Post-Panel
            $("#fg-speaker").addClass("d-none");
            $("#fg-sponsor").removeClass("d-none");
            $("#fg-presenter").addClass("d-none");
            $("#fg-cto").addClass("d-none");
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
        if (resourceCount.length === 10) return toastr.warning("You can only add up to 10 files.");
        resourceCount++;
        var template =
            `<h6>Resource ${resourceCount}</h6>
            <div class="form-group row">
                <div class="col">
                    <input type="text" id="resourceTitle-${resourceCount}" name="resourceTitle-${resourceCount}" class="form-control" placeholder="Resource Title">
                </div>
                <div class="col">
                    <input type="text" id="resourceDescription-${resourceCount}" name="resourceDescription-${resourceCount}" class="form-control" placeholder="Resource Description">
                </div>
            </div>
            <div class="form-group row">
                <div class="col">
                    <input type="file" accept="image/jpeg,image/gif,image/png,application/pdf" id="resourceFile-${resourceCount}" name="resourceFile-${resourceCount}" class="form-control file">
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
        resourceCount = 0;
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
        else resetValidationState($formEl);

        var data = formDataToJson($formEl);
        data.id = parseInt(data.id);
        if (isNaN(data.id)) data.id = 0;
        data.eventTypeId = parseInt(data.eventTypeId);

        data.cohortId = parseInt(data.cohortId);

        var presenterId = parseInt(data.presenterId);
        if (isNaN(presenterId)) data.presenterId = 0;

        var ctoId = parseInt(data.ctoId);
        if (isNaN(ctoId)) data.ctoId = 0;

        var speakers = $("#speakerIds").select2("data");
        if (speakers) data.speakers = speakers.map(function (el) { return { id: el.id, text: el.text } });

        var sponsors = $("#sponsorIds").select2("data");
        if (sponsors) data.sponsors = sponsors.map(function (el) { return { id: el.id, text: el.text } });

        // Check for networking event
        var eventType = $("#eventTypeId").select2("data")[0].text;
        if (eventType != eventTypeConstants.EDC_NETWORKING) {
            data.resources = [];

            for (var i = 1; i <= resourceCount; i++) {
                var title = $(`#resourceTitle-${i}`).val();
                var description = $(`#resourceDescription-${i}`).val();
                var file = $(`#resourceFile-${i}`)[0].files[0];

                if (file) {
                    var newResource = {
                        title: title,
                        description: description,
                        file: file
                    }

                    data.resources.push(newResource);
                }
            }
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
                    toastr.success("Event created successfully!");
                    resetLoadingButton(thisBtn, originalText);
                    $("#event-modal").modal("hide");
                    loadTableData();
                })
                .catch(function (err) {
                    resetLoadingButton(thisBtn, originalText);
                    showResponseError(err);
                });
        }
        else {
            axios.put('/api/events', formData, config)
                .then(function () {
                    toastr.success("Event updated successfully!");
                    resetLoadingButton(thisBtn, originalText);
                    $("#event-modal").modal("hide");
                    loadTableData();
                })
                .catch(function (err) {
                    resetLoadingButton(thisBtn, originalText);
                    showResponseError(err);
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

