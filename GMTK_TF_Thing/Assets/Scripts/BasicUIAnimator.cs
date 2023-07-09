using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicUIAnimator : MonoBehaviour
{
    [SerializeField] List<Sprite> sprites;
    [SerializeField] float delay;

    private float curDelay;
    private int pointer;
    private Image sRenderer;

    private Color c;

    private void Awake()
    {
        sRenderer = GetComponent<Image>();
        sRenderer.sprite = sprites[0];
        c = sRenderer.color;
    }

    private void Update()
    {
        curDelay += Time.deltaTime;
        if (curDelay >= delay)
        {
            curDelay -= delay;
            pointer++;
            if (pointer >= sprites.Count)
            {
                this.enabled = false;
                return;
            }
            sRenderer.sprite = sprites[pointer];
            sRenderer.color = sRenderer.sprite == null ? new Color(0, 0, 0, 0) : c;
        }
    }
}
