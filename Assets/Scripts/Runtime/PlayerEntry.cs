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
        public int answerCount => day.category switch {
            DayCategory.Default => day.answers.Count,
            DayCategory.AllPlayers => playerAnswers.Count,
            DayCategory.AllPlayersExceptSpeaker => playerAnswers.Count - 1,
            DayCategory.Event => 2,
            _ => throw new NotImplementedException(),
        };
        public IEnumerable<string> answers => day.category switch {
            DayCategory.Default => day
                .answers,
            DayCategory.AllPlayers => playerAnswers
                .Keys
                .Select(p => p.nameWithColor),
            DayCategory.AllPlayersExceptSpeaker => playerAnswers
                .Keys
                .Where(p => p != askingPlayer)
                .Select(p => p.nameWithColor),
            DayCategory.Event => new[] { "I did the thing.", "I did not do the thing." },
            _ => throw new NotImplementedException(),
        };

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
