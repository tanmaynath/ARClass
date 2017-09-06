using UnityEngine;
using UnityEngine.Events;

namespace Lean.Touch
{
	// This script calls the OnFingerTap event when a finger taps the screen
	public class PhotonLeanFingerTapHighlight : Photon.MonoBehaviour
	{
        GameObject GameObjectSelected = null;
        // Event signature
        [System.Serializable] public class LeanFingerEvent : UnityEvent<LeanFinger> {}

		[Tooltip("If the finger is over the GUI, ignore it?")]
		public bool IgnoreIfOverGui;

		[Tooltip("If the finger started over the GUI, ignore it?")]
		public bool IgnoreIfStartedOverGui;

		[Tooltip("How many times must this finger tap before OnFingerTap gets called? (0 = every time)")]
		public int RequiredTapCount;

		[Tooltip("How many times repeating must this finger tap before OnFingerTap gets called? (e.g. 2 = 2, 4, 6, 8, etc) (0 = every time)")]
		public int RequiredTapInterval;

		public LeanFingerEvent OnFingerTap;
		
		protected virtual void OnEnable()
		{
			// Hook events
			LeanTouch.OnFingerTap += FingerTap;
		}
		
		protected virtual void OnDisable()
		{
			// Unhook events
			LeanTouch.OnFingerTap -= FingerTap;
		}

        Ray GenerateTouchRay()
        {

            Vector3 touchPosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
            Vector3 touchPosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);

            Vector3 touchPosF = Camera.main.ScreenToWorldPoint(touchPosFar);
            Vector3 touchPosN = Camera.main.ScreenToWorldPoint(touchPosNear);

            Ray ray = new Ray(touchPosN, touchPosF - touchPosN);

            return ray;
        }


        private void FingerTap(LeanFinger finger)
        {
            // Ignore?
            if (IgnoreIfOverGui == true && finger.IsOverGui == true)
            {
                return;
            }

            if (IgnoreIfStartedOverGui == true && finger.StartedOverGui == true)
            {
                return;
            }

            if (RequiredTapCount > 0 && finger.TapCount != RequiredTapCount)
            {
                return;
            }

            if (RequiredTapInterval > 0 && (finger.TapCount % RequiredTapInterval) != 0)
            {
                return;
            }

            // Call event
            //OnFingerTap.Invoke(finger);

            if (RequiredTapCount == 1)
            {
                Ray touchRay = GenerateTouchRay();
                RaycastHit hit;

                if (Physics.Raycast(touchRay.origin, touchRay.direction, out hit))
                {
                    GameObjectSelected = hit.transform.gameObject;
                    Debug.Log("Hit" + GameObjectSelected.name);
                    photonView.RPC("HighlightSelectedObject", PhotonTargets.AllBufferedViaServer, GameObjectSelected.GetPhotonView().viewID);
                }
            }
        }
	}
}