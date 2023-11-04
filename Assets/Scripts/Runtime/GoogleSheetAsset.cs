using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace GossipGang {
    [CreateAssetMenu]
    sealed class GoogleSheetAsset : ScriptableAsset {
        [SerializeField]
        string id = "1gfnagqw3ySRh9GeTb3Emuy3RAd96msUj7WpWPA0rB2M";
        [SerializeField, TextArea(10, 100)]
        string data = "";

        public IEnumerable<Day> days {
            get {
                string[] rows = data.Split('\r');
                string[] header = rows[0].Split(',');

                foreach (string row in rows.Skip(1)) {
                    if (Day.TryCreateFromCSV(header, row.Split(','), out var day)) {
                        yield return day;
                    }
                }
            }
        }

        string url => $"https://docs.google.com/spreadsheets/d/{id}/export?format=csv";

        public IEnumerator DownloadSheet_Co() {
            using var request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();

            switch (request.result) {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(request.error, this);
                    break;
                case UnityWebRequest.Result.Success:
                    data = request.downloadHandler.text;
                    break;
            }
        }
    }
}
