﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelScript : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer gemsMapRenderer;
    public void SetDisplayed(bool value)
    {
        spriteRenderer.enabled = value;
        gemsMapRenderer.enabled = value;
    }
}
