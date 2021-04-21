using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject HexTilePrefab;
    

    [Header("Dimensions")]
    public int count_w;
    public int count_h;

    private float xOffset = 0.77f;
    private float yOffset = 0.9f;

    [Header("Colors")]
    public List<ColorInfoSO> ColorAssets = new List<ColorInfoSO>();

    private HexTile[,] HexTileBases;

    void Start()
    {
        InitializeMap(count_w,count_h);

        

    }
    

    public void InitializeMap(int w,int h) {
        HexTileBases = new HexTile[w, h];

        for (int i = 0; i < w; i++) {
            for (int j = 0; j < h; j++) {
                HexTile HexT = Instantiate(HexTilePrefab,this.transform).GetComponent<HexTile>();
                HexT.Init(i,j);
                HexTileBases[i, j] = HexT;
                HexT.gameObject.name = "HexBase_" + i + "_" + j;


                //Positions setup
                float x = i * xOffset;
                float y = j * yOffset;
                if (i % 2 == 1) y -= yOffset/2f;
                HexT.transform.position = new Vector3(x,y,0);


                //Color setup
                ApplyRandomColor(HexT.GetComponentInChildren<HexagonScript>());

            }

        }
    }


    public void ApplyRandomColor(HexagonScript hex) {
        if (!hex) return;
        hex.SetColor(ColorAssets[Random.Range(0, ColorAssets.Count)]);
    }

    

    public HexTile GetTileBase(int rw,int cl) {
        if (rw > count_w || rw < 0) return null;
        if (cl > count_h || cl < 0) return null;

        return HexTileBases[rw, cl];
    }


}
