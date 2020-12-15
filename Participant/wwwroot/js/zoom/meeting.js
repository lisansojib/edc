(function () {
    var testTool = window.testTool;
    // get meeting args from url
    debugger;
  var tmpArgs = testTool.parseQuery();
  var meetingConfig = {
    apiKey: tmpArgs.apiKey,
    meetingNumber: tmpArgs.mn,
    userName: (function () {
      if (tmpArgs.name) {
        try {
          return testTool.b64DecodeUnicode(tmpArgs.name);
        } catch (e) {
          return tmpArgs.name;
        }
      }
      return (
        "CDN#" +
        tmpArgs.version +
        "#" +
        testTool.detectOS() +
        "#" +
        testTool.getBrowserInfo()
      );
    })(),
    passWord: tmpArgs.pwd,
    role: parseInt(tmpArgs.role, 10),
    userEmail: (function () {
      try {
        return testTool.b64DecodeUnicode(tmpArgs.email);
      } catch (e) {
        return tmpArgs.email;
      }
    })(),
    lang: tmpArgs.lang,
    signature: tmpArgs.signature || "",
    china: tmpArgs.china === "1",
  };

  // a tool use debug mobile device
  if (testTool.isMobileDevice()) {
    vConsole = new VConsole();
  }
  console.log(JSON.stringify(ZoomMtg.checkSystemRequirements()));

  // it's option if you want to change the WebSDK dependency link resources. setZoomJSLib must be run at first
  // ZoomMtg.setZoomJSLib("https://source.zoom.us/1.7.10/lib", "/av"); // CDN version defaul
  if (meetingConfig.china)
    ZoomMtg.setZoomJSLib("https://jssdk.zoomus.cn/1.7.10/lib", "/av"); // china cdn option
  ZoomMtg.preLoadWasm();
  ZoomMtg.prepareJssdk();
    function beginJoin(signature) {
        var initObj = {
            webEndpoint: meetingConfig.webEndpoint,
            success: function () {
                console.log(meetingConfig);
                console.log("signature", signature);
                $.i18n.reload(meetingConfig.lang);
                ZoomMtg.join({
                    meetingNumber: meetingConfig.meetingNumber,
                    userName: meetingConfig.userName,
                    signature: signature,
                    apiKey: meetingConfig.apiKey,
                    userEmail: meetingConfig.userEmail,
                    passWord: meetingConfig.passWord,
                    success: function (res) {
                        console.log("join meeting success");
                        console.log("get attendeelist");
                        ZoomMtg.getAttendeeslist({});
                        ZoomMtg.getCurrentUser({
                            success: function (res) {
                                console.log("success getCurrentUser", res.result.currentUser);
                            },
                        });
                    },
                    error: function (res) {
                        console.log(res);
                    },
                });
            },
            error: function (res) {
                console.log(res);
            },
        };

        debugger;
        if (tmpArgs.isGuest == 1) {
            initObj.leaveUrl = "/home/index";
            initObj.showMeetingHeader = false; //option
            initObj.disableInvite = false; //optional
            initObj.disableCallOut = false; //optional
            initObj.disableRecord = false; //optional
            initObj.disableJoinAudio = false; //optional
            initObj.audioPanelAlwaysOpen = true; //optional
            initObj.showPureSharingContent = false; //optional
            initObj.isSupportAV = false; //optional
            initObj.isSupportChat = false; //optional
            initObj.isSupportQA = true; //optional
            initObj.isSupportCC = true; //optional
            initObj.screenShare = true; //optional
            initObj.rwcBackup = ''; //optional
            initObj.videoDrag = true; //optional
            initObj.sharingMode = 'both'; //optional
            initObj.videoHeader = true; //optional
            initObj.isLockBottom = true; // optional
            initObj.isSupportNonverbal = true; // optional
            initObj.isShowJoiningErrorDialog = true; // optional
            initObj.inviteUrlFormat = ''; // optional
            initObj.meetingInfo = [ // optional
                'topic',
                'host',
                'mn',
                'pwd'
            ];
            initObj.disableVoIP = false; // optional
            initObj.disableReport = false; // optional
        }
        else {

            initObj.leaveUrl = "/portal/session",
        }

        ZoomMtg.init(initObj);
    }

    beginJoin(meetingConfig.signature);
})();
