using System;
using System.Collections;
using System.Collections.Generic;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace GossipGang {
    sealed class GameManager : MonoBehaviour {
        public static GameManager instance;
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
        public void AddDay(Day day) {
            if (m_days.Add(day)) {
                onAddDay?.Invoke(day);
            }
        }

        static readonly List<Func<IEnumerator>> loaders = new();
        public static void RegisterDayLoader(Func<IEnumerator> loader) {
            loaders.Add(loader);
        }
        public static void RemoveDayLoader(Func<IEnumerator> loader) {
            loaders.Remove(loader);
        }

        int activePlayerIndex = 0;
        readonly List<Player> m_players = new();
        public IReadOnlyList<Player> players => m_players;
        public int playerCount => m_players.Count;
        public Player activePlayer => m_players[activePlayerIndex % m_players.Count];
        public void AddPlayer(Player player) {
            m_players.Add(player);
            onAddPlayer?.Invoke(player);
        }

        void Awake() {
            instance = this;
        }

        public IEnumerator Start() {
            foreach (var loader in loaders) {
                yield return loader();
            }

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
            var entry = new PlayerEntry(m_days.RandomElement(), activePlayer, m_players);
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

        public Player GetNextPlayer(Player previousPlayer) {
            int index = m_players.IndexOf(previousPlayer);
            return m_players[(index + 1) % playerCount];
        }

        [Header("Player Colors")]
        [SerializeField, Range(0, 1)]
        float playerSaturation = 0.5f;
        [SerializeField, Range(0, 1)]
        float playerValue = 0.25f;

        public Color GetPlayerColor(Player player) {
            float index = m_players.IndexOf(player);
            return Color.HSVToRGB(index / playerCount, playerSaturation, playerValue);
        }

        public void AdvancePlayer() => activePlayerIndex++;
    }
}