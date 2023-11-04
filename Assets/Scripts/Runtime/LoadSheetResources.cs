using System;
using System.Collections;
using UnityEngine;

namespace GossipGang {
    sealed class LoadSheetResources : MonoBehaviour {
        [SerializeField]
        GoogleSheetAsset[] sheets = Array.Empty<GoogleSheetAsset>();

        void Start() {
            GameManager.RegisterDayLoader(Load_Co);
        }
        void OnDisable() {
            GameManager.RemoveDayLoader(Load_Co);
        }

        IEnumerator Load_Co() {
            foreach (var sheet in sheets) {
                yield return sheet.DownloadSheet_Co();

                foreach (var day in sheet.days) {
                    GameManager.instance.AddDay(day);
                }
            }
        }
    }
}