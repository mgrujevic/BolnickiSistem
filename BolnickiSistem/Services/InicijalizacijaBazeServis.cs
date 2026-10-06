using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using BolnickiSistem.Data;
using BolnickiSistem.Models;

namespace BolnickiSistem.Services;

public class InicijalizacijaBazeServis
{
    private readonly BolnicaContext _kontekst;

    public InicijalizacijaBazeServis(BolnicaContext kontekst)
    {
        _kontekst = kontekst;
    }


    public async Task InicijalizujAsync()
    {
        await _kontekst.Database.MigrateAsync();

        await KreirajPocetneKorisnikeAsync();

        await KreirajPocetnePacijenteAsync();

        await KreirajPocetnePregledeAsync();

        await KreirajPocetneDijagnozeAsync();

        await KreirajPocetneTerapijeAsync();

        await KreirajPocetneLaboratorijskeNalazeAsync();

        await KreirajPocetneDokumenteAsync();
    }


    // =========================================================
    // KORISNICI
    // =========================================================

    private async Task KreirajPocetneKorisnikeAsync()
    {
        await KreirajKorisnikaAkoNePostojiAsync(
            "admin",
            "admin123",
            "Glavni",
            "Administrator",
            "admin@bolnica.rs",
            1,
            true);


        await KreirajKorisnikaAkoNePostojiAsync(
            "admin2",
            "admin123",
            "Pomoćni",
            "Administrator",
            "admin2@bolnica.rs",
            1,
            true);


        await KreirajKorisnikaAkoNePostojiAsync(
            "lekar1",
            "lekar123",
            "Marko",
            "Petrović",
            "marko.petrovic@bolnica.rs",
            2,
            true);


        await KreirajKorisnikaAkoNePostojiAsync(
            "lekar2",
            "lekar123",
            "Jelena",
            "Jovanović",
            "jelena.jovanovic@bolnica.rs",
            2,
            true);


        await KreirajKorisnikaAkoNePostojiAsync(
            "lekar3",
            "lekar123",
            "Nikola",
            "Nikolić",
            "nikola.nikolic@bolnica.rs",
            2,
            false);


        // Ako lekar3 već postoji, osiguravamo da bude deaktiviran.
        Korisnik? lekar3 =
            await _kontekst.Korisnici
                .FirstOrDefaultAsync(k =>
                    k.KorisnickoIme == "lekar3");

        if (lekar3 != null && lekar3.Aktivan)
        {
            lekar3.Aktivan = false;

            await _kontekst.SaveChangesAsync();
        }
    }


    private async Task KreirajKorisnikaAkoNePostojiAsync(
        string korisnickoIme,
        string lozinka,
        string ime,
        string prezime,
        string email,
        int ulogaId,
        bool aktivan)
    {
        bool postoji =
            await _kontekst.Korisnici
                .AnyAsync(k =>
                    k.KorisnickoIme == korisnickoIme);

        if (postoji)
            return;


        Korisnik korisnik = new Korisnik
        {
            KorisnickoIme = korisnickoIme,
            LozinkaHash = KreirajHashLozinke(lozinka),
            Ime = ime,
            Prezime = prezime,
            Email = email,
            Aktivan = aktivan,
            DatumKreiranja = DateTime.Now,
            UlogaId = ulogaId
        };


        _kontekst.Korisnici.Add(korisnik);

        await _kontekst.SaveChangesAsync();
    }


    // =========================================================
    // PACIJENTI
    // =========================================================

    private async Task KreirajPocetnePacijenteAsync()
    {
        Korisnik? lekar1 =
            await _kontekst.Korisnici
                .FirstOrDefaultAsync(k =>
                    k.KorisnickoIme == "lekar1");

        Korisnik? lekar2 =
            await _kontekst.Korisnici
                .FirstOrDefaultAsync(k =>
                    k.KorisnickoIme == "lekar2");

        if (lekar1 == null || lekar2 == null)
            return;


        var pacijenti = new List<Pacijent>
        {
            new Pacijent
            {
                BrojZdravstveneKartice = "BK-000001",
                JMBG = "0101990710001",
                Ime = "Petar",
                Prezime = "Jovanović",
                DatumRodjenja = new DateTime(1990, 1, 1),
                Pol = "Muški",
                Adresa = "Beograd",
                Telefon = "0601111111",
                Email = "petar.jovanovic@email.com",
                KrvnaGrupa = "A+",
                Alergije = "Nema",
                HronicneBolesti = "Nema",
                LekarId = lekar1.KorisnikId
            },

            new Pacijent
            {
                BrojZdravstveneKartice = "BK-000002",
                JMBG = "1503985710002",
                Ime = "Milica",
                Prezime = "Marković",
                DatumRodjenja = new DateTime(1998, 3, 15),
                Pol = "Ženski",
                Adresa = "Novi Beograd",
                Telefon = "0602222222",
                Email = "milica.markovic@email.com",
                KrvnaGrupa = "0+",
                Alergije = "Penicilin",
                HronicneBolesti = "Nema",
                LekarId = lekar1.KorisnikId
            },

            new Pacijent
            {
                BrojZdravstveneKartice = "BK-000003",
                JMBG = "2207960710003",
                Ime = "Stefan",
                Prezime = "Nikolić",
                DatumRodjenja = new DateTime(1996, 7, 22),
                Pol = "Muški",
                Adresa = "Zemun",
                Telefon = "0603333333",
                Email = "stefan.nikolic@email.com",
                KrvnaGrupa = "B+",
                Alergije = "Nema",
                HronicneBolesti = "Astma",
                LekarId = lekar1.KorisnikId
            },

            new Pacijent
            {
                BrojZdravstveneKartice = "BK-000004",
                JMBG = "0502855710004",
                Ime = "Ana",
                Prezime = "Petrović",
                DatumRodjenja = new DateTime(1985, 2, 5),
                Pol = "Ženski",
                Adresa = "Voždovac",
                Telefon = "0604444444",
                Email = "ana.petrovic@email.com",
                KrvnaGrupa = "A-",
                Alergije = "Nema",
                HronicneBolesti = "Hipertenzija",
                LekarId = lekar1.KorisnikId
            },

            new Pacijent
            {
                BrojZdravstveneKartice = "BK-000005",
                JMBG = "1206920710005",
                Ime = "Nikola",
                Prezime = "Stojanović",
                DatumRodjenja = new DateTime(1992, 6, 12),
                Pol = "Muški",
                Adresa = "Čukarica",
                Telefon = "0605555555",
                Email = "nikola.stojanovic@email.com",
                KrvnaGrupa = "AB+",
                Alergije = "Nema",
                HronicneBolesti = "Nema",
                LekarId = lekar1.KorisnikId
            },

            new Pacijent
            {
                BrojZdravstveneKartice = "BK-000006",
                JMBG = "2811940710006",
                Ime = "Jovana",
                Prezime = "Ilić",
                DatumRodjenja = new DateTime(1994, 11, 28),
                Pol = "Ženski",
                Adresa = "Palilula",
                Telefon = "0606666666",
                Email = "jovana.ilic@email.com",
                KrvnaGrupa = "0-",
                Alergije = "Polen",
                HronicneBolesti = "Nema",
                LekarId = lekar1.KorisnikId
            },

            new Pacijent
            {
                BrojZdravstveneKartice = "BK-000007",
                JMBG = "0301810710007",
                Ime = "Marko",
                Prezime = "Đorđević",
                DatumRodjenja = new DateTime(1981, 1, 3),
                Pol = "Muški",
                Adresa = "Zvezdara",
                Telefon = "0607777777",
                Email = "marko.djordjevic@email.com",
                KrvnaGrupa = "B-",
                Alergije = "Nema",
                HronicneBolesti = "Dijabetes",
                LekarId = lekar1.KorisnikId
            },

            new Pacijent
            {
                BrojZdravstveneKartice = "BK-000008",
                JMBG = "1804870710008",
                Ime = "Tamara",
                Prezime = "Simić",
                DatumRodjenja = new DateTime(1987, 4, 18),
                Pol = "Ženski",
                Adresa = "Rakovica",
                Telefon = "0608888888",
                Email = "tamara.simic@email.com",
                KrvnaGrupa = "A+",
                Alergije = "Nema",
                HronicneBolesti = "Nema",
                LekarId = lekar1.KorisnikId
            },

            new Pacijent
            {
                BrojZdravstveneKartice = "BK-000009",
                JMBG = "2505990710009",
                Ime = "Luka",
                Prezime = "Milošević",
                DatumRodjenja = new DateTime(1999, 5, 25),
                Pol = "Muški",
                Adresa = "Surčin",
                Telefon = "0609999999",
                Email = "luka.milosevic@email.com",
                KrvnaGrupa = "A+",
                Alergije = "Nema",
                HronicneBolesti = "Nema",
                LekarId = lekar1.KorisnikId
            },

            new Pacijent
            {
                BrojZdravstveneKartice = "BK-000010",
                JMBG = "1002900710010",
                Ime = "Marija",
                Prezime = "Ristić",
                DatumRodjenja = new DateTime(1990, 2, 10),
                Pol = "Ženski",
                Adresa = "Vračar",
                Telefon = "0611010101",
                Email = "marija.ristic@email.com",
                KrvnaGrupa = "AB-",
                Alergije = "Nema",
                HronicneBolesti = "Hipotireoza",
                LekarId = lekar1.KorisnikId
            },

            new Pacijent
            {
                BrojZdravstveneKartice = "BK-000011",
                JMBG = "1407830710011",
                Ime = "Miloš",
                Prezime = "Kovačević",
                DatumRodjenja = new DateTime(1983, 7, 14),
                Pol = "Muški",
                Adresa = "Novi Beograd",
                Telefon = "0611111111",
                Email = "milos.kovacevic@email.com",
                KrvnaGrupa = "0+",
                Alergije = "Nema",
                HronicneBolesti = "Nema",
                LekarId = lekar2.KorisnikId
            },

            new Pacijent
            {
                BrojZdravstveneKartice = "BK-000012",
                JMBG = "0901950710012",
                Ime = "Sara",
                Prezime = "Vasić",
                DatumRodjenja = new DateTime(1995, 1, 9),
                Pol = "Ženski",
                Adresa = "Zemun",
                Telefon = "0612222222",
                Email = "sara.vasic@email.com",
                KrvnaGrupa = "B+",
                Alergije = "Nema",
                HronicneBolesti = "Nema",
                LekarId = lekar2.KorisnikId
            },

            new Pacijent
            {
                BrojZdravstveneKartice = "BK-000013",
                JMBG = "2005780710013",
                Ime = "Vladimir",
                Prezime = "Pavlović",
                DatumRodjenja = new DateTime(1978, 5, 20),
                Pol = "Muški",
                Adresa = "Voždovac",
                Telefon = "0613333333",
                Email = "vladimir.pavlovic@email.com",
                KrvnaGrupa = "A+",
                Alergije = "Nema",
                HronicneBolesti = "Hipertenzija",
                LekarId = lekar2.KorisnikId
            },

            new Pacijent
            {
                BrojZdravstveneKartice = "BK-000014",
                JMBG = "1706860710014",
                Ime = "Ivana",
                Prezime = "Lukić",
                DatumRodjenja = new DateTime(1986, 6, 17),
                Pol = "Ženski",
                Adresa = "Palilula",
                Telefon = "0614444444",
                Email = "ivana.lukic@email.com",
                KrvnaGrupa = "0+",
                Alergije = "Nema",
                HronicneBolesti = "Nema",
                LekarId = lekar2.KorisnikId
            },

            new Pacijent
            {
                BrojZdravstveneKartice = "BK-000015",
                JMBG = "0207940710015",
                Ime = "Nemanja",
                Prezime = "Savić",
                DatumRodjenja = new DateTime(1994, 7, 2),
                Pol = "Muški",
                Adresa = "Čukarica",
                Telefon = "0615555555",
                Email = "nemanja.savic@email.com",
                KrvnaGrupa = "B+",
                Alergije = "Nema",
                HronicneBolesti = "Nema",
                LekarId = lekar2.KorisnikId
            },

            new Pacijent
            {
                BrojZdravstveneKartice = "BK-000016",
                JMBG = "1108890710016",
                Ime = "Teodora",
                Prezime = "Janković",
                DatumRodjenja = new DateTime(1989, 8, 11),
                Pol = "Ženski",
                Adresa = "Rakovica",
                Telefon = "0616666666",
                Email = "teodora.jankovic@email.com",
                KrvnaGrupa = "A-",
                Alergije = "Nema",
                HronicneBolesti = "Nema",
                LekarId = lekar2.KorisnikId
            },

            new Pacijent
            {
                BrojZdravstveneKartice = "BK-000017",
                JMBG = "2301820710017",
                Ime = "Dušan",
                Prezime = "Matić",
                DatumRodjenja = new DateTime(1982, 1, 23),
                Pol = "Muški",
                Adresa = "Zvezdara",
                Telefon = "0617777777",
                Email = "dusan.matic@email.com",
                KrvnaGrupa = "0+",
                Alergije = "Nema",
                HronicneBolesti = "Astma",
                LekarId = lekar2.KorisnikId
            },

            new Pacijent
            {
                BrojZdravstveneKartice = "BK-000018",
                JMBG = "0603900710018",
                Ime = "Kristina",
                Prezime = "Milić",
                DatumRodjenja = new DateTime(1990, 3, 6),
                Pol = "Ženski",
                Adresa = "Vračar",
                Telefon = "0618888888",
                Email = "kristina.milic@email.com",
                KrvnaGrupa = "AB+",
                Alergije = "Nema",
                HronicneBolesti = "Nema",
                LekarId = lekar2.KorisnikId
            },

            new Pacijent
            {
                BrojZdravstveneKartice = "BK-000019",
                JMBG = "2907850710019",
                Ime = "Aleksandar",
                Prezime = "Vuković",
                DatumRodjenja = new DateTime(1985, 7, 29),
                Pol = "Muški",
                Adresa = "Novi Beograd",
                Telefon = "0619999999",
                Email = "aleksandar.vukovic@email.com",
                KrvnaGrupa = "B-",
                Alergije = "Nema",
                HronicneBolesti = "Dijabetes",
                LekarId = lekar2.KorisnikId
            },

            new Pacijent
            {
                BrojZdravstveneKartice = "BK-000020",
                JMBG = "1301970710020",
                Ime = "Nađa",
                Prezime = "Nikolić",
                DatumRodjenja = new DateTime(1997, 1, 13),
                Pol = "Ženski",
                Adresa = "Zemun",
                Telefon = "0621010101",
                Email = "nadja.nikolic@email.com",
                KrvnaGrupa = "0+",
                Alergije = "Nema",
                HronicneBolesti = "Nema",
                LekarId = lekar2.KorisnikId
            }
        };


        foreach (Pacijent pacijent in pacijenti)
        {
            Pacijent? postojeciPacijent =
                await _kontekst.Pacijenti
                    .FirstOrDefaultAsync(p => p.JMBG == pacijent.JMBG);

            if (postojeciPacijent == null)
            {
                _kontekst.Pacijenti.Add(pacijent);
            }
            else if (postojeciPacijent.LekarId == null)
            {
                postojeciPacijent.LekarId = pacijent.LekarId;
            }
        }

        await _kontekst.SaveChangesAsync();
    }


    // =========================================================
    // POČETNI PREGLEDI - POVEZIVANJE PACIJENATA I LEKARA
    // =========================================================

    private async Task KreirajPocetnePregledeAsync()
    {
        Korisnik? lekar1 =
            await _kontekst.Korisnici
                .FirstOrDefaultAsync(k =>
                    k.KorisnickoIme == "lekar1");

        Korisnik? lekar2 =
            await _kontekst.Korisnici
                .FirstOrDefaultAsync(k =>
                    k.KorisnickoIme == "lekar2");


        if (lekar1 == null || lekar2 == null)
            return;


        List<Pacijent> pacijenti =
            await _kontekst.Pacijenti
                .OrderBy(p => p.PacijentId)
                .Take(20)
                .ToListAsync();


        for (int i = 0; i < pacijenti.Count; i++)
        {
            Pacijent pacijent = pacijenti[i];

            int lekarId =
                i < 10
                    ? lekar1.KorisnikId
                    : lekar2.KorisnikId;


            bool postojiPregled =
                await _kontekst.Pregledi
                    .AnyAsync(p =>
                        p.PacijentId == pacijent.PacijentId &&
                        p.LekarId == lekarId);


            if (postojiPregled)
                continue;


            Pregled pregled = new Pregled
            {
                PacijentId = pacijent.PacijentId,
                LekarId = lekarId,
                DatumPregleda = DateTime.Now.AddDays(-(i + 1)),
                RazlogDolaska = "Kontrolni pregled",
                Simptomi = "Pacijent došao na redovnu kontrolu.",
                Nalaz = "Opšte stanje pacijenta stabilno.",
                Preporuka = "Nastaviti sa redovnim kontrolama.",
                DatumKontrole = DateTime.Now.AddMonths(6)
            };


            _kontekst.Pregledi.Add(pregled);
        }


        await _kontekst.SaveChangesAsync();
    }


    // =========================================================
    // POČETNE DIJAGNOZE
    // =========================================================

    private async Task KreirajPocetneDijagnozeAsync()
    {
        List<Pregled> pregledi =
            await _kontekst.Pregledi
                .OrderBy(p => p.PregledId)
                .Take(20)
                .ToListAsync();

        if (pregledi.Count == 0)
            return;

        var dijagnoze = new List<Dijagnoza>();

        if (pregledi.Count >= 1)
        {
            dijagnoze.Add(new Dijagnoza
            {
                PregledId = pregledi[0].PregledId,
                Sifra = "J06.9",
                Naziv = "Akutna infekcija gornjih respiratornih puteva",
                Opis = "Klinička slika odgovara akutnoj infekciji gornjih disajnih puteva."
            });

            dijagnoze.Add(new Dijagnoza
            {
                PregledId = pregledi[0].PregledId,
                Sifra = "R50.9",
                Naziv = "Povišena telesna temperatura",
                Opis = "Febrilnost prisutna tokom prethodnih nekoliko dana."
            });
        }

        if (pregledi.Count >= 2)
        {
            dijagnoze.Add(new Dijagnoza
            {
                PregledId = pregledi[1].PregledId,
                Sifra = "I10",
                Naziv = "Esencijalna hipertenzija",
                Opis = "Povišene vrednosti krvnog pritiska uz potrebu za redovnim praćenjem."
            });
        }

        if (pregledi.Count >= 3)
        {
            dijagnoze.Add(new Dijagnoza
            {
                PregledId = pregledi[2].PregledId,
                Sifra = "J45.9",
                Naziv = "Astma",
                Opis = "Bronhijalna astma bez akutnog pogoršanja."
            });

            dijagnoze.Add(new Dijagnoza
            {
                PregledId = pregledi[2].PregledId,
                Sifra = "R06.2",
                Naziv = "Otežano disanje",
                Opis = "Povremeno otežano disanje pri fizičkom naporu."
            });
        }

        if (pregledi.Count >= 4)
        {
            dijagnoze.Add(new Dijagnoza
            {
                PregledId = pregledi[3].PregledId,
                Sifra = "E11.9",
                Naziv = "Dijabetes melitus tip 2",
                Opis = "Dijabetes tipa 2 bez evidentiranih komplikacija."
            });
        }

        if (pregledi.Count >= 5)
        {
            dijagnoze.Add(new Dijagnoza
            {
                PregledId = pregledi[4].PregledId,
                Sifra = "E03.9",
                Naziv = "Hipotireoza",
                Opis = "Smanjena funkcija štitaste žlezde uz redovno praćenje terapije."
            });
        }

        if (pregledi.Count >= 6)
        {
            dijagnoze.Add(new Dijagnoza
            {
                PregledId = pregledi[5].PregledId,
                Sifra = "M54.5",
                Naziv = "Bol u donjem delu leđa",
                Opis = "Lumbalni bol bez znakova akutnog neurološkog deficita."
            });
        }

        if (pregledi.Count >= 7)
        {
            dijagnoze.Add(new Dijagnoza
            {
                PregledId = pregledi[6].PregledId,
                Sifra = "K21.9",
                Naziv = "Gastroezofagealna refluksna bolest",
                Opis = "Tegobe odgovaraju gastroezofagealnom refluksu."
            });
        }

        if (pregledi.Count >= 8)
        {
            dijagnoze.Add(new Dijagnoza
            {
                PregledId = pregledi[7].PregledId,
                Sifra = "J30.1",
                Naziv = "Alergijski rinitis",
                Opis = "Sezonske alergijske tegobe sa zapušenim nosom i kijanjem."
            });
        }

        if (pregledi.Count >= 9)
        {
            dijagnoze.Add(new Dijagnoza
            {
                PregledId = pregledi[8].PregledId,
                Sifra = "G43.9",
                Naziv = "Migrena",
                Opis = "Ponavljajuće epizode glavobolje karakteristične za migrenu."
            });
        }

        if (pregledi.Count >= 10)
        {
            dijagnoze.Add(new Dijagnoza
            {
                PregledId = pregledi[9].PregledId,
                Sifra = "D50.9",
                Naziv = "Anemija usled nedostatka gvožđa",
                Opis = "Laboratorijski nalazi ukazuju na sideropenijsku anemiju."
            });
        }

        if (pregledi.Count >= 11)
        {
            dijagnoze.Add(new Dijagnoza
            {
                PregledId = pregledi[10].PregledId,
                Sifra = "M17.9",
                Naziv = "Gonartroza",
                Opis = "Degenerativne promene kolena uz povremeni bol pri hodu."
            });
        }

        if (pregledi.Count >= 12)
        {
            dijagnoze.Add(new Dijagnoza
            {
                PregledId = pregledi[11].PregledId,
                Sifra = "L20.9",
                Naziv = "Atopijski dermatitis",
                Opis = "Hronične promene na koži sa povremenim pogoršanjima."
            });
        }

        if (pregledi.Count >= 13)
        {
            dijagnoze.Add(new Dijagnoza
            {
                PregledId = pregledi[12].PregledId,
                Sifra = "E78.5",
                Naziv = "Poremećaj metabolizma lipida",
                Opis = "Povišene vrednosti ukupnog holesterola i LDL holesterola."
            });
        }

        if (pregledi.Count >= 14)
        {
            dijagnoze.Add(new Dijagnoza
            {
                PregledId = pregledi[13].PregledId,
                Sifra = "N39.0",
                Naziv = "Infekcija urinarnog trakta",
                Opis = "Tegobe i laboratorijski nalazi odgovaraju infekciji urinarnog trakta."
            });
        }

        if (pregledi.Count >= 15)
        {
            dijagnoze.Add(new Dijagnoza
            {
                PregledId = pregledi[14].PregledId,
                Sifra = "R51.9",
                Naziv = "Glavobolja",
                Opis = "Povremena tenziona glavobolja bez drugih neuroloških simptoma."
            });
        }

        foreach (Dijagnoza dijagnoza in dijagnoze)
        {
            bool postoji =
                await _kontekst.Dijagnoze.AnyAsync(d =>
                    d.PregledId == dijagnoza.PregledId &&
                    d.Sifra == dijagnoza.Sifra);

            if (!postoji)
            {
                _kontekst.Dijagnoze.Add(dijagnoza);
            }
        }

        await _kontekst.SaveChangesAsync();
    }


    // =========================================================
    // POČETNE TERAPIJE
    // =========================================================

    private async Task KreirajPocetneTerapijeAsync()
    {
        List<Pregled> pregledi =
            await _kontekst.Pregledi
                .OrderBy(p => p.PregledId)
                .Take(20)
                .ToListAsync();

        if (pregledi.Count == 0)
            return;

        var terapije = new List<Terapija>();

        DateTime danas = DateTime.Today;

        if (pregledi.Count >= 1)
        {
            terapije.Add(new Terapija
            {
                PregledId = pregledi[0].PregledId,
                NazivLeka = "Paracetamol",
                Doziranje = "500 mg",
                Ucestalost = "3 puta dnevno po potrebi",
                DatumPocetka = danas.AddDays(-3),
                DatumZavrsetka = danas.AddDays(2),
                Uputstvo = "Uzimati nakon obroka. Ne prekoračivati preporučenu dnevnu dozu."
            });

            terapije.Add(new Terapija
            {
                PregledId = pregledi[0].PregledId,
                NazivLeka = "Vitamin C",
                Doziranje = "500 mg",
                Ucestalost = "1 put dnevno",
                DatumPocetka = danas.AddDays(-3),
                DatumZavrsetka = danas.AddDays(7),
                Uputstvo = "Uzimati uz obrok."
            });
        }

        if (pregledi.Count >= 2)
        {
            terapije.Add(new Terapija
            {
                PregledId = pregledi[1].PregledId,
                NazivLeka = "Ramipril",
                Doziranje = "5 mg",
                Ucestalost = "1 put dnevno",
                DatumPocetka = danas.AddDays(-30),
                DatumZavrsetka = null,
                Uputstvo = "Uzimati ujutru, u približno isto vreme svakog dana."
            });
        }

        if (pregledi.Count >= 3)
        {
            terapije.Add(new Terapija
            {
                PregledId = pregledi[2].PregledId,
                NazivLeka = "Budesonid",
                Doziranje = "200 mcg",
                Ucestalost = "2 puta dnevno",
                DatumPocetka = danas.AddDays(-10),
                DatumZavrsetka = danas.AddDays(20),
                Uputstvo = "Primena putem inhalatora prema uputstvu lekara."
            });

            terapije.Add(new Terapija
            {
                PregledId = pregledi[2].PregledId,
                NazivLeka = "Salbutamol",
                Doziranje = "100 mcg",
                Ucestalost = "Po potrebi",
                DatumPocetka = danas.AddDays(-10),
                DatumZavrsetka = danas.AddDays(20),
                Uputstvo = "Koristiti kod pojave otežanog disanja prema uputstvu lekara."
            });
        }

        if (pregledi.Count >= 4)
        {
            terapije.Add(new Terapija
            {
                PregledId = pregledi[3].PregledId,
                NazivLeka = "Metformin",
                Doziranje = "500 mg",
                Ucestalost = "2 puta dnevno",
                DatumPocetka = danas.AddDays(-60),
                DatumZavrsetka = null,
                Uputstvo = "Uzimati uz doručak i večeru."
            });
        }

        if (pregledi.Count >= 5)
        {
            terapije.Add(new Terapija
            {
                PregledId = pregledi[4].PregledId,
                NazivLeka = "Levotiroksin",
                Doziranje = "50 mcg",
                Ucestalost = "1 put dnevno",
                DatumPocetka = danas.AddDays(-90),
                DatumZavrsetka = null,
                Uputstvo = "Uzimati ujutru natašte, najmanje 30 minuta pre doručka."
            });
        }

        if (pregledi.Count >= 6)
        {
            terapije.Add(new Terapija
            {
                PregledId = pregledi[5].PregledId,
                NazivLeka = "Ibuprofen",
                Doziranje = "400 mg",
                Ucestalost = "2 puta dnevno po potrebi",
                DatumPocetka = danas.AddDays(-5),
                DatumZavrsetka = danas.AddDays(5),
                Uputstvo = "Uzimati nakon obroka i uz dovoljno tečnosti."
            });
        }

        if (pregledi.Count >= 7)
        {
            terapije.Add(new Terapija
            {
                PregledId = pregledi[6].PregledId,
                NazivLeka = "Pantoprazol",
                Doziranje = "40 mg",
                Ucestalost = "1 put dnevno",
                DatumPocetka = danas.AddDays(-14),
                DatumZavrsetka = danas.AddDays(28),
                Uputstvo = "Uzimati ujutru pre obroka."
            });
        }

        if (pregledi.Count >= 8)
        {
            terapije.Add(new Terapija
            {
                PregledId = pregledi[7].PregledId,
                NazivLeka = "Loratadin",
                Doziranje = "10 mg",
                Ucestalost = "1 put dnevno",
                DatumPocetka = danas.AddDays(-7),
                DatumZavrsetka = danas.AddDays(23),
                Uputstvo = "Uzimati jednom dnevno, po mogućnosti u isto vreme."
            });
        }

        if (pregledi.Count >= 9)
        {
            terapije.Add(new Terapija
            {
                PregledId = pregledi[8].PregledId,
                NazivLeka = "Sumatriptan",
                Doziranje = "50 mg",
                Ucestalost = "Po potrebi kod pojave migrene",
                DatumPocetka = danas.AddDays(-15),
                DatumZavrsetka = danas.AddDays(45),
                Uputstvo = "Primena prema uputstvu lekara na početku napada."
            });
        }

        if (pregledi.Count >= 10)
        {
            terapije.Add(new Terapija
            {
                PregledId = pregledi[9].PregledId,
                NazivLeka = "Gvožđe",
                Doziranje = "100 mg",
                Ucestalost = "1 put dnevno",
                DatumPocetka = danas.AddDays(-20),
                DatumZavrsetka = danas.AddDays(70),
                Uputstvo = "Uzimati prema preporuci lekara. Kontrola krvne slike prema planu."
            });
        }

        if (pregledi.Count >= 11)
        {
            terapije.Add(new Terapija
            {
                PregledId = pregledi[10].PregledId,
                NazivLeka = "Diklofenak gel",
                Doziranje = "Naneti tanko na bolno mesto",
                Ucestalost = "2-3 puta dnevno",
                DatumPocetka = danas.AddDays(-7),
                DatumZavrsetka = danas.AddDays(7),
                Uputstvo = "Ne nanositi na oštećenu kožu."
            });
        }

        if (pregledi.Count >= 12)
        {
            terapije.Add(new Terapija
            {
                PregledId = pregledi[11].PregledId,
                NazivLeka = "Hidrokortizon krema",
                Doziranje = "Tanki sloj",
                Ucestalost = "2 puta dnevno",
                DatumPocetka = danas.AddDays(-5),
                DatumZavrsetka = danas.AddDays(5),
                Uputstvo = "Naneti na zahvaćenu regiju prema uputstvu lekara."
            });
        }

        foreach (Terapija terapija in terapije)
        {
            bool postoji =
                await _kontekst.Terapije.AnyAsync(t =>
                    t.PregledId == terapija.PregledId &&
                    t.NazivLeka == terapija.NazivLeka);

            if (!postoji)
            {
                _kontekst.Terapije.Add(terapija);
            }
        }

        await _kontekst.SaveChangesAsync();
    }

    // =========================================================
    // POČETNI LABORATORIJSKI NALAZI
    // =========================================================

    private async Task KreirajPocetneLaboratorijskeNalazeAsync()
    {
        List<Pacijent> pacijenti =
            await _kontekst.Pacijenti
                .OrderBy(p => p.PacijentId)
                .Take(10)
                .ToListAsync();

        List<Pregled> pregledi =
            await _kontekst.Pregledi
                .OrderBy(p => p.PregledId)
                .Take(10)
                .ToListAsync();

        if (pacijenti.Count == 0)
            return;

        var nalazi = new List<LaboratorijskiNalaz>();

        if (pacijenti.Count >= 1)
        {
            nalazi.Add(new LaboratorijskiNalaz
            {
                PacijentId = pacijenti[0].PacijentId,
                PregledId = pregledi.Count >= 1 ? pregledi[0].PregledId : null,
                NazivAnalize = "Krvna slika - Hemoglobin",
                Rezultat = "145",
                Jedinica = "g/L",
                ReferentnaVrednost = "120-160",
                DatumAnalize = DateTime.Today.AddDays(-4),
                Napomena = "Nalaz uredan."
            });

            nalazi.Add(new LaboratorijskiNalaz
            {
                PacijentId = pacijenti[0].PacijentId,
                PregledId = pregledi.Count >= 1 ? pregledi[0].PregledId : null,
                NazivAnalize = "Leukociti",
                Rezultat = "7.2",
                Jedinica = "x10^9/L",
                ReferentnaVrednost = "4.0-10.0",
                DatumAnalize = DateTime.Today.AddDays(-4),
                Napomena = "Nalaz uredan."
            });

            nalazi.Add(new LaboratorijskiNalaz
            {
                PacijentId = pacijenti[0].PacijentId,
                PregledId = null,
                NazivAnalize = "Glukoza",
                Rezultat = "5.4",
                Jedinica = "mmol/L",
                ReferentnaVrednost = "3.9-6.1",
                DatumAnalize = DateTime.Today.AddDays(-3),
                Napomena = null
            });
        }

        if (pacijenti.Count >= 2)
        {
            nalazi.Add(new LaboratorijskiNalaz
            {
                PacijentId = pacijenti[1].PacijentId,
                PregledId = pregledi.Count >= 2 ? pregledi[1].PregledId : null,
                NazivAnalize = "Glukoza natašte",
                Rezultat = "6.8",
                Jedinica = "mmol/L",
                ReferentnaVrednost = "3.9-6.1",
                DatumAnalize = DateTime.Today.AddDays(-8),
                Napomena = "Vrednost iznad referentnog opsega."
            });

            nalazi.Add(new LaboratorijskiNalaz
            {
                PacijentId = pacijenti[1].PacijentId,
                PregledId = pregledi.Count >= 2 ? pregledi[1].PregledId : null,
                NazivAnalize = "Ukupni holesterol",
                Rezultat = "5.9",
                Jedinica = "mmol/L",
                ReferentnaVrednost = "<5.2",
                DatumAnalize = DateTime.Today.AddDays(-8),
                Napomena = "Preporučena kontrola lipidnog statusa."
            });
        }

        if (pacijenti.Count >= 3)
        {
            nalazi.Add(new LaboratorijskiNalaz
            {
                PacijentId = pacijenti[2].PacijentId,
                PregledId = pregledi.Count >= 3 ? pregledi[2].PregledId : null,
                NazivAnalize = "CRP",
                Rezultat = "4.2",
                Jedinica = "mg/L",
                ReferentnaVrednost = "<5",
                DatumAnalize = DateTime.Today.AddDays(-6),
                Napomena = "Bez značajnog porasta inflamatornih parametara."
            });

            nalazi.Add(new LaboratorijskiNalaz
            {
                PacijentId = pacijenti[2].PacijentId,
                PregledId = pregledi.Count >= 3 ? pregledi[2].PregledId : null,
                NazivAnalize = "Eozinofili",
                Rezultat = "0.32",
                Jedinica = "x10^9/L",
                ReferentnaVrednost = "0.05-0.50",
                DatumAnalize = DateTime.Today.AddDays(-6),
                Napomena = "Nalaz u referentnom opsegu."
            });
        }

        if (pacijenti.Count >= 4)
        {
            nalazi.Add(new LaboratorijskiNalaz
            {
                PacijentId = pacijenti[3].PacijentId,
                PregledId = pregledi.Count >= 4 ? pregledi[3].PregledId : null,
                NazivAnalize = "HbA1c",
                Rezultat = "7.1",
                Jedinica = "%",
                ReferentnaVrednost = "<6.5",
                DatumAnalize = DateTime.Today.AddDays(-10),
                Napomena = "Potrebna kontrola glikoregulacije."
            });
        }

        if (pacijenti.Count >= 5)
        {
            nalazi.Add(new LaboratorijskiNalaz
            {
                PacijentId = pacijenti[4].PacijentId,
                PregledId = pregledi.Count >= 5 ? pregledi[4].PregledId : null,
                NazivAnalize = "TSH",
                Rezultat = "6.8",
                Jedinica = "mIU/L",
                ReferentnaVrednost = "0.4-4.0",
                DatumAnalize = DateTime.Today.AddDays(-12),
                Napomena = "Povišena vrednost TSH."
            });

            nalazi.Add(new LaboratorijskiNalaz
            {
                PacijentId = pacijenti[4].PacijentId,
                PregledId = pregledi.Count >= 5 ? pregledi[4].PregledId : null,
                NazivAnalize = "FT4",
                Rezultat = "11.2",
                Jedinica = "pmol/L",
                ReferentnaVrednost = "10-22",
                DatumAnalize = DateTime.Today.AddDays(-12),
                Napomena = "Nalaz u donjoj polovini referentnog opsega."
            });
        }

        if (pacijenti.Count >= 6)
        {
            nalazi.Add(new LaboratorijskiNalaz
            {
                PacijentId = pacijenti[5].PacijentId,
                PregledId = pregledi.Count >= 6 ? pregledi[5].PregledId : null,
                NazivAnalize = "Vitamin D",
                Rezultat = "24",
                Jedinica = "ng/mL",
                ReferentnaVrednost = "30-100",
                DatumAnalize = DateTime.Today.AddDays(-15),
                Napomena = "Blago snižena vrednost."
            });
        }

        foreach (LaboratorijskiNalaz nalaz in nalazi)
        {
            bool postoji =
                await _kontekst.LaboratorijskiNalazi.AnyAsync(l =>
                    l.PacijentId == nalaz.PacijentId &&
                    l.NazivAnalize == nalaz.NazivAnalize &&
                    l.DatumAnalize == nalaz.DatumAnalize);

            if (!postoji)
            {
                _kontekst.LaboratorijskiNalazi.Add(nalaz);
            }
        }

        await _kontekst.SaveChangesAsync();
    }


    // =========================================================
    // POČETNI DOKUMENTI
    // =========================================================

    private async Task KreirajPocetneDokumenteAsync()
    {
        List<Pacijent> pacijenti =
            await _kontekst.Pacijenti
                .OrderBy(p => p.PacijentId)
                .Take(10)
                .ToListAsync();

        List<Pregled> pregledi =
            await _kontekst.Pregledi
                .OrderBy(p => p.PregledId)
                .Take(10)
                .ToListAsync();

        Korisnik? lekar1 =
            await _kontekst.Korisnici
                .FirstOrDefaultAsync(k =>
                    k.KorisnickoIme == "lekar1");

        Korisnik? lekar2 =
            await _kontekst.Korisnici
                .FirstOrDefaultAsync(k =>
                    k.KorisnickoIme == "lekar2");

        if (pacijenti.Count == 0 || lekar1 == null || lekar2 == null)
            return;

        var dokumenti = new List<Dokument>();

        if (pacijenti.Count >= 1)
        {
            dokumenti.Add(new Dokument
            {
                PacijentId = pacijenti[0].PacijentId,
                PregledId = pregledi.Count >= 1 ? pregledi[0].PregledId : null,
                Naziv = "Laboratorijski nalazi - krvna slika",
                TipDokumenta = "Laboratorijski nalaz",
                PutanjaDoFajla = @"Dokumenti\BK-000001\krvna-slika.pdf",
                DatumDodavanja = DateTime.Today.AddDays(-4),
                DodaoKorisnikId = lekar1.KorisnikId
            });

            dokumenti.Add(new Dokument
            {
                PacijentId = pacijenti[0].PacijentId,
                PregledId = pregledi.Count >= 1 ? pregledi[0].PregledId : null,
                Naziv = "Izveštaj sa pregleda",
                TipDokumenta = "Lekarski izveštaj",
                PutanjaDoFajla = @"Dokumenti\BK-000001\izvestaj-pregled.pdf",
                DatumDodavanja = DateTime.Today.AddDays(-3),
                DodaoKorisnikId = lekar1.KorisnikId
            });
        }

        if (pacijenti.Count >= 2)
        {
            dokumenti.Add(new Dokument
            {
                PacijentId = pacijenti[1].PacijentId,
                PregledId = pregledi.Count >= 2 ? pregledi[1].PregledId : null,
                Naziv = "EKG nalaz",
                TipDokumenta = "EKG",
                PutanjaDoFajla = @"Dokumenti\BK-000002\ekg.pdf",
                DatumDodavanja = DateTime.Today.AddDays(-8),
                DodaoKorisnikId = lekar1.KorisnikId
            });

            dokumenti.Add(new Dokument
            {
                PacijentId = pacijenti[1].PacijentId,
                PregledId = null,
                Naziv = "Uput za specijalistički pregled",
                TipDokumenta = "Uput",
                PutanjaDoFajla = @"Dokumenti\BK-000002\uput-kardiologija.pdf",
                DatumDodavanja = DateTime.Today.AddDays(-7),
                DodaoKorisnikId = lekar1.KorisnikId
            });
        }

        if (pacijenti.Count >= 3)
        {
            dokumenti.Add(new Dokument
            {
                PacijentId = pacijenti[2].PacijentId,
                PregledId = pregledi.Count >= 3 ? pregledi[2].PregledId : null,
                Naziv = "Pulmološki izveštaj",
                TipDokumenta = "Specijalistički izveštaj",
                PutanjaDoFajla = @"Dokumenti\BK-000003\pulmologija.pdf",
                DatumDodavanja = DateTime.Today.AddDays(-6),
                DodaoKorisnikId = lekar1.KorisnikId
            });
        }

        if (pacijenti.Count >= 4)
        {
            dokumenti.Add(new Dokument
            {
                PacijentId = pacijenti[3].PacijentId,
                PregledId = pregledi.Count >= 4 ? pregledi[3].PregledId : null,
                Naziv = "Kontrolni laboratorijski nalaz",
                TipDokumenta = "Laboratorijski nalaz",
                PutanjaDoFajla = @"Dokumenti\BK-000004\kontrola.pdf",
                DatumDodavanja = DateTime.Today.AddDays(-10),
                DodaoKorisnikId = lekar1.KorisnikId
            });
        }

        if (pacijenti.Count >= 5)
        {
            dokumenti.Add(new Dokument
            {
                PacijentId = pacijenti[4].PacijentId,
                PregledId = pregledi.Count >= 5 ? pregledi[4].PregledId : null,
                Naziv = "Endokrinološki izveštaj",
                TipDokumenta = "Specijalistički izveštaj",
                PutanjaDoFajla = @"Dokumenti\BK-000005\endokrinologija.pdf",
                DatumDodavanja = DateTime.Today.AddDays(-12),
                DodaoKorisnikId = lekar1.KorisnikId
            });
        }

        if (pacijenti.Count >= 6)
        {
            dokumenti.Add(new Dokument
            {
                PacijentId = pacijenti[5].PacijentId,
                PregledId = null,
                Naziv = "Stari lekarski izveštaj",
                TipDokumenta = "Lekarski izveštaj",
                PutanjaDoFajla = @"Dokumenti\BK-000006\stari-izvestaj.pdf",
                DatumDodavanja = DateTime.Today.AddDays(-30),
                DodaoKorisnikId = lekar2.KorisnikId
            });
        }

        foreach (Dokument dokument in dokumenti)
        {
            bool postoji =
                await _kontekst.Dokumenti.AnyAsync(d =>
                    d.PacijentId == dokument.PacijentId &&
                    d.Naziv == dokument.Naziv);

            if (!postoji)
            {
                _kontekst.Dokumenti.Add(dokument);
            }
        }

        await _kontekst.SaveChangesAsync();
    }






    // =========================================================
    // HASH LOZINKE
    // =========================================================

    private string KreirajHashLozinke(string lozinka)
    {
        using SHA256 sha256 = SHA256.Create();

        byte[] bajtovi =
            Encoding.UTF8.GetBytes(lozinka);

        byte[] hash =
            sha256.ComputeHash(bajtovi);

        return Convert.ToHexString(hash);
    }
}