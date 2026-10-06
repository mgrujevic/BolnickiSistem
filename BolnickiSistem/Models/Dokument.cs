namespace BolnickiSistem.Models;

public class Dokument
{
    public int DokumentId { get; set; }

    public int PacijentId { get; set; }

    public Pacijent? Pacijent { get; set; }


    public int? PregledId { get; set; }

    public Pregled? Pregled { get; set; }


    public string Naziv { get; set; } = string.Empty;

    public string TipDokumenta { get; set; } = string.Empty;

    public string PutanjaDoFajla { get; set; } = string.Empty;

    public DateTime DatumDodavanja { get; set; } = DateTime.Now;


    public int DodaoKorisnikId { get; set; }

    public Korisnik? DodaoKorisnik { get; set; }
}