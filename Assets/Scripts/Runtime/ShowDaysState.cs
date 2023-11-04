using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GossipGang {
    sealed class ShowDaysState : UIState {
        enum NextState {
            Unknown,
            Back
        }
        [SerializeField]
        Transform dayContainer;
        [SerializeField]
        GameObject dayPrefab;

        [Space]
        [SerializeField]
        Button backButton;

        NextState state = NextState.Unknown;

        void Start() {
            foreach (var day in GameManager.instance.days) {
                var instance = Instantiate(dayPrefab, dayContainer);
                instance.BindTo(day);
            }

            backButton.onClick.AddListener(() => {
                state = NextState.Back;
            });
        }

        public override IEnumerator WaitForDone() {
            yield return new WaitWhile(() => state == NextState.Unknown);

            Destroy(gameObject);

            switch (state) {
                case NextState.Back:
                    break;
            }
        }
    }
}
