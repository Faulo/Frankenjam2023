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

        int currentRound = 0;

        [SerializeField]
        int roundMaximum = 3;

        bool hasFinished => currentRound >= roundMaximum;

        [SerializeField]
        UIState mainMenuState;

        [SerializeField]
        UIState newPlayerState;

        [SerializeField]
        UIState newRoundState;

        [SerializeField]
        UIState endgameState;

        [SerializeField]
        UIState showDaysState;

        readonly Dictionary<string, Day> m_days = new();
        public int dayCount => m_days.Count;
        public IReadOnlyCollection<Day> days => m_days.Values;
        public void AddDay(Day day) {
            m_days[day.name] = day;
            onAddDay?.Invoke(day);
        }

        int activePlayerIndex = 0;
        readonly List<Player> m_players = new();
        public IReadOnlyList<Player> players => m_players;
        public int playerCount => m_players.Count;
        public Player activePlayer => m_players[activePlayerIndex % m_players.Count];

        public Player firstPlayer => playerCount == 0
            ? new Player()
            : m_players[0];

        public void AddPlayer(Player player) {
            m_players.Add(player);
            onAddPlayer?.Invoke(player);
        }

        void Awake() {
            instance = this;
        }

        public IEnumerator Start() {
            yield return new WaitUntil(() => dayCount > 0);

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

            if (hasFinished) {
                var instance = Instantiate(endgameState);
                instance.gameObject.BindTo(this);
                yield return instance.WaitForDone();
            } else {
                var instance = Instantiate(newRoundState);
                var entry = new PlayerEntry(days.RandomElement(), activePlayer, m_players);
                instance.gameObject.BindTo(entry);
                yield return instance.WaitForDone();
            }

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

        public void AdvancePlayer() {
            activePlayerIndex = (activePlayerIndex + 1) % playerCount;

            if (activePlayerIndex == 0) {
                AdvanceRound();
            }
        }

        void AdvanceRound() => currentRound++;
    }
}