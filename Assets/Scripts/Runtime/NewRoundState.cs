using System.Collections;
using TMPro;
using UnityEngine;

namespace GossipGang {
    sealed class NewRoundState : UIState, IPlayerEntryReceiver {
        [SerializeField]
        TMP_Text dateText;
        [SerializeField]
        TMP_Text descriptionText;
        [SerializeField]
        TMP_Text questionText;

        [SerializeField]
        Transform buttonContainer;
        [SerializeField]
        GameObject answerPrefab;

        bool isDone;

        void Start() {
        }

        public override IEnumerator WaitForDone() {
            yield return new WaitUntil(() => isDone);

            Destroy(gameObject);

            yield return GameManager.instance.LoadNewRoundState();
        }

        public void Bind(PlayerEntry entry) {
            dateText.text = entry.date.ToShortDateString();
            descriptionText.text = entry.day.description;
            questionText.text = entry.day.question;
        }
    }
}