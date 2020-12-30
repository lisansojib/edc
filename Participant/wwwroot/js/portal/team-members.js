(function () {
    var uniqueUserId;

    $(function () {
        getAllTeamMembers();

        uniqueUserId = $("#uuid").val();
        getSiteSettings();
    })

    function getSiteSettings() {
        axios.get("/site-settings")
            .then(function (response) {
                pKeys = response.data;
                initPubnub();
            });
    }

    function getAllTeamMembers() {
        axios.get(`/api/portals/my-team-members`)
            .then(function (response) {
                initTable(response.data);
            })
            .catch(showResponseError);
    }

    function initTable(data) {
        $("#tbl-team-members").bootstrapTable('destroy');
        $("#tbl-team-members").bootstrapTable({
            cache: false,
            sortable: true,
            columns: [
                {
                    title: 'Actions',
                    align: 'center',
                    formatter: function (value, row, index, field) {
                        var template =
                            `<a class="btn btn-primary btn-sm view"  title="View Details">
                              <i class="fa fa-eye" aria-hidden="true"></i> 
                            </a>
                            <a class="btn btn-primary btn-sm send"  title="Send Message">
                              <i class="fa fa-paper-plane" aria-hidden="true"></i> 
                            </a>`;
                        return template;
                    },
                    events: {
                        'click .view': function (e, value, row, index) {
                            e.preventDefault();
                            getParticipantDetails(row.id);
                        },
                        'click .send': function (e, value, row, index) {
                            e.preventDefault();
                            sendMessage(row);
                        }
                    }
                },
                {
                    sortable: true,
                    field: "name",
                    title: "Name"
                },
                {
                    sortable: true,
                    field: "title",
                    title: "Title"
                },
                {
                    sortable: true,
                    field: "companyName",
                    title: "Company"
                }
            ],
            data: data,
            onDblClickRow: function (row, $element, field) {
                getParticipantDetails(row.teamMemberId);
            }
        });
    }

    function getParticipantDetails(id) {
        axios.get(`/api/portals/participant/${id}`)
            .then(function (response) {
                setFormData($("#participant-form"), response.data);
                $("#photoUrl").attr("src", response.data.photoUrl);
                $("#participant-modal").modal("show");
            })
            .catch(showResponseError);
    }

    function sendMessage(e) {
        bootbox.prompt({
            size: "small",
            title: "Type your message here.",
            inputType: "textarea",
            buttons: {
                confirm: {
                    label: 'Send Message',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'Cancel',
                    className: 'btn-danger'
                }
            },
            callback: function (msg) {
                if (msg) {
                    var message = { text: msg, publisher: uniqueUserId };
                    publishMessage(message);
                }
            }
        });
    }

    function initPubnub() {
        debugger;
        pubnub = new PubNub({
            publishKey: pKeys.publishKey,
            subscribeKey: pKeys.subscribeKey,
            uuid: uniqueUserId
        });

        pubnub.addListener({
            status: function (statusEvent) {
                if (statusEvent.category === "PNConnectedCategory") {
                    //publishMessage("Hello");
                }
            },
            message: function (response) {
                if (!endTimeToken) {
                    localStorage.setItem("endTimeToken", response.timetoken);
                    endTimeToken = response.timetoken;
                }
                if (response.publisher === pubnubUser.uuId) return;

                var publisher = findUser(response.publisher);
                if (!publisher) {
                    pubnubUser.channels.push({ "name": response.channel })
                    getPublisherAndRenderMessage(response);
                }
                else {
                    if (response.message.subscriber && pubnubUser.uuId !== response.message.subscriber) return;

                    var markup = renderReceivedMsg(response);
                    updateChatbox(markup);
                }
            },
            presence: function (presenceEvent) {
                // handle presence
                var occupancy = presenceEvent.occupancy;
            },
            membership: function (membershipEvent) {
                // for Objects, this will trigger when:
                // . user added to a space
                // . user removed from a space
                // . membership updated on a space
            },
            user: function (userEvent) {
                // for Objects, this will trigger when:
                // . user updated
                // . user deleted
            },
        });

        //subscribeToChannels();
    }

    function publishMessage(msg) {
        var publishConfig = {
            channel: "hello-world",
            message: msg
        }
        pubnub.publish(publishConfig, function (status, response) {
            debugger;
            console.log(status, response);
        })
    }

    /**
     * Subscribe to pubnub channel
     * @param {any} channelName - channel name
     */
    function subscribeToChannel(channelName) {
        pubnub.subscribe({
            channels: [channelName],
            withPresence: true
        });
    }

    /**
     * Subscribe to pubnub channels
     */
    function subscribeToChannels() {
        pubnub.subscribe({
            channels: pubnubUser.channels.map(function (el) { return el.name }),
            withPresence: true
        });
    }
})();