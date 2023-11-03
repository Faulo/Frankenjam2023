using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GossipGang {
    sealed class NewPlayerState : MonoBehaviour {
        [SerializeField]
        TMP_InputField nameField;
        [SerializeField]
        TMP_InputField birthdayField;
        [SerializeField]
        TMP_InputField secretField;
        [SerializeField]
        Button nextPlayerButton;
        [SerializeField]
        Button donePlayerButton;

        bool isDone;
        bool nextPlayer;

        void Start() {
            nextPlayerButton.onClick.AddListener(() => {
                nextPlayer = true;
                isDone = true;
            });

            donePlayerButton.onClick.AddListener(() => {
                isDone = true;
            });
        }

        public IEnumerator WaitForDone() {
            yield return new WaitUntil(() => isDone);

            Destroy(gameObject);

            if (nextPlayer) {
                yield return GameManager.instance.AddPlayer();
            }
        }
    }
}