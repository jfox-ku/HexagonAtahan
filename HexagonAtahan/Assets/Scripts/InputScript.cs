using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputScript : MonoBehaviour
{
    public bool LockInput = false;
    private bool InternalLock = false;

    public VoidEvent DragToStartRotateEvent;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButton(1) && !InternalLock) {

            DragToStartRotateEvent.Raise(new Void());
            return;
        }

        if (Input.GetMouseButton(0) && !(LockInput || InternalLock)) {

            

            Vector3 TouchWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            TouchWorldPos.z = 0;

            //Debug.Log("Touch World Pos: "+TouchWorldPos);
            Vector3 Direction = (TouchWorldPos - Camera.main.transform.position).normalized;
            //Debug.Log("Ray Direction: " + Direction);
            Debug.DrawRay(Camera.main.transform.position, Direction);

            if (Physics.Raycast(Camera.main.transform.position, Direction, out RaycastHit hit,15f)) {
                var hex = hit.transform.GetComponent<HexTile>();
                if (hex) {
                    
                    hex.HexClicked();
                    StartCoroutine(InternalInputWait());
                    //hex.DebugHelp();
                    //var MatchingNeigbours = hex.GetNeighboursData().GetMatchingNeighbours();
                    //foreach (HexTile tile in MatchingNeigbours) {
                    //    tile.HexMatched();
                    //}
                    //if(MatchingNeigbours.Count!=0)
                    //hex.HexMatched();
                    
                }

                
            }

            
            

        }
        
    }

    private IEnumerator InternalInputWait() {
        InternalLock = true;
        yield return new WaitForSeconds(0.5f);
        InternalLock = false;

    }

    public void LockClear() {
        LockInput = false;
    }

    public void LockLock() {
        //Who's there?
        LockInput = true;
        // ._.

    }




}
