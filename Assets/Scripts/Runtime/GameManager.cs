using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using UnityEngine;

namespace GossipGang {
    sealed class GameManager : MonoBehaviour {
        public static GameManager instance;
        public static event Action onChangeDays;
        public static event Action<Player> onAddPlayer;

        public static readonly GameConfiguration config = new();
        public static GameState state;

        [SerializeField]
        int roundMaximum = 3;

        [Separator]
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
        void RaiseDayChange() {
            allDays = m_days
                .Values
                .Where(day => config.IsAllowed(day.category))
                .Where(day => day.tags.Count == 0 || config.IsAllowed(day.tags))
                .ToList();
            onChangeDays?.Invoke();
        }
        public int dayCount => allDays.Count;
        public IReadOnlyCollection<Day> allDays { get; private set; } = new List<Day>();
        public void AddDay(Day day) {
            config.AddCategory(day.category);
            foreach (var tag in day.tags) {
                config.AddTag(tag);
            }

            m_days[day.name] = day;
            RaiseDayChange();
        }

        readonly List<Player> m_players = new();
        IReadOnlyList<Player> players => m_players;
        public int playerCount => m_players.Count;

        public void AddPlayer(Player player) {
            m_players.Add(player);
            onAddPlayer?.Invoke(player);
        }

        void Awake() {
            instance = this;
            config.onChange += RaiseDayChange;
        }

        public IEnumerator Start() {
            yield return new WaitUntil(() => dayCount > 0);

            yield return LoadMainMenu();
        }

        public IEnumerator LoadMainMenu() {
            state = null;
            m_players.Clear();

            yield return ProcessState(mainMenuState);
        }

        public IEnumerator LoadNewPlayerState() {
            yield return ProcessState(newPlayerState);
        }

        public IEnumerator LoadNewRoundState() {
            yield return null;

            if (state is null) {
                var entries = new List<PlayerEntry>();
                state = new(roundMaximum, allDays, players);
            } else {
                state.AdvancePlayer();
            }

            if (state.hasFinished) {
                var instance = Instantiate(endgameState);
                instance.gameObject.BindTo(this);
                yield return instance.WaitForDone();
            } else {
                var instance = Instantiate(newRoundState);
                instance.gameObject.BindTo(state.currentEntry);
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
    }
}