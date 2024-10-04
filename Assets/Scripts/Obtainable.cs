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
        protected Color color;
        [SerializeField]
        protected string rarity;
        [SerializeField]
        protected string explanation;

        public Obtainable(Obtainable other, int amount)
        {
            this.itemName = other.itemName;
            this.image = other.image;
            this.amount = amount;
            this.color = other.color;
            this.rarity = other.rarity;
            this.explanation = other.explanation;
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
            return color;
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
