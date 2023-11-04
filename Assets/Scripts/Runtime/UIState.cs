using System.Collections;
using UnityEngine;

namespace GossipGang {
    abstract class UIState : MonoBehaviour {
        public abstract IEnumerator WaitForDone();
    }
}