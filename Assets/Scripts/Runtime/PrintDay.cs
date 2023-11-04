using TMPro;
using UnityEngine;

namespace GossipGang {
    sealed class PrintDay : MonoBehaviour, IDayReceiver {
        [SerializeField]
        TMP_Text titleText;
        [SerializeField]
        TMP_Text descriptionText;
        [SerializeField]
        TMP_Text questionText;

        public void Bind(Day day) {
            titleText.text = day.title;
            descriptionText.text = day.description;
            questionText.text = day.question;
        }
    }
}
