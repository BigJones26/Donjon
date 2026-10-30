using UnityEngine;

public class DungeonLevel1 : MonoBehaviour
{
    private string[] map = new[] {
        "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        "X    X             X                       X",
        "X    X XXXXXXXXXXX X XXXXXXXXXXXXXXXXXXXXX X",
        "X    X X         X X X              X   XX X",
        "X    X X         X X X              X X XX X",
        "X    X X         X X X              X X XX X",
        "X    X XXXXXXXXXXX X X              X X XX X",
        "X    X X         X X X              X X XX X",
        "X    X X XXXXXXXXX XXXXXXXXXXXXXXXXXXXX XX X",
        "X    X X         X                      XX X",
        "X    X XXXXXXXXX X   XXXXXXXXXXXXXXXXXXXXX X",
        "X    X           X                         X",
        "X    XXXXXXXXXXXXX XXXXXXXXXXXXXXXXXXXXXXXXX",
        "X    X                X                     X",
        "X    X X XXXXXXXXXX   X                     X",
        "X    X X X        X   X                     X",
        "XXXXXX X X        X   X                     X",
        "X    X X X        X   X                     X",
        "X X  X X X        XXXXX                     X",
        "X X  X X X        X XXX                     X",
        "X X  X X X        X XXX                     X",
        "X X  X XXXXXXXXXXXX XXX                     X",
        "X                   XXX                     X",
        "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
    };

    [Header("Prefabs")]
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public GameObject ceilingPrefab;

    [Header("Materials")]
    public Material[] wallMaterials;
    public Material[] floorMaterials;
    public Material[] ceilingMaterials;

    // Persistent random selection maps
    private int[,] wallTextureMap;
    private int[,] floorTextureMap;
    private int[,] ceilingTextureMap;

    void Awake()
    {
        int rows = map.Length;
        int cols = map[0].Length;

        wallTextureMap = new int[rows, cols];
        floorTextureMap = new int[rows, cols];
        ceilingTextureMap = new int[rows, cols];

        // Load saved texture indices (0 = unassigned)
        for (int z = 0; z < rows; z++)
        {
            for (int x = 0; x < cols; x++)
            {
                wallTextureMap[z, x] = PlayerPrefs.GetInt($"walltex_{z}_{x}", 0);
                floorTextureMap[z, x] = PlayerPrefs.GetInt($"floortex_{z}_{x}", 0);
                ceilingTextureMap[z, x] = PlayerPrefs.GetInt($"ceiltex_{z}_{x}", 0);
            }
        }
    }

    public bool IsWalkable(Vector3 newPosition)
    {
        int x = Mathf.RoundToInt(newPosition.x);
        int z = Mathf.RoundToInt(newPosition.z);

        if (z < 0 || z >= map.Length) return false;
        if (x < 0 || x >= map[z].Length) return false;

        return map[z][x] == ' ';
    }

    void Start()
    {
        AssignRandomTexturesOnce();
        SaveTextureMaps();
        BuildDungeon();
    }

    // ---------------------------------------------------------
    // RANDOMISE ONLY ONCE
    // ---------------------------------------------------------
    void AssignRandomTexturesOnce()
    {
        for (int z = 0; z < map.Length; z++)
        {
            for (int x = 0; x < map[z].Length; x++)
            {
                bool isWall = map[z][x] == 'X';
                bool isFloor = map[z][x] == ' ';
                bool isCeiling = isFloor; // ceilings cover walkable spaces

                if (isWall && wallTextureMap[z, x] == 0)
                    wallTextureMap[z, x] = Random.Range(1, wallMaterials.Length + 1);

                if (isFloor && floorTextureMap[z, x] == 0)
                    floorTextureMap[z, x] = Random.Range(1, floorMaterials.Length + 1);

                if (isCeiling && ceilingTextureMap[z, x] == 0)
                    ceilingTextureMap[z, x] = Random.Range(1, ceilingMaterials.Length + 1);
            }
        }
    }

    // ---------------------------------------------------------
    // SAVE PERMANENTLY
    // ---------------------------------------------------------
    void SaveTextureMaps()
    {
        for (int z = 0; z < map.Length; z++)
        {
            for (int x = 0; x < map[z].Length; x++)
            {
                PlayerPrefs.SetInt($"walltex_{z}_{x}", wallTextureMap[z, x]);
                PlayerPrefs.SetInt($"floortex_{z}_{x}", floorTextureMap[z, x]);
                PlayerPrefs.SetInt($"ceiltex_{z}_{x}", ceilingTextureMap[z, x]);
            }
        }

        PlayerPrefs.Save();
    }

    // ---------------------------------------------------------
    // BUILD DUNGEON
    // ---------------------------------------------------------
    void BuildDungeon()
    {
        for (int z = 0; z < map.Length; z++)
        {
            for (int x = 0; x < map[z].Length; x++)
            {
                Vector3 pos = new Vector3(x, 0, z);

                if (map[z][x] == 'X')
                {
                    // WALL
                    GameObject wall = Instantiate(wallPrefab, pos, Quaternion.identity);
                    int index = wallTextureMap[z, x] - 1;
                    wall.GetComponent<MeshRenderer>().material = wallMaterials[index];
                }
                else
                {
                    // FLOOR (y = -1)
                    GameObject floor = Instantiate(floorPrefab, pos + Vector3.down, Quaternion.identity);
                    int findex = floorTextureMap[z, x] - 1;
                    floor.GetComponent<MeshRenderer>().material = floorMaterials[findex];

                    // CEILING (y = +1)
                    GameObject ceiling = Instantiate(ceilingPrefab, pos + Vector3.up, Quaternion.identity);
                    int cindex = ceilingTextureMap[z, x] - 1;
                    ceiling.GetComponent<MeshRenderer>().material = ceilingMaterials[cindex];
                }
            }
        }
    }
}

