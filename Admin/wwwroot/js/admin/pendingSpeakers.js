$(document).ready(function () {
    $table = $("#tblList");

    initTbl();
    loadTableData();

});

var tableParams = {
    offset: 0,
    limit: 10,
    sort: 'name',
    order: '',
    filter: ''
};


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
                        `<a class="btn btn-primary btn-sm accept" title="Accept Speaker">
                              <i class="fa fa-check" aria-hidden="true"></i> 
                            </a>
                            <a class="btn btn-danger btn-sm ml-2 reject" href="javascript:" title="Reject Speaker">
                              <i class="fa fa-ban" aria-hidden="true"></i>
                            </a>`;
                    return template;
                },
                events: {
                    'click .accept': function (e, value, row, index) {
                        console.log(value);
                        // Create speaker

                        e.preventDefault();
                    },
                    'click .reject': function (e, value, row, index) {
                        console.log(value);

                        e.preventDefault();
                        showBootboxConfirm("Reject Speaker", "Are you sure you want to do this?", function (yes) {
                            if (yes) {

                                // Update speaker
                                axios.put(`/api/pending-speakers`)
                                    .then(function () {
                                        toastr.success(appConstants.ITEM_DELETED_SUCCESSFULLY);
                                        $table.bootstrapTable('refresh');
                                    })
                                    .catch(showResponseError)
                            }
                        })
                    }
                }
            },
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
                field: "email",
                title: "Email",
                width: 100
            },
            
            {
                sortable: true,
                searchable: false,
                field: "interestInTopic",
                title: "Interest In Topic",
                width: 100,
            },
            {
                sortable: true,
                searchable: false,
                field: "refferedBy",
                title: "Reffered By",
                width: 100,
            },
            {
                sortable: true,
                searchable: false,
                field: "phone",
                title: "Phone",
                width: 100,
            },
            {
                sortable: true,
                searchable: false,
                field: "linkedInUrl",
                title: "Linked In URL",
                width: 100,
            },
            {
                sortable: true,
                searchable: false,
                field: "isRefferer",
                title: "Is Refferer",
                width: 100,
                formatter: function (value, row, index, field) {
                    return value ? "Yes" : "No";
                }
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
    var url = `/api/pending-speakers?${queryParams}`;
    axios.get(url)
        .then(function (response) {
            console.log(response.data);
            $table.bootstrapTable('load', response.data);
            $table.bootstrapTable('hideLoading');
        })
        .catch(showResponseError)
}

function resetTableParams() {
    tableParams.offset = 0;
    tableParams.limit = 10;
    tableParams.filter = '';
    tableParams.sort = 'name';
    tableParams.order = '';
}

