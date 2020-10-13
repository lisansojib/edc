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

    $(function () {
        if (localStorage.getItem("endTimeToken")) {
            endTimeToken = localStorage.getItem("endTimeToken");
            shouldLoadHistory = true;
        }

        initPubnubUser();

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

        $("#txt-new-message").keyup(function (e) {
            if ((e.keyCode || e.which) == 13) { //Enter keycode
                var text = $(this).val();
                if (!text) return;

                message = { text: text, publisher: pubnubUser.uuId };
                if ($channelEl.val() !== "all-panelists") message.subscriber = $channelEl.val();
                publishMessage(message);

                $(this).val("");
                var markup = renderSentMsg(message);
                updateChatbox(markup);
            }
        });

        $("#btn-send-message").on("click", function (e) {
            e.preventDefault();
            var text = $("#txt-new-message").val();
            if (!text) return;

            message = { text: text, publisher: pubnubUser.uuId };
            if ($channelEl.val() !== "all-panelists") message.subscriber = $channelEl.val();
            publishMessage(message);

            $("#txt-new-message").val("");
            var markup = renderSentMsg(message);
            updateChatbox(markup);
        });

        $channelEl = $("#ddlChannel");
        $channelEl.on("select2:select", updateActiveChannel);

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

    function showSwithUserPrompt() {
        var channels = pubnubUser.channels.map(function (item) { return { value: item, text: item } })

        showBootboxSelectPrompt("Switch chat user/channel.", channels, "sm", activeChannel, function (value) {
            if (value) activeChannel = value;
        })
    }

    function initPubnubUser() {
        axios.get("/api/auth/me")
            .then(function (response) {
                pubnubUser = response.data;

                initPubnub();

                //activeChannel = findChannel("All");
            })
            .catch(function (err) {
                console.error(err.response.data);
            });
    }

    function getPublisherAndRenderMessage(messageResponse) {
        axios.get(`/api/user/publisher?userId=${messageResponse.publisher}&channelName=${messageResponse.channel}`)
            .then(function (response) {
                var data = response.data;
                pubnubUser.Users.push(data.User);

                if (data.Channel.Name !== "All") pubnubUser.channels.push(data.Channel);

                var markup = renderReceivedMsg(messageResponse);
                updateChatbox(markup);

                debugger;
                var selectedUser = !$channelEl.val() ? "all-panelists" : $channelEl.val();
                initSelect2($channelEl, pubnubUser.Users, false, "Select To User");
                $channelEl.val(selectedUser).trigger("change");
            })
            .catch(function (err) {
                console.error(err.response.data);
            });
    }

    function updateActiveChannel(e) {
        var userId = e.params.data.id;

        if (userId === "all-panelists") {
            activeChannel = findChannel("All");
            $("#chat-mode").text("(Publically)");
        }
        else {
            activeChannel = findChannelByUserId(userId);
            $("#chat-mode").text("(Privately)");
        }

        if (!activeChannel) saveChannel(userId);
    }

    window.updateActiveChannelGlobal = function (userId) {
        $channelEl.val(userId).trigger("change");

        if (userId === "all-panelists") {
            activeChannel = findChannel("All");
            $("#chat-mode").text("(Publically)");
        }
        else {
            activeChannel = findChannelByUserId(userId);
            $("#chat-mode").text("(Privately)");
        }
    };

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
            channel: activeChannel.Name,
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
                    getPublisherAndRenderMessage(response);
                }
                else {
                    if (response.channel === "All") {
                        var markup = renderReceivedMsg(response);
                        updateChatbox(markup);
                    }
                    else {
                        if (pubnubUser.uuId !== response.message.subscriber) return;

                        var markup = renderReceivedMsg(response);
                        updateChatbox(markup);
                    }
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
            channels: pubnubUser.channels,
            withPresence: true
        });
    }

    function renderSentMsg(message) {
        var privately = activeChannel.Name == "All" ? "" : `<span class="text-danger">(privately)</span>`;
        var subscriber = $channelEl.select2("data")[0];
        var markup =
            `<li>
                <p>
                    <span class="text-muted">From Me to</span>&nbsp;
                    <span style="cursor: pointer;" class="btn-link" onclick="updateActiveChannelGlobal('${subscriber.id}')">${subscriber.text}: </span>${privately}
                    <br />
                    ${message.text}
                    <br>
                    <small>${getChatTime()}</small>
                </p>
            </li>`;
        return markup;
    }

    function renderReceivedMsg(messageResponse) {
        var toSpan = "";
        if (messageResponse.channel === "All") {
            var subscriber = findUser("all-panelists");
            toSpan = `<span style="cursor:pointer;" class="btn-link" onclick="updateActiveChannelGlobal('${subscriber.id}')" > ${subscriber.text}:</span>`;
        }
        else {
            toSpan = `<span class="text-primary">Me:</span><span class="text-danger">(privately)</span>`;
        }

        var publisher = findUser(messageResponse.publisher);

        var markup =
            `<li>
                <p>
                    From <span style="cursor: pointer;" class="btn-link" onclick="updateActiveChannelGlobal('${publisher.id}')">${publisher.text}:</span>
                    &nbsp;to&nbsp;${toSpan}
                    <br />
                    ${messageResponse.message.text}
                    <br>
                    <small>${getDate(messageResponse.timetoken)}</small>
                </p>
            </li>`;
        return markup;
    }

    //function updateChatbox(markup) {
    //    $(".chat-box .mCSB_container").append(markup);
    //    $(".chat-box").mCustomScrollbar("scrollTo", "bottom");
    //}

    function findChannel(name) {
        return pubnubUser.channels.find(function (c) { return c.Name === name });
    }

    function findChannelByUserId(userId) {
        return pubnubUser.channels.find(function (c) { return c.UserId === userId });
    }

    function findUser(userId) {
        return pubnubUser.Users.find(function (u) { return u.id === userId });
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
})();