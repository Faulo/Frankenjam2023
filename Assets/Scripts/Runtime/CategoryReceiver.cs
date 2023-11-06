using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace GossipGang {
    sealed class CategoryReceiver : MonoBehaviour, IBindingReceiver<(DayCategory category, bool isAllowed)> {
        public void Bind((DayCategory category, bool isAllowed) model) {
            gameObject.BindTo(new LocalizedString("Default", $"{nameof(DayCategory)}/{model.category}"));

            if (TryGetComponent<Toggle>(out var component)) {
                component.isOn = model.isAllowed;
                component.onValueChanged.AddListener(value => GameManager.config.SetCategoryAllowed(model.category, value));
            }
        }
    }
}
