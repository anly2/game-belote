﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Schema;
using CommonUtils;

namespace Belote
{
    public class Game
    {
        public const byte WinningScore = 151;

        private readonly GameState _state;
        public IGameState State => _state;

        private MatchState? _match
        {
            get => (MatchState?) _state.Match;
            set => _state.Match = value;
        }
        
        
        public Game(List<Card> deck, List<IPlayer> players)
        {
            _state = new GameState(deck, players);
        }


        public static List<Card> GetPlayingDeck()
        {
            var deck = new List<Card>(Enum.GetValues<Card>());
            deck.RemoveAll(card => card.SuitOrdinal() < 6);
            return deck;
        }
        
        
        public static IReadOnlyList<byte> AssignPlayerTeams(IList<IPlayer> players)
        {
            if (players.Count == 4)
                return new byte[] {0, 1, 0, 1};
            return Enumerable.Range(0, players.Count-1).Select(b => (byte) b).ToList().AsReadOnly();
        }

        public virtual void PlayGame()
        {
            //#! With this naive condition the rule of "Cannot win with Valat" is not implemented
            while (!State.Scores.Any(s => s >= WinningScore))
            {
                ShuffleBeforeGame();
                PlayMatch();
            }
        }

        protected virtual void ShuffleBeforeGame()
        {
            _state.Deck.Shuffle();
        }

        protected virtual void PlayMatch()
        {
            // init match state
            var prevDealer = _match?.Dealer;
            
            _match = _match?.Fresh() ?? new MatchState(_state.Players.Count);
            
            _match.Dealer = _state.NextPlayer(prevDealer);

            
            // play match
            DealInitial();
            // GatherBids();
            // DealRemaining();

            // finish match
            // var scores = CountScore();
            // for (var i = 0; i < State.Scores.Length; i++)
            //     State.Scores[i] += scores[i];
        }

        protected virtual void DealInitial()
        {
            if (_match == null) throw new InvalidOperationException("There is no Match started!");
            
            for (var i = 0; i < _state.Players.Count; i++)
                _state.Deck.Move(3, _match.PlayerCards[i]);
            
            for (var i = 0; i < _state.Players.Count; i++)
                _state.Deck.Move(2, _match.PlayerCards[i]);
        }
        
        
        private class GameState : IGameState
        {
            public GameState(List<Card> deck, List<IPlayer> players)
            {
                Deck = deck;
                Players = players;
                PlayerTeams = AssignPlayerTeams(players);
                Scores = new List<byte>(new byte[PlayerTeams.Distinct().Count()]);
            }

            public readonly List<Card> Deck;
            IReadOnlyList<Card> IGameState.Deck { get => Deck.AsReadOnly(); }

            public IReadOnlyList<IPlayer> Players { get; }

            public IReadOnlyList<byte> PlayerTeams { get; }
            
            public readonly List<byte> Scores;
            IReadOnlyList<byte> IGameState.Scores { get => Scores.AsReadOnly(); }
            
            public IMatchState? Match { get; set; }
        }

        private class MatchState : IMatchState
        {
            public MatchState(int playerCount)
            {
                PlayerCards = new List<IList<Card>>();
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
                CommittedPlayer = null;
                TrickInitiator = null;
                return this;
            }

            public byte Dealer { get; set; }
            
            public IList<IList<Card>> PlayerCards { get; }
            IReadOnlyList<IReadOnlyList<Card>> IMatchState.PlayerCards => PlayerCards.Select(s => new ReadOnlyCollection<Card>(s)).ToList().AsReadOnly();
            
            public byte? CommittedPlayer { get; set; }
            
            public IList<Declaration> Declarations { get; set; }
            IReadOnlyList<Declaration> IMatchState.Declarations => new ReadOnlyCollection<Declaration>(Declarations); 
            
            public byte? TrickInitiator { get; set; }
            public IList<Card> TrickCards { get; set; }
            IReadOnlyList<Card> IMatchState.TrickCards => new ReadOnlyCollection<Card>(TrickCards); 
            
            public IList<IList<Card>> WonCards { get; set; }
            IReadOnlyList<IReadOnlyList<Card>> IMatchState.WonCards => WonCards.Select(s => new ReadOnlyCollection<Card>(s)).ToList().AsReadOnly();
        }
    }

    public static class IndexUtils
    {
        public static byte NextPlayer(this IGameState gameState, byte? playerIndex)
        {
            if (playerIndex == null)
                return 0;

            return (byte)((byte) (playerIndex + 1) % gameState.Players.Count);
        }
    }
    
    public static class CardUtils
    {
        public static int SuitOrdinal(this Card card)
        {
            return (int) card % 14;
        }
    }
}