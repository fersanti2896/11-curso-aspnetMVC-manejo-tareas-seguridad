using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManejoTareas.Migrations {
    public partial class PropiedadesTarea : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.AlterColumn<string>(
                name        : "Titulo",
                table       : "Tareas",
                type        : "nvarchar(250)",
                maxLength   : 250,
                nullable    : false,
                defaultValue: "",
                oldClrType  : typeof(string),
                oldType     : "nvarchar(max)",
                oldNullable : true
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.AlterColumn<string>(
                name        : "Titulo",
                table       : "Tareas",
                type        : "nvarchar(max)",
                nullable    : true,
                oldClrType  : typeof(string),
                oldType     : "nvarchar(250)",
                oldMaxLength: 250
            );
        }
    }
}
