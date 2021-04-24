using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorScript : MonoBehaviour
{
    public HexTile[] hexes = new HexTile[3];  
    private int RotationCount;
    [SerializeField]private bool isRotating;

    private void Start() {
        
    }


    public void SetRotator(List<HexTile> TouchingTiles) {
        if (isRotating) return;

        if (TouchingTiles.Count != 3) {
            Debug.LogError("Wrong format for Rotator");
            return;
        }

        hexes[0] = TouchingTiles[0];
        hexes[1] = TouchingTiles[1];
        hexes[2] = TouchingTiles[2];
        RotationCount = 0;
        isRotating = false;

        this.transform.position = (hexes[0].transform.position + hexes[1].transform.position + hexes[2].transform.position) / 3f;
        //this.transform.rotation = Quaternion.LookRotation(hexes[0].transform.position-transform.position);
    }

    private bool ClockWise = true;
    public void Rotate(int dir) {
        if (!isRotating) {

            if (dir < 0) ClockWise = false;
            else ClockWise = true;

            //ParentToMe();
            StartCoroutine(RotateAnim());
        } 
    }

    //private void ParentToMe() {
    //    for (int i = 0; i < hexes.Length; i++) {
    //        var hexa = hexes[i].CurrentHexagon;
    //        hexagons[i] = hexa;
    //        hexa.transform.SetParent(this.transform);
    //    }
    //}

    //private void ReturnToTiles() {
    //    for (int i = 0; i < hexes.Length; i++) {
    //        if(hexagons[i]!=null)
    //        hexagons[i].transform.SetParent(hexes[i].transform);
    //    }
    //}


    private void RotateHexagonsInTiles() {
        List<HexagonScript> hexagonList = new List<HexagonScript>() {
            {hexes[0].EjectHexagon()},
            {hexes[1].EjectHexagon()},
            {hexes[2].EjectHexagon()}
        };

        if (ClockWise) {
            hexes[0].PullHexagonToTile(hexagonList[1]);
            hexes[1].PullHexagonToTile(hexagonList[2]);
            hexes[2].PullHexagonToTile(hexagonList[0]);
        } else {
            hexes[0].PullHexagonToTile(hexagonList[2]);
            hexes[1].PullHexagonToTile(hexagonList[0]);
            hexes[2].PullHexagonToTile(hexagonList[1]);
        }
        

        RotationCount++;
    }

    

    
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
            //ReturnToTiles();
        }

        yield return null;

    }

}

