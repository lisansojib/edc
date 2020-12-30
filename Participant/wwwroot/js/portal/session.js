(function () {
    var pubnub = {};
    var pubnubUser = {};
    var activeChannel = {};
    var message = {
        text: "", // message text
        publisher: "", // message published by,
        subscriber: "" // optional parameter only for one to one chat
    };

    var $channelEl;
    var channelHub;
    var endTimeToken;
    var shouldLoadHistory = false;
    var chatBoxPerfectScrollbar;
    var eventId;

    $(function () {
        if (localStorage.getItem("endTimeToken")) {
            endTimeToken = localStorage.getItem("endTimeToken");
            shouldLoadHistory = true;
        }

        eventId = $("#eventId").val();

        getSiteSettings();

        var connection = new signalR.HubConnectionBuilder().withUrl('/channelHub').build();

        connection.on("channelAdded", function (channelName) {
            var existChannel = findChannel(channelName);
            if (!existChannel) {
                pubnubUser.channels.push(channel);
                subscribeToChannel(channelName);
            }
        });

        connection.start().then(function () {
            console.log("SignalR Started");
        }).catch(showResponseError);

        $("#txt-message").keyup(function (e) {
            e.preventDefault();
            if ((e.keyCode || e.which) == 13) { //Enter keycode
                var text = this.value ? this.value.trim() : this.value;
                if (!text) return;

                message = { text: text, publisher: pubnubUser.uuId };
                publishMessage(message);

                this.value = "";
                var markup = renderSentMsg(message);
                updateChatbox(markup);
            }
        });

        $("#btn-send").on("click", function (e) {
            e.preventDefault();
            var text = $("#txt-new-message").val();
            if (!text) return;

            message = { text: text, publisher: pubnubUser.uuId };
            publishMessage(message);

            $("#txt-new-message").val("");
            var markup = renderSentMsg(message);
            updateChatbox(markup);
        });

        //$channelEl = $("#ddlChannel");
        //$channelEl.on("select2:select", updateActiveChannel);

        if ($('.chat-content .chat-body').length) {
            chatBoxPerfectScrollbar = new PerfectScrollbar('.chat-content .chat-body');
        }

        setInterval(getOnlineUsers, 15000);

        $('.chat-content').toggleClass('show');

        //$(document).idle({
        //    onIdle: function () {
        //        console.log("You have been idle.");
        //    },
        //    onActive: function () {
        //        getBatchHistory();
        //    },
        //    idle: 60000
        //})

        $("#swith-user").on("click", showSwithUserPrompt);
    });

    function getSiteSettings() {
        axios.get("/site-settings")
            .then(function (response) {
                pKeys = response.data;
                initPubnubUser();
            });
    }

    function showSwithUserPrompt() {
        var channels = pubnubUser.channels.map(function (item) { return { value: item.name, text: item.name } })

        showBootboxSelectPrompt("Switch chat user/channel.", channels, "sm", activeChannel, function (value) {
            if (value) setActiveChannel(value);
        })
    }

    function initPubnubUser() {
        axios.get(`/api/sessions/pubnub-user/${eventId}`)
            .then(function (response) {
                pubnubUser = response.data;

                initPubnub();

                activeChannel = findDefaultChannel();

                initEventInfo();
            })
            .catch(function (err) {
                console.error(err.response.data);
            });
    }

    function getPublisherAndRenderMessage(messageResponse) {
        axios.get(`/api/sessions/teammembers/${messageResponse.channel}`)
            .then(function (response) {
                pubnubUser.teamMembers.push.apply(pubnubUser.teamMembers, response.data);

                var markup = renderReceivedMsg(messageResponse);
                updateChatbox(markup);
            })
            .catch(function (err) {
                console.error(err.response.data);
            });
    }

    function saveChannel(userId) {
        if (userId === "all-panelists") return;
        axios.post('/api/channel/create', { UserId: userId })
            .then(function (response) {
                activeChannel = response.data;
                pubnubUser.channels.push(response.data);
                subscribeToChannels();

                channelHub.server.send(response.data);
            })
            .catch(function (err) {
                console.log(err);
                toastr.error(err.response.data.Message);
            });
    }

    function publishMessage(msg) {
        var publishConfig = {
            channel: activeChannel.name,
            message: msg
        }
        pubnub.publish(publishConfig, function (status, response) {
            console.log(status, response);
        })
    }

    function getBatchHistory() {
        var channelNames = pubnubUser.channels.map(function (el) { return el.Name; });
        pubnub.fetchMessages(
            {
                channels: channelNames,
                end: endTimeToken,
                count: 25
            },
            (status, response) => {
                if (response) {
                    var channels = response.channels;
                    var channelMessages = [];
                    for (const property in channels) {
                        channelMessages.push.apply(channelMessages, channels[property])
                    }

                    channelMessages = _.sortBy(channelMessages, 'timetoken');
                    console.log(channelMessages);

                    var markup = "";
                    channelMessages.forEach(function (item) {
                        markup += renderBatchMessage(item);
                    });

                    $(".chat-box li").remove();
                    updateChatbox(markup);
                }
            }
        );
    }

    function renderBatchMessage(item) {
        var markup = "";
        if (item.message.publisher === pubnubUser.uuId) { // sent message
            var subscriber;
            var privately;
            if (item.channel === "All") {
                subscriber = findUser("all-panelists");
                privately = "";
            }
            else {
                subscriber = findUser(item.message.subscriber);
                privately = `<span class="text-danger">(privately)</span>`;
            }

            markup =
                `<li>
                    <p>
                        <span class="text-muted">From Me to</span>&nbsp;
                        <span style="cursor: pointer;" class="btn-link" onclick="updateActiveChannelGlobal('${subscriber.id}')">${subscriber.text}: </span>${privately}
                        <br />
                        ${item.message.text}
                        <br>
                        <small>${getDate(item.timetoken)}</small>
                    </p>
                </li>`;
        }
        else {
            var toSpan = "";
            if (item.channel === "All") {
                var subscriber = findUser("all-panelists");
                toSpan = `<span style="cursor:pointer;" class="btn-link" onclick="updateActiveChannelGlobal('${subscriber.id}')" > ${subscriber.text}:</span>`;
            }
            else {
                toSpan = `<span class="text-primary">Me:</span><span class="text-danger">(privately)</span>`;
            }

            var publisher = findUser(item.message.publisher);

            var markup =
                `<li>
                <p>
                    From <span style="cursor: pointer;" class="btn-link" onclick="updateActiveChannelGlobal('${publisher.id}')">${publisher.text}:</span>
                    &nbsp;to&nbsp;${toSpan}
                    <br />
                    ${item.message.text}
                    <br>
                    <small>${getDate(item.timetoken)}</small>
                </p>
            </li>`;
        }

        return markup;
    }

    function getHistory() {
        var historyConfig = {
            channel: activeChannel.Name,
            count: 100
        };
        pubnub.history(historyConfig, function (status, response) {
            if (!status.statusCode === 200) return;
            var markup = "";

            response.messages.forEach(function (item) {
                markup += item.entry.publisher === pubnubUser.uuId ? renderSentMsg(item.entry) : renderReceivedMsg(item.entry);
            });

            updateChatbox(markup);
        })
    }

    function deleteHistory() {
        pubnub.deleteMessages(
            {
                channel: activeChannel.Name
            },
            (result) => {
                console.log(result);
            }
        );
    }

    function initPubnub() {
        pubnub = new PubNub({
            publishKey: pKeys.publishKey,
            subscribeKey: pKeys.subscribeKey,
            uuid: pubnubUser.uuId
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

        subscribeToChannels();

        getOnlineUsers();

        if (shouldLoadHistory) getBatchHistory();
    }

    function getOnlineUsers() {
        pubnub.hereNow(
            {
                channels: ["All"],
                includeState: true
            },
            function (status, response) {
                if (status.statusCode !== 200) return;
                var channelInfo = response.channels.All;
                var channels = [];
                channels.push({ id: "", text: "" });
                //channels.push({ id: "all-panelists", text: "All Panelists" });
                var channelsMarkup = "";
                channelInfo.occupants.forEach(function (occupant) {
                    var user = findUser(occupant.uuid);
                    if (user) {
                        channels.push(user);

                        channelsMarkup += `<li class="list-inline-item text-primary border">${user.text}</li>`;
                    }
                });

                var userId = $channelEl.val();
                initSelect2($channelEl, channels, false, "Select Team.");

                if (userId === "all-panelists") {
                    $channelEl.val("all-panelists").trigger("change");
                    $("#chat-mode").text("(Publically)");
                }
                else {
                    var user = findUser(userId);
                    if (!user) {
                        $channelEl.val("all-panelists").trigger("change");
                        $("#chat-mode").text("(Publically)");
                    }
                    else {
                        $channelEl.val(userId).trigger("change");
                        $("#chat-mode").text("(Privately)");
                    }
                }

                var onlineNow = channelInfo.occupancy > 0 ? channelInfo.occupancy - 1 : 0
                $("#totalOccupancy").html(onlineNow);
                $("#ul-online-users").empty().append(onlineUsersMarkup);
            }
        );
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

    function renderSentMsg(message) {
        var template =
            `<li class="message-item me">
                <img src="https://via.placeholder.com/43x43" class="img-xs rounded-circle" alt="avatar">
                <div class="content">
                    <div class="message">
                        <div class="bubble">
                            <p>${message.text}</p>
                        </div>
                        <span>${getChatTime(new Date())}</span>
                    </div>
                </div>
            </li>`;

        return template;
    }

    function renderReceivedMsg(messageResponse) {
        var template =
            `<li class="message-item friend">
                <img src="https://via.placeholder.com/43x43" class="img-xs rounded-circle" alt="avatar">
                <div class="content">
                    <div class="message">
                        <div class="bubble">
                            <p>${messageResponse.message.text}</p>
                        </div>
                        <span>${getDate(messageResponse.timetoken)}</span>
                    </div>
                </div>
            </li>`;

        return template;
    }

    function updateChatbox(markup) {
        $(".messages").append(markup);
        chatBoxPerfectScrollbar.update();
    }

    function findDefaultChannel() {
        return pubnubUser.channels.find(function (item) { return item.isDefault })
    }

    function findChannel(name) {
        return pubnubUser.channels.find(function (c) { return c.name === name });
    }

    //function findChannelByUserId(userId) {
    //    return pubnubUser.channels.find(function (c) { return c.userId === userId });
    //}

    function findUser(userId) {
        return pubnubUser.teamMembers.find(function (u) { return u.uuId === userId });
    }

    function setActiveChannel(channel) {
        activeChannel = channel;
        $("#d-channel").text(channel);
    }

    function getTime(timetoken) {
        var unixTimestamp = parseInt(timetoken / 10000000);
        var msgDate = new Date(unixTimestamp * 1000);
        return msgDate.toLocaleTimeString();
    }

    function getDate(timetoken) {
        var unixTimestamp = parseInt(timetoken / 10000000);
        var msgDate = new Date(unixTimestamp * 1000);
        return getChatTime(msgDate);
    }

    function initEventInfo() {        
        pubnubUser.events.forEach(function (event) {
            var template = "";
            var details = "";
            if (event.cto) {
                details +=
                    `<div class="col-12">
                        <div class="py-3">
                            <h5>CTO</h5>
                            <p>${event.cto}</p>
                        </div>
                    </div>`;
            }

            if (event.speakers) {
                details +=
                    `<div class="col-12">
                        <div class="py-3">
                            <h5>Speakers</h5>
                            <p>${event.speakers}</p>
                        </div>
                    </div>`;
            }

            if (event.sponsors) {
                details +=
                    `<div class="col-12">
                        <div class="py-3">
                            <h5>Sponsors</h5>
                            <p>${event.sponsors}</p>
                        </div>
                    </div>`;
            }

            var eventResources = "";
            var initialPreview = [];
            var initialPreveiwConfig = [];
            if (event.resources.length > 0) {
                eventResources =
                    `<div class="col-12 py-3">
                        <h5>Resources</h5>
                        <input id="previewFile-${event.id}" name="previewFile" type="file" multiple disabled>
                    </div>`;

                event.resources.forEach(function (resource) {
                    initialPreview.push(`${appConstants.BASE_URL}resource.filePath`);

                    initialPreveiwConfig.push({ type: resource.previewType, caption: resource.title, key: resource.id });
                })
            }

            template +=
                `<div class="col-md-6 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <div class="d-flex justify-content-between align-items-baseline">
                                <h3 class="card-title mb-3">${event.eventType}</h3>
                            </div>
                            <div class="row">
                                <div class="media d-block d-sm-flex">
                                    <img src="${appConstants.BASE_URL}/nobleui/assets/images/placeholder.jpg" class="wd-100p wd-sm-200 mb-3 mb-sm-0 mr-3" alt="...">
                                    <div class="media-body">
                                        <h5 class="card-title mt-0">${event.title} <small>${formatDateToHHMMADMMMYYYY(event.eventDate)}</small></h5>
                                        <p>${event.description}</p>
                                    </div>
                                </div>
                                ${details}
                                ${eventResources}
                            </div>
                        </div>
                    </div>
                </div>`;

            $("#eventInfo").append(template);

            if (event.resources.length > 0) {
                $(`#previewFile-${event.id}`).fileinput({
                    showUpload: false,
                    theme: "fa",
                    initialPreview: initialPreview,
                    initialPreviewAsData: true,
                    initialPreviewFileType: 'image',
                    initialPreviewConfig: initialPreveiwConfig
                });
            }
        });
    }
})();