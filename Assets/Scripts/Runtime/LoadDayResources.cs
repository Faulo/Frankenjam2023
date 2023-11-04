using System;
using UnityEngine;

namespace GossipGang {
    sealed class LoadDayResources : MonoBehaviour {
        void OnEnable() {
            GameManager.onLoadResources += Load;
        }
        void OnDisable() {
            GameManager.onLoadResources -= Load;
        }

        void Load(Action<Day> add) {
            foreach (var day in Resources.LoadAll<Day>("")) {
                add(day);
            }
        }
    }
}