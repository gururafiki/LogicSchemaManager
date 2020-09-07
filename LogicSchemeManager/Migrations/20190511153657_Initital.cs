using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LogicSchemeManager.Migrations
{
    public partial class Initital : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ElementTypes",
                columns: table => new
                {
                    ElementTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElementTypes", x => x.ElementTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Ports",
                columns: table => new
                {
                    PortId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    IsOutput = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ports", x => x.PortId);
                });

            migrationBuilder.CreateTable(
                name: "Scheme",
                columns: table => new
                {
                    SchemaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scheme", x => x.SchemaId);
                });

            migrationBuilder.CreateTable(
                name: "Combinations",
                columns: table => new
                {
                    CombinationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ElementTypeId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Combinations", x => x.CombinationId);
                    table.ForeignKey(
                        name: "FK_Combinations_ElementTypes_ElementTypeId",
                        column: x => x.ElementTypeId,
                        principalTable: "ElementTypes",
                        principalColumn: "ElementTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Elements",
                columns: table => new
                {
                    ElementId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    SchemaId = table.Column<int>(nullable: false),
                    ElementTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elements", x => x.ElementId);
                    table.ForeignKey(
                        name: "FK_Elements_ElementTypes_ElementTypeId",
                        column: x => x.ElementTypeId,
                        principalTable: "ElementTypes",
                        principalColumn: "ElementTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Elements_Scheme_SchemaId",
                        column: x => x.SchemaId,
                        principalTable: "Scheme",
                        principalColumn: "SchemaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CombinationPorts",
                columns: table => new
                {
                    CombinationPortId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CombinationId = table.Column<int>(nullable: false),
                    PortId = table.Column<int>(nullable: false),
                    Value = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CombinationPorts", x => x.CombinationPortId);
                    table.ForeignKey(
                        name: "FK_CombinationPorts_Combinations_CombinationId",
                        column: x => x.CombinationId,
                        principalTable: "Combinations",
                        principalColumn: "CombinationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CombinationPorts_Ports_PortId",
                        column: x => x.PortId,
                        principalTable: "Ports",
                        principalColumn: "PortId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ElementPorts",
                columns: table => new
                {
                    ElementPortId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ElementId = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    PortId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElementPorts", x => x.ElementPortId);
                    table.ForeignKey(
                        name: "FK_ElementPorts_Elements_ElementId",
                        column: x => x.ElementId,
                        principalTable: "Elements",
                        principalColumn: "ElementId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ElementPorts_ElementPorts_ParentId",
                        column: x => x.ParentId,
                        principalTable: "ElementPorts",
                        principalColumn: "ElementPortId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ElementPorts_Ports_PortId",
                        column: x => x.PortId,
                        principalTable: "Ports",
                        principalColumn: "PortId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CombinationPorts_CombinationId",
                table: "CombinationPorts",
                column: "CombinationId");

            migrationBuilder.CreateIndex(
                name: "IX_CombinationPorts_PortId",
                table: "CombinationPorts",
                column: "PortId");

            migrationBuilder.CreateIndex(
                name: "IX_Combinations_ElementTypeId",
                table: "Combinations",
                column: "ElementTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ElementPorts_ElementId",
                table: "ElementPorts",
                column: "ElementId");

            migrationBuilder.CreateIndex(
                name: "IX_ElementPorts_ParentId",
                table: "ElementPorts",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ElementPorts_PortId",
                table: "ElementPorts",
                column: "PortId");

            migrationBuilder.CreateIndex(
                name: "IX_Elements_ElementTypeId",
                table: "Elements",
                column: "ElementTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Elements_SchemaId",
                table: "Elements",
                column: "SchemaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CombinationPorts");

            migrationBuilder.DropTable(
                name: "ElementPorts");

            migrationBuilder.DropTable(
                name: "Combinations");

            migrationBuilder.DropTable(
                name: "Elements");

            migrationBuilder.DropTable(
                name: "Ports");

            migrationBuilder.DropTable(
                name: "ElementTypes");

            migrationBuilder.DropTable(
                name: "Scheme");
        }
    }
}
