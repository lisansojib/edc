(function () {
    var profile;

    var validationConstraints = {
        Title: {
            presence: true,
            length: {
                maximum: 100
            }
        },
        PhoneNumber: {
            presence: true,
            length: {
                maximum: 20
            }
        },
        Company: {
            length: {
                maximum: 100
            }
        },
        Bio: {
            length: {
                maximum: 500
            }
        },
        BringsToGroup: {
            length: {
                maximum: 100
            }
        },
        GoalsForProgram: {
            length: {
                maximum: 100
            }
        },
    }; 

    $(function () {
        $("#btn-edit-profile").click(function () {
            if (profile) {
                $("#Title").val(profile.Title);
                $("#PhoneNumber").val(profile.PhoneNumber);
                $("#Company").val(profile.Company);
                $("#Bio").val(profile.Bio);
                $("#BringsToGroup").val(profile.BringsToGroup);
                $("#GoalsForProgram").val(profile.GoalsForProgram);
            }

            $("#view-profile").hide();
            $("#edit-profile").show();
        })

        $("#btn-cancel").click(function () {
            $("#edit-profile").hide();
            $("#view-profile").show();
        })

        $("#btn-save-profile").click(saveProfile);

        $("input,textarea").on("input", function () {
            if (this.value.length > this.maxLength) return false;

            var charRemaining = this.maxLength - this.value.length;
            $(`#${this.id}Help`).html(charRemaining + " characters remaining.");
        })

        getProfileInfo();
    })

    function saveProfile(evt) {
        evt.preventDefault();

        var thisBtn = $(this);
        var originalText = thisBtn.html();
        setLoadingButton(thisBtn);

        var $formEl = $("#profile-form");

        initializeValidation($formEl, validationConstraints);

        var errorObj = isValidForm($formEl, validationConstraints);
        if (errorObj) {
            showValidationToast(errorObj);
            resetLoadingButton(thisBtn, originalText);
            return;
        }

        var formData = new FormData();
        formData.append("Title", $("#Title").val());
        formData.append("PhoneNumber", $("#PhoneNumber").val());
        formData.append("Company", $("#Company").val());
        formData.append("Bio", $("#Bio").val());
        formData.append("BringsToGroup", $("#BringsToGroup").val());
        formData.append("GoalsForProgram", $("#GoalsForProgram").val());

        var profilePic = $("#ProfilePic")[0].files[0];
        if (profilePic) formData.append("ProfilePic", profilePic);

        var logoImage = $("#LogoImage")[0].files[0];
        if (logoImage) formData.append("LogoImage", logoImage);

        var config = {
            headers: {
                'content-type': 'multipart/form-data',
                'Authorization': "Bearer " + localStorage.getItem("token")
            }
        }

        axios.post("/api/user/profile", formData, config)
            .then(function () {
                toastr.success("Profile picture uploaded successfully!");
                disableElement(thisBtn);

                window.location.reload();
            })
            .catch(function (err) {
                toastr.error(err.response.data);
            })
            .then(function () {
                resetLoadingButton(thisBtn, btnContent);
            });
    }

    function getProfileInfo() {
        axios.get("/api/user/profile")
            .then(function (response) {
                profile = response.data;
                setProfileInfo();
            })
            .catch(function (err) {
                console.error(err);
            })
    }

    function setProfileInfo() {
        if (!profile) {
            toastr.info("User profile is not set.");
            $("#profileNotSet").show();
            $("#profileSet").hide();
            return;
        }

        $("#profileNotSet").hide();
        $("#profileSet").show();

        $(".card-title").html(`${profile.Name} <small class="text-muted">${profile.Title}</small>`);
        $("#bio").html(profile.Bio);
        $("#company").html(profile.Company);
        $("#phoneNumber").html(profile.PhoneNumber);
        $("#bringsToGroup").html(profile.BringsToGroup);
        $("#goalsForProgram").html(profile.GoalsForProgram);

        if (!profile.PhotoUrl) profile.PhotoUrl = "https://via.placeholder.com/400"
        $("#photo").attr("src", profile.PhotoUrl); 

        if (!profile.LogoUrl) profile.LogoUrl = "https://via.placeholder.com/100"
        $("#logo").attr("src", profile.LogoUrl); 
    }
})();