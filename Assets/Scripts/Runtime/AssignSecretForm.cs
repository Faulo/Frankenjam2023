using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GossipGang {
    sealed class AssignSecretForm : MonoBehaviour, IBindingReceiver<Player>, ISecretForm {
        public event Action<Player, Player> onGuessSecret;

        [SerializeField]
        GameObject nameField;
        [SerializeField]
        Slider slider;

        [Space]
        [SerializeField]
        TMP_Dropdown dropdown;

        [Space]
        [SerializeField]
        GameObject secretField;

        Player secret;

        public void Bind(Player secret) {
            this.secret = secret;

            secretField.BindTo(secret.secret);

            if (slider) {
                slider.minValue = -1;
                slider.maxValue = GameManager.state.playerCount - 1;

                slider.onValueChanged.AddListener(i => HandleValue(Mathf.RoundToInt(i)));

                slider.value = -1;
            }

            if (dropdown) {
                dropdown.options = GameManager
                    .state
                    .players
                    .Select(p => p.nameWithColor)
                    .Prepend("???")
                    .Select(text => new TMP_Dropdown.OptionData(text))
                    .ToList();

                dropdown.onValueChanged.AddListener(HandleValue);

                dropdown.value = 0;
            }
        }

        void HandleValue(int i) {
            var player = i < 0
                ? null
                : GameManager.state.GetPlayerByIndex(i);

            if (nameField) {
                string name = player is null
                            ? "???"
                            : player.nameWithColor;
                nameField.BindTo(name);
            }

            onGuessSecret?.Invoke(secret, player);
        }
    }
}