using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GossipGang {
    sealed class LeaderboardState : UIState {
        [SerializeField]
        Transform secretContainer;
        [SerializeField]
        GameObject secretPrefab;

        [Space]
        [SerializeField]
        Button doneButton;

        bool isDone;

        void Start() {
            foreach (var player in GameManager.state.points.OrderByDescending(keyval => keyval.Value)) {
                var instance = Instantiate(secretPrefab, secretContainer);
                instance.BindTo(player);
            }

            doneButton.onClick.AddListener(() => {
                isDone = true;
            });
        }

        public override IEnumerator WaitForDone() {
            yield return new WaitUntil(() => isDone);

            Destroy(gameObject);
        }
    }
}
