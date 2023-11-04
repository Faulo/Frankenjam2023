using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GossipGang {
    sealed class PrintRoundResultsState : UIState, IBindingReceiver<PlayerEntry> {
        [SerializeField]
        Transform answerContainer;
        [SerializeField]
        GameObject answerPrefab;

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

        public void Bind(PlayerEntry entry) {
            for (int i = 0; i < entry.answerCount; i++) {
                var instance = Instantiate(answerPrefab, answerContainer);
                instance.BindTo((entry, i));
            }
        }
    }
}