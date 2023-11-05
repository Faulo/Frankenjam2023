using UnityEngine;

namespace GossipGang {
    sealed class LoadDayResources : MonoBehaviour {
        void Start() {
            foreach (var day in Resources.LoadAll<Day>("")) {
                GameManager.instance.AddDay(day);
            }
        }
    }
}