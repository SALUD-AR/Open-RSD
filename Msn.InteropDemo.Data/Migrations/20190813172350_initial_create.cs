using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Msn.InteropDemo.Data.Migrations
{
    public partial class initial_create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityTypeDescriptor",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Enabled = table.Column<bool>(nullable: false),
                    Nombre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 200, nullable: false),
                    Orden = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityTypeDescriptor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoDocumento",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false),
                    Nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Orden = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoDocumento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Enabled = table.Column<bool>(nullable: false),
                    CreatedUserId = table.Column<string>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "SmallDateTime", nullable: false),
                    UpdatedUserId = table.Column<string>(nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(type: "SmallDateTime", nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    SessionUserId = table.Column<string>(unicode: false, maxLength: 200, nullable: false),
                    ActivityTypeDescriptorId = table.Column<int>(nullable: false),
                    ActivityRequest = table.Column<string>(unicode: false, maxLength: 1000, nullable: true),
                    ActivityResponse = table.Column<string>(unicode: false, maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityLog_ActivityTypeDescriptor_ActivityTypeDescriptorId",
                        column: x => x.ActivityTypeDescriptorId,
                        principalTable: "ActivityTypeDescriptor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActivityLog_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActivityLog_AspNetUsers_UpdatedUserId",
                        column: x => x.UpdatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Paciente",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Enabled = table.Column<bool>(nullable: false),
                    CreatedUserId = table.Column<string>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "SmallDateTime", nullable: false),
                    UpdatedUserId = table.Column<string>(nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(type: "SmallDateTime", nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    PrimerNombre = table.Column<string>(maxLength: 50, nullable: false),
                    OtrosNombres = table.Column<string>(maxLength: 50, nullable: true),
                    PrimerApellido = table.Column<string>(maxLength: 50, nullable: false),
                    OtrosApellidos = table.Column<string>(maxLength: 50, nullable: true),
                    TipoDocumentoId = table.Column<int>(nullable: false),
                    NroDocumento = table.Column<int>(nullable: false),
                    Sexo = table.Column<string>(type: "char(1)", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "Date", nullable: false),
                    DomicilioCalle = table.Column<string>(nullable: true),
                    DomicilioCalleAltura = table.Column<string>(nullable: true),
                    DomicilioPiso = table.Column<string>(nullable: true),
                    DomicilioDepto = table.Column<string>(nullable: true),
                    DomicilioCodPostal = table.Column<string>(nullable: true),
                    FederadorId = table.Column<int>(nullable: true),
                    FederadoDateTime = table.Column<DateTime>(type: "SmallDateTime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paciente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Paciente_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Paciente_TipoDocumento_TipoDocumentoId",
                        column: x => x.TipoDocumentoId,
                        principalTable: "TipoDocumento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Paciente_AspNetUsers_UpdatedUserId",
                        column: x => x.UpdatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Evolucion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Enabled = table.Column<bool>(nullable: false),
                    CreatedUserId = table.Column<string>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "SmallDateTime", nullable: false),
                    UpdatedUserId = table.Column<string>(nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(type: "SmallDateTime", nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    PacienteId = table.Column<int>(nullable: false),
                    Observacion = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evolucion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Evolucion_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Evolucion_Paciente_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Paciente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Evolucion_AspNetUsers_UpdatedUserId",
                        column: x => x.UpdatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EvolucionDiagnostico",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EvolucionId = table.Column<int>(nullable: false),
                    SctConceptId = table.Column<decimal>(type: "numeric(18,0)", nullable: false),
                    SctDescriptionTerm = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvolucionDiagnostico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvolucionDiagnostico_Evolucion_EvolucionId",
                        column: x => x.EvolucionId,
                        principalTable: "Evolucion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EvolucionMedicamento",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EvolucionId = table.Column<int>(nullable: false),
                    SctConceptId = table.Column<decimal>(type: "numeric(18,0)", nullable: false),
                    SctDescriptionTerm = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvolucionMedicamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvolucionMedicamento_Evolucion_EvolucionId",
                        column: x => x.EvolucionId,
                        principalTable: "Evolucion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EvolucionVacunaAplicacion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EvolucionId = table.Column<int>(nullable: false),
                    SctConceptId = table.Column<decimal>(type: "numeric(18,0)", nullable: false),
                    SctDescriptionTerm = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvolucionVacunaAplicacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvolucionVacunaAplicacion_Evolucion_EvolucionId",
                        column: x => x.EvolucionId,
                        principalTable: "Evolucion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLog_ActivityTypeDescriptorId",
                table: "ActivityLog",
                column: "ActivityTypeDescriptorId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLog_CreatedDateTime",
                table: "ActivityLog",
                column: "CreatedDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLog_CreatedUserId",
                table: "ActivityLog",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLog_SessionUserId",
                table: "ActivityLog",
                column: "SessionUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLog_UpdatedUserId",
                table: "ActivityLog",
                column: "UpdatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Evolucion_CreatedDateTime",
                table: "Evolucion",
                column: "CreatedDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Evolucion_CreatedUserId",
                table: "Evolucion",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Evolucion_PacienteId",
                table: "Evolucion",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Evolucion_UpdatedUserId",
                table: "Evolucion",
                column: "UpdatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EvolucionDiagnostico_EvolucionId",
                table: "EvolucionDiagnostico",
                column: "EvolucionId");

            migrationBuilder.CreateIndex(
                name: "IX_EvolucionDiagnostico_SctConceptId",
                table: "EvolucionDiagnostico",
                column: "SctConceptId");

            migrationBuilder.CreateIndex(
                name: "IX_EvolucionMedicamento_EvolucionId",
                table: "EvolucionMedicamento",
                column: "EvolucionId");

            migrationBuilder.CreateIndex(
                name: "IX_EvolucionMedicamento_SctConceptId",
                table: "EvolucionMedicamento",
                column: "SctConceptId");

            migrationBuilder.CreateIndex(
                name: "IX_EvolucionVacunaAplicacion_EvolucionId",
                table: "EvolucionVacunaAplicacion",
                column: "EvolucionId");

            migrationBuilder.CreateIndex(
                name: "IX_EvolucionVacunaAplicacion_SctConceptId",
                table: "EvolucionVacunaAplicacion",
                column: "SctConceptId");

            migrationBuilder.CreateIndex(
                name: "IX_Paciente_CreatedUserId",
                table: "Paciente",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Paciente_TipoDocumentoId",
                table: "Paciente",
                column: "TipoDocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Paciente_UpdatedUserId",
                table: "Paciente",
                column: "UpdatedUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityLog");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "EvolucionDiagnostico");

            migrationBuilder.DropTable(
                name: "EvolucionMedicamento");

            migrationBuilder.DropTable(
                name: "EvolucionVacunaAplicacion");

            migrationBuilder.DropTable(
                name: "ActivityTypeDescriptor");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Evolucion");

            migrationBuilder.DropTable(
                name: "Paciente");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "TipoDocumento");
        }
    }
}
