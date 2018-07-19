using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions{


        public static Vector2 GetOrthogonalVector(this Vector2 vec)
        {
            return new Vector2(-vec.y, vec.x);
        }

}
