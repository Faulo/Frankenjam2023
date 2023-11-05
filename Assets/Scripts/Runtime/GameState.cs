using System.Collections.Generic;
using System.Linq;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace GossipGang {
    sealed class GameState {
        public int currentRound = 1;
        public readonly int lastRound;
        readonly List<PlayerEntry> m_entries = new();
        public IReadOnlyList<PlayerEntry> entries => m_entries;
        public bool hasFinished => currentRound > lastRound;

        public int askingPlayerIndex;
        public PlayerEntry currentEntry => entries[askingPlayerIndex];
        readonly Dictionary<Player, int> m_points = new();
        public IReadOnlyDictionary<Player, int> points => m_points;
        public readonly int playerCount = 0;

        public IEnumerable<Day> days => entries.Select(entry => entry.day);
        public IEnumerable<Player> players => points.Keys;
        public IEnumerable<Player> firstPlayers {
            get {
                int max = points.Values.Max();
                return players.Where(p => points[p] == max);
            }
        }

        public GameState(int lastRound, IEnumerable<Day> days, IEnumerable<Player> players) {
            this.lastRound = lastRound;

            foreach (var player in players) {
                if (m_points.TryAdd(player, 0)) {
                    playerCount++;
                }
            }

            var entries = days
                .Shuffle()
                .Take(playerCount * lastRound)
                .Select(d => (d, d.randomDate))
                .OrderBy(e => e.randomDate)
                .ToList();

            for (int i = 0; i < entries.Count; i++) {
                var (day, date) = entries[i];
                m_entries.Add(new(day, date, GetPlayer(i), players));

                Debug.Log(m_entries[^1]);
            }
        }

        Player GetPlayer(int index) => players.ElementAt(index % playerCount);

        public void AdvancePlayer() {
            askingPlayerIndex = (askingPlayerIndex + 1) % playerCount;

            if (askingPlayerIndex == 0) {
                AdvanceRound();
            }
        }

        void AdvanceRound() => currentRound++;
    }
}