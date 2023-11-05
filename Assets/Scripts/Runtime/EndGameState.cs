using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GossipGang {
    sealed class EndGameState : UIState {
        [SerializeField]
        Transform personContainer;
        [SerializeField]
        GameObject personPrefab;

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

            Debug.Log(pickedPlayer.nameWithColor);
            Destroy(gameObject);
        }
    }
}