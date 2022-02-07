using System;
using System.Collections.Generic;
using System.Linq;
using Belote.Domain;
using Belote.Game.State;
using Belote.Player;

namespace Belote.Game
{

    public partial class Game
    {

        // State implementations //

        private class GameState : IGameState
        {
            public GameState(List<Card> deck, List<IPlayer> players)
            {
                Deck = deck;
                Players = players;
                PlayerTeams = AssignPlayerTeams(players);
                Scores = new List<byte>(new byte[PlayerTeams.Distinct().Count()]);
                Match = new MatchState(players.Count);
            }

            public readonly List<Card> Deck;
            IReadOnlyList<Card> IGameState.Deck => Deck.AsReadOnly();

            public IReadOnlyList<IPlayer> Players { get; }

            public IReadOnlyList<byte> PlayerTeams { get; }

            public readonly List<byte> Scores;
            IReadOnlyList<byte> IGameState.Scores => Scores.AsReadOnly();

            public MatchState Match { get; set; }
            IMatchState IGameState.Match => Match;


            public event Action<(IGameState state, int[] score)>? OnMatchEnd;
            public event Action<IGameState>? OnGameEnd;


            public void EndMatch(int[] score) => OnMatchEnd?.Invoke((this, score));
            public void EndGame() => OnGameEnd?.Invoke(this);
        }
    }
}