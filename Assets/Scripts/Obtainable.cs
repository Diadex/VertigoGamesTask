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
        protected int amount;
        [SerializeField]
        protected Color colorType;
        [SerializeField]
        protected string rarity;
        [SerializeField]
        protected string explanation;

        public static Obtainable Clone(Obtainable original, int newAmount)
        {
            Obtainable clone = ScriptableObject.CreateInstance<Obtainable>();
            clone.itemName = original.itemName;
            clone.image = original.image;
            clone.colorType = original.colorType;
            clone.rarity = original.rarity;
            clone.explanation = original.explanation;
            clone.amount = newAmount; // Update the amount here
            return clone;
        }

        public string GetName()
        {
            return itemName;
        }

        public Sprite GetImage()
        {
            return image;
        }

        public int GetAmount()
        {
            return amount;
        }

        public Color GetColor()
        {
            return colorType;
        }
        public string GetRarity()
        {
            return rarity;
        }
        public string GetExplanation()
        {
            return explanation;
        }
    }
}
