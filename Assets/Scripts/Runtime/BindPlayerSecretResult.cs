using System.Linq;
using UnityEngine;

namespace GossipGang {
    sealed class BindPlayerSecretResult : MonoBehaviour, IBindingReceiver<Player> {
        [SerializeField]
        GameObject nameField;
        [SerializeField]
        GameObject scoreField;
        [SerializeField]
        GameObject secretField;

        [Space]
        [SerializeField]
        string redacted = "*";

        public void Bind(Player model) {
            nameField.BindTo(model.nameWithColor);
            scoreField.BindTo(GetScore(model));
            secretField.BindTo(GetSecret(model));
        }

        string GetSecret(Player player) {
            if (GameManager.state.IsSecretAvailable(player)) {
                return player.secret;
            } else {
                return string.Join("", Enumerable.Repeat(0, player.secret.Length).Select(_ => redacted));
            }
        }

        static string GetScore(Player player) {
            return GameManager.state.points[player].ToString();
        }
    }
}