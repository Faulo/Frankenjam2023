using TMPro;
using UnityEngine;

namespace GossipGang {
    sealed class PrintDay : MonoBehaviour, IBindingReceiver<Day> {
        [SerializeField]
        TMP_Text dateText;
        [SerializeField]
        TMP_Text descriptionText;
        [SerializeField]
        TMP_Text questionText;

        public void Bind(Day day) {
            dateText.text = day.randomDate.ToShortDateString();
            descriptionText.text = day.description;
            questionText.text = day.question;
        }
    }
}
