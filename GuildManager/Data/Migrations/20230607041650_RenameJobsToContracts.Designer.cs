﻿// <auto-generated />
using System;
using GuildManager.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GuildManager.Data.Migrations
{
    [DbContext(typeof(GMContext))]
    [Migration("20230607041650_RenameJobsToContracts")]
    partial class RenameJobsToContracts
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("ContractMinion", b =>
                {
                    b.Property<Guid>("AssignedMinionsId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ContractsId")
                        .HasColumnType("TEXT");

                    b.HasKey("AssignedMinionsId", "ContractsId");

                    b.HasIndex("ContractsId");

                    b.ToTable("ContractMinion");
                });

            modelBuilder.Entity("GuildManager.Models.Contract", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("PatronId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PatronId");

                    b.ToTable("Contracts");
                });

            modelBuilder.Entity("GuildManager.Models.Minion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("BossId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BossId");

                    b.ToTable("Minions");
                });

            modelBuilder.Entity("GuildManager.Models.Player", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("GuildManager.Models.Privilege", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Code");

                    b.ToTable("Privileges");
                });

            modelBuilder.Entity("GuildManager.Models.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("PlayerRole", b =>
                {
                    b.Property<Guid>("PlayersId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("RolesId")
                        .HasColumnType("TEXT");

                    b.HasKey("PlayersId", "RolesId");

                    b.HasIndex("RolesId");

                    b.ToTable("PlayerRole");
                });

            modelBuilder.Entity("PrivilegeRole", b =>
                {
                    b.Property<string>("PrivilegesCode")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("RolesId")
                        .HasColumnType("TEXT");

                    b.HasKey("PrivilegesCode", "RolesId");

                    b.HasIndex("RolesId");

                    b.ToTable("PrivilegeRole");
                });

            modelBuilder.Entity("ContractMinion", b =>
                {
                    b.HasOne("GuildManager.Models.Minion", null)
                        .WithMany()
                        .HasForeignKey("AssignedMinionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GuildManager.Models.Contract", null)
                        .WithMany()
                        .HasForeignKey("ContractsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GuildManager.Models.Contract", b =>
                {
                    b.HasOne("GuildManager.Models.Player", "Patron")
                        .WithMany("Contracts")
                        .HasForeignKey("PatronId");

                    b.Navigation("Patron");
                });

            modelBuilder.Entity("GuildManager.Models.Minion", b =>
                {
                    b.HasOne("GuildManager.Models.Player", "Boss")
                        .WithMany("Minions")
                        .HasForeignKey("BossId");

                    b.Navigation("Boss");
                });

            modelBuilder.Entity("PlayerRole", b =>
                {
                    b.HasOne("GuildManager.Models.Player", null)
                        .WithMany()
                        .HasForeignKey("PlayersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GuildManager.Models.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PrivilegeRole", b =>
                {
                    b.HasOne("GuildManager.Models.Privilege", null)
                        .WithMany()
                        .HasForeignKey("PrivilegesCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GuildManager.Models.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GuildManager.Models.Player", b =>
                {
                    b.Navigation("Contracts");

                    b.Navigation("Minions");
                });
#pragma warning restore 612, 618
        }
    }
}
