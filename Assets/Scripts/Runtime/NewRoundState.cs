using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GossipGang {
    sealed class NewRoundState : UIState {
        static int round = 0;

        [SerializeField]
        TMP_InputField roundField;

        [SerializeField]
        Button nextRoundButton;

        bool isDone;

        void Start() {
            round++;

            roundField.text = round.ToString();

            nextRoundButton.onClick.AddListener(() => {
                isDone = true;
            });
        }

        public override IEnumerator WaitForDone() {
            yield return new WaitUntil(() => isDone);

            Destroy(gameObject);

            yield return GameManager.instance.NextRound();
        }
    }
}