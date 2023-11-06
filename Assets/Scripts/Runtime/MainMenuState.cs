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
        Transform tagContainer;
        [SerializeField]
        GameObject tagPrefab;

        [Space]
        [SerializeField]
        Transform categoryContainer;
        [SerializeField]
        GameObject categoryPrefab;

        [Space]
        [SerializeField]
        Button startGameButton;
        [SerializeField]
        Button showDaysButton;
        [SerializeField]
        Button exitGameButton;

        NextState state = NextState.Unknown;

        void OnEnable() {
            GameManager.config.onAddTag += InstantiateTag;
            GameManager.config.onAddCategory += InstantiateCategory;
        }

        void OnDisable() {
            GameManager.config.onAddTag -= InstantiateTag;
            GameManager.config.onAddCategory -= InstantiateCategory;
        }

        void InstantiateTag(DayTag tag, bool isAllowed) {
            var instance = Instantiate(tagPrefab, tagContainer);
            instance.AddComponent<TagReceiver>();
            instance.BindTo((tag, isAllowed));
        }

        void InstantiateCategory(DayCategory category, bool isAllowed) {
            var instance = Instantiate(categoryPrefab, categoryContainer);
            instance.AddComponent<CategoryReceiver>();
            instance.BindTo((category, isAllowed));
        }

        void Start() {
            foreach (var (category, isAllowed) in GameManager.config.allowedCategories) {
                InstantiateCategory(category, isAllowed);
            }

            foreach (var (tag, isAllowed) in GameManager.config.allowedTags) {
                InstantiateTag(tag, isAllowed);
            }

            startGameButton.onClick.AddListener(() => {
                if (GameManager.instance.allDays.Count > 0) {
                    state = NextState.StartGame;
                }
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