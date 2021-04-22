using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour {
    [Header("Prefabs")]
    public GameObject HexTilePrefab;
    public GameObject HexagonPrefab;
    public GameObject RotatorPrefab;

    [Header("SO Events")]
    public VoidEvent ReadyForNextInputEvent;

    [Header("Dimensions")]
    public int count_w;
    public int count_h;

    [Header("Colors")]
    public List<ColorInfoSO> ColorAssets = new List<ColorInfoSO>();

    private HexTile[,] HexTileBases;
    private float xOffset = 0.77f;
    private float yOffset = 0.9f;

    private bool CheckingForFall = false;

    void Start() {
        InitializeMap(count_w, count_h);
    }

    public void InitializeMap(int w, int h) {
        HexTileBases = new HexTile[w, h];

        for (int i = 0; i < w; i++) {
            for (int j = 0; j < h; j++) {
                HexTile HexT = Instantiate(HexTilePrefab, this.transform).GetComponent<HexTile>();
                HexT.Init(i, j);
                HexTileBases[i, j] = HexT;
                HexT.gameObject.name = "HexBase_" + i + "_" + j;


                //Positions setup
                float x = i * xOffset;
                float y = j * yOffset;
                if (i % 2 == 1) y -= yOffset / 2f;
                HexT.transform.position = new Vector3(x, y, 0);


                //Color setup
                ApplyRandomColor(HexT.GetComponentInChildren<HexagonScript>());

            }

        }
    }


    public void ApplyRandomColor(HexagonScript hex) {
        if (!hex) return;
        hex.SetColor(ColorAssets[Random.Range(0, ColorAssets.Count)]);
    }


    public void StartFallCheck() {
        if(!CheckingForFall)
        StartCoroutine(ColumnFallCheck());
    }

    public HexagonScript SpawnHexagonAt(int i, int j) {
        var tile = HexTileBases[i, j];
        var hex = Instantiate(HexagonPrefab, tile.transform).GetComponent<HexagonScript>();
        ApplyRandomColor(hex);
        hex.FadeIn();

        tile.CurrentHexagon = hex;
        return hex;

    }

    public HexTile GetTileBase(int rw, int cl) {
        if (rw >= count_w || rw < 0) return null;
        if (cl >= count_h || cl < 0) return null;

        return HexTileBases[rw, cl];
    }

    private IEnumerator ColumnFallCheck() {
        CheckingForFall = true;
        for (int i = 0; i < count_w; i++) {
            for (int j = 0; j < count_h; j++) {
                HexTile tile = HexTileBases[i, j];
                if (tile.isEmpty()) {
                    for (int k = j + 1; k < count_h; k++) {
                        HexTile UpperTile = HexTileBases[i, k];
                        if (!UpperTile.isEmpty()) {
                            //Move the Hexagon in the upper tile to bottom empty tile
                            UpperTile.HexDrop(tile);
                            break;
                        }
                        //UpperTile.DebugHelp();



                    }
                }

                //If tile is still empty after all drops, spawn a new hexagon to drop
                if (tile.isEmpty()) {
                    var TopHexTile = HexTileBases[i, count_h - 1];
                    var newHex = SpawnHexagonAt(i, count_h - 1);
                    
                    //Drop if not the top tile
                    if (TopHexTile != tile) {
                        HexTileBases[i, count_h - 1].HexDrop(tile);
                    }



                }
            }
        }

        StartCoroutine(WaitAndClearLock());
        CheckingForFall = false;
        yield return null;
    }

    private IEnumerator WaitAndClearLock() {
        yield return new WaitForSeconds(0.3f);
        ReadyForNextInputEvent.Raise(new Void());
    }




}
