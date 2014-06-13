using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class GameObjectManager
{
    public static HashSet<Character> characters = new HashSet<Character>();


    public static Character findByRange(Vector3 pos, int range, int type)
    {
        foreach (Character o in characters)
        {
            if (o.type != type)
                continue;

            float dis = Vector3.Distance(o.gameObject.transform.position, pos);
            if (dis <= range)
                return o;
        }

        return null;
    }
}

