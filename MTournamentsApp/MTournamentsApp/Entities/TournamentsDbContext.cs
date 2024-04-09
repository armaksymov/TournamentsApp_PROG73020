using Microsoft.EntityFrameworkCore;

namespace MTournamentsApp.Entities
{
    public class TournamentsDbContext : DbContext
    {
        public TournamentsDbContext(DbContextOptions<TournamentsDbContext> options) : base(options) { }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerRole> PlayerRoles { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>()
               .HasOne(p => p.Role)
               .WithMany()
               .HasForeignKey(p => p.PlayerRoleId);

            modelBuilder.Entity<Team>()
                .HasOne(t => t.MainTeamGame)
                .WithMany()
                .HasForeignKey(t => t.MainTeamGameId);

            modelBuilder.Entity<Tournament>()
                .HasOne(t => t.TournamentGame)
                .WithMany()
                .HasForeignKey(t => t.TournamentGameId);

            modelBuilder.Entity<Tournament>()
                .HasOne(t => t.Address)
                .WithMany()
                .HasForeignKey(t => t.AddressID);

            modelBuilder.Entity<Tournament>()
                .HasMany(t => t.TournamentTeams)
                .WithMany(t => t.Tournaments);

            modelBuilder.Entity<Team>()
                .HasMany(t => t.Players)
                .WithOne(p => p.Team)
                .HasForeignKey(p => p.TeamId)
                .OnDelete(DeleteBehavior.Cascade); // Delete Players when a Team is deleted

            modelBuilder.Entity<Game>().HasData(
                new Game() { GameId = "val", GameName = "Valorant" },
                new Game() { GameId = "halInf", GameName = "Halo Infinite" }
            );

            modelBuilder.Entity<PlayerRole>().HasData(
                new PlayerRole() { PlayerRoleId = "C", PlayerRoleName = "Coach" },
                new PlayerRole() { PlayerRoleId = "L", PlayerRoleName = "Leader" },
                new PlayerRole() { PlayerRoleId = "P", PlayerRoleName = "Player" }
            );

            modelBuilder.Entity<Address>().HasData(
                new Address() { Id = 1, StreetAddress = "123 Random St S", TournamentCity = "Toronto", TournamentCountry = "Canada", TournamentPostalCode = "H0H 0H0" }
            );

            modelBuilder.Entity<Player>().HasData(
                new Player() { Id = 1, FirstName = "Test", LastName = "User #1", DateOfBirth = DateTime.Parse("01-01-2000"), PlayerRoleId = "C", TeamId = "ConCE" },
                new Player() { Id = 2, FirstName = "Test", LastName = "User #2", DateOfBirth = DateTime.Parse("01-01-2002"), PlayerRoleId = "P", TeamId = "ConCE" }
            );

            modelBuilder.Entity<Team>().HasData(
                new Team() { TeamId = "ConCE", TeamName = "Conestoga Condors E-Sports", TeamDescription = "Conestoga's Esports Team", MainTeamGameId = "val", PlayerIds = new List<int> { 1, 2 }, TournamentIds = new List<int> { 1 } }
            );

            modelBuilder.Entity<Tournament>().HasData(
                new Tournament() { Id = 1, TournamentName = "Conestoga College Home Tournament", TournamentDate = DateTime.Parse("01-01-2024"), AddressID = 1, TournamentGameId = "val", TeamIds = new List<string> { "ConCE" } }
            );
        }

    }
}
