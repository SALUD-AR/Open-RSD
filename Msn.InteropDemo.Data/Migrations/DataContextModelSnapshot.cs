﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Msn.InteropDemo.Data.Context;

namespace Msn.InteropDemo.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Msn.InteropDemo.Entities.Activity.ActivityLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ActivityRequest")
                        .HasMaxLength(1000)
                        .IsUnicode(false);

                    b.Property<string>("ActivityRequestBody");

                    b.Property<string>("ActivityRequestUI");

                    b.Property<string>("ActivityResponse")
                        .HasMaxLength(1000)
                        .IsUnicode(false);

                    b.Property<string>("ActivityResponseBody");

                    b.Property<int>("ActivityTypeDescriptorId");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("SmallDateTime");

                    b.Property<string>("CreatedUserId")
                        .IsRequired();

                    b.Property<bool>("Enabled");

                    b.Property<bool>("RequestIsJson");

                    b.Property<bool>("RequestIsURL");

                    b.Property<bool>("ResponseIsJson");

                    b.Property<bool>("ResponseIsURL");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("SessionUserId")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false);

                    b.Property<DateTime?>("UpdatedDateTime")
                        .HasColumnType("SmallDateTime");

                    b.Property<string>("UpdatedUserId");

                    b.HasKey("Id");

                    b.HasIndex("ActivityTypeDescriptorId");

                    b.HasIndex("CreatedDateTime");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("SessionUserId");

                    b.HasIndex("UpdatedUserId");

                    b.ToTable("ActivityLog");
                });

            modelBuilder.Entity("Msn.InteropDemo.Entities.Activity.ActivityTypeDescriptor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Enabled");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(200)
                        .IsUnicode(false);

                    b.Property<int?>("Orden");

                    b.HasKey("Id");

                    b.ToTable("ActivityTypeDescriptor");
                });

            modelBuilder.Entity("Msn.InteropDemo.Entities.Codificacion.Cie10", b =>
                {
                    b.Property<string>("SubcategoriaId")
                        .HasColumnType("char(4)")
                        .HasMaxLength(4)
                        .IsUnicode(false);

                    b.Property<string>("CategoriaNombre")
                        .IsRequired()
                        .HasMaxLength(300)
                        .IsUnicode(true);

                    b.Property<string>("SubcategoriaNombre")
                        .IsRequired()
                        .HasMaxLength(300)
                        .IsUnicode(true);

                    b.HasKey("SubcategoriaId");

                    b.ToTable("Cie10");
                });

            modelBuilder.Entity("Msn.InteropDemo.Entities.Evoluciones.Evolucion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("SmallDateTime");

                    b.Property<string>("CreatedUserId")
                        .IsRequired();

                    b.Property<bool>("Enabled");

                    b.Property<string>("Observacion")
                        .HasMaxLength(500)
                        .IsUnicode(true);

                    b.Property<int>("PacienteId");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<DateTime?>("UpdatedDateTime")
                        .HasColumnType("SmallDateTime");

                    b.Property<string>("UpdatedUserId");

                    b.HasKey("Id");

                    b.HasIndex("CreatedDateTime");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("PacienteId");

                    b.HasIndex("UpdatedUserId");

                    b.ToTable("Evolucion");
                });

            modelBuilder.Entity("Msn.InteropDemo.Entities.Evoluciones.EvolucionDiagnostico", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Cie10SubcategoriaId")
                        .HasColumnType("char(4)")
                        .HasMaxLength(4)
                        .IsUnicode(false);

                    b.Property<string>("Cie10SubcategoriaNombre")
                        .HasMaxLength(300)
                        .IsUnicode(true);

                    b.Property<int>("EvolucionId");

                    b.Property<decimal?>("SctConceptId")
                        .IsRequired()
                        .HasColumnType("numeric(18,0)");

                    b.Property<string>("SctDescriptionTerm")
                        .IsRequired()
                        .HasMaxLength(500)
                        .IsUnicode(true);

                    b.HasKey("Id");

                    b.HasIndex("Cie10SubcategoriaId");

                    b.HasIndex("EvolucionId");

                    b.HasIndex("SctConceptId");

                    b.ToTable("EvolucionDiagnostico");
                });

            modelBuilder.Entity("Msn.InteropDemo.Entities.Evoluciones.EvolucionMedicamento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("EvolucionId");

                    b.Property<decimal?>("SctConceptId")
                        .IsRequired()
                        .HasColumnType("numeric(18,0)");

                    b.Property<string>("SctDescriptionTerm")
                        .IsRequired()
                        .HasMaxLength(500)
                        .IsUnicode(true);

                    b.HasKey("Id");

                    b.HasIndex("EvolucionId");

                    b.HasIndex("SctConceptId");

                    b.ToTable("EvolucionMedicamento");
                });

            modelBuilder.Entity("Msn.InteropDemo.Entities.Evoluciones.EvolucionVacunaAplicacion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("EvolucionId");

                    b.Property<decimal?>("SctConceptId")
                        .IsRequired()
                        .HasColumnType("numeric(18,0)");

                    b.Property<string>("SctDescriptionTerm")
                        .IsRequired()
                        .HasMaxLength(500)
                        .IsUnicode(true);

                    b.HasKey("Id");

                    b.HasIndex("EvolucionId");

                    b.HasIndex("SctConceptId");

                    b.ToTable("EvolucionVacunaAplicacion");
                });

            modelBuilder.Entity("Msn.InteropDemo.Entities.Identity.SystemUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(true);

                    b.Property<long?>("CUIT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(true);

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Msn.InteropDemo.Entities.Pacientes.Paciente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("SmallDateTime");

                    b.Property<string>("CreatedUserId")
                        .IsRequired();

                    b.Property<string>("DomicilioCalle");

                    b.Property<string>("DomicilioCalleAltura");

                    b.Property<string>("DomicilioCodPostal");

                    b.Property<string>("DomicilioDepto");

                    b.Property<string>("DomicilioPiso");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<bool>("Enabled");

                    b.Property<DateTime>("FechaNacimiento")
                        .HasColumnType("Date");

                    b.Property<DateTime?>("FederadoDateTime")
                        .HasColumnType("SmallDateTime");

                    b.Property<int?>("FederadorId");

                    b.Property<int>("NroDocumento");

                    b.Property<string>("OtrosApellidos")
                        .HasMaxLength(50);

                    b.Property<string>("OtrosNombres")
                        .HasMaxLength(50);

                    b.Property<string>("PrimerApellido")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("PrimerNombre")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("Sexo")
                        .IsRequired()
                        .HasColumnType("char(1)");

                    b.Property<int>("TipoDocumentoId");

                    b.Property<DateTime?>("UpdatedDateTime")
                        .HasColumnType("SmallDateTime");

                    b.Property<string>("UpdatedUserId");

                    b.HasKey("Id");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("TipoDocumentoId");

                    b.HasIndex("UpdatedUserId");

                    b.ToTable("Paciente");
                });

            modelBuilder.Entity("Msn.InteropDemo.Entities.Pacientes.TipoDocumento", b =>
                {
                    b.Property<int>("Id");

                    b.Property<bool>("Enabled");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50);

                    b.Property<int?>("Orden");

                    b.HasKey("Id");

                    b.ToTable("TipoDocumento");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Msn.InteropDemo.Entities.Identity.SystemUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Msn.InteropDemo.Entities.Identity.SystemUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Msn.InteropDemo.Entities.Identity.SystemUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Msn.InteropDemo.Entities.Identity.SystemUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Msn.InteropDemo.Entities.Activity.ActivityLog", b =>
                {
                    b.HasOne("Msn.InteropDemo.Entities.Activity.ActivityTypeDescriptor", "ActivityTypeDescriptor")
                        .WithMany()
                        .HasForeignKey("ActivityTypeDescriptorId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Msn.InteropDemo.Entities.Identity.SystemUser", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Msn.InteropDemo.Entities.Identity.SystemUser", "UpdatedUser")
                        .WithMany()
                        .HasForeignKey("UpdatedUserId");
                });

            modelBuilder.Entity("Msn.InteropDemo.Entities.Evoluciones.Evolucion", b =>
                {
                    b.HasOne("Msn.InteropDemo.Entities.Identity.SystemUser", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Msn.InteropDemo.Entities.Pacientes.Paciente", "Paciente")
                        .WithMany()
                        .HasForeignKey("PacienteId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Msn.InteropDemo.Entities.Identity.SystemUser", "UpdatedUser")
                        .WithMany()
                        .HasForeignKey("UpdatedUserId");
                });

            modelBuilder.Entity("Msn.InteropDemo.Entities.Evoluciones.EvolucionDiagnostico", b =>
                {
                    b.HasOne("Msn.InteropDemo.Entities.Evoluciones.Evolucion", "Evolucion")
                        .WithMany("Diagnosticos")
                        .HasForeignKey("EvolucionId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Msn.InteropDemo.Entities.Evoluciones.EvolucionMedicamento", b =>
                {
                    b.HasOne("Msn.InteropDemo.Entities.Evoluciones.Evolucion", "Evolucion")
                        .WithMany("Medicamentos")
                        .HasForeignKey("EvolucionId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Msn.InteropDemo.Entities.Evoluciones.EvolucionVacunaAplicacion", b =>
                {
                    b.HasOne("Msn.InteropDemo.Entities.Evoluciones.Evolucion", "Evolucion")
                        .WithMany("Vacunas")
                        .HasForeignKey("EvolucionId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Msn.InteropDemo.Entities.Pacientes.Paciente", b =>
                {
                    b.HasOne("Msn.InteropDemo.Entities.Identity.SystemUser", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Msn.InteropDemo.Entities.Pacientes.TipoDocumento", "TipoDocumento")
                        .WithMany()
                        .HasForeignKey("TipoDocumentoId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Msn.InteropDemo.Entities.Identity.SystemUser", "UpdatedUser")
                        .WithMany()
                        .HasForeignKey("UpdatedUserId");
                });
#pragma warning restore 612, 618
        }
    }
}
