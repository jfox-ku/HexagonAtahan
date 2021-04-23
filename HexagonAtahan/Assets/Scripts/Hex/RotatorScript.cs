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

    public void Rotate() {
        if (!isRotating) {

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

        hexes[0].PullHexagonToTile(hexagonList[1]);
        hexes[1].PullHexagonToTile(hexagonList[2]);
        hexes[2].PullHexagonToTile(hexagonList[0]);

        RotationCount++;
    }

    

    
    private IEnumerator RotateAnim() {

        isRotating = true;
        if (RotationCount < 3) {
            //Debug.Log("Turning: " + RotationCount);
            RotateHexagonsInTiles();
            
            yield return new WaitForSeconds(0.4f);
            if (HexTile.Map.CheckForMatches() == 0) {
                //If no matches, turn again
                StartCoroutine(RotateAnim());
            } else {
                //if match, just reset
                yield return new WaitForSeconds(0.4f);
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


//Vector3 relativePos = hexes[RotationCount%3].transform.position - transform.position;

//Quaternion BaseRot = this.transform.rotation;
//Quaternion TargetRot = Quaternion.LookRotation(relativePos,Vector3.right);
//TargetRot.y = BaseRot.y;
//TargetRot.z = BaseRot.z;


//float RotateTime = 0.5f;
//float RotateTimer = RotateTime;

//while (RotateTimer>0) {
//    RotateTimer -= Time.deltaTime;
//    Quaternion temp_rot = Quaternion.Lerp(BaseRot,TargetRot,(RotateTime-RotateTimer)/RotateTime);
//    transform.rotation = temp_rot;
//    yield return null;

//}
//this.transform.rotation = TargetRot;