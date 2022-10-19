using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedUpdateFollow : Follow
{
    
    void FixedUpdate()
    {
        Move(Time.fixedDeltaTime);
    }
}
