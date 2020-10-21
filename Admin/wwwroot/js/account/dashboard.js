var profile;

(function () {

    function initPage() {
        getUserData();
    }

    initPage();


    function getUserData() {
        const config = {
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('token')}`
            }
        }
        axios.get(`/api/auth/me`, config)
            .then(res => {
                renderUserData(res.data);
            })
            .catch(err => {
                console.log(err.response);
                showResponseError(err);
            })
    }
    
    function renderUserData(data) {
        $("#d-full-name").text(`${data.firstName} ${data.lastName}`);
        $("#d-email").text(data.email);
        $("#d-gravatar").attr("src", data.photoUrl);
        $("#d-gravatar-2").attr("src", data.photoUrl);
    }


}) ();