using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GossipGang {
    sealed class MainMenuState : UIState {
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

        public override IEnumerator WaitForDone() {
            yield return new WaitWhile(() => state == NextState.Unknown);


            switch (state) {
                case NextState.StartGame:
                    Destroy(gameObject);
                    yield return GameManager.instance.LoadNewPlayerState();
                    break;
                case NextState.ShowDays:
                    yield return GameManager.instance.LoadShowDaysState();
                    state = NextState.Unknown;
                    yield return WaitForDone();
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