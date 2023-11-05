using System;
using System.Collections;
using UnityEngine;

namespace GossipGang {
    sealed class LoadSheetResources : MonoBehaviour {
        [SerializeField]
        GoogleSheetAsset[] sheets = Array.Empty<GoogleSheetAsset>();

        IEnumerator Start() {
            foreach (var sheet in sheets) {
                foreach (var day in sheet.days) {
                    GameManager.instance.AddDay(day);
                }

                yield return sheet.DownloadSheet_Co();

                foreach (var day in sheet.days) {
                    GameManager.instance.AddDay(day);
                }
            }
        }
    }
}