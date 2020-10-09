(function () {
    var teams = [];

    $(function () {
        getMyTeams();

        $("#btn-send-message").on("click", sendMessage);
    })

    function getMyTeams() {
        axios.get(`/api/portals/my-teams`)
            .then(function (response) {
                teams = response.data;
                teams.forEach(function (team) {
                    $("#teams-container").append(
                        `<div class="col-6 grid-margin stretch-card">
                            <div class="card">
                                <div class="card-body">
                                    <div class="d-flex justify-content-between align-items-baseline mb-2">
                                        <h6 class="card-title mb-0">${team.name}</h6>
                                    </div>
                                    <table id="tbl-team-${team.id}" class="table table-hover mb-0"></table>
                                </div>
                            </div>
                        </div>`);

                    var $tableEl = $(`#tbl-team-${team.id}`);
                    initTable($tableEl, team.participants);


                })
            })
            .catch(showResponseError);
    }

    function initTable($el, data) {
        $el.bootstrapTable('destroy');
        $el.bootstrapTable({
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
                            debugger;
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
                    field: "participantName",
                    title: "Participant Name"
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