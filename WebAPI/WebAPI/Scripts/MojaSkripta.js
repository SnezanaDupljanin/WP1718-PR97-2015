let IspisiPodatke = function (korisnik) {
    var pol = ``;
    if (korisnik.Pol == "1") {
        pol = `Zenski`;
    } else {
        pol = `Muski`;
    }
    $("#prikazPodataka").html(`<table class="table table-bordered">
        <thead>
            <tr class="success">
                <th colspan="2">Moji podaci</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Korisnicko ime:</td>
                <td>` + ((korisnik.KorisnickoIme != null) ? korisnik.KorisnickoIme : `-`) + `</td>
            </tr>
            <tr>
                <td>Ime:</td>
                <td>` + ((korisnik.Ime != null) ? korisnik.Ime : `-`) + `</td>
            </tr>
            <tr>
                <td>Prezime:</td>
                <td>` + ((korisnik.Prezime != null) ? korisnik.Prezime : `-`) + `</td>
            </tr>
            <tr>
                <td>Kontakt telefon:</td>
                <td>` + ((korisnik.KontaktTelefon != null) ? korisnik.KontaktTelefon : `-`) + `</td>
            </tr>
            <tr>
                <td>JMBG:</td><td>` + ((korisnik.Jmbg != null) ? korisnik.Jmbg : `-`) + `</td>
            </tr>
            <tr>
                <td>Pol:</td>
                <td>` + pol + `</td>
            </tr>
            <tr>
                <td>Email:</td>
                <td>` + ((korisnik.Email != null) ? korisnik.Email : `-`) + `</td>
            </tr>
        </tbody>
    </table>`);
};

let IzmijeniSifru = function (korisnik) {
    $("#prikazPodataka").html(`<table class="table table-bordered">
                    <thead>
                        <tr class="success">
                            <th colspan="2">
                                Promjena sifre                         
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr id="upozorenje" class="hidden">
                            <td colspan="2"><strong>Greska!</strong> Nova i ponovljena lozinka se ne poklapaju.</td>                
                        </tr>
                        <tr id="upozorenje2" class="hidden">
                            <td colspan="2"><strong>Greska!</strong> Pogresno unesena stara lozinka.</td>                
                        </tr>
                        <tr>
                            <td>Stara sifra:</td>
                            <td>
                                <input type="password" id="txtStaraSifra" placeholder="Stara sifra..." />
                            </td>
                        </tr>
                        <tr>
                            <td>Nova sifra:</td>
                            <td>
                                <input type="password" id="txtNovaSifra" placeholder="Nova sifra..." />
                            </td>
                        </tr>
                           <tr>
                            <td>Ponovi novu sifru:</td>
                            <td>
                                <input type="password" id="txtPonovljenaSifra" placeholder="Nova sifra..." />
                            </td>
                        </tr>
                        <tr class="success">
                            <td colspan="2">
                                <input id="btnSacuvajIzmjenu" class="btn btn-success pull-right" type="button" value="Sacuvaj">
                        <!--/form-->
                            </td>
                        </tr>
                    </tbody>
                </table>`
    );
    $("#btnSacuvajIzmjenu").click(function () {
        var stara = korisnik.Lozinka;
        var staraUnesena = $("#txtStaraSifra").val();
        var novaUnesena = $("#txtNovaSifra").val();
        var novaPonovoUnesena = $("#txtPonovljenaSifra").val();
        if (stara != staraUnesena) {
            $("#upozorenje").addClass(`hidden`);
            $("#upozorenje2").removeClass(`hidden`);
            $("#upozorenje2").addClass(`alert-danger`);
            $("#txtStaraSifra").val(``);
            $("#txtStaraSifra").focus();
            $("#txtNovaSifra").val(``);
            $("#txtPonovljenaSifra").val(``);
            //alert(`stara sifra nije u redu`);
        } else if (novaUnesena != novaPonovoUnesena) {
            $("#upozorenje2").addClass(`hidden`);
            $("#upozorenje").removeClass(`hidden`);
            $("#upozorenje").addClass(`alert-danger`);
            $("#txtNovaSifra").val(``);
            $("#txtNovaSifra").focus();
            $("#txtPonovljenaSifra").val(``);
            //alert(`nova i ponovljena nisu iste`);
        } else {
            $("#upozorenje2").addClass(`hidden`);
            $("#upozorenje").addClass(`hidden`);
            //ispravno, sacuvaj
            //alert(`moze`);
            $.post("/api/Dispecer", { KorisnickoIme: korisnik.KorisnickoIme, Lozinka: novaUnesena, Email: korisnik.Email, Jmbg: korisnik.Jmbg, KontaktTelefon: korisnik.KontaktTelefon, Ime: korisnik.Ime, Prezime: korisnik.Prezime, Pol: korisnik.Pol }, function (data) {
                //alert(remember);
                //alert(data);
            })
                .done(function () {
                    //alert("uspjesno izmijenjeno");
                    location.href = "Dispecer.html";
                })
                .fail(function () {
                    //alert("ne moze");
                });
        }
    });
};

let IzmijeniPodatke = function (korisnik) {
    var pol = ``;
    if (korisnik.Pol == "1") {
        pol = `<input type="radio"  name="pol" value="0"> Muski
               <input type="radio"  name="pol" value="1" checked="checked"> Zenski`;
    } else {
        pol = `<input type="radio"  name="pol" value="0" checked="checked"> Muski
               <input type="radio"  name="pol" value="1"> Zenski`;
    }
    $("#prikazPodataka").html(`<table class="table table-bordered">
        <thead>
            <tr class="success">
                <th colspan="2">
                    Registracija                    
                </th>
            </tr>
        </thead>
        <tbody>
            <tr class="hidden" id="upozorenje">
                <td colspan="2"></td>
            </tr>
            <tr>
                <td>Korisnicko ime:</td>
                <td>`
        + ((korisnik.KorisnickoIme != null) ? korisnik.KorisnickoIme : ``) +
        `</td>
            </tr>
            
            <tr>
                <td>Ime:</td>
                <td>
                    <input type="text" id="txtIme" value="`+ ((korisnik.Ime != null) ? korisnik.KorisnickoIme : ``) + `"/>
                </td>
            </tr>
            <tr>
                <td>Prezime:</td>
                <td>
                    <input type="text" id="txtPrezime" value="`+ ((korisnik.Prezime != null) ? korisnik.Prezime : ``) + `"/>
                </td>
            </tr>
            <tr>
                <td>Kontakt telefon:</td>
                <td>
                    <input type="text" id="txtTelefon" value="`+ ((korisnik.KontaktTelefon != null) ? korisnik.KontaktTelefon : ``) + `"/>
                </td>
            </tr>
            <tr>
                <td>JMBG:</td>
                <td>
                    <input type="text" id="txtJMBG" value="`+ ((korisnik.Jmbg != null) ? korisnik.Jmbg : ``) + `"/>
                </td>
            </tr>
            <tr>
                <td>Pol:</td>
                <td>`
        + pol +
        `</td>
            </tr>
            <tr>
                <td>Email:</td>
                <td>
                    <input type="email" id="txtEmail" value="`+ ((korisnik.Email != null) ? korisnik.Email : ``) + `"/>
                </td>
            </tr>
            <tr class="success">
                <td colspan="2">
                    <input id="btnIzmijeniPodatke" class="btn btn-success" type="button"
                           value="Izmijeni podatke" />
                </td>
            </tr>
        </tbody>
    </table>`);
    $("#txtIme").keyup(function () {
        if (!validateString($("#txtIme").val())) {
            $(this).addClass("alert-danger");
        } else {
            $(this).removeClass("alert-danger");
        }
    });

    $("#txtPrezime").keyup(function () {
        if (!validateString($(this).val())) {
            $(this).addClass("alert-danger");
        } else {
            $(this).removeClass("alert-danger");
        }
    });
    $("#txtTelefon").keyup(function () {
        if (!validateNumber($(this).val())) {
            $(this).addClass("alert-danger");
        } else {
            $(this).removeClass("alert-danger");
        }
    });
    $("#txtJMBG").keyup(function () {
        if (!validateNumber($(this).val())) {
            $(this).addClass("alert-danger");
        } else {
            $(this).removeClass("alert-danger");
        }
    });
    //isValidEmailAddress
    $("#txtEmail").keyup(function () {
        if (!isValidEmailAddress($(this).val())) {
            $(this).addClass("alert-danger");
        } else {
            $(this).removeClass("alert-danger");
        }
    });
    $("#btnIzmijeniPodatke").click(function () {
        //alert(`prije promjenljivih`);

        var Email = $("#txtEmail").val();
        var Jmbg = $("#txtJMBG").val();
        var KontaktTelefon = $("#txtTelefon").val();
        var Ime = $("#txtIme").val();
        var Prezime = $("#txtPrezime").val();
        var Pol = $("input:radio[name=pol]:checked").val();
        //alert(Pol);

        if (isValidEmailAddress(Email) && validateString(Ime) && KontaktTelefon.length == 10 && Jmbg.length == 13 && validateString(Prezime) && validateNumber(Jmbg) && validateNumber(KontaktTelefon)) {
            $("#upozorenje").addClass(`hidden`);
            //alert("posle promjenljivih");
            $.post("/api/Dispecer", { KorisnickoIme: korisnik.KorisnickoIme, Lozinka: korisnik.Lozinka, Email: Email, Jmbg: Jmbg, KontaktTelefon: KontaktTelefon, Ime: Ime, Prezime: Prezime, Pol: Pol }, function (data) {
                //alert(remember);
                //alert(data);
            })
                .done(function () {
                    //alert("uspjesno izmijenjeni podaci");
                    location.href = "Dispecer.html";
                })
                .fail(function () {
                    //alert("greska prilikom izmjene podataka");
                });
        } else {
            if (!validateString(Ime)) {
                $("#upozorenje").removeClass(`hidden`);
                $("#upozorenje").addClass(`alert-danger`);
                $("#upozorenje").html(`<td colspan="2"><strong>Greska!</strong> Ime moze sadrzati samo slova.</td>`);
                $("#txtIme").focus();
            } else if (!validateString(Prezime)) {
                $("#upozorenje").removeClass(`hidden`);
                $("#upozorenje").addClass(`alert-danger`);
                $("#upozorenje").html(`<td colspan="2"><strong>Greska!</strong> Prezime moze sadrzati samo slova.</td>`);
                $("#txtPrezime").focus();
            } else if (!validateNumber(KontaktTelefon) || KontaktTelefon.length != 10) {
                $("#upozorenje").removeClass(`hidden`);
                $("#upozorenje").addClass(`alert-danger`);
                $("#upozorenje").html(`<td colspan="2"><strong>Greska!</strong> Telefon moze sadrzati samo 10 brojeva.</td>`);
                $("#txtTelefon").focus();
            } else if (!validateNumber(Jmbg) || Jmbg.length != 13) {
                $("#upozorenje").removeClass(`hidden`);
                $("#upozorenje").addClass(`alert-danger`);
                $("#upozorenje").html(`<td colspan="2"><strong>Greska!</strong> Jmbg mora sadrzati 13 brojeva.</td>`);
                $("#txtJMBG").focus();
            } else if (!isValidEmailAddress(Email)) {
                $("#upozorenje").removeClass(`hidden`);
                $("#upozorenje").addClass(`alert-danger`);
                $("#upozorenje").html(`<td colspan="2"><strong>Greska!</strong> Email mora biti napisan u formatu nesto@yahoo.com.</td>`);
                $("#txtEmail").focus();
            }
        }
    });
};

let PrikaziPodatkeOAutomobilu = function (korisnik) {
    var tip = ``;
    switch (korisnik.Automobil.TipAutomobila) {
        case 0:
            tip = "Nepoznato";
            break;
        case 1:
            tip = "Putnicki automobil";
            break;
        case 2:
            tip = "Kombi vozilo";
            break;
        default:
            tip = "";
    }
    $("#prikazPodataka").html(`<table class="table table-bordered">
        <thead>
            <tr class="success">
                <th colspan="2">Podaci o automobilu</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Godiste automobila:</td>
                <td>` + ((korisnik.Automobil.GodisteAutomobila != null) ? korisnik.Automobil.GodisteAutomobila : `-`) + `</td>
            </tr>
            <tr>
                <td>Broj taksi vozila:</td>
                <td>` + ((korisnik.Automobil.BrojTaksiVozila != null) ? korisnik.Automobil.BrojTaksiVozila : `-`) + `</td>
            </tr>
            <tr>
                <td>Tip automobila:</td>
                <td>` + tip + `</td>
            </tr>
        </tbody>
    </table>`);
};

let IzmijeniPodatke2 = function (korisnik) {
    var pol = ``;
    if (korisnik.Pol == "1") {
        pol = `<input type="radio"  name="pol" value="0"> Muski
               <input type="radio"  name="pol" value="1" checked="checked"> Zenski`;
    } else {
        pol = `<input type="radio"  name="pol" value="0" checked="checked"> Muski
               <input type="radio"  name="pol" value="1"> Zenski`;
    }
    $("#prikazPodataka").html(`<table class="table table-bordered">
        <thead>
            <tr class="success">
                <th colspan="2">
                    Registracija                    
                </th>
            </tr>
        </thead>
        <tbody>
            <tr class="hidden" id="upozorenje">
                <td colspan="2"></td>
            </tr>
            <tr>
                <td>Korisnicko ime:</td>
                <td>`
        + ((korisnik.KorisnickoIme != null) ? korisnik.KorisnickoIme : ``) +
        `</td>
            </tr>
            
            <tr>
                <td>Ime:</td>
                <td>
                    <input type="text" id="txtIme" value="`+ ((korisnik.Ime != null) ? korisnik.Ime : ``) + `"/>
                </td>
            </tr>
            <tr>
                <td>Prezime:</td>
                <td>
                    <input type="text" id="txtPrezime" value="`+ ((korisnik.Prezime != null) ? korisnik.Prezime : ``) + `"/>
                </td>
            </tr>
            <tr>
                <td>Kontakt telefon:</td>
                <td>
                    <input type="text" id="txtTelefon" value="`+ ((korisnik.KontaktTelefon != null) ? korisnik.KontaktTelefon : ``) + `"/>
                </td>
            </tr>
            <tr>
                <td>JMBG:</td>
                <td>
                    <input type="text" id="txtJMBG" value="`+ ((korisnik.Jmbg != null) ? korisnik.Jmbg : ``) + `"/>
                </td>
            </tr>
            <tr>
                <td>Pol:</td>
                <td>`
        + pol +
        `</td>
            </tr>
            <tr>
                <td>Email:</td>
                <td>
                    <input type="email" id="txtEmail" value="`+ ((korisnik.Email != null) ? korisnik.Email : ``) + `"/>
                </td>
            </tr>
            <tr class="success">
                <td colspan="2">
                    <input id="btnIzmijeniPodatke" class="btn btn-success" type="button"
                           value="Izmijeni podatke" />
                </td>
            </tr>
        </tbody>
    </table>`);
    $("#txtIme").keyup(function () {
        if (!validateString($("#txtIme").val())) {
            $(this).addClass("alert-danger");
        } else {
            $(this).removeClass("alert-danger");
        }
    });

    $("#txtPrezime").keyup(function () {
        if (!validateString($(this).val())) {
            $(this).addClass("alert-danger");
        } else {
            $(this).removeClass("alert-danger");
        }
    });
    $("#txtTelefon").keyup(function () {
        if (!validateNumber($(this).val())) {
            $(this).addClass("alert-danger");
        } else {
            $(this).removeClass("alert-danger");
        }
    });
    $("#txtJMBG").keyup(function () {
        if (!validateNumber($(this).val())) {
            $(this).addClass("alert-danger");
        } else {
            $(this).removeClass("alert-danger");
        }
    });
    //isValidEmailAddress
    $("#txtEmail").keyup(function () {
        if (!isValidEmailAddress($(this).val())) {
            $(this).addClass("alert-danger");
        } else {
            $(this).removeClass("alert-danger");
        }
    });
    $("#btnIzmijeniPodatke").click(function () {
        //alert(`prije promjenljivih`);

        var Email = $("#txtEmail").val();
        var Jmbg = $("#txtJMBG").val();
        var KontaktTelefon = $("#txtTelefon").val();
        var Ime = $("#txtIme").val();
        var Prezime = $("#txtPrezime").val();
        var Pol = $("input:radio[name=pol]:checked").val();
        //alert(Pol);

        if (isValidEmailAddress(Email) && validateString(Ime) && KontaktTelefon.length == 10 && Jmbg.length == 13 && validateString(Prezime) && validateNumber(Jmbg) && validateNumber(KontaktTelefon)) {
            $("#upozorenje").addClass(`hidden`);
            //alert("posle promjenljivih");
            $.post("/api/Musterija", { KorisnickoIme: korisnik.KorisnickoIme, Lozinka: korisnik.Lozinka, Email: Email, Jmbg: Jmbg, KontaktTelefon: KontaktTelefon, Ime: Ime, Prezime: Prezime, Pol: Pol }, function (data) {
                //alert(remember);
                //alert(data);
            })
                .done(function () {
                    //alert("uspjesno registrovano, sad se uloguj");
                    location.href = "Musterija.html";
                })
                .fail(function () {
                    //alert("vec postoji korisnik sa datim korisnickim imenom");
                });
        } else {
            if (!validateString(Ime)) {
                $("#upozorenje").removeClass(`hidden`);
                $("#upozorenje").addClass(`alert-danger`);
                $("#upozorenje").html(`<td colspan="2"><strong>Greska!</strong> Ime moze sadrzati samo slova.</td>`);
                $("#txtIme").focus();
            } else if (!validateString(Prezime)) {
                $("#upozorenje").removeClass(`hidden`);
                $("#upozorenje").addClass(`alert-danger`);
                $("#upozorenje").html(`<td colspan="2"><strong>Greska!</strong> Prezime moze sadrzati samo slova.</td>`);
                $("#txtPrezime").focus();
            } else if (!validateNumber(KontaktTelefon) || KontaktTelefon.length != 10) {
                $("#upozorenje").removeClass(`hidden`);
                $("#upozorenje").addClass(`alert-danger`);
                $("#upozorenje").html(`<td colspan="2"><strong>Greska!</strong> Telefon moze sadrzati samo 10 brojeva.</td>`);
                $("#txtTelefon").focus();
            } else if (!validateNumber(Jmbg) || Jmbg.length != 13) {
                $("#upozorenje").removeClass(`hidden`);
                $("#upozorenje").addClass(`alert-danger`);
                $("#upozorenje").html(`<td colspan="2"><strong>Greska!</strong> Jmbg mora sadrzati 13 brojeva.</td>`);
                $("#txtJMBG").focus();
            } else if (!isValidEmailAddress(Email)) {
                $("#upozorenje").removeClass(`hidden`);
                $("#upozorenje").addClass(`alert-danger`);
                $("#upozorenje").html(`<td colspan="2"><strong>Greska!</strong> Email mora biti napisan u formatu nesto@yahoo.com.</td>`);
                $("#txtEmail").focus();
            }
        }
    });
};

function validateString(nekiString) {
    var re = /^[A-Za-z]+$/;
    if (re.test(nekiString))
        return true;
    else
        return false;
}

function validateNumber(nekiString) {
    var re = /^[0-9]+$/;
    if (re.test(nekiString))
        return true;
    else
        return false;
}

function isValidEmailAddress(emailAddress) {
    var pattern = new RegExp(/^(("[\w-+\s]+")|([\w-+]+(?:\.[\w-+]+)*)|("[\w-+\s]+")([\w-+]+(?:\.[\w-+]+)*))(@((?:[\w-+]+\.)*\w[\w-+]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$)|(@\[?((25[0-5]\.|2[0-4][\d]\.|1[\d]{2}\.|[\d]{1,2}\.))((25[0-5]|2[0-4][\d]|1[\d]{2}|[\d]{1,2})\.){2}(25[0-5]|2[0-4][\d]|1[\d]{2}|[\d]{1,2})\]?$)/i);
    return pattern.test(emailAddress);
};

let IzmijeniPodatke3 = function (korisnik) {
    var pol = ``;
    if (korisnik.Pol == "1") {
        pol = `<input type="radio"  name="pol" value="0"> Muski
               <input type="radio"  name="pol" value="1" checked="checked"> Zenski`;
    } else {
        pol = `<input type="radio"  name="pol" value="0" checked="checked"> Muski
               <input type="radio"  name="pol" value="1"> Zenski`;
    }
    $("#prikazPodataka").html(`<table class="table table-bordered">
        <thead>
            <tr class="success">
                <th colspan="2">
                    Registracija                    
                </th>
            </tr>
        </thead>
        <tbody>
            <tr class="hidden" id="upozorenje">
                <td colspan="2"></td>
            </tr>
            <tr>
                <td>Korisnicko ime:</td>
                <td>`
        + ((korisnik.KorisnickoIme != null) ? korisnik.KorisnickoIme : ``) +
        `</td>
            </tr>
            
            <tr>
                <td>Ime:</td>
                <td>
                    <input type="text" id="txtIme" value="`+ ((korisnik.Ime != null) ? korisnik.Ime : ``) + `"/>
                </td>
            </tr>
            <tr>
                <td>Prezime:</td>
                <td>
                    <input type="text" id="txtPrezime" value="`+ ((korisnik.Prezime != null) ? korisnik.Prezime : ``) + `"/>
                </td>
            </tr>
            <tr>
                <td>Kontakt telefon:</td>
                <td>
                    <input type="text" id="txtTelefon" value="`+ ((korisnik.KontaktTelefon != null) ? korisnik.KontaktTelefon : ``) + `"/>
                </td>
            </tr>
            <tr>
                <td>JMBG:</td>
                <td>
                    <input type="text" id="txtJMBG" value="`+ ((korisnik.Jmbg != null) ? korisnik.Jmbg : ``) + `"/>
                </td>
            </tr>
            <tr>
                <td>Pol:</td>
                <td>`
        + pol +
        `</td>
            </tr>
            <tr>
                <td>Email:</td>
                <td>
                    <input type="email" id="txtEmail" value="`+ ((korisnik.Email != null) ? korisnik.Email : ``) + `"/>
                </td>
            </tr>
            <tr class="success">
                <td colspan="2">
                    <input id="btnIzmijeniPodatke" class="btn btn-success" type="button"
                           value="Izmijeni podatke" />
                </td>
            </tr>
        </tbody>
    </table>`);
    $("#txtIme").keyup(function () {
        if (!validateString($("#txtIme").val())) {
            $(this).addClass("alert-danger");
        } else {
            $(this).removeClass("alert-danger");
        }
    });

    $("#txtPrezime").keyup(function () {
        if (!validateString($(this).val())) {
            $(this).addClass("alert-danger");
        } else {
            $(this).removeClass("alert-danger");
        }
    });
    $("#txtTelefon").keyup(function () {
        if (!validateNumber($(this).val())) {
            $(this).addClass("alert-danger");
        } else {
            $(this).removeClass("alert-danger");
        }
    });
    $("#txtJMBG").keyup(function () {
        if (!validateNumber($(this).val())) {
            $(this).addClass("alert-danger");
        } else {
            $(this).removeClass("alert-danger");
        }
    });
    //isValidEmailAddress
    $("#txtEmail").keyup(function () {
        if (!isValidEmailAddress($(this).val())) {
            $(this).addClass("alert-danger");
        } else {
            $(this).removeClass("alert-danger");
        }
    });

    $("#btnIzmijeniPodatke").click(function () {
        //alert(`prije promjenljivih`);

        var Email = $("#txtEmail").val();
        var Jmbg = $("#txtJMBG").val();
        var KontaktTelefon = $("#txtTelefon").val();
        var Ime = $("#txtIme").val();
        var Prezime = $("#txtPrezime").val();
        var Pol = $("input:radio[name=pol]:checked").val();
        if (isValidEmailAddress(Email) && validateString(Ime) && KontaktTelefon.length == 10 && Jmbg.length == 13 && validateString(Prezime) && validateNumber(Jmbg) && validateNumber(KontaktTelefon)) {
            $("#upozorenje").addClass(`hidden`);
            $.post("/api/Vozac", { KorisnickoIme: korisnik.KorisnickoIme, Lozinka: korisnik.Lozinka, Email: Email, Jmbg: Jmbg, KontaktTelefon: KontaktTelefon, Ime: Ime, Prezime: Prezime, Pol: Pol }, function (data) {

            })
                .done(function () {
                    //alert("uspjesno registrovano, sad se uloguj");
                    location.href = "Vozac.html";
                })
                .fail(function () {
                    //alert("vec postoji korisnik sa datim korisnickim imenom");
                });
        } else {
            if (!validateString(Ime)) {
                $("#upozorenje").removeClass(`hidden`);
                $("#upozorenje").addClass(`alert-danger`);
                $("#upozorenje").html(`<td colspan="2"><strong>Greska!</strong> Ime moze sadrzati samo slova.</td>`);
                $("#txtIme").focus();
            } else if (!validateString(Prezime)) {
                $("#upozorenje").removeClass(`hidden`);
                $("#upozorenje").addClass(`alert-danger`);
                $("#upozorenje").html(`<td colspan="2"><strong>Greska!</strong> Prezime moze sadrzati samo slova.</td>`);
                $("#txtPrezime").focus();
            } else if (!validateNumber(KontaktTelefon) || KontaktTelefon.length != 10) {
                $("#upozorenje").removeClass(`hidden`);
                $("#upozorenje").addClass(`alert-danger`);
                $("#upozorenje").html(`<td colspan="2"><strong>Greska!</strong> Telefon moze sadrzati samo 10 brojeva.</td>`);
                $("#txtTelefon").focus();
            } else if (!validateNumber(Jmbg) || Jmbg.length != 13) {
                $("#upozorenje").removeClass(`hidden`);
                $("#upozorenje").addClass(`alert-danger`);
                $("#upozorenje").html(`<td colspan="2"><strong>Greska!</strong> Jmbg mora sadrzati 13 brojeva.</td>`);
                $("#txtJMBG").focus();
            } else if (!isValidEmailAddress(Email)) {
                $("#upozorenje").removeClass(`hidden`);
                $("#upozorenje").addClass(`alert-danger`);
                $("#upozorenje").html(`<td colspan="2"><strong>Greska!</strong> Email mora biti napisan u formatu nesto@yahoo.com.</td>`);
                $("#txtEmail").focus();
            }
        }
    });
};

let IzmijeniSifru2 = function (korisnik) {
    $("#prikazPodataka").html(`<table class="table table-bordered">
                    <thead>
                        <tr class="success">
                            <th colspan="2">
                                Promjena sifre                         
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                         <tr id="upozorenje" class="hidden">
                            <td colspan="2"><strong>Greska!</strong> Nova i ponovljena lozinka se ne poklapaju.</td>                
                        </tr>
                        <tr id="upozorenje2" class="hidden">
                            <td colspan="2"><strong>Greska!</strong> Pogresno unesena stara lozinka.</td>                
                        </tr>
                        <tr>
                            <td>Stara sifra:</td>
                            <td>
                                <input type="password" id="txtStaraSifra" placeholder="Stara sifra..." />
                            </td>
                        </tr>
                        <tr>
                            <td>Nova sifra:</td>
                            <td>
                                <input type="password" id="txtNovaSifra" placeholder="Nova sifra..." />
                            </td>
                        </tr>
                           <tr>
                            <td>Ponovi novu sifru:</td>
                            <td>
                                <input type="password" id="txtPonovljenaSifra" placeholder="Nova sifra..." />
                            </td>
                        </tr>
                        <tr class="success">
                            <td colspan="2">
                                <input id="btnSacuvajIzmjenu" class="btn btn-success pull-right" type="button" value="Sacuvaj">
                        <!--/form-->
                            </td>
                        </tr>
                    </tbody>
                </table>`
    );
    $("#btnSacuvajIzmjenu").click(function () {
        var stara = korisnik.Lozinka;
        var staraUnesena = $("#txtStaraSifra").val();
        var novaUnesena = $("#txtNovaSifra").val();
        var novaPonovoUnesena = $("#txtPonovljenaSifra").val();
        if (stara != staraUnesena) {
            $("#upozorenje").addClass(`hidden`);
            $("#upozorenje2").removeClass(`hidden`);
            $("#upozorenje2").addClass(`alert-danger`);
            $("#txtStaraSifra").val(``);
            $("#txtStaraSifra").focus();
            $("#txtNovaSifra").val(``);
            $("#txtPonovljenaSifra").val(``);
            //alert(`stara sifra nije u redu`);
        } else if (novaUnesena != novaPonovoUnesena) {
            $("#upozorenje2").addClass(`hidden`);
            $("#upozorenje").removeClass(`hidden`);
            $("#upozorenje").addClass(`alert-danger`);
            $("#txtNovaSifra").val(``);
            $("#txtNovaSifra").focus();
            $("#txtPonovljenaSifra").val(``);
            //alert(`nova i ponovljena nisu iste`);
        } else {
            //ispravno, sacuvaj
            $("#upozorenje2").addClass(`hidden`);
            $("#upozorenje").addClass(`hidden`);
            //alert(`moze`);
            $.post("/api/Musterija", { KorisnickoIme: korisnik.KorisnickoIme, Lozinka: novaUnesena, Email: korisnik.Email, Jmbg: korisnik.Jmbg, KontaktTelefon: korisnik.KontaktTelefon, Ime: korisnik.Ime, Prezime: korisnik.Prezime, Pol: korisnik.Pol }, function (data) {
                //alert(remember);
                //alert(data);
            })
                .done(function () {
                    //alert("uspjesno izmijenjeno");
                    location.href = "Musterija.html";
                })
                .fail(function () {
                    //alert("ne moze");
                });
        }
    });
};

let IzmijeniSifru3 = function (korisnik) {
    $("#prikazPodataka").html(`<table class="table table-bordered">
                    <thead>
                        <tr class="success">
                            <th colspan="2">
                                Promjena sifre                         
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                         <tr id="upozorenje" class="hidden">
                            <td colspan="2"><strong>Greska!</strong> Nova i ponovljena lozinka se ne poklapaju.</td>                
                        </tr>
                        <tr id="upozorenje2" class="hidden">
                            <td colspan="2"><strong>Greska!</strong> Pogresno unesena stara lozinka.</td>                
                        </tr>
                        <tr>
                            <td>Stara sifra:</td>
                            <td>
                                <input type="password" id="txtStaraSifra" placeholder="Stara sifra..." />
                            </td>
                        </tr>
                        <tr>
                            <td>Nova sifra:</td>
                            <td>
                                <input type="password" id="txtNovaSifra" placeholder="Nova sifra..." />
                            </td>
                        </tr>
                           <tr>
                            <td>Ponovi novu sifru:</td>
                            <td>
                                <input type="password" id="txtPonovljenaSifra" placeholder="Nova sifra..." />
                            </td>
                        </tr>
                        <tr class="success">
                            <td colspan="2">
                                <input id="btnSacuvajIzmjenu" class="btn btn-success pull-right" type="button" value="Sacuvaj">
                        <!--/form-->
                            </td>
                        </tr>
                    </tbody>
                </table>`
    );
    $("#btnSacuvajIzmjenu").click(function () {
        var stara = korisnik.Lozinka;
        var staraUnesena = $("#txtStaraSifra").val();
        var novaUnesena = $("#txtNovaSifra").val();
        var novaPonovoUnesena = $("#txtPonovljenaSifra").val();
        if (stara != staraUnesena) {
            $("#upozorenje").addClass(`hidden`);
            $("#upozorenje2").removeClass(`hidden`);
            $("#upozorenje2").addClass(`alert-danger`);
            $("#txtStaraSifra").val(``);
            $("#txtStaraSifra").focus();
            $("#txtNovaSifra").val(``);
            $("#txtPonovljenaSifra").val(``);
            //alert(`stara sifra nije u redu`);
        } else if (novaUnesena != novaPonovoUnesena) {
            $("#upozorenje2").addClass(`hidden`);
            $("#upozorenje").removeClass(`hidden`);
            $("#upozorenje").addClass(`alert-danger`);
            $("#txtNovaSifra").val(``);
            $("#txtNovaSifra").focus();
            $("#txtPonovljenaSifra").val(``);
            //alert(`nova i ponovljena nisu iste`);
        } else {
            //ispravno, sacuvaj
            $("#upozorenje2").hide();
            $("#upozorenje").hide();
            //alert(`moze`);
            $.post("/api/Vozac", { KorisnickoIme: korisnik.KorisnickoIme, Lozinka: novaUnesena, Email: korisnik.Email, Jmbg: korisnik.Jmbg, KontaktTelefon: korisnik.KontaktTelefon, Ime: korisnik.Ime, Prezime: korisnik.Prezime, Pol: korisnik.Pol }, function (data) {
                //alert(remember);
                //alert(data);
            })
                .done(function () {
                    //alert("uspjesno izmijenjeno");
                    location.href = "Vozac.html";
                })
                .fail(function () {
                    //alert("ne moze");
                });
        }
    });
};

let jsonObjekat;

function reverseGeocode(coords) {
    fetch('https://nominatim.openstreetmap.org/reverse?format=json&lon=' + coords[0] + '&lat=' + coords[1])
        .then(function (response) {
            //alert(response);
            return response.json();
        }).then(function (json) {
            console.log(json);
            jsonObjekat = json;
        });
};

let pomocna = function () {
    var map = new ol.Map({
        target: 'map',
        layers: [
            new ol.layer.Tile({
                source: new ol.source.OSM()
            })
        ],
        view: new ol.View({
            center: ol.proj.fromLonLat([19.8424, 45.2541]),
            zoom: 15
        })
    });
    //var jsonObjekat;
    map.on('click', function (evt) {
        var coord = ol.proj.toLonLat(evt.coordinate);
        alert(coord);
        reverseGeocode(coord);
        var iconFeatures = [];
        var lon = coord[0];
        var lat = coord[1];
        var icon = "marker.png";
        var iconGeometry = new ol.geom.Point(ol.proj.transform([lon, lat], 'EPSG:4326', 'EPSG:3857'));
        var iconFeature = new ol.Feature({
            geometry: iconGeometry
        });

        iconFeatures.push(iconFeature);

        var vectorSource = new ol.source.Vector({
            features: iconFeatures //add an array of features
        });

        var iconStyle = new ol.style.Style({
            image: new ol.style.Icon(/** @type {olx.style.IconOptions} */({
                anchor: [0.5, 46],
                anchorXUnits: 'fraction',
                anchorYUnits: 'pixels',
                opacity: 0.95,
                src: icon
            }))
        });

        var vectorLayer = new ol.layer.Vector({
            source: vectorSource,
            style: iconStyle
        });

        map.addLayer(vectorLayer);
        //addMarker(coord[0], coord[1], "marker.png");

    });
    $("#btnMijenjaj").click(function () {
        //alert(jsonObjekat);
        $.post("/api/Vozac/IzmijeniLokaciju/", { jsonResult: jsonObjekat }, function () {
            location.href = `Vozac.html`;
        });
    });
    $("#kreiranjeVoz").click(function () {
        //alert(jsonObjekat);
        $.post("/api/Vozac/GetLokacija/", { jsonResult: jsonObjekat }, function (data) {
            //alert(data.KoordinataX);
            $("#txtUlica").val(data.Adresa.Ulica);
            $("#txtBroj").val(data.Adresa.Broj);
            $("#txtGrad").val(data.Adresa.NaseljenoMjesto);
            $("#txtPostanskiBroj").val(data.Adresa.PozivniBrojMjesta);
            $("#txtKoordinataX").val(data.KoordinataX);
            $("#txtKoordinataY").val(data.KoordinataY);
            //location.href = uloga + `.html`;
        });
    });
    $("#kreiranje").click(function () {
        //alert(jsonObjekat);
        $.post("/api/Musterija/GetLokacija/", { jsonResult: jsonObjekat }, function (data) {
            //alert(data.KoordinataX);
            $("#txtUlica").val(data.Adresa.Ulica);
            $("#txtBroj").val(data.Adresa.Broj);
            $("#txtGrad").val(data.Adresa.NaseljenoMjesto);
            $("#txtPostanskiBroj").val(data.Adresa.PozivniBrojMjesta);
            $("#txtKoordinataX").val(data.KoordinataX);
            $("#txtKoordinataY").val(data.KoordinataY);
            //location.href = uloga + `.html`;

        });
    });
    $("#kreiranjeDisp").click(function () {
        //alert(jsonObjekat);
        $.post("/api/Dispecer/GetLokacija/", { jsonResult: jsonObjekat }, function (data) {
            //alert(data.KoordinataX);
            $("#txtUlica").val(data.Adresa.Ulica);
            $("#txtBroj").val(data.Adresa.Broj);
            $("#txtGrad").val(data.Adresa.NaseljenoMjesto);
            $("#txtPostanskiBroj").val(data.Adresa.PozivniBrojMjesta);
            $("#txtKoordinataX").val(data.KoordinataX);
            $("#txtKoordinataY").val(data.KoordinataY);
            //location.href = uloga + `.html`;
            $.get("/api/Dispecer/VratiSlobodneVozace1/", { x: data.KoordinataX, y: data.KoordinataY, tipAuta: $("#tipAuta").val() }, function (data1) {
                //alert(data1[0].KorisnickoIme);
                //alert(`iz geta`);
                //KreiranjeVoznjeDisp(korisnik, data1);
                var vozaci = ``;

                for (v in data1) {
                    vozaci += `<option value="${data1[v].KorisnickoIme}">${data1[v].KorisnickoIme}</option>`;
                }
                $("#cbVozaci").html(vozaci);
                $("div[name=pretraga]").hide();
            });
        });
    });
};

let PrikaziMapu = function () {

    $("#prikazPodataka").html(`<h2>Moja lokacija</h2>
    <button id="btnMijenjaj">Izmijeni lokaciju</button>
    <div id="map" class="map"></div>`);
    pomocna();
    var icon1 = "marker.png";

};5