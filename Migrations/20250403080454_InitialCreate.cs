using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebInstituto.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Asignatura",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Course = table.Column<int>(type: "INTEGER", nullable: false),
                    IdProfesor = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asignatura", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Horario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Day = table.Column<int>(type: "INTEGER", nullable: false),
                    Start = table.Column<string>(type: "TEXT", nullable: false),
                    End = table.Column<string>(type: "TEXT", nullable: false),
                    AsignaturaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Horario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Horario_Asignatura_AsignaturaId",
                        column: x => x.AsignaturaId,
                        principalTable: "Asignatura",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Persona",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Teacher = table.Column<int>(type: "INTEGER", nullable: false),
                    EmergencyPhone = table.Column<string>(type: "TEXT", nullable: false),
                    Mail = table.Column<string>(type: "TEXT", nullable: false),
                    ContrasenyaHash = table.Column<string>(type: "TEXT", nullable: false),
                    AsignaturaId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persona", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Persona_Asignatura_AsignaturaId",
                        column: x => x.AsignaturaId,
                        principalTable: "Asignatura",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AsignaturaPersona",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdAsignatura = table.Column<int>(type: "INTEGER", nullable: false),
                    IdAlumno = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsignaturaPersona", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AsignaturaPersona_Asignatura_IdAsignatura",
                        column: x => x.IdAsignatura,
                        principalTable: "Asignatura",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AsignaturaPersona_Persona_IdAlumno",
                        column: x => x.IdAlumno,
                        principalTable: "Persona",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Asignatura_IdProfesor",
                table: "Asignatura",
                column: "IdProfesor");

            migrationBuilder.CreateIndex(
                name: "IX_AsignaturaPersona_IdAlumno",
                table: "AsignaturaPersona",
                column: "IdAlumno");

            migrationBuilder.CreateIndex(
                name: "IX_AsignaturaPersona_IdAsignatura",
                table: "AsignaturaPersona",
                column: "IdAsignatura");

            migrationBuilder.CreateIndex(
                name: "IX_Horario_AsignaturaId",
                table: "Horario",
                column: "AsignaturaId");

            migrationBuilder.CreateIndex(
                name: "IX_Persona_AsignaturaId",
                table: "Persona",
                column: "AsignaturaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Asignatura_Persona_IdProfesor",
                table: "Asignatura",
                column: "IdProfesor",
                principalTable: "Persona",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Asignatura_Persona_IdProfesor",
                table: "Asignatura");

            migrationBuilder.DropTable(
                name: "AsignaturaPersona");

            migrationBuilder.DropTable(
                name: "Horario");

            migrationBuilder.DropTable(
                name: "Persona");

            migrationBuilder.DropTable(
                name: "Asignatura");
        }
    }
}
