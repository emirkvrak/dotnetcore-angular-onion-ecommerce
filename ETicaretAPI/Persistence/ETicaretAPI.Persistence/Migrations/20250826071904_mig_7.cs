using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ETicaretAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductProductImageFile_Files_ProfuctImageFilesId",
                table: "ProductProductImageFile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductProductImageFile",
                table: "ProductProductImageFile");

            migrationBuilder.DropIndex(
                name: "IX_ProductProductImageFile_ProfuctImageFilesId",
                table: "ProductProductImageFile");

            migrationBuilder.RenameColumn(
                name: "ProfuctImageFilesId",
                table: "ProductProductImageFile",
                newName: "ProductImageFilesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductProductImageFile",
                table: "ProductProductImageFile",
                columns: new[] { "ProductImageFilesId", "ProductsId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductProductImageFile_ProductsId",
                table: "ProductProductImageFile",
                column: "ProductsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProductImageFile_Files_ProductImageFilesId",
                table: "ProductProductImageFile",
                column: "ProductImageFilesId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductProductImageFile_Files_ProductImageFilesId",
                table: "ProductProductImageFile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductProductImageFile",
                table: "ProductProductImageFile");

            migrationBuilder.DropIndex(
                name: "IX_ProductProductImageFile_ProductsId",
                table: "ProductProductImageFile");

            migrationBuilder.RenameColumn(
                name: "ProductImageFilesId",
                table: "ProductProductImageFile",
                newName: "ProfuctImageFilesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductProductImageFile",
                table: "ProductProductImageFile",
                columns: new[] { "ProductsId", "ProfuctImageFilesId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductProductImageFile_ProfuctImageFilesId",
                table: "ProductProductImageFile",
                column: "ProfuctImageFilesId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProductImageFile_Files_ProfuctImageFilesId",
                table: "ProductProductImageFile",
                column: "ProfuctImageFilesId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
