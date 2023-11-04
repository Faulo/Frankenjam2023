using TMPro;
using UnityEngine;

namespace GossipGang {
    sealed class TextReceiver : MonoBehaviour, IBindingReceiver<string> {
        public void Bind(string model) {
            if (TryGetComponent<TMP_Text>(out var component)) {
                component.text = model;
            }
        }
    }
}
