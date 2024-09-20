using System.Collections;
using System.Reflection;
using UnityEngine;

namespace BNGExtension
{
    public class PlayerTeleport : BNG.PlayerTeleport
    {

        
        void OnEnable()
        {
            EventManager.OnPlayerTeleport += TeleportPlayerToFloorDelayed;

        }


        bool previousAimState = false;
        void AimEvent()
        {
            if (AimingTeleport != previousAimState)
            {
                if (AimingTeleport)
                {
                    EventManager.TeleporterAimStart();
                }
                else
                {
                    EventManager.TeleporterAimEnd();
                }
                previousAimState = AimingTeleport;
            }
        }

        public override void BeforeTeleport()
        {
            base.BeforeTeleport();
            EventManager.PlayerTeleportBeginning();
            

        }
        public override void AfterTeleport() {
            
            base.AfterTeleport();
             EventManager.PlayerTeleportEnd();
        }

        void LateUpdate()
        {


            // Are we pressing button to check for teleport?
            aimingTeleport = KeyDownForTeleport();

            if (aimingTeleport)
            {
                DoCheckTeleport();
            }
            // released key, finish teleport or just hide graphics
            else if (KeyUpFromTeleport())
            {
                TryOrHideTeleport();
            }

            if (aimingTeleport)
            {
                calculateParabola();
            }
            AimEvent();
        }

        protected override void calculateParabola()
        {
            base.calculateParabola();
            Collider _hitObject = (Collider)typeof(PlayerTeleport).BaseType.GetField("_hitObject", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(this);
           
            if (_hitObject?.GetComponent<TeleportDestinationMarker>() != null)
            {
                _hitObject?.GetComponent<TeleportDestinationMarker>().HideMarker();
            }

        }

        IEnumerator TeleportAfter(Vector3 destination, Quaternion rotation, bool keepRotation, GameObject destinationobject)
        {
            //yield return new WaitForSeconds(TeleportDelay);
            TeleportPlayerToFloor(destination, rotation, keepRotation, destinationobject);
            yield return null;
            
        }
        public void TeleportPlayerToFloorDelayed(Vector3 destination, Quaternion rotation, bool keepRotation, GameObject destinationobject)
        {
            StartCoroutine(TeleportAfter(destination, rotation, keepRotation, destinationobject));
        }
        public void TeleportPlayerToFloor(Vector3 destination, Quaternion rotation, bool keepRotation, GameObject destinationobject)
        {
            Transform cameraRig = (Transform)typeof(PlayerTeleport).BaseType.GetField("cameraRig", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(this);
            BNG.BNGPlayerController playerController = (BNG.BNGPlayerController)typeof(PlayerTeleport).BaseType.GetField("playerController", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(this);
          

            if (cameraRig == null)
            {
                cameraRig = (Transform)Helper.ReflectionValue(typeof(BNG.PlayerTeleport), "cameraRig", this);
            }
            if (playerController == null)
            {
                playerController = (BNG.BNGPlayerController)Helper.ReflectionValue(typeof(BNG.PlayerTeleport), "playerController", this);
            }

            float yOffset = 1.5f + cameraRig.localPosition.y - playerController.CharacterControllerYOffset;

            if (keepRotation)
            {
                rotation = playerController.transform.rotation;
            }

            if (destinationobject?.GetComponent<TeleportDestinationMarker>())
            {
                destinationobject.SetActive(true);
                destinationobject.GetComponent<TeleportDestinationMarker>().OnBeforePlayerTeleported.Invoke();
                if (!destinationobject.GetComponent<TeleportDestinationMarker>().ForcePlayerRotation)
                {
                    rotation = playerController.transform.rotation;
                }
            }
            TeleportPlayer(destination + new Vector3(0, yOffset, 0), rotation);
        }
        void OnDisable()
        {
            EventManager.OnPlayerTeleport -= TeleportPlayerToFloorDelayed;
        }
        protected override void tryTeleport()
        {

            if (validTeleport)
            {

                // Call any events, fade screen, etc.
                //                    Debug.Log("tryTeleport");
                //BeforeTeleportFade();
                ScreenFader.Instance?.StartTransition();

                Vector3 destination = TeleportDestination.position;
                Quaternion rotation = TeleportMarker.transform.rotation;
                // Store our rotation setting. This can be overriden by a TeleportDestination's ForcePlayerRotation setting
                bool allowTeleportationRotation = AllowTeleportRotation;

                // Override if we're looking at a teleport destination
                if (DestinationObject == null)
                {
                    Collider _hitObject = (Collider)typeof(PlayerTeleport).BaseType.GetField("_hitObject", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(this);
            
                    DestinationObject = _hitObject.GetComponent<BNG.TeleportDestination>();
                }

                if (DestinationObject != null)
                {
                    destination = DestinationObject.DestinationTransform.position;

                    // ForcePlayerRotation will get passed to the coroutine if true
                    if (DestinationObject.ForcePlayerRotation)
                    {
                        rotation = DestinationObject.DestinationTransform.rotation;
                        allowTeleportationRotation = true;
                    }
                    else
                    {
                        BNG.BNGPlayerController playerController = (BNG.BNGPlayerController)typeof(PlayerTeleport).BaseType.GetField("playerController", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(this);
                        
                        rotation = playerController.transform.rotation;
                    }
                }

                // Offset our teleport vector if specified
                if (TeleportYOffset != 0)
                {
                    destination += new Vector3(0, TeleportYOffset, 0);
                }
                TeleportPlayerToFloor(destination, rotation, false, null);
                //StartCoroutine(doTeleport(destination, rotation, allowTeleportationRotation));
            }

            // We teleported, so update this value for next raycast
            validTeleport = false;
            aimingTeleport = false;

            hideTeleport();
        }


    }
}
