using System.Collections.Generic;
using Belote.Domain;
using Belote.Game.State;

namespace Belote.Player
{
    public abstract class NamedPlayer : IPlayer
    {
        public string? Name { get; protected set; }

        public virtual void BindStateView(IPlayerStateView state)
        {
            if (Name != null) return;
            var person = new[] {"Alice", "Bob", "Carol", "Dave"}[state.PlayerIndex];
            var team = new[] {"Red", "Blue", "Green"}[state.GameState.PlayerTeams[state.PlayerIndex]];
            Name = person + " (" + team + ")";
        }

        public abstract Contract? Bid();
        public abstract Card Play(List<Declaration> declarations);



        protected void Print(string message) => System.Console.Out.WriteLine("[" + Name + "] " + message);

        public override string? ToString() => Name ?? base.ToString();
    }
}