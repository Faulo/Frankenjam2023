using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

namespace GossipGang {
    [CreateAssetMenu]
    sealed class Day : ScriptableObject {
        [SerializeField]
        DayTag[] m_tags = Array.Empty<DayTag>();
        public IReadOnlyList<DayTag> tags => m_tags;

        [SerializeField]
        DayCategory m_category = DayCategory.Default;
        public DayCategory category => m_category;

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

        [SerializeField]
        DateTime m_start = DateTime.Now;
        public DateTime start => m_start;

        [SerializeField]
        DateTime m_end = DateTime.Now;
        public DateTime end => m_end;

        public DateTime randomDate => new((long)UnityRandom.Range(start.Ticks, end.Ticks));

        [SerializeField]
        Sprite m_image;
        public Sprite image => m_image;

        public static bool TryCreateFromCSV(out Day day, DayCategory category, string description, string question, IEnumerable<string> answers, IEnumerable<DayTag> tags, DateTime start, DateTime end, Sprite image) {
            day = CreateInstance<Day>();

            day.m_category = category;
            day.m_description = description.Trim();
            day.m_question = question.Trim();
            day.m_answers = answers.ToArray();
            day.m_tags = tags.ToArray();
            day.m_start = start;
            day.m_end = end;
            day.m_image = image;

            if (string.IsNullOrWhiteSpace(day.m_question)) {
                return false;
            }

            if (day.m_answers.Length == 0) {
                return false;
            }

            return true;
        }

        public static bool TryCreateFromCSV(string[] header, string[] data, out Day day) {
            day = CreateInstance<Day>();

            if (!TryFind(header, data, "Title", out day.m_title)) {
            }

            if (!TryFind(header, data, "Description", out day.m_description)) {
                return false;
            }

            if (!TryFind(header, data, "Question", out day.m_question)) {
                return false;
            }

            day.m_tags = FindEnumerable(header, data, "Tag")
                .Select(t => Enum.Parse<DayTag>(t))
                .ToArray();

            day.m_answers = FindEnumerable(header, data, "Answer")
                .ToArray();
            if (day.m_answers.Length == 0) {
                return false;
            }

            return true;
        }

        static bool TryFind(string[] header, string[] data, string key, out string value) {
            for (int i = 0; i < header.Length; i++) {
                if (key.Equals(header[i], StringComparison.InvariantCultureIgnoreCase)) {
                    value = data[i];
                    return !string.IsNullOrEmpty(value);
                }
            }

            value = default;
            return false;
        }

        static IEnumerable<string> FindEnumerable(string[] header, string[] data, string key) {
            for (int i = 0; i < 20; i++) {
                if (TryFind(header, data, $"{key}_{i}", out string value)) {
                    yield return value;
                }
            }
        }
    }
}
