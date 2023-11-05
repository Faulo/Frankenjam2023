using System.Collections;
using System.Collections.Generic;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.UI;

namespace GossipGang {
    sealed class AssignSecretsState : UIState, IBindingReceiver<Player> {
        [SerializeField]
        GameObject nameField;
        [SerializeField]
        Transform secretContainer;
        [SerializeField]
        GameObject secretPrefab;

        [Space]
        [SerializeField]
        Button doneButton;

        Player activePlayer;
        bool isDone = false;
        readonly Dictionary<Player, Player> guessedSecrets = new();

        public void Bind(Player model) {
            activePlayer = model;

            nameField.BindTo(model.nameWithColor);

            foreach (var player in GameManager.state.playersWithSecrets.Shuffle()) {
                var instance = Instantiate(secretPrefab, secretContainer);
                instance.BindTo(player);
                instance.GetComponent<ISecretForm>().onGuessSecret += (secret, player) => guessedSecrets[secret] = player;
            }

            doneButton.onClick.AddListener(() => isDone = true);
        }

        public override IEnumerator WaitForDone() {
            yield return new WaitUntil(() => isDone);

            foreach (var (secret, player) in guessedSecrets) {
                if (secret == player) {
                    GameManager.state.AwardPointTo(activePlayer);
                }
            }

            Destroy(gameObject);
        }
    }
}