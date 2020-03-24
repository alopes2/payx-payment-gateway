using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PayX.Data.Migrations
{
    public partial class AddSeedUsersAndAdjustRoleDefaultValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "Users",
                nullable: false,
                defaultValue: 2,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Password", "Role" },
                values: new object[] { new Guid("5c6e15ff-508f-487a-a753-1119e831eabb"), "admin@payx.io", "$2a$12$iSfNL2fnxQN1hLVXd8PcT.5aorGzJFS8ARBZpDaEJkkQ8eniLP9X6", 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("5c6e15ff-508f-487a-a753-1119e831eabb"));

            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldDefaultValue: 2);
        }
    }
}
