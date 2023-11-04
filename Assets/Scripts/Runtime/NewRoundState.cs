using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GossipGang {
    sealed class NewRoundState : UIState, IBindingReceiver<PlayerEntry> {
        [SerializeField]
        TMP_Text playerText;
        [SerializeField]
        TMP_Text dateText;
        [SerializeField]
        TMP_Text descriptionText;
        [SerializeField]
        TMP_Text questionText;

        [SerializeField]
        Transform buttonContainer;
        [SerializeField]
        GameObject answerPrefab;

        [SerializeField]
        UIState popupPrefab;
        [SerializeField]
        UIState resultsPrefab;

        bool isDone => entry is not null && entry.playerAnswers.Values.All(a => a != -1);

        void Start() {
        }

        public override IEnumerator WaitForDone() {
            yield return new WaitUntil(() => isDone);

            var instance = Instantiate(popupPrefab);
            instance.BindTo(askingPlayer);
            yield return instance.WaitForDone();

            Destroy(gameObject);

            instance = Instantiate(resultsPrefab);
            instance.BindTo(entry);
            yield return instance.WaitForDone();

            GameManager.instance.AdvancePlayer();

            yield return GameManager.instance.LoadNewRoundState();
        }

        PlayerEntry entry;
        bool activePlayerIsSelf => answeringPlayer == askingPlayer;
        Player askingPlayer => entry.player;

        Player m_answeringPlayer;
        Player answeringPlayer {
            get => m_answeringPlayer;
            set {
                m_answeringPlayer = value;

                playerText.text = activePlayerIsSelf
                    ? $"{askingPlayer.name}'s turn"
                    : $"{askingPlayer.name}'s turn\r\n{answeringPlayer.name}'s answer";
            }
        }

        public void Bind(PlayerEntry entry) {
            this.entry = entry;
            answeringPlayer = entry.player;

            dateText.text = entry.dateString;
            descriptionText.text = entry.day.description;
            questionText.text = entry.day.question;

            int i = 0;
            foreach (string answer in entry.day.answers) {
                var instance = Instantiate(answerPrefab, buttonContainer);
                instance.BindTo(answer);

                if (instance.TryGetComponent<Button>(out var button)) {
                    button.onClick.AddListener(() => SetAnswer(button.transform.GetSiblingIndex()));
                }

                i++;
            }
        }

        void SetAnswer(int answer) {
            entry.playerAnswers[answeringPlayer] = answer;

            answeringPlayer = GameManager.instance.GetNextPlayer(answeringPlayer);
        }
    }
}