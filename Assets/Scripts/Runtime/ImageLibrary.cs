using System.Linq;
using UnityEngine;

namespace GossipGang {
    [CreateAssetMenu]
    sealed class ImageLibrary : ScriptableAsset {
        [SerializeField]
        Sprite image;

        Sprite[] images;
        public Sprite LookUp(string name) {
            images ??= Resources.LoadAll<Sprite>("");

            return images
                .DefaultIfEmpty(image)
                .FirstOrDefault(image => image.name == name);
        }
    }
}
