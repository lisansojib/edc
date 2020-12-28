(function () {
  var testTool = window.testTool;
  //if (testTool.isMobileDevice()) {
  //  vConsole = new VConsole();
  //}
  console.log("checkSystemRequirements");
  console.log(JSON.stringify(ZoomMtg.checkSystemRequirements()));

  // it's option if you want to change the WebSDK dependency link resources. setZoomJSLib must be run at first
  // if (!china) ZoomMtg.setZoomJSLib('https://source.zoom.us/1.7.10/lib', '/av'); // CDN version default
  // else ZoomMtg.setZoomJSLib('https://jssdk.zoomus.cn/1.7.10/lib', '/av'); // china cdn option
  // ZoomMtg.setZoomJSLib('http://localhost:9999/node_modules/@zoomus/websdk/dist/lib', '/av'); // Local version default, Angular Project change to use cdn version
  ZoomMtg.preLoadWasm();

  var API_KEY = "jTplI2yZQKOY-E2GNRchuQ";

  /**
   * NEVER PUT YOUR ACTUAL API SECRET IN CLIENT SIDE CODE, THIS IS JUST FOR QUICK PROTOTYPING
   * The below generateSignature should be done server side as not to expose your api secret in public
   * You can find an eaxmple in here: https://marketplace.zoom.us/docs/sdk/native-sdks/web/essential/signature
   */
  var API_SECRET = "O8pzE32gpG5SAWIy0kwnydDYV95wd5Hbp9bn";

  // some help code, remember mn, pwd, lang to cookie, and autofill.
  document.getElementById("display_name").value =
    "CDN" +
    ZoomMtg.getJSSDKVersion()[0] +
    testTool.detectOS() +
    "#" +
    testTool.getBrowserInfo();
  
  if (testTool.getCookie("meeting_lang"))
    document.getElementById("meeting_lang").value = testTool.getCookie(
      "meeting_lang"
    );

  document
    .getElementById("meeting_lang")
    .addEventListener("change", function (e) {
      testTool.setCookie(
        "meeting_lang",
        document.getElementById("meeting_lang").value
      );
      testTool.setCookie(
        "_zm_lang",
        document.getElementById("meeting_lang").value
      );
    });

    testTool.setCookie("meeting_pwd", document.getElementById("meeting_pwd").value);
    testTool.setCookie("meeting_number", document.getElementById("meeting_number").value);
      
  // click join iframe buttong
  document.getElementById("join_meeting")
      .addEventListener("click", function (e) {
          e.preventDefault();
          localStorage.setItem("is_guest", document.getElementById("is_guest").value);
          var meetingConfig = testTool.getMeetingConfig();
          if (!meetingConfig.mn || !meetingConfig.name) {
            alert("Meeting number or username is empty");
            return false;
          }
          var signature = ZoomMtg.generateSignature({
            meetingNumber: meetingConfig.mn,
            apiKey: API_KEY,
            apiSecret: API_SECRET,
            role: meetingConfig.role,
              success: function (res) {
                  console.log(res.result);
                  $("#join_meeting").hide();
                  meetingConfig.signature = res.result;
                  meetingConfig.apiKey = API_KEY;
                  var joinUrl = testTool.getCurrentDomain() + "/guest/meeting?" + testTool.serialize(meetingConfig);
                  document.getElementById("websdk-iframe").src = joinUrl;
            },
          });
      });
})();
