using System;
using System.Collections.Generic;
using System.Linq;

namespace GossipGang {
    sealed class PlayerEntry {
        public readonly Day day;
        public readonly DateTime date;
        public string dateString => date.ToShortDateString();
        public readonly Player player;
        public readonly Dictionary<Player, int> playerAnswers;
        public int answerCount => day.answers.Count;

        public PlayerEntry(Day day, Player player, IEnumerable<Player> allPlayers) {
            this.day = day;
            this.player = player;

            date = day.randomDate;
            playerAnswers = allPlayers.ToDictionary(p => p, p => -1);
        }
    }
}
