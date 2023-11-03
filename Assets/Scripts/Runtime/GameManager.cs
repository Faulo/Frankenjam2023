using System.Collections;
using UnityEngine;

namespace GossipGang {
    sealed class GameManager : MonoBehaviour {
        public static GameManager instance;

        [SerializeField]
        NewPlayerState newPlayerState;

        [SerializeField]
        NewRoundState newRoundState;

        void Awake() {
            instance = this;
        }

        public IEnumerator Start() {
            yield return AddPlayer();

            yield return NextRound();
        }

        public IEnumerator AddPlayer() {
            var newPlayer = Instantiate(newPlayerState);
            yield return newPlayer.WaitForDone();
        }

        public IEnumerator NextRound() {
            var newRound = Instantiate(newRoundState);
            yield return newRound.WaitForDone();
        }
    }
}