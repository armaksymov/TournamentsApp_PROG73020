﻿using MTournamentsApp.Entities;

namespace MTournamentsApp.Services
{
    public interface IMail
    {
        bool SendInvite(Tournament tournament, Invitation recipient);

        bool SendCancellation(Tournament tournament, Player recipient);
    }
}
