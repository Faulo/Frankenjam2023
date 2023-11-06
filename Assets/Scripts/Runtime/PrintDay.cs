using System;
using TMPro;
using UnityEngine;

namespace GossipGang {
    sealed class PrintDay : MonoBehaviour, IBindingReceiver<(Day day, DateTime randomDate)> {
        [SerializeField]
        TMP_Text dateText;
        [SerializeField]
        TMP_Text descriptionText;
        [SerializeField]
        TMP_Text questionText;

        public void Bind((Day day, DateTime randomDate) entry) {
            dateText.text = entry.randomDate.ToShortDateString();
            descriptionText.text = entry.day.description;
            questionText.text = entry.day.question;
        }
    }
}
