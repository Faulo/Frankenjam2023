using UnityEngine;

namespace GossipGang {
    static class Binding {
        public static void BindTo<T>(this GameObject gameObject, T model) {
            foreach (var receiver in gameObject.GetComponentsInChildren<IBindingReceiver<T>>()) {
                receiver.Bind(model);
            }
        }
    }
}
