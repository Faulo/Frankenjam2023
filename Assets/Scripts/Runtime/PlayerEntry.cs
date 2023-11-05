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
        public readonly int answerCount;
        public IReadOnlyList<string> answers { get; }

        public PlayerEntry(Day day, DateTime date, Player askingPlayer, IEnumerable<Player> allPlayers) {
            this.day = day;
            this.date = date;
            this.askingPlayer = askingPlayer;

            playerAnswers = day.category switch {
                DayCategory.Event => new Dictionary<Player, int>() {
                    [askingPlayer] = -1,
                },
                _ => allPlayers.ToDictionary(p => p, p => -1),
            };

            answers = day.category switch {
                DayCategory.AllPlayers => playerAnswers
                    .Keys
                    .Select(p => p.nameWithColor)
                    .ToList(),
                DayCategory.AllPlayersExceptSpeaker => playerAnswers
                    .Keys
                    .Where(p => p != askingPlayer)
                    .Select(p => p.nameWithColor)
                    .ToList(),
                DayCategory.Event => new[] {
                    "I did the thing.",
                    "I did not do the thing."
                },
                _ => day.answers
            };

            answerCount = answers.Count;
        }

        public void AwardScores() {
            if (day.category == DayCategory.Event) {
                if (playerAnswers[askingPlayer] == 0) {
                    GameManager.state.AwardPointTo(askingPlayer);
                } else {
                    GameManager.state.RetractPointFrom(askingPlayer);
                }
            } else {
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
}
