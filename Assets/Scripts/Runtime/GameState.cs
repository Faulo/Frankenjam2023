using System.Collections.Generic;
using System.Linq;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace GossipGang {
    sealed class GameState {
        public int currentRound { get; private set; } = 1;
        public readonly int lastRound;
        readonly List<PlayerEntry> m_entries = new();
        public IReadOnlyList<PlayerEntry> entries => m_entries;
        public bool hasFinished => currentRound > lastRound;

        int askingPlayerIndex;
        int currentEntryIndex => askingPlayerIndex + ((currentRound - 1) * playerCount);
        public PlayerEntry currentEntry => entries[currentEntryIndex];
        readonly Dictionary<Player, int> m_points = new();
        public IReadOnlyDictionary<Player, int> points => m_points;
        public readonly int playerCount = 0;

        public IEnumerable<Day> days => entries.Select(entry => entry.day);
        public IReadOnlyList<Player> players { get; private set; }
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

            this.players = m_points.Keys.ToList();

            var entries = days
                .Shuffle()
                .Take(playerCount * lastRound)
                .Select(d => (d, d.randomDate))
                .OrderBy(e => e.randomDate)
                .ToList();

            for (int i = 0; i < entries.Count; i++) {
                var (day, date) = entries[i];
                m_entries.Add(new(day, date, GetPlayerByIndex(i), players));
            }
        }

        public Player GetPlayerByIndex(int index) => players.ElementAt(index % playerCount);

        public void AdvancePlayer() {
            askingPlayerIndex = (askingPlayerIndex + 1) % playerCount;

            if (askingPlayerIndex == 0) {
                AdvanceRound();
            }
        }

        void AdvanceRound() => currentRound++;

        public IEnumerable<Player> playersWithSecrets => players
            .Where(p => !removedSecrets.Contains(p));

        readonly HashSet<Player> removedSecrets = new();

        public void RemoveSecret(Player player) {
            removedSecrets.Add(player);
        }

        public bool IsSecretAvailable(Player player) => !removedSecrets.Contains(player);

        public void AwardPointTo(Player player) {
            Debug.Log($"Awarding point to {player}");
            m_points[player]++;
        }

        public void RestoreSecrets() => removedSecrets.Clear();
    }
}