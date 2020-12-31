(function () {
    var profile, $formEl;

    var validationConstraints = {
        firstName: {
            presence: true,
            length: {
                maximum: 100
            }
        },
        lastName: {
            presence: true,
            length: {
                maximum: 100
            }
        },
        emailCorp: {
            email: true,
            length: {
                maximum: 500
            }
        },
        emailPersonal: {
            email: true,
            length: {
                maximum: 500
            }
        },
        phone: {
            length: {
                maximum: 20
            }
        },
        PhoneCorp: {
            length: {
                maximum: 20
            }
        },
        title: {
            length: {
                maximum: 100
            }
        },
        linkedinUrl: {
            //url: true,
            length: {
                maximum: 250
            }
        }
    };

    $(function () {
        $(".btn-edit-profile").click(function () {
            $("#edit-profile-modal").modal("show");
        })

        $formEl = $("#edit-profile-form");

        $("#btn-save-profile").click(saveProfile);
       
        getProfileInfo();

        //$("input,textarea").on("input", function () {
        //    if (this.value.length > this.maxLength) return false;

        //    var charRemaining = this.maxLength - this.value.length;
        //    $(`#${this.id}Help`).html(charRemaining + " characters remaining.");
        //})
    })

    function saveProfile(e) {
        e.preventDefault();

        var thisBtn = $(this);
        var originalText = thisBtn.html();
        setLoadingButton(thisBtn);

        initializeValidation($formEl, validationConstraints);

        var errorObj = isValidForm($formEl, validationConstraints);
        if (errorObj) {
            showValidationToast(errorObj);
            resetLoadingButton(thisBtn, originalText);
            return;
        }
        else resetValidationState($formEl);

        var formData = getFormData($formEl);
  
        //var files = $("#photo")[0].files;
        //if (files.length > 0) formData.append("photo", files[0]);

        var email = $("input[name='primaryEmail']:checked").val() === 'work' ? $formEl.find("#emailCorp").val() : $formEl.find("#emailPersonal").val();
        formData.append("email", email);

        var config = {
            headers: {
                'Content-Type': 'multipart/form-data',
                'Authorization': "Bearer " + localStorage.getItem("token")
            }
        }

        axios.put('/api/profile', formData, config)
            .then(function () {
                toastr.success("Profile updated successfully!");
                resetLoadingButton(thisBtn, originalText);
                $("#edit-profile-modal").modal("hide");
                getProfileInfo();
            })
            .catch(function (err) {
                resetLoadingButton(thisBtn, originalText);
                showResponseError(err);
            });
        
    }

    function getProfileInfo() {
        axios.get("/api/profile")
            .then(function (response) {
                profile = response.data;
                console.log(profile);
                setProfileInfo();
                setEditFormData(profile);
            })
            .catch(function (err) {
                console.error(err);
            })
    }


    function setEditFormData(formData) {
        setFormData($formEl, formData);
        previewFileInput(profile.id, formData.photoUrl, $("#photo"));
    }

    function setProfileInfo() {
        if (!profile) {
            toastr.info("User profile is not set.");
            $("#profileNotSet").show();
            $("#profileSet").hide();
            return;
        }

        var fullName = `${profile.title? profile.title: ""}${profile.firstName} ${profile.lastName}`;

        $("#d-profile-name").text(fullName);
        $("#d-profile-email").text(profile.email)

        if (!profile.phone) profile.phone = "N/A";
        if (!profile.mobile) profile.mobile = "N/A";

        $("#d-profile-phone").text(profile.phone);
        $("#d-profile-mobile").text(profile.mobile);


        if (!profile.photoUrl) profile.photoUrl = "https://dummyimage.com/100"
        $("#d-profile-photo").attr("src", profile.photoUrl);
    }
})();
