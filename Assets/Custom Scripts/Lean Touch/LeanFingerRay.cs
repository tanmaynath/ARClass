using UnityEngine;
using UnityEngine.Events;

namespace Lean.Touch
{
    // This script calls the OnFingerTap event when a finger taps the screen
    public class LeanFingerRay : Photon.MonoBehaviour
    {
        GameObject GameObjectSelected = null;
        // Event signature
        [System.Serializable] public class LeanFingerEvent : UnityEvent<LeanFinger> { }

        [Tooltip("If the finger is over the GUI, ignore it?")]
        public bool IgnoreIfOverGui;

        [Tooltip("If the finger started over the GUI, ignore it?")]
        public bool IgnoreIfStartedOverGui;

        [Tooltip("How many times must this finger tap before OnFingerTap gets called? (0 = every time)")]
        public int RequiredTapCount;

        [Tooltip("How many times repeating must this finger tap before OnFingerTap gets called? (e.g. 2 = 2, 4, 6, 8, etc) (0 = every time)")]
        public int RequiredTapInterval;

        //When the finger is pressed
        public LeanFingerEvent OnFingerDown;

       // public LeanFingerEvent OnFingerSet;
       // public LeanFingerEvent OnFingerUp;

        protected virtual void OnEnable()
        {
            // Hook events
            LeanTouch.OnFingerDown += FingerDown;
            //LeanTouch.OnFingerSet += FingerDown;
        }

        protected virtual void OnDisable()
        {
            // Unhook events
            LeanTouch.OnFingerDown -= FingerDown;
            //LeanTouch.OnFingerSet -= FingerDown;
            //LeanTouch.OnFingerUp -= FingerDown;
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

        private void FingerDown(LeanFinger finger)
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
            Ray touchRay = GenerateTouchRay();
            RaycastHit hit;

            if (Physics.Raycast(touchRay.origin, touchRay.direction, out hit))
            {
                //Fetch the GO which is touched
                GameObjectSelected = hit.transform.parent.gameObject;
                Debug.Log("Hit" + GameObjectSelected.name);

                //Determine it's child index 
                int indexOfChild=GameObjectSelected.transform.GetSiblingIndex();
                print(indexOfChild + " is the index of the child");

                //Fetch the parent of the GO
                GameObject ParentOfGameObjectSelected = GameObjectSelected.transform.parent.gameObject;

                //Activate the first sibling and de-active the second sibling and vice-versa
                if (indexOfChild == 0)
                {
                    ParentOfGameObjectSelected.transform.GetChild(0).GetComponent<LeanTranslateSmooth>().enabled = true;
                    ParentOfGameObjectSelected.transform.GetChild(0).GetComponent<LeanRotate>().enabled = true;
                    ParentOfGameObjectSelected.transform.GetChild(0).GetComponent<LeanScale>().enabled = true;

                    ParentOfGameObjectSelected.transform.GetChild(1).GetComponent<LeanTranslateSmooth>().enabled = false;
                    ParentOfGameObjectSelected.transform.GetChild(1).GetComponent<LeanRotate>().enabled = false;
                    ParentOfGameObjectSelected.transform.GetChild(1).GetComponent<LeanScale>().enabled = false;
                }
                else
                {
                    ParentOfGameObjectSelected.transform.GetChild(0).GetComponent<LeanTranslateSmooth>().enabled = false;
                    ParentOfGameObjectSelected.transform.GetChild(0).GetComponent<LeanRotate>().enabled = false;
                    ParentOfGameObjectSelected.transform.GetChild(0).GetComponent<LeanScale>().enabled = false;

                    ParentOfGameObjectSelected.transform.GetChild(1).GetComponent<LeanTranslateSmooth>().enabled = true;
                    ParentOfGameObjectSelected.transform.GetChild(1).GetComponent<LeanRotate>().enabled = true;
                    ParentOfGameObjectSelected.transform.GetChild(1).GetComponent<LeanScale>().enabled = true;
                }
                
            }
        }
    }
}