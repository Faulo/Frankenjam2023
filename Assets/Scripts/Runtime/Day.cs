using System.Collections.Generic;
using UnityEngine;

namespace GossipGang {
    [CreateAssetMenu]
    sealed class Day : ScriptableObject {
        [SerializeField]
        string m_title = nameof(title);
        public string title => m_title;

        [SerializeField]
        string m_description = nameof(description);
        public string description => m_description;

        [SerializeField]
        string m_question = nameof(question);
        public string question => m_question;

        [SerializeField]
        string[] m_answers = new[] { "A", "B", "C", "D" };
        public IReadOnlyList<string> answers => m_answers;

        public void BindTo(GameObject gameObject) {
            foreach (var receiver in gameObject.GetComponents<IDayReceiver>()) {
                receiver.Bind(this);
            }
        }
    }
}
