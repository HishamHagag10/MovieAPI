using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePKofActorsAwardsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ActorAwards",
                table: "ActorAwards");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActorAwards",
                table: "ActorAwards",
                columns: new[] { "ActorId", "AwardId", "YearOfHonor" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ActorAwards",
                table: "ActorAwards");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActorAwards",
                table: "ActorAwards",
                columns: new[] { "ActorId", "AwardId" });
        }
    }
}
