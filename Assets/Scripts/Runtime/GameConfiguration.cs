using System;
using System.Collections.Generic;
using System.Linq;

namespace GossipGang {
    sealed class GameConfiguration {
        public event Action<DayCategory, bool> onAddCategory;
        public event Action<DayTag, bool> onAddTag;
        public event Action onChange;

        readonly Dictionary<DayTag, bool> m_allowedTags = new();
        public IReadOnlyDictionary<DayTag, bool> allowedTags => m_allowedTags;

        public void AddTag(DayTag tag) {
            if (m_allowedTags.TryAdd(tag, true)) {
                onAddTag?.Invoke(tag, true);
            }
        }

        public void SetTagAllowed(DayTag tag, bool value) {
            if (m_allowedTags[tag] != value) {
                m_allowedTags[tag] = value;
                onChange?.Invoke();
            }
        }

        readonly Dictionary<DayCategory, bool> m_allowedCategories = new();

        public IReadOnlyDictionary<DayCategory, bool> allowedCategories => m_allowedCategories;

        public void AddCategory(DayCategory category) {
            if (m_allowedCategories.TryAdd(category, true)) {
                onAddCategory?.Invoke(category, true);
            }
        }

        public void SetCategoryAllowed(DayCategory category, bool value) {
            if (m_allowedCategories[category] != value) {
                m_allowedCategories[category] = value;
                onChange?.Invoke();
            }
        }

        public bool IsAllowed(IEnumerable<DayTag> tags) => tags.All(IsAllowed);
        public bool IsAllowed(DayTag tag) => allowedTags.TryGetValue(tag, out bool isAllowed) && isAllowed;
        public bool IsAllowed(DayCategory category) => allowedCategories.TryGetValue(category, out bool isAllowed) && isAllowed;
    }
}
