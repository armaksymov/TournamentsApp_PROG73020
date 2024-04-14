using MTournamentsApp.Entities;

namespace MTournamentsApp.Services
{
    public interface IMail
    {
        bool SendInvite(Tournament tournament, Player player);
    }
}
