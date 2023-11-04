using System.Collections.Generic;
using UnityEngine;

namespace GossipGang {
    [CreateAssetMenu]
    sealed class Day : ScriptableObject {
        [SerializeField]
        string m_title = nameof(title);
        public string title => m_title;

        [SerializeField, TextArea(3, 12)]
        string m_description = nameof(description);
        public string description => m_description;

        [SerializeField, TextArea(3, 12)]
        string m_question = nameof(question);
        public string question => m_question;

        [SerializeField, TextArea(3, 12)]
        string[] m_answers = new[] { "A", "B", "C", "D" };
        public IReadOnlyList<string> answers => m_answers;
    }
}
