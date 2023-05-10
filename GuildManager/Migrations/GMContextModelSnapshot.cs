﻿// <auto-generated />
using System;
using GuildManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GuildManager.Migrations
{
    [DbContext(typeof(GMContext))]
    partial class GMContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("GuildManager.Models.Assignment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("JobId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MinionId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("JobId");

                    b.HasIndex("MinionId");

                    b.ToTable("Assignments");
                });

            modelBuilder.Entity("GuildManager.Models.Job", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("GuildManager.Models.Minion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BossId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BossId");

                    b.ToTable("Minions");
                });

            modelBuilder.Entity("GuildManager.Models.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ApiKey")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("JobMinion", b =>
                {
                    b.Property<int>("AssignedMinionsId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("JobsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("AssignedMinionsId", "JobsId");

                    b.HasIndex("JobsId");

                    b.ToTable("JobMinion");
                });

            modelBuilder.Entity("GuildManager.Models.Assignment", b =>
                {
                    b.HasOne("GuildManager.Models.Job", "Job")
                        .WithMany()
                        .HasForeignKey("JobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GuildManager.Models.Minion", "Minion")
                        .WithMany()
                        .HasForeignKey("MinionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Job");

                    b.Navigation("Minion");
                });

            modelBuilder.Entity("GuildManager.Models.Minion", b =>
                {
                    b.HasOne("GuildManager.Models.Player", "Boss")
                        .WithMany("Minions")
                        .HasForeignKey("BossId");

                    b.Navigation("Boss");
                });

            modelBuilder.Entity("JobMinion", b =>
                {
                    b.HasOne("GuildManager.Models.Minion", null)
                        .WithMany()
                        .HasForeignKey("AssignedMinionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GuildManager.Models.Job", null)
                        .WithMany()
                        .HasForeignKey("JobsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GuildManager.Models.Player", b =>
                {
                    b.Navigation("Minions");
                });
#pragma warning restore 612, 618
        }
    }
}
