using System;
using UnityEngine;
using UnityEngine.UI;

namespace GossipGang {
    sealed class AssignSecretForm : MonoBehaviour, IBindingReceiver<Player>, ISecretForm {
        public event Action<Player, Player> onGuessSecret;

        [SerializeField]
        GameObject nameField;
        [SerializeField]
        Slider dropdown;
        [SerializeField]
        GameObject secretField;

        public void Bind(Player secret) {
            secretField.BindTo(secret.secret);

            dropdown.minValue = -1;
            dropdown.maxValue = GameManager.state.playerCount - 1;

            dropdown.onValueChanged.AddListener(i => {
                var player = i < 0
                    ? null
                    : GameManager.state.GetPlayerByIndex(Mathf.RoundToInt(i));

                string name = player is null
                    ? "???"
                    : player.nameWithColor;

                nameField.BindTo(name);

                onGuessSecret?.Invoke(secret, player);
            });

            dropdown.value = -1;
        }
    }
}