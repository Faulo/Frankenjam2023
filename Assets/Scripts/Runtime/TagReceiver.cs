using UnityEngine;
using UnityEngine.UI;

namespace GossipGang {
    sealed class TagReceiver : MonoBehaviour, IBindingReceiver<(DayTag tag, bool isAllowed)> {
        [SerializeField]
        GameObject labelField;

        public void Bind((DayTag tag, bool isAllowed) model) {
            labelField.BindTo(model.tag.ToString());

            if (TryGetComponent<Toggle>(out var component)) {
                component.isOn = model.isAllowed;
                component.onValueChanged.AddListener(value => GameManager.config.SetTagAllowed(model.tag, value));
            }
        }
    }
}
