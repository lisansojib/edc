const MAX_MOUSE_ACTIVITY = 1000000;
var mouse_activity = 0;

let userState = {
    pageViewTime : Date.now,
    pageQuitTime: Date.now,
    onPageDurationMin: 0,
    onPageDurationSec:0,
    mouseActivity: 0,
    isFullScreen: false,
}

// Mouse move activity
document.onmousemove = function () {
    mouse_activity++;

    userState.mouseActivity = ((mouse_activity /MAX_MOUSE_ACTIVITY) * 100).toFixed(4);
}


// Full screen activity
let player = document.getElementById("#zoom-player");
player.addEventListener("fullscreenchange", function () {
    userState.isFullScreen = true;
})



// Load activity
window.onpageShow = function () {
    userState.pageViewTime = new Date().getTime();
    console.log('Page showing');
}

window.onpagehide = function () {
    userState.pageQuitTime = new Date().getTime();
    console.log('Page hidden');
    userState.onPageDurationMin = getDurationInMinutes();
    userState.onPageDurationSec = getDurationInSeconds();
}

function getDuration() {
    return (userState.pageQuitTime - userState.pageViewTime);
}

function getDurationInSeconds() {
    return (getDuration() / 1000).toFixed(1);
}

function getDurationInMinutes() {
    return (getDurationInSeconds() / 60).toFixed(2);
}

function printUserActivity() {
    console.log(`Mouse activity : ${userState.mouseActivity} %`);
    console.log(`Page viewed: ${userState.onPageDurationMin} mins or, ${userState.onPageDurationSec} seconds`);
    console.log(`Was full screen: ${userState.isFullScreen ? 'Yes': 'No'} `)
}