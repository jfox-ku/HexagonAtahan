using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonScript : MonoBehaviour
{
    [SerializeField] private SpriteRenderer HexSpriteRenderer;

    public ColorInfoSO ColorSO { get; private set; }
    public void SetColor(ColorInfoSO info) {
        ColorSO = info;
        HexSpriteRenderer.color = ColorSO.Clr;
    }


}
