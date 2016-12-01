using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HerbalDrugstore.Data.Migrations
{
    public partial class Eighth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DrugChanges",
                columns: table => new
                {
                    ChangeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    DrugId = table.Column<int>(nullable: false),
                    Increasing = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugChanges", x => x.ChangeId);
                    table.ForeignKey(
                        name: "FK_DrugChanges_Drug_DrugId",
                        column: x => x.DrugId,
                        principalTable: "Drug",
                        principalColumn: "DrugId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddColumn<int>(
                name: "DrugChangesChangeId",
                table: "Supplier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_DrugChangesChangeId",
                table: "Supplier",
                column: "DrugChangesChangeId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugChanges_DrugId",
                table: "DrugChanges",
                column: "DrugId");

            migrationBuilder.AddForeignKey(
                name: "FK_Supplier_DrugChanges_DrugChangesChangeId",
                table: "Supplier",
                column: "DrugChangesChangeId",
                principalTable: "DrugChanges",
                principalColumn: "ChangeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Supplier_DrugChanges_DrugChangesChangeId",
                table: "Supplier");

            migrationBuilder.DropIndex(
                name: "IX_Supplier_DrugChangesChangeId",
                table: "Supplier");

            migrationBuilder.DropColumn(
                name: "DrugChangesChangeId",
                table: "Supplier");

            migrationBuilder.DropTable(
                name: "DrugChanges");
        }
    }
}
