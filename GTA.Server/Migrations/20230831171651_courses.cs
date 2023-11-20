using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GTA.Server.Migrations
{
    /// <inheritdoc />
    public partial class courses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tees = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressID = table.Column<int>(type: "int", nullable: false),
                    PhoneID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Courses_Addresses_AddressID",
                        column: x => x.AddressID,
                        principalTable: "Addresses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Courses_Phones_PhoneID",
                        column: x => x.PhoneID,
                        principalTable: "Phones",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "People",
                columns: new[] { "ID", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 2, "David", "Okinsky" },
                    { 3, "Nathan", "Pannell" },
                    { 4, "Bob", "Souls" },
                    { 5, "Ryan", "Wheeler" },
                    { 6, "Mathew", "Hess" },
                    { 7, "Brian", "Corr" },
                    { 8, "Danny", "Martel" }
                });

            migrationBuilder.UpdateData(
                table: "PeopleEmails",
                keyColumn: "ID",
                keyValue: 1,
                column: "Code",
                value: "5f8b0d33-c736-4bbf-a504-b0fdb2591543");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_AddressID",
                table: "Courses",
                column: "AddressID");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_PhoneID",
                table: "Courses",
                column: "PhoneID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "ID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "ID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "ID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "ID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "ID",
                keyValue: 8);

            migrationBuilder.UpdateData(
                table: "PeopleEmails",
                keyColumn: "ID",
                keyValue: 1,
                column: "Code",
                value: "647ffe78-95c4-4407-8d86-e8454a595859");
        }
    }
}
