using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
            public string Category { get; set; }

            public string Tag_1 { get; set; }
            public string Tag_2 { get; set; }
            public string Tag_3 { get; set; }

            IEnumerable<string> StringTags => new string[] {
                Tag_1,
                Tag_2,
                Tag_3,
            }
            .Where(a => !string.IsNullOrWhiteSpace(a));
            public IEnumerable<DayTag> Tags {
                get {
                    foreach (string tag in StringTags) {
                        if (Enum.TryParse<DayTag>(tag, out var t)) {
                            yield return t;
                        }
                    }
                }
            }

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
            .Where(a => !string.IsNullOrWhiteSpace(a));

            public string Image { get; set; }
        }
#pragma warning restore IDE1006 // Naming Styles

        [SerializeField]
        ImageLibrary library;
        [SerializeField]
        string id = "1gfnagqw3ySRh9GeTb3Emuy3RAd96msUj7WpWPA0rB2M";
        [SerializeField]
        string sheet = "Marie";

        string url => $"https://docs.google.com/spreadsheets/d/{id}/gviz/tq?tqx=out:csv&sheet={sheet}";

        string data;

        public IEnumerable<Day> days {
            get {
                if (string.IsNullOrEmpty(data)) {
                    yield break;
                }

                var config = new CsvConfiguration(CultureInfo.InvariantCulture) {
                    TrimOptions = TrimOptions.Trim
                };
                using var reader = new StringReader(data);
                using var parser = new CsvReader(reader, config);

                foreach (var row in parser.GetRecords<Row>()) {
                    if (!Enum.TryParse<DayCategory>(row.Category, out var category)) {
                        category = DayCategory.Default;
                    }

                    var image = library.LookUp(row.Image);

                    if (Day.TryCreateFromCSV(out var day, category, row.Description, row.Question, row.Answers, row.Tags, row.StartDate, row.EndDate, image)) {
                        yield return day;
                    }
                }
            }
        }

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
                    CreateDays();
#endif
                    break;
            }
        }

#if UNITY_EDITOR
        void CreateDays() {
            var ids = days.ToDictionary(d => d.name);
            string path = UnityEditor.AssetDatabase.GetAssetPath(this);

            foreach (var day in UnityEditor.AssetDatabase.LoadAllAssetsAtPath(path).OfType<Day>()) {
                if (ids.Remove(day.name, out var newDay)) {
                    JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(newDay), day);
                } else {
                    UnityEditor.AssetDatabase.RemoveObjectFromAsset(day);
                }
            }

            foreach (var day in ids.Values) {
                UnityEditor.AssetDatabase.AddObjectToAsset(day, this);
            }

            UnityEditor.AssetDatabase.SaveAssets();
        }
#endif
    }
}
