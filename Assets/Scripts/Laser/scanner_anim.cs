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
        // Sz�moljuk az �j offset �rt�ket az id� alapj�n
        float offsetX = Time.time * scrollSpeedX;
        float offsetY = Time.time * scrollSpeedY;

        // Alkalmazzuk az offsetet a text�r�ra
        material.mainTextureOffset = new Vector2(offsetX, offsetY);
    }
}

