using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class OnlineBankingMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerNo = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(maxLength: 50, nullable: false),
                    Password = table.Column<string>(maxLength: 63, nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    Address = table.Column<string>(maxLength: 50, nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Balance = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    AccountNo = table.Column<int>(nullable: false),
                    AccountType = table.Column<string>(nullable: true),
                    CustomerId = table.Column<int>(nullable: false),
                    AccountStatus = table.Column<bool>(nullable: false),
                    Interestrate = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Account_Customer_Id",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payee",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    PayeeName = table.Column<string>(maxLength: 200, nullable: false),
                    PayeeAccountNumber = table.Column<int>(nullable: false),
                    BankCode = table.Column<string>(maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payee_Customer_Id",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionNo = table.Column<int>(nullable: false),
                    Accountno = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    TransactionInfo = table.Column<string>(nullable: true),
                    Time = table.Column<DateTime>(nullable: false),
                    TransactionType = table.Column<string>(unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transaction_Customer_Id",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRolesMapping",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRolesMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRolesMapping_CUstomer_Id",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRolesMapping_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    AccountId = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    FromAccount = table.Column<int>(nullable: false),
                    ToAccount = table.Column<int>(nullable: false),
                    PayeeId = table.Column<int>(nullable: false),
                    Reference = table.Column<string>(maxLength: 100, nullable: true),
                    TransactionDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentHistory_Account_Id",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentHistory_Customer_Id",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentHistory_Payee_Id",
                        column: x => x.PayeeId,
                        principalTable: "Payee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Customer",
                columns: new[] { "Id", "Address", "CustomerNo", "Email", "FirstName", "LastName", "Password", "PhoneNumber", "UserName" },
                values: new object[] { 1, "Times Square Arena,New York, USA", 1111, "Admin@banking.com", "Administrator", "Administrator", "admin", "+17023456789", "admin" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Teller" },
                    { 3, "Customer" }
                });

            migrationBuilder.InsertData(
                table: "UserRolesMapping",
                columns: new[] { "Id", "CustomerId", "RoleId" },
                values: new object[] { 1, 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Account_CustomerId",
                table: "Account",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Payee_CustomerId",
                table: "Payee",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentHistory_AccountId",
                table: "PaymentHistory",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentHistory_CustomerId",
                table: "PaymentHistory",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentHistory_PayeeId",
                table: "PaymentHistory",
                column: "PayeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_CustomerId",
                table: "Transaction",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRolesMapping_CustomerId",
                table: "UserRolesMapping",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRolesMapping_RoleId",
                table: "UserRolesMapping",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentHistory");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "UserRolesMapping");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Payee");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
