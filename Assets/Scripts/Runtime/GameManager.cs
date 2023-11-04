using System;
using System.Collections;
using System.Collections.Generic;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace GossipGang {
    sealed class GameManager : MonoBehaviour {
        public static GameManager instance;
        public static event Action<Action<Day>> onLoadResources;
        public static event Action<Day> onAddDay;
        public static event Action<Player> onAddPlayer;

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

        int activePlayerIndex = 0;
        readonly List<Player> m_players = new();
        public IReadOnlyList<Player> players => m_players;
        public Player activePlayer => m_players[activePlayerIndex % m_players.Count];
        public void AddPlayer(Player player) {
            m_players.Add(player);
            onAddPlayer?.Invoke(player);
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

        public IEnumerator LoadNewPlayerState() {
            yield return ProcessState(newPlayerState);
        }

        public IEnumerator LoadNewRoundState() {
            yield return null;

            var instance = Instantiate(newRoundState);
            var entry = new PlayerEntry(m_days.RandomElement(), DateTime.Now, activePlayer, m_players);
            instance.gameObject.BindTo(entry);
            yield return instance.WaitForDone();

            yield return null;
        }

        public IEnumerator LoadShowDaysState() {
            yield return ProcessState(showDaysState);
        }

        IEnumerator ProcessState(UIState prefab) {
            yield return null;
            yield return Instantiate(prefab).WaitForDone();
            yield return null;
        }
    }
}