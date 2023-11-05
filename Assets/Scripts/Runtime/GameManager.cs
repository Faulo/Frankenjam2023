using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace GossipGang {
    sealed class GameManager : MonoBehaviour {
        public static GameManager instance;
        public static event Action<Day> onAddDay;
        public static event Action<Player> onAddPlayer;

        public static GameState state;

        [SerializeField]
        int roundMaximum = 3;

        [SerializeField]
        SerializableKeyValuePairs<DayCategory, bool> allowedCategories = new();
        public IEnumerable<DayCategory> allCategories => allowedCategories.Keys;

        [SerializeField]
        SerializableKeyValuePairs<DayTag, bool> allowedTags = new();
        public IEnumerable<DayTag> allTags => allowedTags.Keys;
        public void SetTag(DayTag tag, bool value) {
            var dictionary = new Dictionary<DayTag, bool>(allowedTags) {
                [tag] = value
            };
            allowedTags.SetItems(dictionary);
        }

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
        int dayCount => m_days.Count;
        public IReadOnlyCollection<Day> allDays => m_days.Values;
        public void AddDay(Day day) {
            if (!allowedCategories[day.category]) {
                return;
            }

            m_days[day.name] = day;
            onAddDay?.Invoke(day);
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
        }

        public IEnumerator Start() {
            yield return new WaitUntil(() => dayCount > 0);

            yield return LoadMainMenu();
        }

        public IEnumerator LoadMainMenu() {
            state = null;

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