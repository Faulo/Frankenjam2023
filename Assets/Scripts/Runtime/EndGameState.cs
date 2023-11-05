using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GossipGang {
    sealed class EndGameState : UIState {
        [SerializeField]
        Transform personContainer;
        [SerializeField]
        GameObject personPrefab;

        [Space]
        [SerializeField]
        UIState assignSecretState;
        [SerializeField]
        UIState returnSecretState;
        [SerializeField]
        UIState leaderboardState;

        Player pickedPlayer;

        void Start() {
            foreach (var player in GameManager.state.players) {
                var instance = Instantiate(personPrefab, personContainer);
                instance.BindTo(player.nameWithColor);
                instance.GetComponent<Button>().onClick.AddListener(() => pickedPlayer = player);
            }
        }

        public override IEnumerator WaitForDone() {
            yield return new WaitWhile(() => pickedPlayer is null);

            Destroy(gameObject);

            GameManager.state.RemoveSecret(pickedPlayer);

            foreach (var player in GameManager.state.players) {
                var instance = Instantiate(assignSecretState);

                instance.BindTo(player);

                yield return instance.WaitForDone();
            }

            yield return Instantiate(returnSecretState).WaitForDone();

            yield return Instantiate(leaderboardState).WaitForDone();

            yield return GameManager.instance.LoadMainMenu();
        }
    }
}