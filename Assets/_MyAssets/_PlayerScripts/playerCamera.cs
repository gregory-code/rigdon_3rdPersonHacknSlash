using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

[ExecuteAlways]
public class playerCamera
{
    GameObject _owner;

    public playerCamera(GameObject myOwner)
    {
        _owner = myOwner;

    }

    public void HandleRotation(Vector2 lookVector)
    {

    }
}
