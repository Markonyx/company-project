$(document).ready(() => {
    // podaci od interesa
    var host = 'http://' + window.location.host;
    var zaposleniUrl = host + "/api/zaposleni/";
    var kompanijeUrl = host + "/api/kompanije/";
    var pretragaUrl = host + "/api/zaposlenje";
    var token = null;
    var headers = {};
    var formAction;
    var editingId;
    var userInfoUrl = host + "/api/Account/UserInfo";
    var userEmail = null;

    // registracija korisnika
    $("#btnRegistracija").click(function (e) {
        e.preventDefault();

        var email = $("#priEmail").val();
        var loz1 = $("#priLoz").val();

        // objekat koji se salje
        var sendData = {
            "Email": email,
            "Password": loz1,
            "ConfirmPassword": loz1
        };


        $.ajax({
            type: "POST",
            url: host + "/api/Account/Register",
            data: sendData

        }).done(function (data) {
            alert("Uspesna registracija na sistem!");
            $("#priEmail").val('');
            $("#priLoz").val('');

        }).fail(function (data) {
            alert("Greska prilikom registracije! Proverite unos!");
        });

    });


    // prijava korisnika
    $("#btnPrijava").click(function (e) {
        e.preventDefault();

        var email = $("#priEmail").val();
        var loz = $("#priLoz").val();

        // objekat koji se salje
        var sendData = {
            "grant_type": "password",
            "username": email,
            "password": loz
        };

        $.ajax({
            "type": "POST",
            "url": host + "/Token",
            "data": sendData

        }).done(function (data) {
            console.log(data);
            token = data.access_token;
            userEmail = data.userName;
            $("#priEmail").val('');
            $("#priLoz").val('');
            $("#regIPrijava").css("display", "none");
            $("#neprijavljenKorisnikDiv").css("dispaly", "none");
            $("#welcomeDiv").css("display", "block");
            $("#pretragaDiv").css("display", "block");
            refreshZaposleni();

        }).fail(function (data) {
            alert("Greska prilikom prijave!");
        });
    });

    // odjava korisnika sa sistema
    $("#odjavise").click(function () {
        token = null;
        headers = {};
        $("#neprijavljeniKorisnikDiv").css("display", "block");
        $("#welcomeDiv").css("display", "none");
        $("#pretragaDiv").css("display", "none");
        $("#formaIzmenaDiv").css("display", "none");
        refreshZaposleni();
    });

    refreshZaposleni();
    refreshKompanije();

    $("#btnRegIPrijava").click(() => {
        $("#neprijavljeniKorisnikDiv").css("display", "none");
        $("#regIPrijava").css("display", "block");
    });

    $("#btnPocetak").click(() => {
        $("#neprijavljeniKorisnikDiv").css("display", "block");
        $("#regIPrijava").css("display", "none");
    });

    //odustajanje od izmene
    $("#btnOdustajanjeIzmena").click(() => {
        $("#formaIzmenaDiv").css("display", "none");
    });

    // pripremanje dogadjaja za brisanje
    $("body").on("click", "#btnDelete", deleteZaposleni);

    // priprema dogadjaja za izmenu
    $("body").on("click", "#btnEdit", editZaposleni);

    function setZaposleni(data, status) {
        console.log("Status: " + status);

        var $container = $("#data");
        $container.empty();

        console.log(data);
        // ispis naslova
        var div = $("<div></div>");
        var h1 = $("<h1 align='center'>Zaposleni</h1><br />");
        div.append(h1);
        // ispis tabele
        var table = $("<table class='table table-bordered table-hover'></table>");
        if (!token) {
            var header = $("<thead><tr><th>Ime i prezime</th><th>Godina rodjenja</th><th>Godina zaposlenja</th><th>Kompanija</th></tr></thead>");
            table.append(header);
        }
        if (token) {
            var headerToken = $("<thead><tr><th>Ime i prezime</th><th>Godina rodjenja</th><th>Godina zaposlenja</th><th>Kompanija</th><th>Plata</th><th>Brisanje</th><th>Izmena</th></tr></thead>");
            table.append(headerToken);
        }
        for (i = 0; i < data.length; i++) {
            // prikazujemo novi red u tabeli
            var row = "<tbody><tr>";
            // prikaz podataka
            var displayData = "<td>" + data[i].ImeIPrezime + "</td><td>" + data[i].GodinaRodjenja + "</td><td>" + data[i].GodinaZaposlenja + "</td><td>" + data[i].KompanijaNaziv + "</td>";
            var displayDataToken = "<td>" + data[i].ImeIPrezime + "</td><td>" + data[i].GodinaRodjenja + "</td><td>" + data[i].GodinaZaposlenja + "</td><td>" + data[i].KompanijaNaziv + "</td><td>" + data[i].Plata + "</td>";
            // prikaz dugmadi za izmenu i brisanje
            var stringId = data[i].Id.toString();
            var displayDelete = "<td><button class='btn btn-danger' id=btnDelete name=" + stringId + ">Delete</button></td>";
            var displayEdit = "<td><button class='btn btn-warning' id=btnEdit name=" + stringId + ">Edit</button></td>";
            if (!token) {
                row += displayData + "</tr></tbody>";
            }
            else if (token) {
                row += displayDataToken + displayDelete + displayEdit + "</tr></tbody>";
                $("#welcomeMessage").text(userEmail);
                //$("#pretragaDiv").css("display", "block");
            }
            table.append(row);
        }

        div.append(table);

        // ispis novog sadrzaja
        $container.append(div);
    }

    //pretraga
    $("#pretragaForm").submit((e) => {
        e.preventDefault();
        // korisnik mora biti ulogovan
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        var pocetak = $("#pocetak").val();
        var kraj = $("#kraj").val();

        //objekat koji se salje
        var sendData = {
            "Pocetak": pocetak,
            "Kraj": kraj
        };

        $.ajax({
            "url": pretragaUrl,
            "type": "POST",
            "data": sendData,
            "headers": headers
        })
            .done((data, status) => {
                setZaposleni(data, status);
                $("#pocetak").val('');
                $("#kraj").val('');
            })
            .fail((data, status) => {
                alert("Greska prilikom pretrage!");
            });

    });

    // izmena form zaposleni
    $("#izmenaForm").submit(function (e) {
        // sprecavanje default akcije forme
        e.preventDefault();
        // korisnik mora biti ulogovan
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        var kompanijaId = $("#kompanijaId").val();
        var imePrezime = $("#imePrezime").val();
        var godinaRodjenja = $("#godinaRodjenja").val();
        var godinaZaposlenja = $("#godinaZaposlenja").val();
        var plata = $("#plata").val();

        var httpAction;
        var sendData;
        var url;


        httpAction = "PUT";
        url = zaposleniUrl + editingId.toString();
        sendData = {
            "Id": editingId,
            "ImeIPrezime": imePrezime,
            "GodinaRodjenja": godinaRodjenja,
            "GodinaZaposlenja": godinaZaposlenja,
            "Plata": plata,
            "KompanijaId": kompanijaId
        };

        console.log("Objekat za slanje");
        console.log(sendData);

        $.ajax({
            url: url,
            type: httpAction,
            data: sendData,
            "headers": headers
        })
            .done(function (data, status) {
                refreshZaposleni();
                refreshKompanije();
                $("#formaIzmenaDiv").css("display", "none");
            })
            .fail(function (data, status) {
                alert("Greska prilikom izmene!");
            });

    });

    // izmena zaposlenog
    function editZaposleni() {
        // izvlacimo id
        var editId = this.name;
        // saljemo zahtev da dobavimo tog zaposlenog
        $.ajax({
            url: zaposleniUrl + editId.toString(),
            type: "GET"
        })
            .done(function (data, status) {
                $("#formaIzmenaDiv").css("display", "block");
                $("#kompanijaId").val(data.KompanijaId);
                $("#imePrezime").val(data.ImeIPrezime);
                $("#godinaRodjenja").val(data.GodinaRodjenja);
                $("#godinaZaposlenja").val(data.GodinaZaposlenja);
                $("#plata").val(data.Plata);
                editingId = data.Id;
                formAction = "Update";
            })
            .fail(function (data, status) {
                alert("Desila se greska!");
            });

    }

    // brisanje zaposlenog
    function deleteZaposleni() {
        // korisnik mora biti ulogovan
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }
        // izvlacimo {id}
        var deleteID = this.name;
        // saljemo zahtev 
        $.ajax({
            url: zaposleniUrl + deleteID.toString(),
            type: "DELETE",
            "headers": headers
        })
            .done(function (data, status) {
                refreshZaposleni();
            })
            .fail(function (data, status) {
                alert("Greska prilikom brisanja!");
            });
    }

    function refreshZaposleni() {
        console.log("URL zahteva: " + zaposleniUrl);
        $.get(zaposleniUrl, setZaposleni)
            .fail(() => {
                alert("Greska prilikom ucitavanja sa servera!");
            });
    }

    function refreshKompanije() {
        let dropown = $("#kompanijaId");
        dropown.empty();
        $.getJSON(kompanijeUrl, (data, status) => {
            $.each(data, (key, entry) => {
                $("#kompanijaId").append($("<option></option>").attr('value', entry.Id).text(entry.Naziv));
            });
        });
    }
    
});