using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="animEvent")]
public class AnimEvent : ScriptableObject
{
    public string functionName;
    public string hitAnim;
    public int time;
    public int speed;
    public int intensity;
    public int spawn;
}
