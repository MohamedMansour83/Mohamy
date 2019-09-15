using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ElMaitre.DAL.Migrations
{
    public partial class SessionNote11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
               name: "SessionNote",
               columns: table => new
               {
                   Id = table.Column<int>(nullable: false)
                       .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                   AddedDate = table.Column<DateTime>(nullable: false),
                   ModifiedDate = table.Column<DateTime>(nullable: false),
                   Title = table.Column<string>(nullable: true),
                   UserId = table.Column<string>(nullable: true),
                   SessionId = table.Column<int>(nullable: false)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_SessionNote", x => x.Id);
                   table.ForeignKey(
                       name: "FK_SessionNote_Session_SessionId",
                       column: x => x.SessionId,
                       principalTable: "Session",
                       principalColumn: "Id",
                       onDelete: ReferentialAction.Cascade);
                   table.ForeignKey(
                       name: "FK_SessionNote_IdentityUsers_UserId",
                       column: x => x.UserId,
                       principalTable: "IdentityUsers",
                       principalColumn: "Id",
                       onDelete: ReferentialAction.Restrict);
               });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
