using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RuletanBublee.Migrations
{
    /// <inheritdoc />
    public partial class AddIndiceVisual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IndiceVisual",
                table: "GirosResultados",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IndiceVisual",
                table: "GirosResultados");
        }
    }
}
