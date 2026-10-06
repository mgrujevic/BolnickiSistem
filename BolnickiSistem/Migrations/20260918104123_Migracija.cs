using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BolnickiSistem.Migrations
{
    /// <inheritdoc />
    public partial class Migracija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pacijenti",
                columns: table => new
                {
                    PacijentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrojZdravstveneKartice = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    JMBG = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatumRodjenja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Pol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KrvnaGrupa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alergije = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HronicneBolesti = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacijenti", x => x.PacijentId);
                });

            migrationBuilder.CreateTable(
                name: "Uloge",
                columns: table => new
                {
                    UlogaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uloge", x => x.UlogaId);
                });

            migrationBuilder.CreateTable(
                name: "Korisnici",
                columns: table => new
                {
                    KorisnikId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KorisnickoIme = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LozinkaHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Aktivan = table.Column<bool>(type: "bit", nullable: false),
                    DatumKreiranja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UlogaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnici", x => x.KorisnikId);
                    table.ForeignKey(
                        name: "FK_Korisnici_Uloge_UlogaId",
                        column: x => x.UlogaId,
                        principalTable: "Uloge",
                        principalColumn: "UlogaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pregledi",
                columns: table => new
                {
                    PregledId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PacijentId = table.Column<int>(type: "int", nullable: false),
                    LekarId = table.Column<int>(type: "int", nullable: false),
                    DatumPregleda = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RazlogDolaska = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Simptomi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nalaz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Preporuka = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatumKontrole = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pregledi", x => x.PregledId);
                    table.ForeignKey(
                        name: "FK_Pregledi_Korisnici_LekarId",
                        column: x => x.LekarId,
                        principalTable: "Korisnici",
                        principalColumn: "KorisnikId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pregledi_Pacijenti_PacijentId",
                        column: x => x.PacijentId,
                        principalTable: "Pacijenti",
                        principalColumn: "PacijentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dijagnoze",
                columns: table => new
                {
                    DijagnozaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PregledId = table.Column<int>(type: "int", nullable: false),
                    Sifra = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dijagnoze", x => x.DijagnozaId);
                    table.ForeignKey(
                        name: "FK_Dijagnoze_Pregledi_PregledId",
                        column: x => x.PregledId,
                        principalTable: "Pregledi",
                        principalColumn: "PregledId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dokumenti",
                columns: table => new
                {
                    DokumentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PacijentId = table.Column<int>(type: "int", nullable: false),
                    PregledId = table.Column<int>(type: "int", nullable: true),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipDokumenta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PutanjaDoFajla = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatumDodavanja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DodaoKorisnikId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dokumenti", x => x.DokumentId);
                    table.ForeignKey(
                        name: "FK_Dokumenti_Korisnici_DodaoKorisnikId",
                        column: x => x.DodaoKorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "KorisnikId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dokumenti_Pacijenti_PacijentId",
                        column: x => x.PacijentId,
                        principalTable: "Pacijenti",
                        principalColumn: "PacijentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dokumenti_Pregledi_PregledId",
                        column: x => x.PregledId,
                        principalTable: "Pregledi",
                        principalColumn: "PregledId");
                });

            migrationBuilder.CreateTable(
                name: "LaboratorijskiNalazi",
                columns: table => new
                {
                    LaboratorijskiNalazId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PacijentId = table.Column<int>(type: "int", nullable: false),
                    PregledId = table.Column<int>(type: "int", nullable: true),
                    NazivAnalize = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rezultat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Jedinica = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferentnaVrednost = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatumAnalize = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Napomena = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaboratorijskiNalazi", x => x.LaboratorijskiNalazId);
                    table.ForeignKey(
                        name: "FK_LaboratorijskiNalazi_Pacijenti_PacijentId",
                        column: x => x.PacijentId,
                        principalTable: "Pacijenti",
                        principalColumn: "PacijentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LaboratorijskiNalazi_Pregledi_PregledId",
                        column: x => x.PregledId,
                        principalTable: "Pregledi",
                        principalColumn: "PregledId");
                });

            migrationBuilder.CreateTable(
                name: "Terapije",
                columns: table => new
                {
                    TerapijaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PregledId = table.Column<int>(type: "int", nullable: false),
                    NazivLeka = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Doziranje = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ucestalost = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatumPocetka = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatumZavrsetka = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Uputstvo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Terapije", x => x.TerapijaId);
                    table.ForeignKey(
                        name: "FK_Terapije_Pregledi_PregledId",
                        column: x => x.PregledId,
                        principalTable: "Pregledi",
                        principalColumn: "PregledId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Uloge",
                columns: new[] { "UlogaId", "Naziv" },
                values: new object[,]
                {
                    { 1, "Administrator" },
                    { 2, "Lekar" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dijagnoze_PregledId",
                table: "Dijagnoze",
                column: "PregledId");

            migrationBuilder.CreateIndex(
                name: "IX_Dokumenti_DodaoKorisnikId",
                table: "Dokumenti",
                column: "DodaoKorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Dokumenti_PacijentId",
                table: "Dokumenti",
                column: "PacijentId");

            migrationBuilder.CreateIndex(
                name: "IX_Dokumenti_PregledId",
                table: "Dokumenti",
                column: "PregledId");

            migrationBuilder.CreateIndex(
                name: "IX_Korisnici_KorisnickoIme",
                table: "Korisnici",
                column: "KorisnickoIme",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Korisnici_UlogaId",
                table: "Korisnici",
                column: "UlogaId");

            migrationBuilder.CreateIndex(
                name: "IX_LaboratorijskiNalazi_PacijentId",
                table: "LaboratorijskiNalazi",
                column: "PacijentId");

            migrationBuilder.CreateIndex(
                name: "IX_LaboratorijskiNalazi_PregledId",
                table: "LaboratorijskiNalazi",
                column: "PregledId");

            migrationBuilder.CreateIndex(
                name: "IX_Pacijenti_BrojZdravstveneKartice",
                table: "Pacijenti",
                column: "BrojZdravstveneKartice",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pacijenti_JMBG",
                table: "Pacijenti",
                column: "JMBG",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pregledi_LekarId",
                table: "Pregledi",
                column: "LekarId");

            migrationBuilder.CreateIndex(
                name: "IX_Pregledi_PacijentId",
                table: "Pregledi",
                column: "PacijentId");

            migrationBuilder.CreateIndex(
                name: "IX_Terapije_PregledId",
                table: "Terapije",
                column: "PregledId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dijagnoze");

            migrationBuilder.DropTable(
                name: "Dokumenti");

            migrationBuilder.DropTable(
                name: "LaboratorijskiNalazi");

            migrationBuilder.DropTable(
                name: "Terapije");

            migrationBuilder.DropTable(
                name: "Pregledi");

            migrationBuilder.DropTable(
                name: "Korisnici");

            migrationBuilder.DropTable(
                name: "Pacijenti");

            migrationBuilder.DropTable(
                name: "Uloge");
        }
    }
}
