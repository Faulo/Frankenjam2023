using UnityEngine;

namespace GossipGang {
    sealed class LoadDayResources : MonoBehaviour {
        [SerializeField]
        string path;

        void Start() {
            foreach (var day in Resources.LoadAll<Day>(path)) {
                GameManager.instance.AddDay(day);
            }
        }
    }
}