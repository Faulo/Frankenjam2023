using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace GossipGang {
    sealed class TagReceiver : MonoBehaviour, IBindingReceiver<(DayTag tag, bool isAllowed)> {
        public void Bind((DayTag tag, bool isAllowed) model) {
            gameObject.BindTo(new LocalizedString("Default", $"{nameof(DayTag)}/{model.tag}"));

            if (TryGetComponent<Toggle>(out var component)) {
                component.isOn = model.isAllowed;
                component.onValueChanged.AddListener(value => GameManager.config.SetTagAllowed(model.tag, value));
            }
        }
    }
}
