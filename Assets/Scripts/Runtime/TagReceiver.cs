using UnityEngine;
using UnityEngine.UI;

namespace GossipGang {
    sealed class TagReceiver : MonoBehaviour, IBindingReceiver<DayTag> {
        [SerializeField]
        GameObject labelField;

        public void Bind(DayTag model) {
            labelField.BindTo(model.ToString());

            if (TryGetComponent<Toggle>(out var component)) {
                component.onValueChanged.AddListener(value => GameManager.instance.SetTag(model, value));
            }
        }
    }
}
