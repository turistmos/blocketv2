//alert("Test!");

let language = "sv";
let svenska = ["Kategori", "Titel", "Pris", "Beskrivning", "Färg", "Bild", "Antal mil", "Årtal", "Storlek", "Kön"];

function UpdateLanguage(){
    if(language == "sv"){
        let labels = document.getElementsByTagName("label");

        //Uppdattera labels för språket
        for (var i = 0; i < labels.length; i++) {
            labels[i].innerHTML = svenska[i];
        }
    }
}

UpdateLanguage();