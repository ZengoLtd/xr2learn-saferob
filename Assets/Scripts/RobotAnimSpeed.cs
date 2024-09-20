using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAnimSpeed : MonoBehaviour
{
    public float waitTime = 2.0f;
    public string speedParameterName = "SpeedMultiplier";
    public string pickUPParameterName = "PickUp";
    public string CNCoutParameterName = "CNCout";
    public string maintenance = "Maintenance";
    public float slowdownFactor = 0.5f;

    public void EnterCollider(bool isInner)
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            if (isInner)
            {
                animator.SetFloat(speedParameterName, 0f);
            }
            else
            {
                animator.SetFloat(speedParameterName, slowdownFactor);
            }
        }
    }

    public void EnableMaintenance()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger(maintenance);
        }
    }
    public void DisableMaintenance()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger(maintenance);
        }
    }

    public void ExitCollider()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetFloat(speedParameterName, 1f);
        }
    }

    public void PickUPAnim()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("New State"))
            {
                animator.SetTrigger(pickUPParameterName);
                animator.ResetTrigger(CNCoutParameterName);
            }
            
        }
    }

    private void CNCOutAnim()
    {
        StartCoroutine(WaitForOut());
    }

    IEnumerator WaitForOut()
    {
       yield return new WaitForSeconds(waitTime);
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger(CNCoutParameterName);
            animator.ResetTrigger(pickUPParameterName);
        }
    }
}
