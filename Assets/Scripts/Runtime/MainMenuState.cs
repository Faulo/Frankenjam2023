using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GossipGang {
    sealed class MainMenuState : MonoBehaviour {
        enum NextState {
            Unknown,
            StartGame,
            ShowDays,
            Exit
        }
        [SerializeField]
        Button startGameButton;
        [SerializeField]
        Button showDaysButton;
        [SerializeField]
        Button exitGameButton;

        NextState state = NextState.Unknown;

        void Start() {
            startGameButton.onClick.AddListener(() => {
                state = NextState.StartGame;
            });

            showDaysButton.onClick.AddListener(() => {
                state = NextState.ShowDays;
            });

            exitGameButton.onClick.AddListener(() => {
                state = NextState.Exit;
            });
        }

        public IEnumerator WaitForDone() {
            yield return new WaitWhile(() => state == NextState.Unknown);

            Destroy(gameObject);

            switch (state) {
                case NextState.StartGame:
                    yield return GameManager.instance.AddPlayer();
                    break;
                case NextState.ShowDays:
                    yield return GameManager.instance.ShowDays();
                    break;
                case NextState.Exit:
                    Application.Quit();
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#endif
                    break;
            }
        }
    }
}