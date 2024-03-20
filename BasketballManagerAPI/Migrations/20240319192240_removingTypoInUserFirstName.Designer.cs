﻿// <auto-generated />
using System;
using BasketballManagerAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BasketballManagerAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240319192240_removingTypoInUserFirstName")]
    partial class removingTypoInUserFirstName
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BasketballManagerAPI.Models.Award", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<bool>("IsIndividualAward")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ModifiedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("ModifiedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PhotoPath")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Name", "Date")
                        .IsUnique();

                    b.ToTable("Awards");
                });

            modelBuilder.Entity("BasketballManagerAPI.Models.Coach", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<int>("CoachStatus")
                        .HasColumnType("int");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateOnly>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ModifiedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("ModifiedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("PhotoPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Specialty")
                        .HasColumnType("int");

                    b.Property<Guid?>("TeamId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.ToTable("Coaches");
                });

            modelBuilder.Entity("BasketballManagerAPI.Models.CoachAward", b =>
                {
                    b.Property<Guid>("AwardId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CoachExperienceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AwardId", "CoachExperienceId");

                    b.HasIndex("CoachExperienceId");

                    b.ToTable("CoachAwards");
                });

            modelBuilder.Entity("BasketballManagerAPI.Models.CoachExperience", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<Guid>("CoachId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateOnly?>("EndDate")
                        .HasColumnType("date");

                    b.Property<Guid?>("ModifiedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("ModifiedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("date");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("TeamId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CoachId");

                    b.HasIndex("TeamId");

                    b.ToTable("CoachExperiences");
                });

            modelBuilder.Entity("BasketballManagerAPI.Models.Match", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<Guid>("AwayTeamId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("AwayTeamScore")
                        .HasColumnType("int");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("HomeTeamId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("HomeTeamScore")
                        .HasColumnType("int");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ModifiedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("ModifiedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AwayTeamId");

                    b.HasIndex("HomeTeamId");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("BasketballManagerAPI.Models.Player", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateOnly>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Height")
                        .HasColumnType("decimal(3,1)");

                    b.Property<int>("JerseyNumber")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ModifiedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("ModifiedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("PhotoPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Position")
                        .HasColumnType("int");

                    b.Property<Guid?>("TeamId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Weight")
                        .HasColumnType("decimal(3,1)");

                    b.HasKey("Id");

                    b.HasIndex("TeamId", "JerseyNumber")
                        .IsUnique()
                        .HasFilter("[TeamId] IS NOT NULL");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("BasketballManagerAPI.Models.PlayerAward", b =>
                {
                    b.Property<Guid>("AwardId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PlayerExperienceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AwardId", "PlayerExperienceId");

                    b.HasIndex("PlayerExperienceId");

                    b.ToTable("PlayerAwards");
                });

            modelBuilder.Entity("BasketballManagerAPI.Models.PlayerExperience", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateOnly?>("EndDate")
                        .HasColumnType("date");

                    b.Property<Guid?>("ModifiedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("ModifiedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("PlayerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("date");

                    b.Property<Guid>("TeamId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.HasIndex("TeamId");

                    b.ToTable("PlayerExperiences");
                });

            modelBuilder.Entity("BasketballManagerAPI.Models.Statistic", b =>
                {
                    b.Property<Guid>("MatchId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PlayerExperienceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("TimeUnit")
                        .HasColumnType("int");

                    b.Property<int>("AssistCount")
                        .HasColumnType("int");

                    b.Property<int>("BlockCount")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("CourtTime")
                        .HasColumnType("time");

                    b.Property<int>("DefensiveReboundCount")
                        .HasColumnType("int");

                    b.Property<int>("OffensiveReboundCount")
                        .HasColumnType("int");

                    b.Property<int>("OnePointShotHitCount")
                        .HasColumnType("int");

                    b.Property<int>("OnePointShotMissCount")
                        .HasColumnType("int");

                    b.Property<int>("StealCount")
                        .HasColumnType("int");

                    b.Property<int>("ThreePointShotHitCount")
                        .HasColumnType("int");

                    b.Property<int>("ThreePointShotMissCount")
                        .HasColumnType("int");

                    b.Property<int>("TurnoverCount")
                        .HasColumnType("int");

                    b.Property<int>("TwoPointShotHitCount")
                        .HasColumnType("int");

                    b.Property<int>("TwoPointShotMissCount")
                        .HasColumnType("int");

                    b.HasKey("MatchId", "PlayerExperienceId", "TimeUnit");

                    b.HasIndex("PlayerExperienceId");

                    b.ToTable("Statistics");
                });

            modelBuilder.Entity("BasketballManagerAPI.Models.Team", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("ModifiedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("ModifiedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PhotoPath")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("BasketballManagerAPI.Models.Ticket", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("MatchId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ModifiedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("ModifiedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(10,2)");

                    b.Property<int>("Row")
                        .HasColumnType("int");

                    b.Property<int>("Seat")
                        .HasColumnType("int");

                    b.Property<string>("Section")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TransactionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("MatchId");

                    b.HasIndex("TransactionId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("BasketballManagerAPI.Models.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("ModifiedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("ModifiedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(10,2)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("BasketballManagerAPI.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(10,2)");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ModifiedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("ModifiedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("PhotoPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BasketballManagerAPI.Models.Coach", b =>
                {
                    b.HasOne("BasketballManagerAPI.Models.Team", "Team")
                        .WithMany("Coaches")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Team");
                });

            modelBuilder.Entity("BasketballManagerAPI.Models.CoachAward", b =>
                {
                    b.HasOne("BasketballManagerAPI.Models.Award", "Award")
                        .WithMany("CoachAwards")
                        .HasForeignKey("AwardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BasketballManagerAPI.Models.CoachExperience", "CoachExperience")
                        .WithMany("CoachAwards")
                        .HasForeignKey("CoachExperienceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Award");

                    b.Navigation("CoachExperience");
                });

            modelBuilder.Entity("BasketballManagerAPI.Models.CoachExperience", b =>
                {
                    b.HasOne("BasketballManagerAPI.Models.Coach", "Coach")
                        .WithMany("CoachExperiences")
                        .HasForeignKey("CoachId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BasketballManagerAPI.Models.Team", "Team")
                        .WithMany("CoachExperiences")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Coach");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("BasketballManagerAPI.Models.Match", b =>
                {
                    b.HasOne("BasketballManagerAPI.Models.Team", "AwayTeam")
                        .WithMany()
                        .HasForeignKey("AwayTeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BasketballManagerAPI.Models.Team", "HomeTeam")
                        .WithMany()
                        .HasForeignKey("HomeTeamId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AwayTeam");

                    b.Navigation("HomeTeam");
                });

            modelBuilder.Entity("BasketballManagerAPI.Models.Player", b =>
                {
                    b.HasOne("BasketballManagerAPI.Models.Team", "Team")
                        .WithMany("Players")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Team");
                });

            modelBuilder.Entity("BasketballManagerAPI.Models.PlayerAward", b =>
                {
                    b.HasOne("BasketballManagerAPI.Models.Award", "Award")
                        .WithMany("PlayerAwards")
                        .HasForeignKey("AwardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BasketballManagerAPI.Models.PlayerExperience", "PlayerExperience")
                        .WithMany("PlayerAwards")
                        .HasForeignKey("PlayerExperienceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Award");

                    b.Navigation("PlayerExperience");
                });

            modelBuilder.Entity("BasketballManagerAPI.Models.PlayerExperience", b =>
                {
                    b.HasOne("BasketballManagerAPI.Models.Player", "Player")
                        .WithMany("PlayerExperiences")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BasketballManagerAPI.Models.Team", "Team")
                        .WithMany("PlayerExperiences")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("BasketballManagerAPI.Models.Statistic", b =>
                {
                    b.HasOne("BasketballManagerAPI.Models.Match", "Match")
                        .WithMany("Statistics")
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BasketballManagerAPI.Models.PlayerExperience", "PlayerExperience")
                        .WithMany("Statistics")
                        .HasForeignKey("PlayerExperienceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Match");

                    b.Navigation("PlayerExperience");
                });

            modelBuilder.Entity("BasketballManagerAPI.Models.Ticket", b =>
                {
                    b.HasOne("BasketballManagerAPI.Models.Match", "Match")
                        .WithMany("Tickets")
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BasketballManagerAPI.Models.Transaction", "Transaction")
                        .WithMany("Tickets")
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Match");

                    b.Navigation("Transaction");
                });

            modelBuilder.Entity("BasketballManagerAPI.Models.Transaction", b =>
                {
                    b.HasOne("BasketballManagerAPI.Models.User", "User")
                        .WithMany("Transactions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BasketballManagerAPI.Models.Award", b =>
                {
                    b.Navigation("CoachAwards");

                    b.Navigation("PlayerAwards");
                });

            modelBuilder.Entity("BasketballManagerAPI.Models.Coach", b =>
                {
                    b.Navigation("CoachExperiences");
                });

            modelBuilder.Entity("BasketballManagerAPI.Models.CoachExperience", b =>
                {
                    b.Navigation("CoachAwards");
                });

            modelBuilder.Entity("BasketballManagerAPI.Models.Match", b =>
                {
                    b.Navigation("Statistics");

                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("BasketballManagerAPI.Models.Player", b =>
                {
                    b.Navigation("PlayerExperiences");
                });

            modelBuilder.Entity("BasketballManagerAPI.Models.PlayerExperience", b =>
                {
                    b.Navigation("PlayerAwards");

                    b.Navigation("Statistics");
                });

            modelBuilder.Entity("BasketballManagerAPI.Models.Team", b =>
                {
                    b.Navigation("CoachExperiences");

                    b.Navigation("Coaches");

                    b.Navigation("PlayerExperiences");

                    b.Navigation("Players");
                });

            modelBuilder.Entity("BasketballManagerAPI.Models.Transaction", b =>
                {
                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("BasketballManagerAPI.Models.User", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
