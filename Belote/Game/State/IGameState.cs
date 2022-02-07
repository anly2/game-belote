using System;
using System.Collections.Generic;
using Belote.Domain;
using Belote.Player;

namespace Belote.Game.State
{
    public interface IGameState
    {
        public IReadOnlyList<Card> Deck { get; }
        public IReadOnlyList<byte> Scores { get; }
        public IReadOnlyList<IPlayer> Players { get; }
        public IReadOnlyList<byte> PlayerTeams { get; }
        
        public IMatchState? Match { get; }

        public event Action<(IGameState state, int[] score)>? OnMatchEnd;
        public event Action<IGameState>? OnGameEnd;
    }
}