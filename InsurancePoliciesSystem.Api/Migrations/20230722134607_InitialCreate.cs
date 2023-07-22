using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsurancePoliciesSystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Agreements");

            migrationBuilder.EnsureSchema(
                name: "WorkInsurance");

            migrationBuilder.EnsureSchema(
                name: "IndividualTravelInsurance");

            migrationBuilder.CreateTable(
                name: "Agreement",
                schema: "Agreements",
                columns: table => new
                {
                    AgreementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AgreementText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Package = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agreement", x => x.AgreementId);
                });

            migrationBuilder.CreateTable(
                name: "Policy",
                schema: "IndividualTravelInsurance",
                columns: table => new
                {
                    PolicyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HouseNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlatNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InsuranceSum = table.Column<int>(type: "int", nullable: false),
                    SelectedPackage = table.Column<int>(type: "int", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PricePerDay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AgreementsIds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PolicyNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Policy", x => x.PolicyId);
                });

            migrationBuilder.CreateTable(
                name: "Policy",
                schema: "WorkInsurance",
                columns: table => new
                {
                    PolicyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HouseNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlatNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfPeople = table.Column<int>(type: "int", nullable: false),
                    InsuranceSum = table.Column<int>(type: "int", nullable: false),
                    SelectedPackage = table.Column<int>(type: "int", nullable: false),
                    DateFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PricePerPerson = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AgreementsIds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PolicyNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Policy", x => x.PolicyId);
                });

            migrationBuilder.CreateTable(
                name: "PriceConfiguration",
                schema: "IndividualTravelInsurance",
                columns: table => new
                {
                    InsuranceSum = table.Column<int>(type: "int", nullable: false),
                    Essential = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Adventure = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Relax = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceConfiguration", x => x.InsuranceSum);
                });

            migrationBuilder.CreateTable(
                name: "PriceConfiguration",
                schema: "WorkInsurance",
                columns: table => new
                {
                    InsuranceSum = table.Column<int>(type: "int", nullable: false),
                    Basic = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Plus = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Max = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceConfiguration", x => x.InsuranceSum);
                });

            migrationBuilder.CreateTable(
                name: "SearchPolicy",
                columns: table => new
                {
                    PolicyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PolicyNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Package = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchPolicy", x => x.PolicyId);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                schema: "WorkInsurance",
                columns: table => new
                {
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    WorkInsurancePolicyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonId);
                    table.ForeignKey(
                        name: "FK_Persons_Policy_WorkInsurancePolicyId",
                        column: x => x.WorkInsurancePolicyId,
                        principalSchema: "WorkInsurance",
                        principalTable: "Policy",
                        principalColumn: "PolicyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Persons_WorkInsurancePolicyId",
                schema: "WorkInsurance",
                table: "Persons",
                column: "WorkInsurancePolicyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Agreement",
                schema: "Agreements");

            migrationBuilder.DropTable(
                name: "Persons",
                schema: "WorkInsurance");

            migrationBuilder.DropTable(
                name: "Policy",
                schema: "IndividualTravelInsurance");

            migrationBuilder.DropTable(
                name: "PriceConfiguration",
                schema: "IndividualTravelInsurance");

            migrationBuilder.DropTable(
                name: "PriceConfiguration",
                schema: "WorkInsurance");

            migrationBuilder.DropTable(
                name: "SearchPolicy");

            migrationBuilder.DropTable(
                name: "Policy",
                schema: "WorkInsurance");
        }
    }
}
