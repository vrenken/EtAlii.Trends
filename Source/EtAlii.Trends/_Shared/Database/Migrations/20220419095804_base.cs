using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EtAlii.Trends.Migrations
{
    public partial class @base : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Diagrams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    DiagramWidth = table.Column<float>(type: "REAL", nullable: false),
                    PropertyGridHeight = table.Column<float>(type: "REAL", nullable: false),
                    DiagramZoom = table.Column<double>(type: "REAL", nullable: false),
                    HorizontalOffset = table.Column<double>(type: "REAL", nullable: false),
                    VerticalOffset = table.Column<double>(type: "REAL", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagrams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Diagrams_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Layers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ParentId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    IsExpanded = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsChecked = table.Column<bool>(type: "INTEGER", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    DiagramId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Layers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Layers_Diagrams_DiagramId",
                        column: x => x.DiagramId,
                        principalTable: "Diagrams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Layers_Layers_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Layers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Trends",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Begin = table.Column<DateTime>(type: "TEXT", nullable: false),
                    End = table.Column<DateTime>(type: "TEXT", nullable: false),
                    X = table.Column<double>(type: "REAL", nullable: false),
                    Y = table.Column<double>(type: "REAL", nullable: false),
                    W = table.Column<double>(type: "REAL", nullable: false),
                    H = table.Column<double>(type: "REAL", nullable: false),
                    LayerId = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsExpanded = table.Column<bool>(type: "INTEGER", nullable: false),
                    DiagramId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trends", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trends_Diagrams_DiagramId",
                        column: x => x.DiagramId,
                        principalTable: "Diagrams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Trends_Layers_LayerId",
                        column: x => x.LayerId,
                        principalTable: "Layers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Components",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    TrendId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Moment = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Components", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Components_Trends_TrendId",
                        column: x => x.TrendId,
                        principalTable: "Trends",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Connections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SourceId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SourceDirection = table.Column<double>(type: "REAL", nullable: false),
                    SourceLength = table.Column<double>(type: "REAL", nullable: false),
                    TargetId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TargetDirection = table.Column<double>(type: "REAL", nullable: false),
                    TargetLength = table.Column<double>(type: "REAL", nullable: false),
                    DiagramId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Connections_Components_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Connections_Components_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Connections_Diagrams_DiagramId",
                        column: x => x.DiagramId,
                        principalTable: "Diagrams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Components_Id",
                table: "Components",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Components_TrendId",
                table: "Components",
                column: "TrendId");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_DiagramId",
                table: "Connections",
                column: "DiagramId");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_Id",
                table: "Connections",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Connections_SourceId",
                table: "Connections",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_TargetId",
                table: "Connections",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagrams_Id",
                table: "Diagrams",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Diagrams_UserId",
                table: "Diagrams",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Layers_DiagramId",
                table: "Layers",
                column: "DiagramId");

            migrationBuilder.CreateIndex(
                name: "IX_Layers_Id",
                table: "Layers",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Layers_ParentId",
                table: "Layers",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Trends_DiagramId",
                table: "Trends",
                column: "DiagramId");

            migrationBuilder.CreateIndex(
                name: "IX_Trends_Id",
                table: "Trends",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trends_LayerId",
                table: "Trends",
                column: "LayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Id",
                table: "Users",
                column: "Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Connections");

            migrationBuilder.DropTable(
                name: "Components");

            migrationBuilder.DropTable(
                name: "Trends");

            migrationBuilder.DropTable(
                name: "Layers");

            migrationBuilder.DropTable(
                name: "Diagrams");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
