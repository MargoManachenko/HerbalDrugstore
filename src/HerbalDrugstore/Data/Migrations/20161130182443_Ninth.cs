using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HerbalDrugstore.Data.Migrations
{
    public partial class Ninth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "DrugChanges",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SupplierName",
                table: "DrugChanges",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "DrugChanges");

            migrationBuilder.DropColumn(
                name: "SupplierName",
                table: "DrugChanges");

            migrationBuilder.AddColumn<int>(
                name: "DrugChangesChangeId",
                table: "Supplier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_DrugChangesChangeId",
                table: "Supplier",
                column: "DrugChangesChangeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Supplier_DrugChanges_DrugChangesChangeId",
                table: "Supplier",
                column: "DrugChangesChangeId",
                principalTable: "DrugChanges",
                principalColumn: "ChangeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
