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
    public VoidEvent LockInputEvent;

    [Header("Dimensions")]
    public int count_w;
    public int count_h;

    [Header("Colors")]
    public List<ColorInfoSO> ColorAssets = new List<ColorInfoSO>();

    private HexTile[,] HexTileBases;
    private float xOffset = 0.77f;
    private float yOffset = 0.9f;
    private const int MAX_SEARCH_DEPTH = 10;
    private const int MAX_TRY_STABLE = 20;
    private RotatorScript RotatorInstance;
    public static bool isInitialized;

    private bool CheckingForFall = false;

    public void SetRotator(List<HexTile> tiles) {
        if(RotatorInstance==null) RotatorInstance = Instantiate(RotatorPrefab).GetComponent<RotatorScript>();
        RotatorInstance.SetRotator(tiles);

    }

    void Start() {
        isInitialized = false;
        InitializeMap(count_w, count_h);
        RotatorInstance = Instantiate(RotatorPrefab).GetComponent<RotatorScript>();
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

        StartCoroutine(MatchDropUntilStable());


    }

    private IEnumerator MatchDropUntilStable() {
        if (!CheckingForFall) {
            CheckingForFall = true;
            LockInputEvent.Raise(new Void());
            int count = 0;
            while (count < MAX_TRY_STABLE) {
                CheckForMatches();
                count++;
                StartCoroutine(ColumnFallCheck());
                if (CheckForMatches() == 0) break;
                yield return new WaitForSeconds(0.5f);

            }

            CheckingForFall = false;
            yield return new WaitForSeconds(1f);
            if (!isInitialized) isInitialized = true;
            ReadyForNextInputEvent.Raise(new Void());
        }
        
    }


    public void ApplyRandomColor(HexagonScript hex) {
        if (!hex) return;
        hex.SetColor(ColorAssets[Random.Range(0, ColorAssets.Count)]);
    }

  

    public HexagonScript SpawnHexagonAt(int i, int j) {
        var tile = HexTileBases[i, j];
        var hex = Instantiate(HexagonPrefab, tile.transform).GetComponent<HexagonScript>();
        ApplyRandomColor(hex);
        hex.FadeIn();

        tile.CurrentHexagon = hex;
        return hex;

    }

    public void SetNextHexagonAsBomb() {

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
                //Find Bottom empty tile
                if (tile.isEmpty()) {
                    //Find first non-empty tile above 
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

                    //Drop if not the top tile itself was empty
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

    public int CheckForMatches() {
        int matchCount = 0;
        for (int i = 0; i < count_w; i++) {
            for (int j = 0; j < count_h; j++) {
                var tile = HexTileBases[i, j];
                if (!tile.isChecked) {
                    tile.isChecked = true;
                    var matches = tile.AllMatch(MAX_SEARCH_DEPTH,tile.CurrentHexColor());
                    
                    foreach (var t in matches) {
                        
                        t.HexMatched();
                        matchCount++;
                        t.isChecked = true;
                    }


                }


            }
        }

        for (int i = 0; i < count_w; i++) {
            for (int j = 0; j < count_h; j++) {
                var tile = HexTileBases[i, j];
                tile.isChecked = false;

            }
        }

        if (matchCount != 0) {
            StartCoroutine(MatchDropUntilStable());
            Debug.Log(matchCount);

        }
        return matchCount;
    }


}
