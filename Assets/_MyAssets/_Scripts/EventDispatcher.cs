using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEventDispatcher
{
    public void SendEvent(AnimEvent animEvent);
}
