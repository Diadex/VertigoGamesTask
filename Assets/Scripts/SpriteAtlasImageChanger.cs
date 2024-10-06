using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SpriteAtlasImageChanger : MonoBehaviour
{
    [SerializeField]
    private SpriteAtlas uiAtlas;

    void Start()
    {
        // Get the current sprite assigned to the Image component and set it as a sprite atlas.
        Image imageComponent = GetComponent<Image>();
        Sprite currentSprite = imageComponent.sprite;

        if (currentSprite != null && uiAtlas != null)
        {
            string spriteName = currentSprite.name.Replace("(Clone)", "").Trim();

            Sprite atlasSprite = uiAtlas.GetSprite(spriteName);
            if (atlasSprite != null)
            {
                imageComponent.sprite = atlasSprite;
                currentSprite = null;
            }
            else
            {
                Debug.LogWarning($"Sprite '{spriteName}' not found in the atlas.");
            }
        }
        else
        {
            Debug.LogWarning("Image component or Sprite Atlas is missing.");
        }
    }
}
