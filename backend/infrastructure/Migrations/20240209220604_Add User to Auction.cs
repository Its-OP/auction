using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUsertoAuction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HostId",
                table: "Auctions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_HostId",
                table: "Auctions",
                column: "HostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Auctions_Users_HostId",
                table: "Auctions",
                column: "HostId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auctions_Users_HostId",
                table: "Auctions");

            migrationBuilder.DropIndex(
                name: "IX_Auctions_HostId",
                table: "Auctions");

            migrationBuilder.DropColumn(
                name: "HostId",
                table: "Auctions");
        }
    }
}
