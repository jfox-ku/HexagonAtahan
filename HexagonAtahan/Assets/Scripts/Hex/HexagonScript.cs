using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonScript : MonoBehaviour {
    [SerializeField] private SpriteRenderer HexSpriteRenderer;
    public VoidEvent HexagonDestroyedEvent;

    public ColorInfoSO ColorSO { get; private set; }
    public void SetColor(ColorInfoSO info) {
        ColorSO = info;
        HexSpriteRenderer.color = ColorSO.Clr;
    }

    public void FadeIn() {
        StartCoroutine(FadeInAnim());
    }

    public IEnumerator FadeInAnim() {

        float fadeTime = 1f;
        float fadeTimer = fadeTime;

        Color baseColor = ColorSO.Clr;

        while (fadeTimer > 0) {
            baseColor.a = (fadeTime - fadeTimer) / fadeTime;

            fadeTimer -= Time.deltaTime;
            HexSpriteRenderer.color = baseColor;
            yield return null;
        }

        HexSpriteRenderer.color = ColorSO.Clr;

    }


    public void FlashAndDestroy() {
        StartCoroutine(FlashDestroy());
    }

    private IEnumerator FlashDestroy() {
        Color flashColor = Color.white;
        flashColor.a = 0.9f;
        HexSpriteRenderer.color = flashColor;
        yield return new WaitForSeconds(0.25f);
        Destroy(this.gameObject);


    }

    private void OnDestroy() {
        HexagonDestroyedEvent.Raise(new Void());
    }


}
