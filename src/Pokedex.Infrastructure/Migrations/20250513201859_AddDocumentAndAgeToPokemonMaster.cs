using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pokedex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDocumentAndAgeToPokemonMaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "PokemonMasters",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Document",
                table: "PokemonMasters",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "PokemonMasters");

            migrationBuilder.DropColumn(
                name: "Document",
                table: "PokemonMasters");
        }
    }
}
