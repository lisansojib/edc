'use strict'

var appConstants = Object.freeze({
    BASE_URL: window.location.origin,
    LOGIN_REDIRECT_PATH: "/portal/index",
    REGISTER_REDIRECT_PATH: "/account/login",
    RECOVER_REDIRECT_PATH: "/account/recovery-successful",
    RESET_REDIRECT_PATH: "/account/login",
    LOGOUT_REDIRECT_PATH: "/account/login"
});

var loginProviders = Object.freeze({
    FACEBOOK: "Facebook",
    GOOGLE: "Google"
});

var pKeys;

$(function () {
    toastr.options.escapeHtml = true;
    axios.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded';

    if (localStorage.getItem("token")) {
        axios.defaults.headers.common['Authorization'] = "Bearer " + localStorage.getItem("token");
    }

    // Extend Bootstrap-table clear search button icon
    if ($.fn.bootstrapTable) {
        $.extend($.fn.bootstrapTable.defaults.icons, { clearSearch: 'fa-refresh' });
    }

    getSiteSettings();

    loadProgressBar();

    $("#btnLogout").click(logout);

    $("#btn-create-password").click(createPassword);
});

function getSiteSettings() {
    axios.get("/site-settings").then(function (response) { pKeys = response.data });
}

function logout(e) {
    e.preventDefault();

    axios.post("/account/logout")
        .then(function () {
            localStorage.clear();
            window.location.href = appConstants.LOGOUT_REDIRECT_PATH;
        })
}

function createPassword(e) {
    e.preventDefault();

    showBootboxPrompt("Change your password.", "Enter your password here", function (result) {
        if (!result || result.length < 6) {
            toastr.error("Password must be atleast 6 characters.");
            return;
        }
        else {
            axios.post("/account/createpassword", { Password: result })
                .then(function () {
                    toastr.success("Password changed successfully. You can now login with your password.");
                    window.location.reload();
                })
                .catch(function (err) {
                    toastr.error(err.response.data.Message);
                });
        }
    })
}

