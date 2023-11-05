using System;
using System.Collections.Generic;
using System.Linq;

namespace GossipGang {
    sealed class PlayerEntry {
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
    }
}
