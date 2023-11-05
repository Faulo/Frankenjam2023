using System;

namespace GossipGang {
    interface ISecretForm {
        event Action<Player, Player> onGuessSecret;
    }
}