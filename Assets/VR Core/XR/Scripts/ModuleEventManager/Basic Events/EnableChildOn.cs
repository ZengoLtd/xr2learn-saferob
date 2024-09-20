/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MEM
{


    //triggers unity event if the listened event is fired
    public class EnableChildOn : ModuleEventListenerBase
    {
        public bool addDelay = false;
        public float delayTime = 1f;

        protected override void OnEvent(string eventName, object value)
        {

            if (!variables.declarations.IsDefined(eventName))
            {
                return;
            }

            if ((variables.declarations.Get(eventName).Equals(value)))
            {
                if (addDelay)
                {
                    StartCoroutine(Delayed(delayTime));
                }
                else
                {
                    transform.GetChild(0).gameObject.SetActive(true);
                }
            }

           

        }
        IEnumerator Delayed(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}*/