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
        public RectTransform RectTransform;
        public Vector2 ClosedSize;
        public Vector2 OpenSize;
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

    protected IEnumerator ExpandContainer(Container containerToUpdate, Vector2 goalSize, float animDuration, Action<bool> onTransitionEnd = null)
    {
        containerToUpdate.IsAnimating = true;
        containerToUpdate.RectTransform.gameObject.SetActive(true);
        RectTransform[] childObjects = containerToUpdate.RectTransform.GetComponentsInChildren<RectTransform>(true);
        for (int i = 1; i < childObjects.Length; i++)
        {
            childObjects[i].gameObject.SetActive(false);
        }
        Vector2 initSize = new Vector2(containerToUpdate.RectTransform.rect.width, containerToUpdate.RectTransform.rect.height);
        float time = 0;
        while (time < 1)
        {
            time += _tickFrequency / animDuration;
            containerToUpdate.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Lerp(initSize.x, goalSize.x, time));
            containerToUpdate.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Lerp(initSize.y, goalSize.y, time));
            yield return _delay;
        }
        containerToUpdate.IsAnimating = false;
        containerToUpdate.IsOpen = !containerToUpdate.IsOpen;
        containerToUpdate.RectTransform.gameObject.SetActive(containerToUpdate.IsOpen);
        if (containerToUpdate.IsOpen)
        {
            for (int i = 1; i < childObjects.Length; i++)
            {
                childObjects[i].gameObject.SetActive(true);
            }
        }
        onTransitionEnd?.Invoke(containerToUpdate.IsOpen);
    }
}