using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighborData 
{
    
    public Dictionary<HexDirection, HexTile> NeighboursDict;
    public ColorInfoSO CenterColor;

    public NeighborData(HexTile Center) {
        CenterColor = Center.CurrentHexColor();
        NeighboursDict = new Dictionary<HexDirection, HexTile>() {
            { HexDirection.Top, Center.GetNeighbour(HexDirection.Top)},
            { HexDirection.TopRight, Center.GetNeighbour(HexDirection.TopRight) },
            { HexDirection.TopLeft, Center.GetNeighbour(HexDirection.TopLeft) },
            { HexDirection.Bottom, Center.GetNeighbour(HexDirection.Bottom) },
            { HexDirection.BottomRight, Center.GetNeighbour(HexDirection.BottomRight) },
            { HexDirection.BottomLeft, Center.GetNeighbour(HexDirection.BottomLeft)}

        };

        

    }

    public List<HexTile> GetMatchingNeighbours() {
        List<HexTile> Ret = new List<HexTile>();

        foreach (KeyValuePair<HexDirection,HexTile> entry in NeighboursDict) {
            if (entry.Value != null) {

                if(entry.Value.CurrentHexColor() == CenterColor) {
                    Ret.Add(entry.Value);


                }

            }
        }

        return Ret;

    }



}
