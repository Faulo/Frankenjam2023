using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GossipGang {
    sealed class GameManager : MonoBehaviour {
        public static GameManager instance;
        public static event Action<Action<Day>> onLoadResources;
        public static event Action<Day> onAddDay;

        [SerializeField]
        MainMenuState mainMenuState;

        [SerializeField]
        NewPlayerState newPlayerState;

        [SerializeField]
        NewRoundState newRoundState;

        readonly HashSet<Day> m_days = new();
        public IReadOnlyCollection<Day> days => m_days;
        void AddDay(Day day) {
            if (m_days.Add(day)) {
                onAddDay?.Invoke(day);
            }
        }

        void Awake() {
            instance = this;
        }

        public IEnumerator Start() {
            onLoadResources?.Invoke(AddDay);

            yield return LoadMainMenu();
        }

        public IEnumerator LoadMainMenu() {
            yield return null;

            var state = Instantiate(mainMenuState);
            yield return state.WaitForDone();
        }

        public IEnumerator AddPlayer() {
            yield return null;

            var state = Instantiate(newPlayerState);
            yield return state.WaitForDone();
        }

        public IEnumerator NextRound() {
            yield return null;

            var state = Instantiate(newRoundState);
            yield return state.WaitForDone();
        }

        public IEnumerator ShowDays() {
            yield return null;

            yield return LoadMainMenu();
        }
    }
}