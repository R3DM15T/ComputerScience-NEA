using UnityEngine;
using System.Collections;
public class CosmeticManager : MonoBehaviour
{
    public Sprite[] sprites;
    public RuntimeAnimatorController[] animationControllers;
    Animator animation;
    SpriteRenderer spriteRender;

    void Start()
    {
        animation = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();
        LoadEquippedCosmetics();
    }

    public void LoadEquippedCosmetics()
    {
        int skin = PlayerPrefs.GetInt("EquippedSkin", 0);
        ApplySkin(skin);
    }

    private void ApplySkin(int id)
    {
        if (id < 0 || id >= sprites.Length)
        {
            return;
        }
            
        for (int i = 0; i < sprites.Length; i++)
        {
            spriteRender.sprite = sprites[id];
            animation.runtimeAnimatorController = animationControllers[id];
        }
    }

    public void EquipSkin(int id)
    {
        PlayerPrefs.SetInt("EquippedSkin", id);
        ApplySkin(id);
    }

    public void PreviewSkin(int id)
    {
        ApplySkin(id);
    }

}
