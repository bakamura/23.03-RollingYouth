using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UI : MonoBehaviour
{
    private WaitForSecondsRealtime _delay;
    private const float _tickFrequency = .02f;

    [Serializable]
    public class Container
    {
        public RectTransform rectTransform;
        public Vector2 closedSize;
        public Vector2 openSize;
        [HideInInspector] public bool IsOpen;
        [HideInInspector] public bool IsAnimating;
    }

    private void Awake()
    {        
        _delay = new WaitForSecondsRealtime(_tickFrequency);
    }

    protected void RoundButton()
    {
        Button[] btns = FindObjectsOfType<Button>();
        foreach (Button btn in btns) btn.image.alphaHitTestMinimumThreshold = 0;
    }

    protected IEnumerator ExpandContainer(Container containerToUpdate, Vector2 goalSize, float animDuration)
    {
        containerToUpdate.IsAnimating = true;
        containerToUpdate.rectTransform.gameObject.SetActive(true);
        Vector2 initSize = new Vector2(containerToUpdate.rectTransform.rect.width, containerToUpdate.rectTransform.rect.height);
        float time = 0;
        while (time < 1)
        {
            time += _tickFrequency / animDuration;
            containerToUpdate.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Lerp(initSize.x, goalSize.x, time));
            containerToUpdate.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Lerp(initSize.y, goalSize.y, time));
            yield return _delay;
        }
        containerToUpdate.IsAnimating = false;
        containerToUpdate.IsOpen = !containerToUpdate.IsOpen;
        containerToUpdate.rectTransform.gameObject.SetActive(containerToUpdate.IsOpen);        
    }
}