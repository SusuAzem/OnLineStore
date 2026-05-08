let submitForm = document.querySelector("#messageForm");
let name, email, message;
//Add Even Listener
submitForm.addEventListener("submit", submitData);

// Submit Function
function submitData(e) {
    e.preventDefault();
    name = document.getElementById("Message_Name").value;
    email = document.getElementById("Message_Email").value;
    message = document.getElementById("Message_Content").value;

    if (name === "") {
        alert("الرجاء كتابة الاسم ");
        return false;
    }
    if (email === "") {
        alert("الرجاء كتابة البريد الالكتروني ");
        return false;
    }
    if (message === "") {
        alert("الرجاء كتابة المحتوى ");
        return false;
    }
    fetch("/Messages/Create", {
        method: "POST",
        headers: {
            Accept: "application/json, text/plain, */*",
            "Content-Type": "application/json",
        },
        body: JSON.stringify({ name: name, email: email, message: message }),
    })
        .then((res) => res.json())
        .then(() => {
            submitForm.reset();
            //alertify.success("Message Send");           
        })
        .catch((error) => {
            console.error("Error", error);
        });
}
