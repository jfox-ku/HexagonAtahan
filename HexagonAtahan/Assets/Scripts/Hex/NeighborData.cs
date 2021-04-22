using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NeighborData 
{
    
    public Dictionary<HexDirection, HexTile> NeighboursDict;
    public ColorInfoSO CenterColor;
    public HexTile MyCenter;

    public NeighborData(HexTile Center) {
        CenterColor = Center.CurrentHexColor();
        MyCenter = Center;
        NeighboursDict = new Dictionary<HexDirection, HexTile>() {
            { HexDirection.Top, Center.GetNeighbour(HexDirection.Top)},
            { HexDirection.TopRight, Center.GetNeighbour(HexDirection.TopRight) },
            { HexDirection.TopLeft, Center.GetNeighbour(HexDirection.TopLeft) },
            { HexDirection.Bottom, Center.GetNeighbour(HexDirection.Bottom) },
            { HexDirection.BottomRight, Center.GetNeighbour(HexDirection.BottomRight) },
            { HexDirection.BottomLeft, Center.GetNeighbour(HexDirection.BottomLeft)}

        };

        

    }


    public List<HexTile> FindViableCorner(int offset=0) {
        List<HexTile> Viables = new List<HexTile>();
        var directions = (HexDirection[])Enum.GetValues(typeof(HexDirection));
        string output = "";
        foreach (HexDirection dir in directions) {
           
            if (NeighboursDict[dir] == null) {
                output += "0";
            } else {
                output += "1";
            }
        }
        //Debug.Log(output);

        //Checking if 2 directions are valid and next to each other
        //Offset is to cycle between valid choices, up to 6, one for each side
        int firstIndex = output.IndexOf("11",offset);

        if (firstIndex != -1) {
            Viables.Add(NeighboursDict[(HexDirection)firstIndex]);
            Viables.Add(NeighboursDict[(HexDirection)firstIndex + 1]);
        }
        
        //Edge case for Top, TopLeft 
        if (firstIndex==-1 && output[0]=='1' && output[output.Length-1] == '1') {
            Viables.Add(NeighboursDict[HexDirection.Top]);
            Viables.Add(NeighboursDict[HexDirection.TopLeft]);
        }

        Viables.Add(MyCenter);

        if(Viables.Count != 3 && offset > 0) {
            Viables = FindViableCorner(offset - 1);
        }

        return Viables;
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
