(function () {
    $(function () {
        getCompanies();
        $("#btn-register").on('click', register);
        $("#login-google").on("click", loginGoogle);
    });   

    function getCompanies() {
        axios.get("/api/select-options/companies")
            .then(function (response) {
                initSelect2($("#companyId"), response.data);
            })
            .catch(function (err) {
                toastr.error(err.response.data);
            });
    }

    function register(e) {
        e.preventDefault();
        var thisBtn = $(this);
        var originalText = thisBtn.html();
        setLoadingButton(thisBtn);

        var $formEl = $("#register-form");

        initializeValidation($formEl, validationConstraints);

        var errorObj = isValidForm($formEl, validationConstraints);
        if (errorObj) {
            showValidationToast(errorObj);
            resetLoadingButton(thisBtn, originalText);
            return;
        }

        var data = formDataToJson($formEl);
        data.companyId = parseInt(data.companyId);

        axios.post('/api/auth/register', data)
            .then(function () {
                resetLoadingButton(thisBtn, originalText);
                window.location.href = appConstants.LOGIN_PATH;
                //localStorage.setItem("token", response.data.accessToken);
                //loginToApp(response.data);
            })
            .catch(function (err) {
                resetLoadingButton(thisBtn, originalText);
                toastr.error(JSON.stringify(err.response.data.errors));
            });
    }

    // #region Login
    function loginGoogle(evt) {
        evt.preventDefault();
        toastr.info("Please be patient while we complete your login process.");
        gapi.load('auth2', function () {
            gapi.auth2.init({
                client_id: "819390282854-j5g7iabuau4c524le3oqfuv94ihhr22e.apps.googleusercontent.com"
            }).then(function (auth2) { // GoogleAuth instance
                if (auth2.isSignedIn.get()) {
                    var googleUser = auth2.currentUser.get();
                    var profile = googleUser.getBasicProfile()

                    externalLoginVm.email = profile.getEmail();
                    externalLoginVm.providerKey = googleUser.getId();
                    externalLoginVm.photoUrl = profile.getImageUrl();
                    externalLoginVm.firstName = profile.getGivenName();
                    externalLoginVm.lastName = profile.getFamilyName();
                    externalLoginVm.loginProvider = loginProviders.GOOGLE;

                    //externalLoginVm.Token = googleUser.getAuthResponse().id_token;

                    externalLogin(externalLoginVm);
                }
                else {
                    auth2.signIn({
                        scope: 'profile email'
                    }).then(function (googleUser) {
                        var profile = googleUser.getBasicProfile()

                        externalLoginVm.email = profile.getEmail();
                        externalLoginVm.providerKey = googleUser.getId();
                        externalLoginVm.photoUrl = profile.getImageUrl();
                        externalLoginVm.firstName = profile.getGivenName();
                        externalLoginVm.lastName = profile.getFamilyName();
                        externalLoginVm.loginProvider = loginProviders.GOOGLE;

                        externalLogin(externalLoginVm);
                    })
                }
            });
        });
    }

    /**
     * Call this function to login with external login provider
     * @param {externalLoginVm} model - instance of externalLoginVm
     */
    function externalLogin(model) {
        axios.post('/api/auth/externallogin', model)
            .then(function (response) {
                localStorage.setItem("token", response.data.accessToken);
                loginToApp(response.data);
            })
            .catch(function (err) {
                toastr.error(JSON.stringify(err.response.data.errors));
            });
    }
    // #endregion

    function loginToApp(data) {
        axios.post(`/account/login`, data)
            .then(function () {
                toastr.success("Successfully logged in to App!");
                window.location.href = appConstants.LOGIN_REDIRECT_PATH;
            })
            .catch(function (err) {
                toastr.error(JSON.stringify(err.response.data.errors));
            });
    }

    var validationConstraints = {
        email: {
            presence: true,
            email: true
        },
        firstName: {
            presence: true
        },
        lastName: {
            presence: true
        },
        password: {
            presence: true,
            length: {
                minimum: 6,
                maximum: 20
            }
        },
        companyId: {
            presence: true
        }
    };
})();