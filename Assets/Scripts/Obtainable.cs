using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StandaloneItems
{
    [CreateAssetMenu(menuName = "StandaloneItems/Obtainable")]
    public class Obtainable : ScriptableObject
    {
        [SerializeField]
        protected string itemName;
        [SerializeField]
        protected Sprite image;
        [SerializeField]
        protected Color color;

        public string GetName()
        {
            return itemName;
        }

        public Sprite GetImage()
        {
            return image;
        }

        public Color GetColor()
        {
            return color;
        }
    }
}
