using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    
    public int row;
    public int column;

    public bool isOdd;

    public HexagonScript CurrentHexagon;
    public static HexMap Map;
    public NeighborData NeighData;

    private Dictionary<HexDirection, Vector2> DictReference;

   

    public void Init(int row,int column) {
        this.row = row;
        this.column = column;
        isOdd = row % 2 == 1 ? true : false;
        DictReference = this.isOdd ? HexDirectionData.OddDict : HexDirectionData.EvenDict;

        if (Map==null) Map = this.transform.parent.GetComponent<HexMap>();


        Map.ApplyRandomColor(CurrentHexagon);
       

    }




    public void DebugHelp() {
        Debug.Log("I am ("+row+","+column+") , Matching Colors("+ CurrentHexColor().name+"):"+GetNeighboursData().GetMatchingNeighbours().Count);
    }



    public HexTile GetNeighbour(HexDirection Dir) {
        Vector2 Direction = DictReference[Dir];
        return Map.GetTileBase(this.row+(int)Direction.x,this.column+(int)Direction.y);

    }

    public NeighborData GetNeighboursData() {
        return new NeighborData(this);

    }

    public ColorInfoSO CurrentHexColor() {
        if (CurrentHexagon != null) {
            return CurrentHexagon.ColorSO;

        } else return null;
    }




}
