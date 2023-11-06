using UnityEngine;
using UnityEngine.UI;

namespace GossipGang {
    sealed class CategoryReceiver : MonoBehaviour, IBindingReceiver<(DayCategory category, bool isAllowed)> {
        [SerializeField]
        GameObject labelField;

        public void Bind((DayCategory category, bool isAllowed) model) {
            labelField.BindTo(model.category.ToString());

            if (TryGetComponent<Toggle>(out var component)) {
                component.isOn = model.isAllowed;
                component.onValueChanged.AddListener(value => GameManager.config.SetCategoryAllowed(model.category, value));
            }
        }
    }
}
