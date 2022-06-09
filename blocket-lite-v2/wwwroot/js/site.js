function show_car() {
    let category = document.getElementById("category").value;

    switch (category) {

        case "vehicle":
            document.querySelector(".forms_div").style.display = "block";
            document.querySelector(".vehicle_form").style.display = "block";
            document.querySelector(".clothes_form").style.display = "none";
            document.querySelector(".submit_form").style.display = "block";
            break;
        case "cloths":
            document.querySelector(".forms_div").style.display = "block";
            document.querySelector(".clothes_form").style.display = "block";
            document.querySelector(".vehicle_form").style.display = "none";
            document.querySelector(".submit_form").style.display = "block";
            break;
        default:
            document.querySelector(".submit_form").style.display = "none";
            document.querySelector(".forms_div").style.display = "none";
            document.querySelector(".vehicle_form").style.display = "none";
            document.querySelector(".clothes_form").style.display = "none";
            break;

    }
}

show_car();