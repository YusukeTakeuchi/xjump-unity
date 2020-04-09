using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lib.PositionUtil
{
    public static class PositionUtil
    {
        public static float PosX(this GameObject gameObject) =>
            gameObject.transform.position.x;

        public static float PosY(this GameObject gameObject) =>
            gameObject.transform.position.y;

    }

}