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
                name: "Ride",
                columns: table => new
                {
                    Ride_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Driver_ID = table.Column<int>(type: "int", nullable: false),
                    Patient_ID = table.Column<int>(type: "int", nullable: false),
                    Pickup_Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dropoff_Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pickup_Time = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pickup_Date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ride_Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ride", x => x.Ride_ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ride");
        }
    }
}
