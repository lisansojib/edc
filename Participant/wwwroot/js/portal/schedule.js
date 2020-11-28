var handleClientLoad;

(function () {
    var $tblEvents, $formAddSpeaker, $formReferSpeaker;

    var addSpeakerValidationConstraints = {
        firstName: {
            length: {
                maximum: 100
            }
        },
        lastName: {
            length: {
                maximum: 100
            }
        },
        email: {
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
        panelId: {
            presence: true,
        },
        interestInTopic: {
            presence: true,
            length: {
                maximum: 500
            }
        },
        linkedinUrl: {
            length: {
                maximum: 500
            }
        }
    };

    var referSpeakerValidationConstraints = {
        firstName: {
            length: {
                maximum: 100
            }
        },
        lastName: {
            length: {
                maximum: 100
            }
        },
        email: {
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
        panelId: {
            presence: true,
        },
        interestInTopic: {
            presence: true,
            length: {
                maximum: 500
            }
        },
        note: {
            length: {
                maximum: 500
            }
        },
        linkedinUrl: {
            length: {
                maximum: 500
            }
        },
        companyId: {
            presence: true
        }
    };

    $(function () {
        $tblEvents = $("#tblEvents");
        initEventsTbl();
        loadEventsData();

        $formAddSpeaker = $("#add-speaker-form");
        $formReferSpeaker = $("#refer-speaker-form");

        $("#btn-save-reffered-speaker").click(saveReferSpeaker);
        $("#btn-save-speaker").click(saveSpeaker);
    })

    // #region Events
    var tableParams = {
        offset: 0,
        limit: 10,
        sort: 'title',
        order: '',
        filter: ''
    };

    function initEventsTbl() {
        $tblEvents.bootstrapTable('destroy');
        $tblEvents.bootstrapTable({
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
                    title: 'Actions',
                    align: 'center',
                    formatter: function (value, row, index, field) {
                        var template =
                            `<div class="dropdown">
                                <button class="btn pt-2" type="button" id="dropdownTblAction-${row.id}" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" title="More Options">
                                    <i class="icon-lg text-muted pb-3px fa fa-ellipsis-h"></i> Options
                                </button>
                                <div class="dropdown-menu dropdown-menu-right" aria-labelledby="dropdownTblAction-${row.id}">
                                    <a class="dropdown-item d-flex align-items-center add-to-google-calendar" href="#"><i class="icon-sm mr-2 fa fa-calendar-plus-o"></i> <span class="">Add to Google</span></a>
                                    <a class="dropdown-item d-flex align-items-center add-to-outlook-calendar" href="#"><i class="icon-sm mr-2 fa fa-calendar-plus-o"></i> <span class="">Add to Outlook</span></a>
                                    <a class="dropdown-item d-flex align-items-center apply-to-speak" href="#"><i class="icon-sm mr-2 fa fa-commenting-o"></i> <span class="">Apply to Speak</span></a>
                                    <a class="dropdown-item d-flex align-items-center refer-to-speak" href="#"><i class="icon-sm mr-2 fa fa-share"></i> <span class="">Refer to Speaker</span></a>
                                </div>
                            </div>`;
                        return template;
                    },
                    events: {
                        'click .add-to-google-calendar': function (e, value, row, index) {
                            e.preventDefault();

                            try {
                                if (!gapi.auth2.getAuthInstance().isSignedIn.get());
                            } catch (e) {
                                gapi.auth2.getAuthInstance().signIn();
                            }
                            finally {
                                createNewGoogleCalenderEvent(row);
                            }

                            if (!gapi.auth2.getAuthInstance().isSignedIn.get()) gapi.auth2.getAuthInstance().signIn();
                            
                        },
                        'click .add-to-outlook-calendar': function (e, value, row, index) {
                            e.preventDefault();
                            alert("This feature is not enabled yet.");
                        },
                        'click .apply-to-speak': function (e, value, row, index) {
                            e.preventDefault();
                            getNewSpeaker();
                        },
                        'click .refer-to-speak': function (e, value, row, index) {
                            e.preventDefault();
                            getNewRefer();
                        }
                    }
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "title",
                    title: "Title"
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "speakers",
                    title: "Speakers"
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "sponsors",
                    title: "Sponsors"
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "eventDate",
                    title: "Event Date",
                    formatter: function (value, row, index, field) {
                        return formatDateToDDMMYYYY(value);
                    }
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "description",
                    title: "Description"
                }
            ],
            onPageChange: function (number, size) {
                var newOffset = (number - 1) * size;
                var newLimit = size;
                if (tableParams.offset == newOffset && tableParams.limit == newLimit)
                    return;

                tableParams.offset = newOffset;
                tableParams.limit = newLimit;

                loadEventsData();
            },
            onSort: function (name, order) {
                tableParams.sort = name;
                tableParams.order = order;
                tableParams.offset = 0;

                loadEventsData();
            },
            onRefresh: function () {
                resetTableParams();
                loadEventsData();
            },
            onSearch: function (text) {
                tableParams.filter = text;
                loadEventsData();
            }
        });
    }

    function loadEventsData() {
        $tblEvents.bootstrapTable('showLoading');
        var queryParams = $.param(tableParams);
        var url = `/api/portals/events?${queryParams}`;
        axios.get(url)
            .then(function (response) {
                $tblEvents.bootstrapTable('load', response.data);
                $tblEvents.bootstrapTable('hideLoading');
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

    function getNewSpeaker() {
        axios.get(`/api/schedules/new-speaker`)
            .then(function (response) {
  
                var data = response.data;
                console.log(data);
                setFormData($formAddSpeaker, data);

                $("#add-speaker-modal").modal("show");
            })
            .catch(function (err) {
                showResponseError(err)
            });
    }

    function getNewRefer() {
        axios.get(`/api/schedules/new-speaker`)
            .then(function (response) {
                var data = response.data;
                console.log(data);
                setFormData($formReferSpeaker, data);

                $("#refer-speaker-modal").modal("show");
            })
            .catch(function (err) {
                showResponseError(err)
            });
    }

    function saveSpeaker(e) {
        e.preventDefault();

        var thisBtn = $(this);
        var originalText = thisBtn.html();
        setLoadingButton(thisBtn);

        initializeValidation($formAddSpeaker, addSpeakerValidationConstraints);

        var errorObj = isValidForm($formAddSpeaker, addSpeakerValidationConstraints);
        if (errorObj) {
            showValidationToast(errorObj);
            resetLoadingButton(thisBtn, originalText);
            return;
        }
        else resetValidationState($formAddSpeaker);

        var data = formDataToJson($formAddSpeaker);
        data.panelId = parseInt(data.panelId);
        axios.post('/api/schedules/save-speaker', data)
            .then(function () {
                resetLoadingButton(thisBtn, originalText);
                toastr.success("Successfully Applied to Speak");
                $("#add-speaker-modal").modal("hide");
                loadEventsData();
            })
            .catch(function (err) {
                resetLoadingButton(thisBtn, originalText);
                console.log(err);
                showResponseError(err);
            });
    }

    function saveReferSpeaker(e) {
        e.preventDefault();

        var thisBtn = $(this);
        var originalText = thisBtn.html();
        setLoadingButton(thisBtn);

        initializeValidation($formReferSpeaker, referSpeakerValidationConstraints);

        var errorObj = isValidForm($formReferSpeaker, referSpeakerValidationConstraints);
        if (errorObj) {
            showValidationToast(errorObj);
            resetLoadingButton(thisBtn, originalText);
            return;
        }
        else resetValidationState($formReferSpeaker);

        var data = formDataToJson($formReferSpeaker);
        data.panelId = parseInt(data.panelId);
        data.companyId = parseInt(data.companyId);
        data.isReferer = true;

        axios.post('/api/schedules/save-speaker', data)
            .then(function () {
                resetLoadingButton(thisBtn, originalText);
                toastr.success("Successfully Reffered to Speak");
                $("#refer-speaker-modal").modal("hide");
                loadEventsData();
            })
            .catch(function (err) {
                resetLoadingButton(thisBtn, originalText);
                console.log(err);
                showResponseError(err);
            });
    }
    // #endregion


    // Array of API discovery doc URLs for APIs used by the quickstart
    window.DISCOVERY_DOCS = ["https://www.googleapis.com/discovery/v1/apis/calendar/v3/rest"];

    // Authorization scopes required by the API; multiple scopes can be
    // included, separated by spaces.
    window.SCOPES = "https://www.googleapis.com/auth/calendar https://www.googleapis.com/auth/calendar.events";

    /**
     *  On load, called to load the auth2 library and API client library.
     */
    handleClientLoad = function () {
        gapi.load('client:auth2', initClient);
    }

    /**
     *  Initializes the API client library and sets up sign-in state
     *  listeners.
     */
    function initClient() {
        gapi.client.init({
            apiKey: "AIzaSyBTop7xBzBNvLyUyOhN_zvIkjd6v-MRfVU",
            clientId: "27017976469-p9svugd1uksek2kmeod8o7ce1v1lvhp4.apps.googleusercontent.com",
            discoveryDocs: DISCOVERY_DOCS,
            scope: SCOPES
        }).then(function () {
            // Listen for sign-in state changes.
            gapi.auth2.getAuthInstance().isSignedIn.listen(updateSigninStatus);

            // Handle the initial sign-in state.
            updateSigninStatus(gapi.auth2.getAuthInstance().isSignedIn.get());
        }, function (error) {
            console.log(JSON.stringify(error, null, 2));
        });
    }

    /**
     *  Called when the signed in status changes, to update the UI
     *  appropriately. After a sign-in, the API is called.
     */
    function updateSigninStatus(isSignedIn) {
        if (isSignedIn) {
            listUpcomingEvents();
        } 
    }

    /**
     *  Sign in the user upon button click.
     */
    function handleAuthClick(event) {
        gapi.auth2.getAuthInstance().signIn();
    }

    /**
     *  Sign out the user upon button click.
     */
    function handleSignoutClick(event) {
        gapi.auth2.getAuthInstance().signOut();
    }

    /**
     * Print the summary and start datetime/date of the next ten events in
     * the authorized user's calendar. If no events are found an
     * appropriate message is printed.
     */
    function listUpcomingEvents() {
        gapi.client.calendar.events.list({
            'calendarId': 'primary',
            'timeMin': (new Date()).toISOString(),
            'showDeleted': false,
            'singleEvents': true,
            'maxResults': 10,
            'orderBy': 'startTime'
        }).then(function (response) {
            var events = response.result.items;
            console.log('Upcoming events:');

            if (events.length > 0) {
                for (i = 0; i < events.length; i++) {
                    var event = events[i];
                    var when = event.start.dateTime;
                    if (!when) {
                        when = event.start.date;
                    }
                    console.log(event.summary + ' (' + when + ')')
                }
            } else {
                console.log('No upcoming events found.');
            }
        });
    }

    function createNewGoogleCalenderEvent(eventData) {
        var event = {
            'summary': eventData.title,
            //'location': '800 Howard St., San Francisco, CA 94103',
            'description': eventData.description,
            'start': {
                //'dateTime': '2020-10-28T09:00:00-07:00',
                'dateTime': eventData.eventDate,
                'timeZone': 'America/New_York'
            },
            'end': {
                'dateTime': eventData.eventDate,
                'timeZone': 'America/New_York'
            },
            'recurrence': [
                'RRULE:FREQ=DAILY;COUNT=2'
            ],
            'attendees': [
                { 'email': 'lpage@example.com' },
                { 'email': 'sbrin@example.com' }
            ],
            'reminders': {
                'useDefault': false,
                'overrides': [
                    { 'method': 'email', 'minutes': 24 * 60 },
                    { 'method': 'popup', 'minutes': 10 }
                ]
            }
        };

        var request = gapi.client.calendar.events.insert({
            'calendarId': 'primary',
            'resource': event
        });

        request.execute(function (event) {
            console.log(event);
            toastr.success("Calendar event created successfully in your google calendar.");
        });
    }
})();