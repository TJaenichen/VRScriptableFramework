using Framework.Variables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleScript : MonoBehaviour
{
    public Vector3Reference ThumbOrientation;

    public void ClickButton(string ButtonName)
    {
        Debug.Log(ButtonName + " Was Clicked !");
    }

    public void ReleaseButton(string ButtonName)
    {
        Debug.Log(ButtonName + " Was Released !");
    }

    public void ThumbTouch(string ButtonName)
    {
        Debug.Log(ButtonName + " Was Touched !");
    }

    private void Update()
    {
        if (ThumbOrientation.Value != Vector3.zero)
        {
            Debug.Log("One Thumb is moving the joystick. The value is : " +
                ThumbOrientation.Value.ToString());
        }
    }
}
