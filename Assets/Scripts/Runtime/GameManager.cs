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
        UIState mainMenuState;

        [SerializeField]
        UIState newPlayerState;

        [SerializeField]
        UIState newRoundState;

        [SerializeField]
        UIState showDaysState;

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
            yield return ProcessState(mainMenuState);
        }

        public IEnumerator AddPlayer() {
            yield return ProcessState(newPlayerState);
        }

        public IEnumerator NextRound() {
            yield return ProcessState(newRoundState);
        }

        public IEnumerator ShowDays() {
            yield return ProcessState(showDaysState);
        }

        IEnumerator ProcessState(UIState prefab) {
            yield return null;
            yield return Instantiate(prefab).WaitForDone();
            yield return null;
        }
    }
}