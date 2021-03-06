﻿'use strict'

var appConstants = Object.freeze({
    LOGIN_PATH: "/account/login",
    LOGIN_REDIRECT_PATH: "/home/index",
    REGISTER_REDIRECT_PATH: "/account/login",
    RECOVER_REDIRECT_PATH: "/account/recovery-successful",
    RESET_REDIRECT_PATH: "/account/login",
    LOGOUT_REDIRECT_PATH: "/account/login",
    SAVE_AD_REDIRECT_PATH: "/my-account/ticket-save-success",
    ITEM_DELETED_SUCCESSFULLY: "Item deleted successfully!",
    USER_PASSWORD_CHANGED_SUCCESSFULLY: "User password changed successfully!",
    CREDENTIALS_EMAILED_SUCCESSFULLY: "User credentials sent via eamil.",
    FORGOT_PASSWORD_SUCCESS: "An email is sent with reset password link. Please check your email.",
    RESET_PASSWORD_SUCCESS: "Password reset successful. Please login."
});

var eventTypeConstants = Object.freeze({
    EDC_PANEL: "EDC Panel",
    EDC_TEAM_SESSION: "EDC Team Session",
    EDC_NETWORKING: "EDC Networking",
    EDC_POST_PANEL: "EDC Post-Panel"
})

var loginProviders = Object.freeze({
    FACEBOOK: "Facebook",
    GOOGLE: "Google"
});

var graphTypes = Object.freeze({
    PIE_CHART: "Pie Chart",
    BAR_CHART: "Bar Chart"
});

var pKeys = Object.freeze({
    PUBLISH_KEY: "pub-c-96e4cfae-dc9f-4a2f-97fd-08794a7a7d48",
    SUBSCRIBE_KEY: "sub-c-3974af1a-858f-11ea-a961-f6bfeb2ef611"
});

$(function () {
    toastr.options.escapeHtml = true;
    axios.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded';

    $.fn.editable.defaults.mode = 'inline';
    $.fn.editable.defaults.onblur = "submit";

    if (localStorage.getItem("token")) {
        axios.defaults.headers.common['Authorization'] = "Bearer " + localStorage.getItem("token");
    }

    // Extend Bootstrap-table clear search button icon
    if ($.fn.bootstrapTable) {
        $.extend($.fn.bootstrapTable.defaults.icons, { clearSearch: 'fa-refresh' });
    }

    loadProgressBar();

    $("#btnLogout").click(logout);

    $("#btn-create-password").click(createPassword);
});

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
                .catch(showResponseError);
        }
    })
}

