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
                    //x position difference determines CW or CCW
                    DragToStartRotateEvent.Raise((int)(firstTouchPos.x - touch.position.x));

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
        }

    }
#endif

#if UNITY_EDITOR
    // These are only for testing with mouse in the editor
    void FixedUpdate() {
        if (Input.GetMouseButton(1) && !(LockInput || InternalLock)) {

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
