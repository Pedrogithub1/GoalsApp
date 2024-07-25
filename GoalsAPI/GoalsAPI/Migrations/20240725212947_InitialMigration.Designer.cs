﻿// <auto-generated />
using System;
using GoalsAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GoalsAPI.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240725212947_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GoalsAPI.Data.Entities.Goal", b =>
                {
                    b.Property<int>("GoalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GoalId"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<int>("TotalTasks")
                        .HasColumnType("int");

                    b.HasKey("GoalId");

                    b.ToTable("Goals");

                    b.HasData(
                        new
                        {
                            GoalId = 1,
                            Date = new DateTime(2024, 7, 25, 0, 0, 0, 0, DateTimeKind.Local),
                            Name = "Configurar plan de compensación",
                            TotalTasks = 2
                        },
                        new
                        {
                            GoalId = 2,
                            Date = new DateTime(2024, 7, 25, 0, 0, 0, 0, DateTimeKind.Local),
                            Name = "Meta 2",
                            TotalTasks = 0
                        });
                });

            modelBuilder.Entity("GoalsAPI.Data.Entities.TaskItem", b =>
                {
                    b.Property<int>("TaskItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TaskItemId"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("GoalId")
                        .HasColumnType("int");

                    b.Property<int?>("GoalId1")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TaskItemId");

                    b.HasIndex("GoalId");

                    b.HasIndex("GoalId1");

                    b.ToTable("TaskItems");

                    b.HasData(
                        new
                        {
                            TaskItemId = 1,
                            Date = new DateTime(2024, 7, 25, 0, 0, 0, 0, DateTimeKind.Local),
                            GoalId = 1,
                            Name = "Tarea 1",
                            Status = "Completada"
                        },
                        new
                        {
                            TaskItemId = 2,
                            Date = new DateTime(2024, 7, 25, 0, 0, 0, 0, DateTimeKind.Local),
                            GoalId = 1,
                            Name = "Tarea 2",
                            Status = "Abierta"
                        });
                });

            modelBuilder.Entity("GoalsAPI.Data.Entities.TaskItem", b =>
                {
                    b.HasOne("GoalsAPI.Data.Entities.Goal", null)
                        .WithMany("TaskItems")
                        .HasForeignKey("GoalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GoalsAPI.Data.Entities.Goal", "Goal")
                        .WithMany()
                        .HasForeignKey("GoalId1");

                    b.Navigation("Goal");
                });

            modelBuilder.Entity("GoalsAPI.Data.Entities.Goal", b =>
                {
                    b.Navigation("TaskItems");
                });
#pragma warning restore 612, 618
        }
    }
}
