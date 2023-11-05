using System;
using System.Collections.Generic;
using System.Linq;

namespace GossipGang {
    sealed record PlayerEntry {
        public readonly Day day;
        public readonly DateTime date;
        public string dateString => date.ToShortDateString();
        public readonly Player askingPlayer;
        public readonly Dictionary<Player, int> playerAnswers;
        public int answerCount => day.answers.Count;

        public PlayerEntry(Day day, DateTime date, Player askingPlayer, IEnumerable<Player> allPlayers) {
            this.day = day;
            this.date = date;
            this.askingPlayer = askingPlayer;

            playerAnswers = allPlayers.ToDictionary(p => p, p => -1);
        }

        public void AwardScores() {
            int corretAnswer = playerAnswers[askingPlayer];
            var correctPlayers = playerAnswers
                .Where(kv => kv.Value == corretAnswer)
                .Select(kv => kv.Key)
                .ToList();

            if (correctPlayers.Count > 1) {
                foreach (var player in correctPlayers) {
                    GameManager.state.AwardPointTo(player);
                }
            }
        }
    }
}
