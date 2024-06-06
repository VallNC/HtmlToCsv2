using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Html2.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigrationAttempt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_candidates_areas__AreaId",
                table: "candidates");

            migrationBuilder.DropColumn(
                name: "PersonNames",
                table: "candidates");

            migrationBuilder.RenameColumn(
                name: "_AreaId",
                table: "candidates",
                newName: "AreaId");

            migrationBuilder.RenameIndex(
                name: "IX_candidates__AreaId",
                table: "candidates",
                newName: "IX_candidates_AreaId");

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    candidateId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => new { x.candidateId, x.Id });
                    table.ForeignKey(
                        name: "FK_Person_candidates_candidateId",
                        column: x => x.candidateId,
                        principalTable: "candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_candidates_areas_AreaId",
                table: "candidates",
                column: "AreaId",
                principalTable: "areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_candidates_areas_AreaId",
                table: "candidates");

            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.RenameColumn(
                name: "AreaId",
                table: "candidates",
                newName: "_AreaId");

            migrationBuilder.RenameIndex(
                name: "IX_candidates_AreaId",
                table: "candidates",
                newName: "IX_candidates__AreaId");

            migrationBuilder.AddColumn<string>(
                name: "PersonNames",
                table: "candidates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_candidates_areas__AreaId",
                table: "candidates",
                column: "_AreaId",
                principalTable: "areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
