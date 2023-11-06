using TMPro;
using UnityEngine;
using UnityEngine.Localization;

namespace GossipGang {
    sealed class LocalizedStringReceiver : MonoBehaviour, IBindingReceiver<LocalizedString> {
        public void Bind(LocalizedString model) {
            if (TryGetComponent<TMP_Text>(out var component)) {
                component.text = "";
                model.StringChanged += value => component.text = value;
            }
        }
    }
}
