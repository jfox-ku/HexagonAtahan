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

    public bool isEmpty() {
        return CurrentHexagon == null;
    }


    public void DebugHelp() {
        if (CurrentHexagon == null) {
            Debug.Log("("+row+", "+column+") is empty.");
            return;

        }
        Debug.Log("I am ("+row+","+column+") , Matching Colors("+ CurrentHexColor().name+"):"+GetNeighboursData().GetMatchingNeighbours().Count);
    }

    private int ClickCount = 0;
    public void HexClicked() {
        ClickCount++;

        var Data = GetNeighboursData();
        var list = Data.FindViableCorner(ClickCount%6);

        //Debug.Log(list.Count);
        if (list.Count == 3) {
            foreach (var item in list) {
                item.HexMatched();
            }

        }

    }

    public void HexMatched() {
        if (CurrentHexagon != null) {
            Destroy(CurrentHexagon.gameObject);
            CurrentHexagon = null;
            
        }
    }

    public void HexDrop(HexTile Destination) {
        if (!Destination.isEmpty()) {
            Debug.LogError("Destination is not empty.");
            return;
        }

        Destination.CurrentHexagon = CurrentHexagon;
        CurrentHexagon.transform.SetParent(Destination.transform);
        StartCoroutine(MoveHexagon(CurrentHexagon,Destination));
        CurrentHexagon = null;

    }

    private IEnumerator MoveHexagon(HexagonScript hexa, HexTile Dest) {
        Transform hexT = hexa.transform;
        Transform dest = Dest.transform;

        float dropTime = (column - Dest.column)/4f;
        float dropTimer = dropTime;
        while (dropTimer > 0) {
            dropTimer -= Time.deltaTime;
            if (hexT != null) {
                hexT.transform.position = Vector3.Lerp(this.transform.position, dest.transform.position, (dropTime - dropTimer) / dropTime);
            }
            yield return null;
        }

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
