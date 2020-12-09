(function () {
    var $table;

    var tableParams = {
        offset: 0,
        limit: 10,
        sort: 'email',
        order: '',
        filter: ''
    };

    $(document).ready(function () {
        $table = $("#tblList");

        initTbl();
        loadTableData();

        $("#add-new-zoomuser").click(getNew);

        $("#btn-save-poll").click(save);
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
                    sortable: true,
                    field: "name",
                    title: "Name",
                    formatter: function (value, row, index, field) {
                        return row.firstName + " " + row.lastName;
                    }
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
                    searchable: true,
                    field: "type",
                    title: "Type",
                    width: 100
                },
                {
                    sortable: true,
                    searchable: true,
                    field: "pmi",
                    title: "PMI",
                    width: 100
                },
                {
                    sortable: true,
                    field: "verified",
                    title: "Verified",
                    formatter: function (value, row, index, field) {
                        return value ? "Yes" : "No";
                    }
                },
                {
                    field: "createdAt",
                    title: "Created At",
                    formatter: function (value, row, index, field) {
                        return formatDateToDDMMYYYY(value);
                    },
                    width: 100
                },
                {
                    field: "lastLoginTime",
                    title: "Last Login",
                    formatter: function (value, row, index, field) {
                        return formatDateToDDMMYYYY(value);
                    },
                    width: 100
                },
                {
                    sortable: true,
                    field: "status",
                    title: "Status",
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
            onRefresh: function () {
                resetTableParams();
                loadTableData();
            }
        });
    }

    function loadTableData() {
        $table.bootstrapTable('showLoading');
        var queryParams = $.param(tableParams);
        var url = `/api/zoom/users?${queryParams}`;
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
        tableParams.sort = 'email';
        tableParams.order = '';
    }

    function getNew() {
        bootbox.alert({
            message: "This feature is not enabled yet!",
            size: 'small'
        });
        return;

        // will work on this later
        //axios.get(`/api/zoom/users/new`)
        //    .then(function (response) {
        //        initSelect2($("#add-new-zoomuser"), response.data);
        //    })
        //    .catch(function (err) {
        //        showResponseError(err)
        //    });
    }
})();
