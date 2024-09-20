using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Laser : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private float laserDistance = 8f;

    [SerializeField] private LayerMask ignoreMask;

    [SerializeField] private UnityEvent onHitTarget;

    public Animator robotToAnimate;

    private RaycastHit rayHit;
    private Ray ray;


    private void Start()
    {
        lineRenderer.positionCount = 2;
    }

    private void Update()
    {
        ray = new(transform.position, transform.forward);

        if (Physics.Raycast(ray, out rayHit, laserDistance, ~ignoreMask))
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, rayHit.point);
            if (rayHit.collider.TryGetComponent(out Target target))

            {
                AnimatorStateInfo stateInfo = robotToAnimate.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.fullPathHash == 533282942)
                {
                    target.Hit();
                    onHitTarget?.Invoke();
                }     
            }
        }
        else
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + transform.forward * laserDistance);
        }

   }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray.origin, ray.direction * laserDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(rayHit.point, 0.23f);


    }

}
