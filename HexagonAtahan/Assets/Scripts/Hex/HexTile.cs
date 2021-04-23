using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    
    public int row;
    public int column;

    public bool isOdd;
    public bool isChecked;
    private bool isMatchFlag = false;

    public HexagonScript CurrentHexagon;
    public static HexMap Map;

    private Dictionary<HexDirection, Vector2> DictReference;

    public void Init(int row,int column) {
        this.row = row;
        this.column = column;
        isOdd = row % 2 == 1 ? true : false;
        isChecked = false;
        DictReference = this.isOdd ? HexDirectionData.OddDict : HexDirectionData.EvenDict;

        if (Map==null) Map = this.transform.parent.GetComponent<HexMap>();


        Map.ApplyRandomColor(CurrentHexagon);
       

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
        Map.SetRotator(Data.FindViableCorner(ClickCount%6));
       
        //var Data = GetNeighboursData();
        //var JM = Data.JointMatches();
        //foreach (var item in JM) {
        //    item.HexMatched();
        //}

        //var list = Data.FindViableCorner(ClickCount%6);

        ////Debug.Log(list.Count);
        //if (list.Count == 3) {
        //    foreach (var item in list) {
        //        item.HexMatched();
        //    }

        //}

    }

    public HexagonScript EjectHexagon() {
        if (CurrentHexagon != null) {
            var temp = CurrentHexagon;
            CurrentHexagon = null;
            if (temp == null) Debug.LogError("Oh no..");
            return temp;
        }
        Debug.LogError("No Hexagon to eject.");
        return null;
    }

    public void PullHexagonToTile(HexagonScript hx) {
        if (CurrentHexagon == null && hx !=null) {
            StartCoroutine(MoveHexagon(hx, this));
            CurrentHexagon = hx;
            hx.transform.SetParent(this.transform);
        } else {
            Debug.LogError("Can't pull to non-empty hex.");
        }

    }

    public static int AllMatchCallCount = 0;


    public HashSet<HexTile> AllMatch(int depth,ColorInfoSO ColorTarget = null) {
        AllMatchCallCount++;
        HashSet<HexTile> allMatches = new HashSet<HexTile>();
        if (ColorTarget == null) ColorTarget = CurrentHexColor();


        if (depth >= 0) {
            var Data = GetNeighboursData();
            var LocalMatches = Data.JointMatches(ColorTarget);

            if(ColorTarget == CurrentHexColor()) {
                allMatches.UnionWith(LocalMatches);

                foreach (var tile in LocalMatches) {
                    if (!tile.isMatchFlag) {
                        allMatches.UnionWith(tile.AllMatch(depth - 1, ColorTarget));
                        tile.isMatchFlag = true;
                    }
                }
            }
            
        }

        
        return allMatches;

    }

    public void HexMatched() {
        if (CurrentHexagon != null) {
            CurrentHexagon.FlashAndDestroy();
            isMatchFlag = false;
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

        float dropTime = (column - Dest.column)/8f;
        float dropTimer = dropTime;
        while (dropTimer > 0) {
            dropTimer -= Time.deltaTime;
            if (hexT != null) {
                hexT.transform.position = Vector3.Lerp(this.transform.position, dest.transform.position, (dropTime - dropTimer) / dropTime);
            }
            yield return null;
        }

        if (hexT != null) {
            hexT.transform.position = Dest.transform.position;
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

    public bool isEmpty() {
        return CurrentHexagon == null;
    }




}
