﻿// <auto-generated />
using System;
using MTournamentsApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MTournamentsApp.Migrations
{
    [DbContext(typeof(TournamentsDbContext))]
    partial class TournamentsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MTournamentsApp.Entities.Game", b =>
                {
                    b.Property<string>("GameId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("GameName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("GameId");

                    b.ToTable("Games");

                    b.HasData(
                        new
                        {
                            GameId = "val",
                            GameName = "Valorant"
                        },
                        new
                        {
                            GameId = "halInf",
                            GameName = "Halo Infinite"
                        });
                });

            modelBuilder.Entity("MTournamentsApp.Entities.Invitation", b =>
                {
                    b.Property<int>("InvitationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InvitationId"));

                    b.Property<string>("PlayerEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.Property<string>("PlayerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("InvitationId");

                    b.HasIndex("PlayerId");

                    b.ToTable("Invations");
                });

            modelBuilder.Entity("MTournamentsApp.Entities.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PlayerRoleId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TeamId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("PlayerRoleId");

                    b.HasIndex("TeamId");

                    b.ToTable("Players");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DateOfBirth = new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "",
                            FirstName = "Test",
                            LastName = "User #1",
                            PlayerRoleId = "C",
                            TeamId = "ConCE"
                        },
                        new
                        {
                            Id = 2,
                            DateOfBirth = new DateTime(2002, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "",
                            FirstName = "Test",
                            LastName = "User #2",
                            PlayerRoleId = "P",
                            TeamId = "ConCE"
                        });
                });

            modelBuilder.Entity("MTournamentsApp.Entities.PlayerRole", b =>
                {
                    b.Property<string>("PlayerRoleId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PlayerRoleName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PlayerRoleId");

                    b.ToTable("PlayerRoles");

                    b.HasData(
                        new
                        {
                            PlayerRoleId = "C",
                            PlayerRoleName = "Coach"
                        },
                        new
                        {
                            PlayerRoleId = "L",
                            PlayerRoleName = "Leader"
                        },
                        new
                        {
                            PlayerRoleId = "P",
                            PlayerRoleName = "Player"
                        });
                });

            modelBuilder.Entity("MTournamentsApp.Entities.Team", b =>
                {
                    b.Property<string>("TeamId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MainTeamGameId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PlayerIds")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TeamDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TeamName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TournamentIds")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TeamId");

                    b.HasIndex("MainTeamGameId");

                    b.ToTable("Teams");

                    b.HasData(
                        new
                        {
                            TeamId = "ConCE",
                            MainTeamGameId = "val",
                            PlayerIds = "[1,2]",
                            TeamDescription = "Conestoga's Esports Team",
                            TeamName = "Conestoga Condors E-Sports",
                            TournamentIds = "[1]"
                        });
                });

            modelBuilder.Entity("MTournamentsApp.Entities.Tournament", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TeamIds")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("TournamentDate")
                        .IsRequired()
                        .HasColumnType("datetime");

                    b.Property<string>("TournamentGameId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TournamentName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("TournamentGameId");

                    b.ToTable("Tournaments");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "",
                            TeamIds = "[\"ConCE\"]",
                            TournamentDate = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TournamentGameId = "val",
                            TournamentName = "Conestoga College Home Tournament"
                        });
                });

            modelBuilder.Entity("TeamTournament", b =>
                {
                    b.Property<string>("TournamentTeamsTeamId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("TournamentsId")
                        .HasColumnType("int");

                    b.HasKey("TournamentTeamsTeamId", "TournamentsId");

                    b.HasIndex("TournamentsId");

                    b.ToTable("TeamTournament");
                });

            modelBuilder.Entity("MTournamentsApp.Entities.Invitation", b =>
                {
                    b.HasOne("MTournamentsApp.Entities.Player", "Player")
                        .WithMany("Invitations")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");
                });

            modelBuilder.Entity("MTournamentsApp.Entities.Player", b =>
                {
                    b.HasOne("MTournamentsApp.Entities.PlayerRole", "Role")
                        .WithMany()
                        .HasForeignKey("PlayerRoleId");

                    b.HasOne("MTournamentsApp.Entities.Team", "Team")
                        .WithMany("Players")
                        .HasForeignKey("TeamId");

                    b.Navigation("Role");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("MTournamentsApp.Entities.Team", b =>
                {
                    b.HasOne("MTournamentsApp.Entities.Game", "MainTeamGame")
                        .WithMany()
                        .HasForeignKey("MainTeamGameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MainTeamGame");
                });

            modelBuilder.Entity("MTournamentsApp.Entities.Tournament", b =>
                {
                    b.HasOne("MTournamentsApp.Entities.Game", "TournamentGame")
                        .WithMany()
                        .HasForeignKey("TournamentGameId");

                    b.Navigation("TournamentGame");
                });

            modelBuilder.Entity("TeamTournament", b =>
                {
                    b.HasOne("MTournamentsApp.Entities.Team", null)
                        .WithMany()
                        .HasForeignKey("TournamentTeamsTeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MTournamentsApp.Entities.Tournament", null)
                        .WithMany()
                        .HasForeignKey("TournamentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MTournamentsApp.Entities.Player", b =>
                {
                    b.Navigation("Invitations");
                });

            modelBuilder.Entity("MTournamentsApp.Entities.Team", b =>
                {
                    b.Navigation("Players");
                });
#pragma warning restore 612, 618
        }
    }
}
