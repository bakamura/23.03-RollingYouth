using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    protected void RoundButton() {
        Button[] btns = FindObjectsOfType<Button>();
        foreach (Button btn in btns) btn.image.alphaHitTestMinimumThreshold = 0;
    }

    protected IEnumerator ExpandContainer(RectTransform containerRect, Vector2 goalSize, float animDuration) {
        Vector2 initSize = new Vector2(containerRect.rect.width, containerRect.rect.height);
        float time = 0;
        while (time < 1) {
            time = Time.fixedDeltaTime / animDuration;
            containerRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Lerp(initSize.x, goalSize.x, time));
            containerRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Lerp(initSize.y, goalSize.y, time));
            yield return null;
        }
    }
}