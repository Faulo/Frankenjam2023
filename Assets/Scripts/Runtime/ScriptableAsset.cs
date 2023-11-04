using UnityEngine;

namespace GossipGang {
    abstract class ScriptableAsset : ScriptableObject {
        [SerializeField]
        ScriptableAsset m_asset;

        protected virtual void OnValidate() {
            m_asset = this;
        }
    }
}
