using System.Collections;
using UnityEngine;

namespace GossipGang {
    sealed class LoadDayResources : MonoBehaviour {
        void OnEnable() {
            GameManager.RegisterDayLoader(Load_Co);
        }
        void OnDisable() {
            GameManager.RemoveDayLoader(Load_Co);
        }

        IEnumerator Load_Co() {
            foreach (var day in Resources.LoadAll<Day>("")) {
                GameManager.instance.AddDay(day);
            }

            yield return null;
        }
    }
}