using UnityEngine;

namespace GossipGang {
    sealed class Player {
        public readonly string name;
        public readonly string birthday;
        public readonly string secret;

        public Color color => GameManager.instance.GetPlayerColor(this);
        public string nameWithColor => $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{name}</color>";

        public Player() {
        }
        public Player(string name, string birthday, string secret) {
            this.name = name;
            this.birthday = birthday;
            this.secret = secret;
        }

        public override string ToString() => name;
    }
}
