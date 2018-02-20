using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.VR.Utils
{
	public class VRInteractiveItem : MonoBehaviour 
	{
        #region PUBLIC_VARIABLES
        public event Action OnOver;             // Called when the gaze moves over this object
        public event Action OnOut;              // Called when the gaze leaves this object
        public event Action OnClick;            // Called when click input is detected whilst the gaze is over this object.
        public event Action OnDoubleClick;      // Called when double click input is detected whilst the gaze is over this object.
        public event Action OnUp;               // Called when Fire1 is released whilst the gaze is over this object.
        public event Action OnDown;             // Called when Fire1 is pressed whilst the gaze is over this object.
        #endregion PUBLIC_VARIABLES
        
        #region PRIVATE_VARIABLES
        protected bool m_IsOver;
        #endregion PRIVATE_VARIABLES
        
        //EMPTY
        #region MONOBEHAVIOUR_METHODS
        #endregion MONOBEHAVIOUR_METHODS


        #region PUBLIC_METHODS
        // The below functions are called by the VREyeRaycaster when the appropriate input is detected.
        // They in turn call the appropriate events should they have subscribers.
        public void Over()
        {
            m_IsOver = true;

            if (OnOver != null)
                OnOver();
        }


        public void Out()
        {
            m_IsOver = false;

            if (OnOut != null)
                OnOut();
        }


        public void Click()
        {
            if (OnClick != null)
                OnClick();
        }


        public void DoubleClick()
        {
            if (OnDoubleClick != null)
                OnDoubleClick();
        }


        public void Up()
        {
            if (OnUp != null)
                OnUp();
        }


        public void Down()
        {
            if (OnDown != null)
                OnDown();
        }
        #endregion PUBLIC_METHODS


        // EMPTY
        #region PRIVATE_METHODS

        #endregion PRIVATE_METHODS

        #region GETTERS_SETTERS
        public bool IsOver
        {
            get { return m_IsOver; }              // Is the gaze currently over this object?
        }
	    #endregion GETTERS_SETTERS
	}
}