using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NeighborData {

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

    //Finds a corner with  3 non-null HexTiles from this center, 
    //offset is used to pick different corners
    public List<HexTile> FindViableCorner(int offset = 0) {
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
    
        //Checking if 2 directions are valid and next to each other
        //Offset is to cycle between valid choices, up to 6, one for each side
        int firstIndex = output.IndexOf("11", offset);

        if (firstIndex != -1) {
            Viables.Add(NeighboursDict[(HexDirection)firstIndex]);
            Viables.Add(NeighboursDict[(HexDirection)firstIndex + 1]);
        }

        //Edge case for Top, TopLeft 
        if (firstIndex == -1 && output[0] == '1' && output[output.Length - 1] == '1') {
            Viables.Add(NeighboursDict[HexDirection.Top]);
            Viables.Add(NeighboursDict[HexDirection.TopLeft]);
        }

        Viables.Add(MyCenter);

        //Recursive call with lower offset in case the offset prevents finding the pair.
        if (Viables.Count != 3 && offset > 0) {
            Viables = FindViableCorner(offset - 1);
        }

        return Viables;
    }

    //Detects adjacent matches
    public HashSet<HexTile> JointMatches(ColorInfoSO CSO) {
        HashSet<HexTile> ret = new HashSet<HexTile>();
        if (MyCenter.CurrentHexColor() != CSO) return ret;

        for (int i = 0; i < 6; i++) {
            var tile1 = NeighboursDict[(HexDirection)i];
            var tile2 = NeighboursDict[(HexDirection)(i + 1 > 5 ? 0 : i + 1)];
            if (tile1 == null || tile2 == null || tile1.isEmpty() || tile2.isEmpty()) continue;
            if (tile1.CurrentHexColor() != CenterColor || tile2.CurrentHexColor() != CenterColor) continue;

            if (tile1.CurrentHexColor() == tile2.CurrentHexColor()) {
                ret.Add(tile1);
                ret.Add(tile2);
            }
        }

        if (ret.Count != 0) {
            ret.Add(MyCenter);
        }
        return ret;
    }


    //This approach return false for some cases where a match was actually possible. (Single piece rotated into a pair)
    //Only checking matching pairs of HexTiles is enough since the tiles this would match with would have to be paired.
    public bool IsMatchPossibleMaybe() {
        for (int i = 0; i < 6; i++) {
            var tile = NeighboursDict[(HexDirection)i];
            //Find neighbour of same color
            if (tile?.CurrentHexColor() == CenterColor) {
                //Get common neighbours with the match
                var clockWiseNeigh = NeighboursDict[(HexDirection)((i + 1) > 5 ? 0 : (i + 1))];
                var cclockWiseNeigh = NeighboursDict[(HexDirection)((i - 1) < 0 ? 5 : (i - 1))];
                //Check if those common neighbours have more than just the original two as neighbours with CenterColor
                if (clockWiseNeigh?.NeighbourColorCount(CenterColor) > 2 || cclockWiseNeigh?.NeighbourColorCount(CenterColor) > 2) {
                    return true;
                }


            }


        }

        return false;

    }


    //Returns matching neighbours of CenterColor or ColorOverride
    public List<HexTile> GetMatchingNeighbours(ColorInfoSO ColorOverride = null) {
        List<HexTile> Ret = new List<HexTile>();
        ColorInfoSO toCheck = CenterColor;
        if (ColorOverride != null) {
            toCheck = ColorOverride;
        }


        foreach (KeyValuePair<HexDirection, HexTile> entry in NeighboursDict) {
            if (entry.Value != null) {
                if (entry.Value.CurrentHexColor() == toCheck) {
                    Ret.Add(entry.Value);
                }
            }
        }
        return Ret;
    }



}
