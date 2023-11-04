using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using UnityEngine;
using UnityEngine.Networking;

namespace GossipGang {
    [CreateAssetMenu]
    sealed class GoogleSheetAsset : ScriptableAsset {
#pragma warning disable IDE1006 // Naming Styles
        record Row {
            public string Tag_1 { get; set; }
            public string Tag_2 { get; set; }
            public string Tag_3 { get; set; }

            public IEnumerable<string> Tags => new string[] {
                Tag_1,
                Tag_2,
                Tag_3,
            }
            .Select(a => a.Trim())
            .Where(a => !string.IsNullOrWhiteSpace(a));

            public string Start { get; set; }
            public DateTime StartDate {
                get {
                    return DateTime.TryParse(Start + DateTime.Now.Year, out var date)
                        ? date
                        : DateTime.Now;
                }
            }

            public string End { get; set; }
            public DateTime EndDate {
                get {
                    return DateTime.TryParse(End + DateTime.Now.Year, out var date)
                        ? date
                        : DateTime.Now;
                }
            }

            public string Description { get; set; }
            public string Question { get; set; }
            public string Answer_1 { get; set; }
            public string Answer_2 { get; set; }
            public string Answer_3 { get; set; }
            public string Answer_4 { get; set; }
            public string Answer_5 { get; set; }
            public string Answer_6 { get; set; }
            public string Answer_7 { get; set; }
            public string Answer_8 { get; set; }
            public string Answer_9 { get; set; }
            public string Answer_10 { get; set; }

            public IEnumerable<string> Answers => new string[] {
                Answer_1,
                Answer_2,
                Answer_3,
                Answer_4,
                Answer_5,
                Answer_6,
                Answer_7,
                Answer_8,
                Answer_9,
                Answer_10,
            }
            .Select(a => a.Trim())
            .Where(a => !string.IsNullOrWhiteSpace(a));
        }
#pragma warning restore IDE1006 // Naming Styles
        [SerializeField]
        string id = "1gfnagqw3ySRh9GeTb3Emuy3RAd96msUj7WpWPA0rB2M";
        [SerializeField, TextArea(10, 100)]
        string data = "";

        public IEnumerable<Day> days {
            get {
                var config = new Configuration();
                using var reader = new StringReader(data);
                using var parser = new CsvReader(reader, config);

                foreach (var row in parser.GetRecords<Row>()) {
                    if (Day.TryCreateFromCSV(out var day, row.Description, row.Question, row.Answers, row.Tags, row.StartDate, row.EndDate)) {
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
#if UNITY_EDITOR
                    UnityEditor.EditorUtility.SetDirty(this);
#endif
                    break;
            }
        }
    }
}
