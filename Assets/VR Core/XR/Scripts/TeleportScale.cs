using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScale : MonoBehaviour
{
    public float desiredSize = 0.2f;
    public float desiredHeight = 1f;
    public float maxScaleDistance = 10f;
    public AnimationCurve scaleByDistance;
    Transform player;

    private void Start()
    {
        player = PersistentManager.Instance.GetPlayer().transform;
    }

    private void OnEnable()
    {
        Scale();
    }

    // Update is called once per frame
    private void Update()
    {
        Scale();
    }

    void Scale()
    {
        if (player != null)
        {
            float dist = Vector3.Distance(transform.position, player.position);
            transform.localScale = Vector3.one * desiredSize * scaleByDistance.Evaluate(Mathf.Clamp(dist / maxScaleDistance, 0f, maxScaleDistance));
            transform.localPosition = Vector3.up * desiredHeight;
        }
    }
}
