using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Final_Project_Farrukh_Rakhmanov.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    GenreId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.GenreId);
                });

            migrationBuilder.CreateTable(
                name: "ProfilePictures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfilePictures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Author = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    AvailableQuantity = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookId);
                    table.ForeignKey(
                        name: "FK_Books_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "GenreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConfirmPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfilePictureId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserType = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_ProfilePictures_ProfilePictureId",
                        column: x => x.ProfilePictureId,
                        principalTable: "ProfilePictures",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MemberBook",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberBook", x => new { x.Id, x.BookId });
                    table.ForeignKey(
                        name: "FK_MemberBook_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberBook_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "GenreId", "Name" },
                values: new object[,]
                {
                    { "1", "Fiction" },
                    { "2", "Non-Fiction" },
                    { "3", "Fantasy" },
                    { "4", "Mystery" },
                    { "5", "Science Fiction" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Age", "ConfirmPassword", "DateOfBirth", "Name", "Password", "ProfilePictureId", "Role", "UserType", "Username" },
                values: new object[] { 1, 28, "Admin@123", new DateTime(1996, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "John", "Admin@123", null, "Admin", "User", "admin" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Age", "ConfirmPassword", "DateOfBirth", "Email", "Name", "Password", "PhoneNumber", "ProfilePictureId", "Role", "UserType", "Username" },
                values: new object[] { 2, 33, "Member@123", new DateTime(1991, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "john@gmail.com", "Alex", "Member@123", "123-456-7890", null, "Member", "Member", "member" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookId", "Author", "AvailableQuantity", "GenreId", "IsAvailable", "Quantity", "Title", "Year" },
                values: new object[,]
                {
                    { 1, "Fitzgerald F. Scott", 4, "1", true, 5, "The Great Gatsby", 1925 },
                    { 2, "Yuval Noah Harari", 5, "2", true, 5, "Sapiens", 2011 },
                    { 3, "Frank Herbert", 5, "1", true, 5, "Dune", 1965 },
                    { 4, "J.R.R. Tolkien", 5, "3", true, 5, "The Hobbit", 1937 },
                    { 5, "J.D. Salinger", 5, "1", true, 5, "The Catcher in the Rye", 1951 }
                });

            migrationBuilder.InsertData(
                table: "MemberBook",
                columns: new[] { "BookId", "Id" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 2, 2 },
                    { 3, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_GenreId",
                table: "Books",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberBook_BookId",
                table: "MemberBook",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProfilePictureId",
                table: "Users",
                column: "ProfilePictureId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberBook");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "ProfilePictures");
        }
    }
}
