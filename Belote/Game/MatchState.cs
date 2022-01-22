﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Belote.Domain;
using Belote.Game.State;

namespace Belote.Game
{

    public partial class Game {

        // State implementations //

        private class MatchState : IMatchState
        {
            public MatchState(int playerCount)
            {
                PlayerCards = new List<IList<Card>>();
                Contract = null;
                Declarations = new List<Declaration>();
                TrickCards = new List<Card>();
                WonCards = new List<IList<Card>>();

                for (int i = 0; i < playerCount; i++)
                {
                    PlayerCards.Add(new List<Card>(8));
                    WonCards.Add(new List<Card>());
                }
            }

            public MatchState Fresh()
            {
                foreach (var hand in PlayerCards) hand.Clear();
                foreach (var pile in WonCards) pile.Clear();
                Declarations.Clear();
                TrickCards.Clear();
                Dealer = 0;
                Contract = null;
                CommittedPlayer = null;
                TrickInitiator = null;
                return this;
            }


            public int Dealer { get; set; }

            public IList<IList<Card>> PlayerCards { get; }
            IReadOnlyList<IReadOnlyList<Card>> IMatchState.PlayerCards => PlayerCards.Select(s => new ReadOnlyCollection<Card>(s)).ToList().AsReadOnly();

            public Contract? Contract { get; set; }

            public int? CommittedPlayer { get; set; }

            public IList<Declaration> Declarations { get; }
            IReadOnlyList<Declaration> IMatchState.Declarations => new ReadOnlyCollection<Declaration>(Declarations);

            public int? TrickInitiator { get; private set; }

            public IList<Card> TrickCards { get; }
            IReadOnlyList<Card> IMatchState.TrickCards => new ReadOnlyCollection<Card>(TrickCards);

            public IList<IList<Card>> WonCards { get; }
            IReadOnlyList<IReadOnlyList<Card>> IMatchState.WonCards => WonCards.Select(s => new ReadOnlyCollection<Card>(s)).ToList().AsReadOnly();
        }
    }
}