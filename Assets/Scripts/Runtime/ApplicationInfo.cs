using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GossipGang {
    sealed class ApplicationInfo : MonoBehaviour {
        void Start() {
            if (TryGetComponent<TMP_Text>(out var text)) {
                text.text = ReplaceTokens(text.text);
            }
        }

        IEnumerable<(string, string)> tokens {
            get {
                yield return ("Application.version", Application.version);
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
