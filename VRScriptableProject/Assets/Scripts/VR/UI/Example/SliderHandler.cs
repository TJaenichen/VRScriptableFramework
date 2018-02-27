using Framework.Variables;
using Framework.VR.Controllers;
using Framework.VR.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// To attach to the slider object
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
	public class SliderHandler : MonoBehaviour
    {
        #region PUBLIC_VARIABLES
        public float fillTime = 3f;
        #endregion PUBLIC_VARIABLES


        #region PRIVATE_VARIABLES
        private Slider m_Slider;
        private bool m_BarFilled;                                           // Whether the bar is currently filled.
        private float m_Timer;                                              // Used to determine how much of the bar should be filled.
        private Coroutine m_FillBarRoutine;                                 // Reference to the coroutine that controls the bar filling up, used to stop it if required.

        private bool isSetup;                                               // For init
        private bool gazeOver;                                    // Whether the user is clicking on the fill bar
        private bool startRotation;
        private PointerRayCast pointerRaycast;                              // To check the controllers raycast hit
        private Collider sliderCollider;

        private Hand handFilling = Hand.NULL;
        private Quaternion startingRot;
        #endregion PRIVATE_VARIABLES

        private const string k_SliderMaterialPropertyName = "_SliderValue"; // The name of the property on the SlidingUV shader that needs to be changed in order for it to fill.


        #region MONOBEHAVIOUR_METHODS
		// Update is called once per frame
		void Update () 
		{
            if (!isSetup)
            {
                CheckReferences();
                return;
            }

            //If user is looking at the bar
            if (HandleDown(pointerRaycast.GazeHits))
                handFilling = Hand.GAZE;
            //If user is not looking at the bar anymore
            else
                HandleUp();

            if (startRotation && transform.root.localRotation.z > -1f)
                transform.root.Rotate(0, 0, 1);
            else if (startRotation && transform.root.localRotation.z < -1f)
            {
                transform.root.localRotation = startingRot;
                startRotation = false;
            }
        }
        #endregion MONOBEHAVIOUR_METHODS
        

        #region PUBLIC_METHODS
        public IEnumerator WaitForBarToFill()
        {
            // Currently the bar is unfilled.
            m_BarFilled = false;

            // Reset the timer and set the slider value as such.
            m_Timer = 0f;
            SetSliderValue(0f);

            // Keep coming back each frame until the bar is filled.
            while (!m_BarFilled)
            {
                yield return null;
            }
        }
        #endregion PUBLIC_METHODS


        #region PRIVATE_METHODS
        private IEnumerator FillBar()
        {
            // When the bar starts to fill, reset the timer.
            m_Timer = 0f;

            // Until the timer is greater than the fill time...
            while (m_Timer < fillTime)
            {
                // ... add to the timer the difference between frames.
                m_Timer += Time.deltaTime;

                // Set the value of the slider or the UV based on the normalised time.
                SetSliderValue(m_Timer / fillTime);

                // Wait until next frame.
                yield return null;

                // If the user is still looking at the bar, go on to the next iteration of the loop.
                if (gazeOver)
                    continue;

                // If the user is no longer looking at the bar, reset the timer and bar and leave the function.
                m_Timer = 0f;
                SetSliderValue(0f);
                yield break;
            }

            // If the loop has finished the bar is now full.
            m_BarFilled = true;

            startRotation = true;
        }


        private void SetSliderValue(float sliderValue)
        {
            // If there is a slider component set it's value to the given slider value.
            if (m_Slider)
                m_Slider.value = sliderValue;
        }
        

        private bool HandleDown(List<RaycastHit> hits)
        {
            foreach (RaycastHit hit in hits)
            {
                // If the user is looking at the bar start the FillBar coroutine and store a reference to it.
                if (hit.collider == sliderCollider)
                {
                    gazeOver = true;
                    m_FillBarRoutine = StartCoroutine(FillBar());
                    return true;
                } 
                else
                {
                    gazeOver = false;
                }
            }
            return false;
        }


        private void HandleUp()
        {
            // If the coroutine has been started (and thus we have a reference to it) stop it.
            if (m_FillBarRoutine != null)
                StopCoroutine(m_FillBarRoutine);

            // Reset the timer and bar values.
            m_Timer = 0f;
            SetSliderValue(0f);
            handFilling = Hand.NULL;
            gazeOver = false;
        }

        private void CheckReferences()
        {
            try
            {
                pointerRaycast = SetupVR.ActiveSDK.GetComponent<PointerRayCast>();
                sliderCollider = GetComponent<BoxCollider>();
                m_Slider = GetComponent<Slider>();
                startingRot = transform.root.localRotation;
                isSetup = true;
            }
            catch
            {
                Debug.Log("CameraRig not in Scene.");
            }
        }
        #endregion PRIVATE_METHODS


        // EMPTY
        #region GETTERS_SETTERS

        #endregion GETTERS_SETTERS
    }
}