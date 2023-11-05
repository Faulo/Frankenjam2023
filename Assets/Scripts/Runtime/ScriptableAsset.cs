using MyBox;
using UnityEngine;

namespace GossipGang {
    abstract class ScriptableAsset : ScriptableObject {
        [SerializeField, ReadOnly]
        ScriptableAsset m_asset;

#if UNITY_EDITOR
        protected virtual void OnValidate() {
            if (m_asset != this) {
                m_asset = this;
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }
#endif
    }
}
