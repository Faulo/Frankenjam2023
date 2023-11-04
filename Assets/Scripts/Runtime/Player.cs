namespace GossipGang {
    sealed class Player {
        public readonly string name;
        public readonly string birthday;
        public readonly string secret;

        public Player(string name, string birthday, string secret) {
            this.name = name;
            this.birthday = birthday;
            this.secret = secret;
        }
    }
}
