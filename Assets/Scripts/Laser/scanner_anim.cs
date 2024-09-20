using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scanner_anim : MonoBehaviour
{
    public Material material;
    public float scrollSpeedX = 0.5f;
    public float scrollSpeedY = 0.5f;

    void Update()
    {
        // Számoljuk az új offset értéket az idõ alapján
        float offsetX = Time.time * scrollSpeedX;
        float offsetY = Time.time * scrollSpeedY;

        // Alkalmazzuk az offsetet a textúrára
        material.mainTextureOffset = new Vector2(offsetX, offsetY);
    }
}

