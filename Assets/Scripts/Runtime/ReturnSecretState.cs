using System.Collections;

namespace GossipGang {
    sealed class ReturnSecretState : UIState {
        public override IEnumerator WaitForDone() {
            yield return null;

            Destroy(gameObject);
        }
    }
}
