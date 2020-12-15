(function () {    

    $(function () {
        var eventId = $("#eventId").val();
        getEventInfo(eventId);
    });

    function getEventInfo(eventId) {
        axios.get(`/api/guest-session/event-info/${eventId}`)
            .then(function (response) {
                initEventInfo(response.data);
            })
            .catch(showResponseError);
    }

    function initEventInfo(event) {        
        var template = "";
        var details = "";
        if (event.cto) {
            details =
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
            `<div class="col-md-12 grid-margin stretch-card">
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
    }
})();