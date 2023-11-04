using System;
using UnityEngine;

namespace GossipGang {

    sealed class PlayerEntry {
        public readonly Day day;
        public readonly DateTime date;

        public PlayerEntry(Day day, DateTime date) {
            this.day = day;
            this.date = date;
        }

        public void BindTo(GameObject gameObject) {
            foreach (var receiver in gameObject.GetComponents<IPlayerEntryReceiver>()) {
                receiver.Bind(this);
            }
        }
    }
}
