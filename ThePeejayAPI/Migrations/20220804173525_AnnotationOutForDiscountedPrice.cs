using Microsoft.EntityFrameworkCore.Migrations;

namespace ThePeejayAPI.Migrations
{
    public partial class AnnotationOutForDiscountedPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PriceAfterDiscount",
                table: "Products",
                type: "decimal(16,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PriceAfterDiscount",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,2)");
        }
    }
}
