 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateFollow : Follow
{
    void Update()
    {
        Move(Time.deltaTime);
    }
}
