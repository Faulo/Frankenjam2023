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
        Button nextButton;

        bool isDone;

        void Start() {
            nextButton.onClick.AddListener(() => isDone = true);
        }

        public override IEnumerator WaitForDone() {
            yield return new WaitUntil(() => isDone);

            Destroy(gameObject);
        }
    }
}