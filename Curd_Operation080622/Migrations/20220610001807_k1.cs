﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Curd_Operation080622.Migrations
{
    public partial class k1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Teacher_Id",
                table: "Hobbies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Teacher_Id",
                table: "Genders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Teacher_Id",
                table: "Countries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hobbies_Teacher_Id",
                table: "Hobbies",
                column: "Teacher_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Genders_Teacher_Id",
                table: "Genders",
                column: "Teacher_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Teacher_Id",
                table: "Countries",
                column: "Teacher_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_teachers_Teacher_Id",
                table: "Countries",
                column: "Teacher_Id",
                principalTable: "teachers",
                principalColumn: "Teacher_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Genders_teachers_Teacher_Id",
                table: "Genders",
                column: "Teacher_Id",
                principalTable: "teachers",
                principalColumn: "Teacher_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Hobbies_teachers_Teacher_Id",
                table: "Hobbies",
                column: "Teacher_Id",
                principalTable: "teachers",
                principalColumn: "Teacher_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Countries_teachers_Teacher_Id",
                table: "Countries");

            migrationBuilder.DropForeignKey(
                name: "FK_Genders_teachers_Teacher_Id",
                table: "Genders");

            migrationBuilder.DropForeignKey(
                name: "FK_Hobbies_teachers_Teacher_Id",
                table: "Hobbies");

            migrationBuilder.DropIndex(
                name: "IX_Hobbies_Teacher_Id",
                table: "Hobbies");

            migrationBuilder.DropIndex(
                name: "IX_Genders_Teacher_Id",
                table: "Genders");

            migrationBuilder.DropIndex(
                name: "IX_Countries_Teacher_Id",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "Teacher_Id",
                table: "Hobbies");

            migrationBuilder.DropColumn(
                name: "Teacher_Id",
                table: "Genders");

            migrationBuilder.DropColumn(
                name: "Teacher_Id",
                table: "Countries");
        }
    }
}
