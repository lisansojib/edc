(function () {
    var $tblEvents, $tblTeams, $tblPolls, $tblAnnouncements;

    $(function () {
        $tblEvents = $("#tblEvents");
        $tblTeams = $("#tblTeams");
        $tblPolls = $("#tblPolls");
        $tblAnnouncements = $("#tblAnnouncements");

        initEventsTbl();
        initTeamsTbl();
        initPollsTbl();
        initAnnouncementsTbl();

        loadEventsData();
        loadTeamsData();
        loadPollsData();
        loadAnnouncementsData();
    })

    // #region Events
    var eventParams = {
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
                            `<a href="/portal/session?eventId=${row.id}" title="Go to Session">Go to Session</a>`;
                        return template;
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
                        return formatDateToDDMMYYYY(value);
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
                if (eventParams.offset == newOffset && eventParams.limit == newLimit)
                    return;

                eventParams.offset = newOffset;
                eventParams.limit = newLimit;

                loadEventsData();
            },
            onSort: function (name, order) {
                eventParams.sort = name;
                eventParams.order = order;
                eventParams.offset = 0;

                loadEventsData();
            },
            onRefresh: function () {
                resetEventParams();
                loadEventsData();
            },
            onSearch: function (text) {
                eventParams.filter = text;
                loadEventsData();
            }
        });
    }

    function loadEventsData() {
        $tblEvents.bootstrapTable('showLoading');
        var queryParams = $.param(eventParams);
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

    function resetEventParams() {
        tableParams.offset = 0;
        tableParams.limit = 10;
        tableParams.filter = '';
        tableParams.sort = 'name';
        tableParams.order = '';
    }
    // #endregion

    // #region Teams
    var teamsParams = {
        offset: 0,
        limit: 10,
        sort: 'name',
        order: '',
        filter: ''
    };

    function initTeamsTbl() {
        $tblTeams.bootstrapTable('destroy');
        $tblTeams.bootstrapTable({
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
                    sortable: true,
                    searchable: true,
                    field: "name",
                    title: "Name",
                    width: 100
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "participants",
                    title: "Participants",
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
                if (teamsParams.offset == newOffset && teamsParams.limit == newLimit)
                    return;

                teamsParams.offset = newOffset;
                teamsParams.limit = newLimit;

                loadTeamsData();
            },
            onSort: function (name, order) {
                teamsParams.sort = name;
                teamsParams.order = order;
                teamsParams.offset = 0;

                loadTeamsData();
            },
            onRefresh: function () {
                resetTeamsParams();
                loadTeamsData();
            },
            onSearch: function (text) {
                teamsParams.filter = text;
                loadTeamsData();
            }
        });
    }

    function loadTeamsData() {
        $tblTeams.bootstrapTable('showLoading');
        var queryParams = $.param(teamsParams);
        var url = `/api/portals/teams?${queryParams}`;
        axios.get(url)
            .then(function (response) {
                $tblTeams.bootstrapTable('load', response.data);
                $tblTeams.bootstrapTable('hideLoading');
            })
            .catch(function (err) {
                toastr.error(err.response.data.Message);
            })
    }

    function resetTeamsParams() {
        tableParams.offset = 0;
        tableParams.limit = 10;
        tableParams.filter = '';
        tableParams.sort = 'name';
        tableParams.order = '';
    }
    // #endregion

    // #region Polls
    var pollsParams = {
        offset: 0,
        limit: 10,
        sort: 'name',
        order: '',
        filter: ''
    };

    function initPollsTbl() {
        $tblPolls.bootstrapTable('destroy');
        $tblPolls.bootstrapTable({
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
                    sortable: true,
                    searchable: true,
                    field: "name",
                    title: "Name",
                    width: 100
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "graphType",
                    title: "Graph Type",
                    width: 100
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "panel",
                    title: "Panel",
                    width: 100
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "origin",
                    title: "Origin",
                    width: 100
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "pollDate",
                    title: "PollDate",
                    formatter: function (value, row, index, field) {
                        return formatDateToDDMMYYYY(value);
                    },
                    width: 100
                }],
            onPageChange: function (number, size) {
                var newOffset = (number - 1) * size;
                var newLimit = size;
                if (pollsParams.offset == newOffset && pollsParams.limit == newLimit)
                    return;

                pollsParams.offset = newOffset;
                pollsParams.limit = newLimit;

                loadPollsData();
            },
            onSort: function (name, order) {
                pollsParams.sort = name;
                pollsParams.order = order;
                pollsParams.offset = 0;

                loadPollsData();
            },
            onRefresh: function () {
                resetPollsParams();
                loadPollsData();
            },
            onSearch: function (text) {
                pollsParams.filter = text;
                loadPollsData();
            }
        });
    }

    function loadPollsData() {
        $tblPolls.bootstrapTable('showLoading');
        var queryParams = $.param(pollsParams);
        var url = `/api/portals/polls?${queryParams}`;
        axios.get(url)
            .then(function (response) {
                $tblPolls.bootstrapTable('load', response.data);
                $tblPolls.bootstrapTable('hideLoading');
            })
            .catch(function (err) {
                toastr.error(err.response.data.Message);
            })
    }

    function resetPollsParams() {
        tableParams.offset = 0;
        tableParams.limit = 10;
        tableParams.filter = '';
        tableParams.sort = 'name';
        tableParams.order = '';
    }
    // #endregion

    // #region Announcements
    var announcementsParams = {
        offset: 0,
        limit: 10,
        sort: 'title',
        order: '',
        filter: ''
    };

    function initAnnouncementsTbl() {
        $tblAnnouncements.bootstrapTable('destroy');
        $tblAnnouncements.bootstrapTable({
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
                    width: 300
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "callAction",
                    title: "Call Action",
                    width: 100
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "linkUrl",
                    title: "Link Url",
                    width: 100
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "expiration",
                    title: "Expiration",
                    formatter: function (value, row, index, field) {
                        return formatDateToDDMMYYYY(value);
                    },
                    width: 100
                }],
            onPageChange: function (number, size) {
                var newOffset = (number - 1) * size;
                var newLimit = size;
                if (announcementsParams.offset == newOffset && announcementsParams.limit == newLimit)
                    return;

                announcementsParams.offset = newOffset;
                announcementsParams.limit = newLimit;

                loadAnnouncementsData();
            },
            onSort: function (name, order) {
                announcementsParams.sort = name;
                announcementsParams.order = order;
                announcementsParams.offset = 0;

                loadAnnouncementsData();
            },
            onRefresh: function () {
                resetAnnouncementsParams();
                loadAnnouncementsData();
            },
            onSearch: function (text) {
                announcementsParams.filter = text;
                loadAnnouncementsData();
            }
        });
    }

    function loadAnnouncementsData() {
        $tblAnnouncements.bootstrapTable('showLoading');
        var queryParams = $.param(announcementsParams);
        var url = `/api/portals/announcements?${queryParams}`;
        axios.get(url)
            .then(function (response) {
                $tblAnnouncements.bootstrapTable('load', response.data);
                $tblAnnouncements.bootstrapTable('hideLoading');
            })
            .catch(function (err) {
                toastr.error(err.response.data.Message);
            })
    }

    function resetAnnouncementsParams() {
        tableParams.offset = 0;
        tableParams.limit = 10;
        tableParams.filter = '';
        tableParams.sort = 'title';
        tableParams.order = '';
    }
    // #endregion

})();