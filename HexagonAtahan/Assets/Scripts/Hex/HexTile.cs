using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour {

    public int row;
    public int column;

    public bool isOdd;
    public bool isChecked;
    public bool isMatchFlag = false;

    public HexagonScript CurrentHexagon;
    public Transform SelectedSpriteTransform;
    public static HexMap Map;

    private Dictionary<HexDirection, Vector2> DictReference;
    private int ClickCount = 0;

    public void Init(int row, int column) {
        this.row = row;
        this.column = column;
        isOdd = row % 2 == 1 ? true : false;
        isChecked = false;
        DictReference = this.isOdd ? HexDirectionData.OddDict : HexDirectionData.EvenDict;

        if (Map == null) Map = this.transform.parent.GetComponent<HexMap>();


        Map.ApplyRandomColor(CurrentHexagon);


    }

    public void HexClicked() {
        ClickCount++;

        var Data = GetNeighboursData();
        Map.SetRotator(Data.FindViableCorner(ClickCount % 6));

    }

    public void HexMatched() {
        if (CurrentHexagon != null) {
            CurrentHexagon.FlashAndDestroy();
            isMatchFlag = false;
            CurrentHexagon = null;

        }
    }

    public void HexSelected() {
        SelectedSpriteTransform.gameObject.SetActive(true);
    }
    public void HexDeselected() {
        SelectedSpriteTransform.gameObject.SetActive(false);
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
        if (CurrentHexagon == null && hx != null) {
            CurrentHexagon = hx;
            StartCoroutine(MoveHexagon(this, this,false));
            
            hx.transform.SetParent(this.transform);
        } else {
            Debug.LogError("Can't pull to non-empty hex.");
        }

    }

    //Gets the whole chunk of matches
    public HashSet<HexTile> GetAllMatches(int depth, ColorInfoSO ColorTarget = null) {
        HashSet<HexTile> allMatches = new HashSet<HexTile>();
        if (ColorTarget == null) ColorTarget = CurrentHexColor();


        if (depth >= 0) {
            var Data = GetNeighboursData();
            var LocalMatches = Data.JointMatches(ColorTarget);

            if (ColorTarget == CurrentHexColor()) {
                allMatches.UnionWith(LocalMatches);

                foreach (var tile in LocalMatches) {
                    if (!tile.isMatchFlag) {
                        allMatches.UnionWith(tile.GetAllMatches(depth - 1, ColorTarget));
                        tile.isMatchFlag = true;
                    }
                }
            }

        }


        return allMatches;

    }

    //Drop CurrentHexagon to Destination HexTile
    public void HexDrop(HexTile Destination) {
        if (!Destination.isEmpty()) {
            Debug.LogError("Destination is not empty.");
            return;
        }

        Destination.CurrentHexagon = CurrentHexagon;
        CurrentHexagon.transform.SetParent(Destination.transform);
        StartCoroutine(MoveHexagon(this, Destination));
        CurrentHexagon = null;

    }

    //Move Animation 
    private IEnumerator MoveHexagon(HexTile StartTile, HexTile Dest, bool fall=true) {
        Transform hexT = StartTile.CurrentHexagon.transform;
        var startPos = hexT.position;
        var destPos = Dest.transform.position;

        float dropTime = fall? (StartTile.column - Dest.column) / 8f : 0.2f;
        float dropTimer = dropTime;
        while (dropTimer > 0) {
            dropTimer -= Time.deltaTime;
            if (hexT != null) {
                hexT.transform.position = Vector3.Lerp(startPos, destPos, (dropTime - dropTimer) / dropTime);
            }
            yield return null;
        }

        if (hexT != null) {
            hexT.transform.position = Dest.transform.position;
        }

    }


    #region Helpers

    public HexTile GetNeighbour(HexDirection Dir) {
        Vector2 Direction = DictReference[Dir];
        return Map.GetTileBase(this.row + (int)Direction.x, this.column + (int)Direction.y);

    }

    private NeighborData GetNeighboursData() {
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

    public bool HasPossibleMatch() {
        var Data = GetNeighboursData();
        return Data.IsMatchPossibleMaybe();
    }

    public int NeighbourColorCount(ColorInfoSO clr) {
        var Data = GetNeighboursData();
        return Data.GetMatchingNeighbours(clr).Count;
    }

    #endregion Helpers

}
