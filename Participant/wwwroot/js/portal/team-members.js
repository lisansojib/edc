(function () {
    var teams = [];

    $(function () {
        getMyTeams();

        $("#btn-send-message").on("click", sendMessage);
    })

    function getMyTeams() {
        axios.get(`/api/portals/my-team-members`)
            .then(function (response) {
                debugger;
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
                            if (row.disabled) return;
                            getParticipantDetails(row.teamMemberId);
                        },
                        'click .send': function (e, value, row, index) {
                            e.preventDefault();
                            if (row.disabled) return;
                            sendMessage(row.teamMemberId);
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
                    field: "email",
                    title: "Email"
                },
                {
                    sortable: true,
                    field: "phone",
                    title: "Phone"
                },
                {
                    sortable: true,
                    field: "mobile",
                    title: "Mobile"
                },
                {
                    field: "photoUrl",
                    title: "Photo",
                    formatter: function (value, row, index, field) {
                        return `<img src="${value}" alt="${row.participantName}" class="" />`;
                    }
                }
            ],
            data: data,
            onDblClickRow: function (row, $element, field) {
                if (row.disabled) return;
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
        if (e) {
            e.preventDefault();
            $("#participant-modal").modal("hide");
        }
        alert("Message is sent");
    }
})();