using System;
using UnityEngine;
using UnityEngine.UI;

namespace GossipGang {
    sealed class AssignSecretForm : MonoBehaviour, IBindingReceiver<Player>, ISecretForm {
        public event Action<Player, Player> onGuessSecret;

        [SerializeField]
        Dropdown dropdown;

        public void Bind(Player secret) {
            gameObject.BindTo(secret.secret);

            dropdown.options.Clear();
            dropdown.options.Add(new() { text = "" });
            foreach (var player in GameManager.state.players) {
                dropdown.options.Add(new() { text = player.name });
            }

            dropdown.onValueChanged.AddListener(i => {
                if (i > 0) {
                    onGuessSecret?.Invoke(secret, GameManager.state.GetPlayerByIndex(i - 1));
                }
            });
        }
    }
}