using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class AddCascadeDeletionForVaccinations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vaccination_Animals_AnimalID",
                table: "Vaccination");

            migrationBuilder.AddForeignKey(
                name: "FK_Vaccination_Animals_AnimalID",
                table: "Vaccination",
                column: "AnimalID",
                principalTable: "Animals",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vaccination_Animals_AnimalID",
                table: "Vaccination");

            migrationBuilder.AddForeignKey(
                name: "FK_Vaccination_Animals_AnimalID",
                table: "Vaccination",
                column: "AnimalID",
                principalTable: "Animals",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
