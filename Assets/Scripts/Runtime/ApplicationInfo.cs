using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GossipGang {
    sealed class ApplicationInfo : MonoBehaviour {
        TMP_Text component;
        string template;

        void Awake() {
            if (TryGetComponent(out component)) {
                template = component.text;
            }
        }

        void Start() {
            UpdateText();
        }

        void OnEnable() {
            GameManager.onAddDay += HandleDay;
        }

        void OnDisable() {
            GameManager.onAddDay -= HandleDay;
        }

        void HandleDay(Day day) {
            UpdateText();
        }
        IEnumerable<(string, string)> tokens {
            get {
                yield return ("Application.version", Application.version);
                yield return ("Game.dayCount", GameManager.instance.days.Count.ToString());
            }
        }

        void UpdateText() {
            if (component) {
                component.text = ReplaceTokens(template);
            }
        }

        string ReplaceTokens(string text) {
            foreach (var (key, value) in tokens) {
                text = text.Replace($"{{{key}}}", value);
            }

            return text;
        }
    }
}
