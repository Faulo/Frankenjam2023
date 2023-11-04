using UnityEngine;
using UnityEngine.UI;

namespace GossipGang {
    sealed class SpriteReceiver : MonoBehaviour, IBindingReceiver<Sprite> {
        public void Bind(Sprite model) {
            if (TryGetComponent<Image>(out var component)) {
                component.sprite = model;
            }
        }
    }
}
