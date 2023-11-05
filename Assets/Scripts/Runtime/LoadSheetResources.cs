using System.Collections;
using UnityEngine;

namespace GossipGang {
    sealed class LoadSheetResources : MonoBehaviour {
        [SerializeField]
        string path;

        IEnumerator Start() {
            foreach (var sheet in Resources.LoadAll<GoogleSheetAsset>(path)) {
                yield return sheet.DownloadSheet_Co();

                foreach (var day in sheet.days) {
                    GameManager.instance.AddDay(day);
                }
            }
        }
    }
}