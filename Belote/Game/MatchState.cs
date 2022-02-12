using System;
using System.Collections.Generic;
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
                PlayerCards = new List<List<Card>>();
                Contract = null;
                Declarations = new List<Declaration>();
                TrickCards = new List<Card>();
                WonCards = new List<List<Card>>();

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
                TrickCount = 0;
                return this;
            }


            public int Dealer { get; set; }

            public IList<List<Card>> PlayerCards { get; }
            IReadOnlyList<IReadOnlyList<Card>> IMatchState.PlayerCards => PlayerCards.Select(s => new ReadOnlyCollection<Card>(s)).ToList().AsReadOnly();

            public Contract? Contract { get; set; }

            public int? CommittedPlayer { get; set; }

            public IList<Declaration> Declarations { get; }
            IReadOnlyList<Declaration> IMatchState.Declarations => new ReadOnlyCollection<Declaration>(Declarations);

            public int? TrickInitiator { get; set; }

            public int TrickCount { get; set; }

            public IList<Card> TrickCards { get; }
            IReadOnlyList<Card> IMatchState.TrickCards => new ReadOnlyCollection<Card>(TrickCards);

            public IList<List<Card>> WonCards { get; }
            IReadOnlyList<IReadOnlyList<Card>> IMatchState.WonCards => WonCards.Select(s => new ReadOnlyCollection<Card>(s)).ToList().AsReadOnly();


            public event Action<(int player, Contract? bid)>? OnBid;
            public event Action<IMatchState>? OnBiddingEnd;
            public event Action<(int player, Card card)>? OnCardPlayed;
            public event Action<(IEnumerable<Card> trickCards, int initiator, int winner)>? OnTrickEnd;


            public void Bid(int playerIndex, Contract? bid)
            {
                if (bid.HasValue)
                {
                    CommittedPlayer = playerIndex;
                    Contract = bid;
                }
                OnBid?.Invoke((playerIndex, bid));
            }

            public void EndBidding()
            {
                OnBiddingEnd?.Invoke(this);
            }

            public void PlayCard(int player, Card card)
            {
                OnCardPlayed?.Invoke((player, card));
            }

            public void EndTrick(IEnumerable<Card> trickCards, int initiator, int winner)
            {
                OnTrickEnd?.Invoke((trickCards, initiator, winner));
            }
        }
    }
}