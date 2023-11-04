using System.Linq;
using UnityEngine;

namespace GossipGang {
    sealed class BindPlayerEntryResult : MonoBehaviour, IBindingReceiver<(PlayerEntry entry, int index)> {
        public void Bind((PlayerEntry entry, int index) model) {
            gameObject.BindTo(model.entry.day.answers[model.index]);
            gameObject.BindTo(model.entry.playerAnswers.Where(a => a.Value == model.index).Select(a => a.Key).ToArray());
        }
    }
}