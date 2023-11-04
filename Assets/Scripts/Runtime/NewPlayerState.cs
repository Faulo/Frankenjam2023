using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GossipGang {
    sealed class NewPlayerState : MonoBehaviour {
        [SerializeField]
        InputField nameField;
        [SerializeField]
        InputField birthdayField;
        [SerializeField]
        InputField secretField;
        [SerializeField]
        Button nextPlayerButton;
        [SerializeField]
        Button donePlayerButton;

        bool isDone;
        bool nextPlayer;

        [Space]
        [SerializeField]
        Color validColor = Color.white;
        [SerializeField]
        Color invalidColor = Color.red;

        void Start() {
            nextPlayerButton.onClick.AddListener(() => {
                nextPlayer = true;
                isDone = Validate();
            });

            donePlayerButton.onClick.AddListener(() => {
                nextPlayer = false;
                isDone = Validate();
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

        public IEnumerator WaitForDone() {
            yield return new WaitUntil(() => isDone);

            Destroy(gameObject);

            if (nextPlayer) {
                yield return GameManager.instance.AddPlayer();
            }
        }
    }
}