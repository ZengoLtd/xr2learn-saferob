
using System.Reflection;
using UnityEngine;



namespace BNGExtension{
    public class PlayerSnapTeleport : BNG.PlayerTeleport
    {
      
        void OnEnable(){
            EventManager.OnPlayerTeleport += TeleportPlayerToFloor;
        }
        
        public void TeleportPlayerToFloor(Vector3 destination, Quaternion rotation,bool keepRotation,GameObject destinationobject) {
                       
            Transform cameraRig =  (Transform)typeof(Transform).BaseType.GetField("cameraRig", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(this);;
            BNG.BNGPlayerController playerController = (BNG.BNGPlayerController)typeof(PlayerSnapTeleport).BaseType.GetField("playerController", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(this);

            if(cameraRig == null){
                cameraRig = (Transform) Helper.ReflectionValue(typeof(BNG.PlayerTeleport), "cameraRig", this);
            }
            
            if(playerController == null){
                playerController = (BNG.BNGPlayerController) Helper.ReflectionValue(typeof(BNG.PlayerTeleport), "playerController", this);
            }
            
            float yOffset = 1 + cameraRig.localPosition.y - playerController.CharacterControllerYOffset;
            Debug.Log("yOffset: " + yOffset);
            if(destinationobject?.GetComponent<TeleportDestinationMarker>()){
                destinationobject.GetComponent<TeleportDestinationMarker>().OnBeforePlayerTeleported.Invoke();
            }
            TeleportPlayer(destination + new Vector3(0,yOffset,0), rotation);
        }

        void OnDisable(){
            EventManager.OnPlayerTeleport -= TeleportPlayerToFloor;
        }
        Vector3[] segments;
        Vector3 segVelocity;
        bool isDestination = false;
        Transform teleportTransform {
            get {
                return HandSide == BNG.ControllerHand.Left ? TeleportBeginTransform : TeleportBeginTransformAlt;
            }
        }
        Collider _hitObject;
        private Vector3 _hitVector;
        float segTime;
        int segCount;
        RaycastHit hit;
        float _hitAngle;
        protected override void calculateParabola() {
            validTeleport = false;
            isDestination = false;

            // Update our array if length was changed dynamically
            if (segments.Length != SegmentCount) {
                segments = new Vector3[SegmentCount];
            }

            segments[0] = teleportTransform.position;
            // Initial velocity
            segVelocity = teleportTransform.forward * SimulationVelocity * Time.fixedDeltaTime;

            // Switch to unscaled delta time if we are in slow-mo, but not paused
            if(Time.timeScale < 0.95f && Time.timeScale > 0f) {
                segVelocity = teleportTransform.forward * SimulationVelocity * Time.fixedUnscaledDeltaTime;
            }

            _hitObject = null;
            segCount = 0;

            for (int i = 1; i < SegmentCount; i++) {
                segCount++;

                // Time it takes to traverse one segment of length segScale (careful if velocity is zero)
                segTime = (segVelocity.sqrMagnitude != 0) ? SegmentScale / segVelocity.magnitude : 0;

                // Add velocity from gravity for this segment's timestep
                segVelocity = segVelocity + Physics.gravity * segTime;

                // Check to see if we're going to hit a physics object
                if (Physics.Raycast(segments[i - 1], segVelocity, out hit, SegmentScale, CollisionLayers)) {

                    // remember who we hit
                    _hitObject = hit.collider;

                    // set next position to the position where we hit the physics object
                    segments[i] = segments[i - 1] + segVelocity.normalized * hit.distance;

                    // correct ending velocity, since we didn't actually travel an entire segment
                    segVelocity = segVelocity - Physics.gravity * (SegmentScale - hit.distance) / segVelocity.magnitude;                   

                    _hitAngle = Vector3.Angle(transform.up, hit.normal);

                    // Align marker to hit normal
                    TeleportMarker.transform.position = segments[i]; // hit.point;
                    TeleportMarker.transform.rotation = Quaternion.FromToRotation(TeleportMarker.transform.up, hit.normal) * TeleportMarker.transform.rotation;

                    // Snap to Teleport Destination
                    DestinationObject = _hitObject.GetComponent<BNG.TeleportDestination>();
                    if(DestinationObject != null) {
                        isDestination = true;
                        TeleportMarker.transform.position = DestinationObject.transform.position;
                        TeleportMarker.transform.rotation = DestinationObject.transform.rotation;
                    }

                    _hitVector = segments[i];

                    break;
                }
                // Nothing hit, continue line by settings next segment to the last
                else {
                    segments[i] = segments[i - 1] + segVelocity * segTime;
                }
            }

            validTeleport = _hitObject != null;

         
            if(validTeleport && !isDestination) {

                // Angle too steep
                if (_hitAngle > MaxSlope) {
                    validTeleport = false;
                }

                if (!teleportClear() ) {
                    validTeleport = false;
                }
            }

          
            TeleportLine.positionCount = segCount;
            for (int i = 0; i < segCount; i++) {
                TeleportLine.SetPosition(i, segments[i]);
            }

        }
        
    }
}
