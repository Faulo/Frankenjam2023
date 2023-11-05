using System.Collections;

namespace GossipGang {
    sealed class LeaderboardState : UIState {
        public override IEnumerator WaitForDone() {
            yield return null;

            Destroy(gameObject);
        }
    }
}
