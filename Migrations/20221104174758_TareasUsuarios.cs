using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManejoTareas.Migrations {
    public partial class TareasUsuarios : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.AddColumn<string>(
                name    : "UsuarioId",
                table   : "Tareas",
                type    : "nvarchar(450)",
                nullable: true
            );

            migrationBuilder.CreateIndex(
                name  : "IX_Tareas_UsuarioId",
                table : "Tareas",
                column: "UsuarioId"
            );

            migrationBuilder.AddForeignKey(
                name           : "FK_Tareas_AspNetUsers_UsuarioId",
                table          : "Tareas",
                column         : "UsuarioId",
                principalTable : "AspNetUsers",
                principalColumn: "Id"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropForeignKey(
                name: "FK_Tareas_AspNetUsers_UsuarioId",
                table: "Tareas"
            );

            migrationBuilder.DropIndex(
                name: "IX_Tareas_UsuarioId",
                table: "Tareas"
            );

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Tareas"
            );
        }
    }
}
