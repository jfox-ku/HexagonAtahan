using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputScript : MonoBehaviour {
    public bool LockInput = false;
    private bool InternalLock = false;

    public IntEvent DragToStartRotateEvent;

#if UNITY_ANDROID

    private Vector2 firstTouchPos;

    private void Update() {

        if (LockInput) { return; }
        if (Input.touchCount != 0) {
            Touch touch = Input.touches[0];
            
            if (TouchPhase.Ended == touch.phase) {
                Debug.Log((firstTouchPos - touch.position).magnitude);
                if ((firstTouchPos - touch.position).magnitude > 200f) {

                    DragToStartRotateEvent.Raise((int)(touch.position.y-firstTouchPos.y));

                } else {
                    
                    Vector3 TouchWorldPos = Camera.main.ScreenToWorldPoint(firstTouchPos);
                    TouchWorldPos.z = 0;
                    Vector3 Direction = (TouchWorldPos - Camera.main.transform.position).normalized;

                    if (Physics.Raycast(Camera.main.transform.position, Direction, out RaycastHit hit, 15f)) {
                        var hex = hit.transform.GetComponent<HexTile>();
                        if (hex) {
                            hex.HexClicked();
                        }
                    }
                   
                }
            } else if (TouchPhase.Began == touch.phase) {
                firstTouchPos = touch.position;
                return;
            }



            //if (TouchPhase.Moved == touch.phase) {
            //    Debug.Log("Touch moved :"+ touch.deltaPosition.magnitude / touch.deltaTime);
            //    if (touch.deltaPosition.magnitude/touch.deltaTime > 0.1f) {
            //        DragToStartRotateEvent.Raise(new Void());
            //        StartCoroutine(InternalInputWait());
            //    }


            //}




        }




    }
#endif

#if UNITY_EDITOR
    // Update is called once per frame
    void FixedUpdate1() {
        if (Input.GetMouseButton(1) && !InternalLock) {

            DragToStartRotateEvent.Raise(100);
            return;
        }

        if (Input.GetMouseButton(0) && !(LockInput || InternalLock)) {

            Vector3 TouchWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            TouchWorldPos.z = 0;
            Vector3 Direction = (TouchWorldPos - Camera.main.transform.position).normalized;
            Debug.DrawRay(Camera.main.transform.position, Direction);

            if (Physics.Raycast(Camera.main.transform.position, Direction, out RaycastHit hit, 15f)) {
                var hex = hit.transform.GetComponent<HexTile>();
                if (hex) {

                    hex.HexClicked();
                    StartCoroutine(InternalInputWait());

                }


            }


        }

    }
#endif


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
