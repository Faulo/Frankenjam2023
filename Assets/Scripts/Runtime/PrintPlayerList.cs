using System.Linq;
using TMPro;
using UnityEngine;

namespace GossipGang {
    sealed class PrintPlayerList : MonoBehaviour, IBindingReceiver<Player[]> {
        [SerializeField]
        string separator = ", ";
        [SerializeField]
        string nobodyText = "<i>Niemand!</i>";

        public void Bind(Player[] models) {
            if (TryGetComponent<TMP_Text>(out var playerText)) {
                playerText.text = models.Length == 0
                    ? nobodyText
                    : string.Join(separator, models.Select(p => p.name));
            }
        }
    }
}