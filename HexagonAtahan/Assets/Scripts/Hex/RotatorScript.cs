using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorScript : MonoBehaviour
{
    public HexTile[] hexes = new HexTile[3];  
    private int RotationCount;
    private bool ClockWise = true;
    [SerializeField]private bool isRotating;


    //Select/deselect hextiles and set rotator position
    public void SetRotator(List<HexTile> TouchingTiles) {
        if (isRotating) return;

        DeselectTiles();
        if (TouchingTiles.Count != 3) {
            Debug.LogError("Wrong format for Rotator");
            return;
        }

        hexes[0] = TouchingTiles[0];
        hexes[1] = TouchingTiles[1];
        hexes[2] = TouchingTiles[2];
        SelectTiles();
       
        RotationCount = 0;
        isRotating = false;

        this.transform.position = (hexes[0].transform.position + hexes[1].transform.position + hexes[2].transform.position) / 3f;
        
    }

    //Decide CW or CCW and start Rotating
    public void Rotate(int dir) {
        if (!isRotating) {

            if (dir < 0) ClockWise = false;
            else ClockWise = true;

            StartCoroutine(RotateAnim());
        } 
    }

    //Swaps Hexagons between the 3 selected tiles CW or CCW
    private void RotateHexagonsInTiles() {
        List<HexagonScript> hexagonList = new List<HexagonScript>() {
            {hexes[0].EjectHexagon()},
            {hexes[1].EjectHexagon()},
            {hexes[2].EjectHexagon()}
        };

        if (ClockWise) { //Clockwise
            hexes[0].PullHexagonToTile(hexagonList[1]);
            hexes[1].PullHexagonToTile(hexagonList[2]);
            hexes[2].PullHexagonToTile(hexagonList[0]);
        } else { //Counterclockwise
            hexes[0].PullHexagonToTile(hexagonList[2]);
            hexes[2].PullHexagonToTile(hexagonList[1]);
            hexes[1].PullHexagonToTile(hexagonList[0]);

        }
        

        RotationCount++;
    }

    //After each rotation, asks the map for matches
    //Stops at 3 rotations
    private IEnumerator RotateAnim() {

        isRotating = true;
        if (RotationCount < 3) {
            //Debug.Log("Turning: " + RotationCount);
            RotateHexagonsInTiles();
            
            yield return new WaitForSeconds(0.4f);
            var matchSet = HexTile.Map.CheckForMatches();
            if (matchSet.Count == 0) {
                //If no matches, turn again
                StartCoroutine(RotateAnim());
            } else {
                //if match, just reset and ask the map to go stable
                yield return new WaitForSeconds(0.15f);
                HexTile.Map.LockUntilStable();
                RotationCount = 0;
                isRotating = false;
                
            }
        //If on last turn, reset
        }else if (RotationCount == 3) {
            isRotating = false;
            
            RotationCount = 0;
          
        }

        yield return null;

    }

    public void SelectTiles() {
        foreach (var hex in hexes) {
            hex?.HexSelected();
        }
    }

    public void DeselectTiles() {
        foreach (var hex in hexes) {
            hex?.HexDeselected();
        }
    }

}

