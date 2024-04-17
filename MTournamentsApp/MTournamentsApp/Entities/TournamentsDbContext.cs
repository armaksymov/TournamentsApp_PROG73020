using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MTournamentsApp.Models;

namespace MTournamentsApp.Entities
{
    public class TournamentsDbContext : IdentityDbContext<User>
    {
        public TournamentsDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerRole> PlayerRoles { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Invitation> Invations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
                .HasForeignKey(p => p.TeamId);

            modelBuilder.Entity<Invitation>()
                .HasOne(i => i.Player)
                .WithMany(p => p.Invitations)
                .HasForeignKey(i => i.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);


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
                new Player() { Id = 1, FirstName = "Test", LastName = "User #1", Email = "", DateOfBirth = DateTime.Parse("01-01-2000"), PlayerRoleId = "C", TeamId = "ConCE" },
                new Player() { Id = 2, FirstName = "Test", LastName = "User #2", Email = "", DateOfBirth = DateTime.Parse("01-01-2002"), PlayerRoleId = "P", TeamId = "ConCE" }
            );

            modelBuilder.Entity<Team>().HasData(
                new Team() { TeamId = "ConCE", TeamName = "Conestoga Condors E-Sports", TeamDescription = "Conestoga's Esports Team", MainTeamGameId = "val", PlayerIds = new List<int> { 1, 2 }, TournamentIds = new List<int> { 1 } }
            );

            modelBuilder.Entity<Tournament>().HasData(
                new Tournament() { Id = 1, TournamentName = "Conestoga College Home Tournament", TournamentDate = DateTime.Parse("01-01-2024"), AddressID = 1, TournamentGameId = "val", TeamIds = new List<string> { "ConCE" } }
            );
        }

        public static async Task CreateAdminUser(IServiceProvider serviceProvider)
        {
            UserManager<User> userManager =
                serviceProvider.GetRequiredService<UserManager<User>>();
            RoleManager<IdentityRole> roleManager = serviceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            string username = "admin";
            string password = "Mtz_123";
            string roleName = "Admin";

            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            if (await userManager.FindByNameAsync(username) == null)
            {
                User user = new User { UserName = username };
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }
    }
}
