using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManejoTareas.Migrations {
    public partial class AdminRol : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT Id FROM AspNetRoles WHERE Id = '40d83358-f62d-4778-a9e0-2f9ca59e06da')
                                   BEGIN
	                                INSERT AspNetRoles (Id, [Name], [NormalizedName])
	                                VALUES ('40d83358-f62d-4778-a9e0-2f9ca59e06da', 'admin', 'ADMIN')
                                   END");
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.Sql(@"DELETE AspNetRoles
                                   WHERE Id = '40d83358-f62d-4778-a9e0-2f9ca59e06da'");
        }
    }
}
