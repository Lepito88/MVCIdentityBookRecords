using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCIdentityBookRecords.Migrations
{
    public partial class booktables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                schema: "Identity",
                columns: table => new
                {
                    Idauthor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Firstname = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Lastname = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Idauthor);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Books",
                schema: "Identity",
                columns: table => new
                {
                    Idbook = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BookName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReleaseDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Type = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Isbn = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Idbook);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "Identity",
                columns: table => new
                {
                    Idcategory = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoryName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Idcategory);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ApplicationUserBook",
                schema: "Identity",
                columns: table => new
                {
                    BooksIdbook = table.Column<int>(type: "int", nullable: false),
                    IdUsersId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserBook", x => new { x.BooksIdbook, x.IdUsersId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserBook_Books_BooksIdbook",
                        column: x => x.BooksIdbook,
                        principalSchema: "Identity",
                        principalTable: "Books",
                        principalColumn: "Idbook",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserBook_User_IdUsersId",
                        column: x => x.IdUsersId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AuthorBook",
                schema: "Identity",
                columns: table => new
                {
                    AuthorsIdauthor = table.Column<int>(type: "int", nullable: false),
                    BooksIdbook = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorBook", x => new { x.AuthorsIdauthor, x.BooksIdbook });
                    table.ForeignKey(
                        name: "FK_AuthorBook_Authors_AuthorsIdauthor",
                        column: x => x.AuthorsIdauthor,
                        principalSchema: "Identity",
                        principalTable: "Authors",
                        principalColumn: "Idauthor",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorBook_Books_BooksIdbook",
                        column: x => x.BooksIdbook,
                        principalSchema: "Identity",
                        principalTable: "Books",
                        principalColumn: "Idbook",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BookCategory",
                schema: "Identity",
                columns: table => new
                {
                    BooksIdbook = table.Column<int>(type: "int", nullable: false),
                    CategoriesIdcategory = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookCategory", x => new { x.BooksIdbook, x.CategoriesIdcategory });
                    table.ForeignKey(
                        name: "FK_BookCategory_Books_BooksIdbook",
                        column: x => x.BooksIdbook,
                        principalSchema: "Identity",
                        principalTable: "Books",
                        principalColumn: "Idbook",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookCategory_Categories_CategoriesIdcategory",
                        column: x => x.CategoriesIdcategory,
                        principalSchema: "Identity",
                        principalTable: "Categories",
                        principalColumn: "Idcategory",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserBook_IdUsersId",
                schema: "Identity",
                table: "ApplicationUserBook",
                column: "IdUsersId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorBook_BooksIdbook",
                schema: "Identity",
                table: "AuthorBook",
                column: "BooksIdbook");

            migrationBuilder.CreateIndex(
                name: "IX_BookCategory_CategoriesIdcategory",
                schema: "Identity",
                table: "BookCategory",
                column: "CategoriesIdcategory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserBook",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "AuthorBook",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "BookCategory",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Authors",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Books",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "Identity");
        }
    }
}
