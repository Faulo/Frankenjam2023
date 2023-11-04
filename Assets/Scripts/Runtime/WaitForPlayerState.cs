using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GossipGang {
    sealed class WaitForPlayerState : UIState, IBindingReceiver<Player> {
        [SerializeField]
        TMP_Text playerText;

        [Space]
        [SerializeField]
        Button nextPlayerButton;

        bool isDone;

        void Start() {
            nextPlayerButton.onClick.AddListener(() => {
                isDone = true;
            });
        }

        public override IEnumerator WaitForDone() {
            yield return new WaitWhile(() => !isDone);

            Destroy(gameObject);
        }

        public void Bind(Player model) {
            playerText.text = model.name;
        }
    }
}