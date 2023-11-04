using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GossipGang {
    sealed class NewPlayerState : UIState {
        enum NextState {
            Unknown,
            AddMorePlayers,
            StartGame,
            Cancel
        }

        [SerializeField]
        InputField nameField;
        [SerializeField]
        InputField birthdayField;
        [SerializeField]
        InputField secretField;

        [Space]
        [SerializeField]
        Button nextPlayerButton;
        [SerializeField]
        Button donePlayerButton;
        [SerializeField]
        Button cancelButton;

        [Space]
        [SerializeField]
        Color validColor = Color.white;
        [SerializeField]
        Color invalidColor = Color.red;

        NextState state = NextState.Unknown;

        void Start() {
            nextPlayerButton.onClick.AddListener(() => {
                if (Validate()) {
                    state = NextState.AddMorePlayers;
                }
            });

            donePlayerButton.onClick.AddListener(() => {
                if (Validate()) {
                    state = NextState.StartGame;
                }
            });

            cancelButton.onClick.AddListener(() => {
                state = NextState.Cancel;
            });
        }

        bool Validate() {
            bool isNameValid = ValidateString(nameField.text);
            nameField.GetComponent<Image>().color = isNameValid
                ? validColor
                : invalidColor;

            bool isBirthdayValid = ValidateString(birthdayField.text);
            birthdayField.GetComponent<Image>().color = isBirthdayValid
                ? validColor
                : invalidColor;

            bool isSecretValid = ValidateString(secretField.text);
            secretField.GetComponent<Image>().color = isSecretValid
                ? validColor
                : invalidColor;

            return isNameValid && isBirthdayValid && isSecretValid;
        }

        bool ValidateString(string text) => !string.IsNullOrWhiteSpace(text);

        public override IEnumerator WaitForDone() {
            yield return new WaitWhile(() => state == NextState.Unknown);

            Destroy(gameObject);

            switch (state) {
                case NextState.AddMorePlayers:
                    yield return GameManager.instance.AddPlayer();
                    break;
                case NextState.StartGame:
                    yield return GameManager.instance.NextRound();
                    break;
                case NextState.Cancel:
                    yield return GameManager.instance.LoadMainMenu();
                    break;
            }
        }
    }
}