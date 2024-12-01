using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class _2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Purchase",
                columns: table => new
                {
                    Purchase_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Selling_ID = table.Column<int>(type: "int", nullable: false),
                    Patient_ID = table.Column<int>(type: "int", nullable: false),
                    Requested_Quantity = table.Column<int>(type: "int", nullable: false),
                    Request_Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Delivery_Date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Delivery_Time = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchase", x => x.Purchase_ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Purchase");
        }
    }
}
