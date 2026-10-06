using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BolnickiSistem.Migrations
{
    /// <inheritdoc />
    public partial class DodeljivanjePacijentaLekaru : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LekarId",
                table: "Pacijenti",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pacijenti_LekarId",
                table: "Pacijenti",
                column: "LekarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pacijenti_Korisnici_LekarId",
                table: "Pacijenti",
                column: "LekarId",
                principalTable: "Korisnici",
                principalColumn: "KorisnikId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pacijenti_Korisnici_LekarId",
                table: "Pacijenti");

            migrationBuilder.DropIndex(
                name: "IX_Pacijenti_LekarId",
                table: "Pacijenti");

            migrationBuilder.DropColumn(
                name: "LekarId",
                table: "Pacijenti");
        }
    }
}
