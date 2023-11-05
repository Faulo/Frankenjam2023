using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GossipGang {
    sealed class ReturnSecretState : UIState {
        enum Answer {
            Unknown,
            Yes,
            No
        }

        [SerializeField]
        Button yesButton;
        [SerializeField]
        Button noButton;

        Answer answer = Answer.Unknown;

        void Start() {
            yesButton.onClick.AddListener(() => {
                answer = Answer.Yes;
            });

            noButton.onClick.AddListener(() => {
                answer = Answer.No;
            });
        }

        public override IEnumerator WaitForDone() {
            yield return new WaitWhile(() => answer == Answer.Unknown);

            Destroy(gameObject);

            if (answer == Answer.Yes) {
                GameManager.state.RestoreSecrets();
            }
        }
    }
}
