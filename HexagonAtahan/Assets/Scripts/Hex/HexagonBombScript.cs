using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HexagonBombScript : HexagonScript {
    public TextMeshProUGUI bombCounterText;
    public VoidEvent BombExplodeEvent;

    public int maxCounter = 3;
    private int currentCounter;

    private void Start() {
        currentCounter = maxCounter;
        bombCounterText.text = "" + currentCounter;
    }

    //Called by the VoidListener on the Prefab 
    public void CountdownBomb() {
        currentCounter--;
        if (currentCounter <= 0) {
            bombCounterText.text = "" + currentCounter;
            BombExplodeEvent.Raise(new Void());
        } else {
            bombCounterText.text = "" + currentCounter;
        }

    }





}
