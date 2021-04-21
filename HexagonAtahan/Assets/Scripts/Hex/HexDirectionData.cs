using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexDirectionData {


    public static Dictionary<HexDirection, Vector2> EvenDict = new Dictionary<HexDirection, Vector2>() {
             { HexDirection.Top, new Vector2(0,1) },
            { HexDirection.TopRight, new Vector2(1,1) },
            { HexDirection.TopLeft, new Vector2(-1,1) },
            { HexDirection.Bottom, new Vector2(0,-1) },
            { HexDirection.BottomRight, new Vector2(1,0) },
            { HexDirection.BottomLeft, new Vector2(-1,0)}


    };


    public static Dictionary<HexDirection, Vector2> OddDict = new Dictionary<HexDirection, Vector2>() {
             { HexDirection.Top, new Vector2(0,1) },
            { HexDirection.TopRight, new Vector2(1,0) },
            { HexDirection.TopLeft, new Vector2(-1,0) },
            { HexDirection.Bottom, new Vector2(0,-1) },
            { HexDirection.BottomRight, new Vector2(1,-1) },
            { HexDirection.BottomLeft, new Vector2(-1,-1)}


    };


}
