﻿// <auto-generated />
using System;
using LogicSchemeManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LogicSchemeManager.Migrations
{
    [DbContext(typeof(LogicSchemeManagerContext))]
    partial class LogicSchemeManagerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LogicSchemeManager.Models.Combination", b =>
                {
                    b.Property<int>("CombinationId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ElementTypeId");

                    b.Property<string>("Name");

                    b.HasKey("CombinationId");

                    b.HasIndex("ElementTypeId");

                    b.ToTable("Combinations");
                });

            modelBuilder.Entity("LogicSchemeManager.Models.CombinationPort", b =>
                {
                    b.Property<int>("CombinationPortId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CombinationId");

                    b.Property<int>("PortId");

                    b.Property<bool>("Value");

                    b.HasKey("CombinationPortId");

                    b.HasIndex("CombinationId");

                    b.HasIndex("PortId");

                    b.ToTable("CombinationPorts");
                });

            modelBuilder.Entity("LogicSchemeManager.Models.Element", b =>
                {
                    b.Property<int>("ElementId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ElementTypeId");

                    b.Property<string>("Name");

                    b.Property<int>("SchemaId");

                    b.Property<double?>("x");

                    b.Property<double?>("y");

                    b.HasKey("ElementId");

                    b.HasIndex("ElementTypeId");

                    b.HasIndex("SchemaId");

                    b.ToTable("Elements");
                });

            modelBuilder.Entity("LogicSchemeManager.Models.ElementPort", b =>
                {
                    b.Property<int>("ElementPortId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ElementId");

                    b.Property<string>("Name");

                    b.Property<int?>("ParentId");

                    b.Property<int>("PortId");

                    b.Property<double?>("x");

                    b.Property<double?>("y");

                    b.HasKey("ElementPortId");

                    b.HasIndex("ElementId");

                    b.HasIndex("ParentId");

                    b.HasIndex("PortId");

                    b.ToTable("ElementPorts");
                });

            modelBuilder.Entity("LogicSchemeManager.Models.ElementType", b =>
                {
                    b.Property<int>("ElementTypeId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("ElementTypeId");

                    b.ToTable("ElementTypes");
                });

            modelBuilder.Entity("LogicSchemeManager.Models.Port", b =>
                {
                    b.Property<int>("PortId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsOutput");

                    b.Property<string>("Name");

                    b.HasKey("PortId");

                    b.ToTable("Ports");
                });

            modelBuilder.Entity("LogicSchemeManager.Models.Schema", b =>
                {
                    b.Property<int>("SchemaId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("SchemaId");

                    b.ToTable("Scheme");
                });

            modelBuilder.Entity("LogicSchemeManager.Models.Combination", b =>
                {
                    b.HasOne("LogicSchemeManager.Models.ElementType", "ElementType")
                        .WithMany("Combinations")
                        .HasForeignKey("ElementTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LogicSchemeManager.Models.CombinationPort", b =>
                {
                    b.HasOne("LogicSchemeManager.Models.Combination", "Combination")
                        .WithMany("CombinationPorts")
                        .HasForeignKey("CombinationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LogicSchemeManager.Models.Port", "Port")
                        .WithMany("CombinationPorts")
                        .HasForeignKey("PortId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LogicSchemeManager.Models.Element", b =>
                {
                    b.HasOne("LogicSchemeManager.Models.ElementType", "ElementType")
                        .WithMany("Elements")
                        .HasForeignKey("ElementTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LogicSchemeManager.Models.Schema", "Schema")
                        .WithMany("Elements")
                        .HasForeignKey("SchemaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LogicSchemeManager.Models.ElementPort", b =>
                {
                    b.HasOne("LogicSchemeManager.Models.Element", "Element")
                        .WithMany("ElementPorts")
                        .HasForeignKey("ElementId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LogicSchemeManager.Models.ElementPort", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");

                    b.HasOne("LogicSchemeManager.Models.Port", "Port")
                        .WithMany("ElementPorts")
                        .HasForeignKey("PortId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
