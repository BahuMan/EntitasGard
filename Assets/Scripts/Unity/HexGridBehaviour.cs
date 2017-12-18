using UnityEngine;
using System.Collections.Generic;
using System;

public class HexGridBehaviour : MonoBehaviour
{
    public int size = 5;
    public float cellsize = 1;
    public HexCellBehaviour _prefab;
    public const int HEXMASK = 1 >> 8;
    public float _chanceForPassage = .4f;

    [SerializeField]
    private HexCellBehaviour[,] _grid;
    private const float SQRT3 = 1.7320508075688772935274463415058723669428052538103806280f;
    private HexCellBehaviour lastHex = null;

    private Dictionary<HexPassable, Vector3> cubeNeighboursCoordinates; 
    
    private void Start()
    {
        cubeNeighboursCoordinates = new Dictionary<HexPassable, Vector3>(10);
        cubeNeighboursCoordinates[HexPassable.N] = new Vector3(0, 1, -1);
        cubeNeighboursCoordinates[HexPassable.NE] = new Vector3(1, 0, -1);
        cubeNeighboursCoordinates[HexPassable.SE] = new Vector3(1, -1, 0);
        cubeNeighboursCoordinates[HexPassable.S] = new Vector3(0, -1, 1);
        cubeNeighboursCoordinates[HexPassable.SW] = new Vector3(-1, 0, 1);
        cubeNeighboursCoordinates[HexPassable.NW] = new Vector3(-1, 1, 0);

        CreateGrid();
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo = new RaycastHit();

        if (Input.GetMouseButtonDown(0)) {
            if (lastHex != null) lastHex.SetHighLight(false);

            if (Physics.Raycast(ray, out hitInfo, 1 << 8))
            {
                Vector3 c = axial_to_cube(pixel_to_hex(hitInfo.point));
                lastHex = GetCell(c);
                if (lastHex != null)
                {
                    lastHex.SetHighLight(true);
                    StartCoroutine(LightUpNeighbours(lastHex));
                }
            }
        }
    }

    private System.Collections.IEnumerator LightUpNeighbours(HexCellBehaviour cell)
    {
        HexCellBehaviour n;

        n = GetNeighbour(cell, HexPassable.N);
        if (n != null)
        {
            Debug.Log("North");
            n.SetHighLight(true);
            yield return new WaitForSeconds(1f);
            n.SetHighLight(false);
        }
        n = GetNeighbour(cell, HexPassable.NE);
        if (n != null)
        {
            Debug.Log("North East");
            if (n != null) n.SetHighLight(true);
            yield return new WaitForSeconds(1f);
            n.SetHighLight(false);
        }

        n = GetNeighbour(cell, HexPassable.SE);
        if (n != null)
        {
            Debug.Log("South East");
            n.SetHighLight(true);
            yield return new WaitForSeconds(1f);
            n.SetHighLight(false);
        }
        n = GetNeighbour(cell, HexPassable.S);
        if (n != null)
        {
            Debug.Log("South");
            n.SetHighLight(true);
            yield return new WaitForSeconds(1f);
            n.SetHighLight(false);
        }
        n = GetNeighbour(cell, HexPassable.SW);
        if (n != null)
        {
            Debug.Log("South West");
            n.SetHighLight(true);
            yield return new WaitForSeconds(1f);
            n.SetHighLight(false);
        }
        n = GetNeighbour(cell, HexPassable.NW);
        if (n != null)
        {
            Debug.Log("North West");
            n.SetHighLight(true);
            yield return new WaitForSeconds(1f);
            n.SetHighLight(false);
        }
    }

    [ContextMenu("Create Grid")]
    public void CreateGrid()
    {
        _grid = new HexCellBehaviour[2 * size + 1, 2 * size + 1];

        //two temp variables to calculate the cell's coordinates:
        Vector2 ax = new Vector2();
        Vector3 cube = new Vector3();

        for (int gx = -size; gx <= size; ++gx)
        {
            for (int gy = Math.Max(-size, -gx - size); gy <= Math.Min(size, -gx + size); ++gy)
            {
                HexCellBehaviour cell = Instantiate(_prefab);
                SetCell(gx, gy, cell);
                cube.x = ax.x = gx;
                cube.y = ax.y = gy;
                cube.z = -gx - gy;
                cell.cubeCoordinates = cube;
                cell.transform.position = hex_to_pixel(ax);
                cell.transform.SetParent(this.transform);
            }
        }
    }

    [ContextMenu("Create Maze")]
    public void CreateMaze()
    {

        //mark all cells as "not visited"
        bool[,] visited = new bool[2 * size + 1, 2 * size + 1];
        for (int q=0; q<2*size+1; ++q)
            for (int r=0; r<2*size+1; ++r)
                visited[q, r] = false;

        //pick a random cell (withing range 'size') to start from:
        int rx = UnityEngine.Random.Range(-size, size);
        int ry = UnityEngine.Random.Range(Math.Max(-size, -rx - size), Math.Min(size, -rx + size));
        HexCellBehaviour randomCell = GetCell(rx, ry);

        //recursively call this function to make passage ways:
        MazeRun(randomCell, ref visited);
    }

    private void MazeRun(HexCellBehaviour cell, ref bool[,] visited)
    {
        Vector2 axial =  this.cube_to_axial(cell.cubeCoordinates);
        int aq = Mathf.RoundToInt(axial.x);
        int ar = Mathf.RoundToInt(axial.y);

        visited[aq, ar] = true; //mark visited

        List<HexCellBehaviour> CandidateNeighbors = new List<HexCellBehaviour>(6);

        foreach (HexPassable d in Enum.GetValues(typeof(HexPassable)))
        {
            Vector3 ncube = cubeNeighboursCoordinates[d];
            ncube += cell.cubeCoordinates;

            if (IsOutOfBounds(ncube)) continue; //not interested in neighbours out of bounds

            Vector2 naxial = this.cube_to_axial(ncube);
            int naq = Mathf.RoundToInt(axial.x);
            int nar = Mathf.RoundToInt(axial.y);

            if (visited[naq, nar]) continue; //not interested in already visited neighbours

            //we are interested:
            CandidateNeighbors.Add(GetCell(ncube));

        }

        //nothing interesting? we're done
        if (CandidateNeighbors.Count == 0) return;

        //from viable candidates, choose 1 direction to continue:
        int chosen = UnityEngine.Random.Range(0, CandidateNeighbors.Count);
        CreatePathWay(cell, CandidateNeighbors[chosen]);
        MazeRun(CandidateNeighbors[chosen], ref visited);
    }

    private void CreatePathWay(HexCellBehaviour cell, HexCellBehaviour neighbour)
    {
        Vector3 diff = cell.cubeCoordinates - neighbour.cubeCoordinates;
        int dx = Mathf.RoundToInt(diff.x);
        int dy = Mathf.RoundToInt(diff.y);
        int dz = Mathf.RoundToInt(diff.z);


    }

    public HexCellBehaviour GetNeighbour(HexCellBehaviour cell, HexPassable dir)
    {
        Vector3 vd = cubeNeighboursCoordinates[dir];
        vd += cell.cubeCoordinates;
        if (IsOutOfBounds(vd)) return null;
        else return GetCell(vd);
    }

    public IEnumerator<HexCellBehaviour> GetAllNeighbours(Vector3 cube)
    {
        foreach(HexPassable d in Enum.GetValues(typeof(HexPassable)))
        {
            Vector3 c = cubeNeighboursCoordinates[d] + cube;
            if (!IsOutOfBounds(c)) yield return GetCell(c);
        }
    }

    public Vector3 hex_to_pixel(Vector2 hex)
    {
        Vector3 r = new Vector3();
        r.x = cellsize * 3 / 2 * hex.x;
        r.y = 0;
        r.z = cellsize * SQRT3 * (hex.x/2 + hex.y);
        return r;
    }

    public Vector2 pixel_to_hex(Vector3 pixel) {
        Vector2 r = new Vector2();
        r.x = pixel.x * 2 / 3 / cellsize;
        r.y = (-pixel.x / 3 + SQRT3 / 3 * pixel.z) / cellsize;
        return hex_round(r);
    }

    public Vector2 hex_round(Vector2 hex)
    {
        return cube_to_axial(cube_round(axial_to_cube(hex)));
    }

    public Vector3 cube_round(Vector3 cube) {
        Vector3 r = new Vector3();
        r.x = Mathf.Round(cube.x);
        r.y = Mathf.Round(cube.y);
        r.z = Mathf.Round(cube.z);

        var x_diff = Mathf.Abs(r.x - cube.x);
        var y_diff = Mathf.Abs(r.y - cube.y);
        var z_diff = Mathf.Abs(r.z - cube.z);

        if (x_diff > y_diff && x_diff > z_diff) r.x = -r.y - r.z;
        else if (y_diff > z_diff) r.y = -r.x - r.z;
        else r.z = -r.x - r.y;

        return r;
    }

    public Vector3 axial_to_cube(Vector2 v)
    {
        return new Vector3(v.x, v.y, -v.x - v.y);
    }

    public Vector2 cube_to_axial(Vector3 cube)
    {
        if (cube.x + cube.y + cube.z != 0) throw new IndexOutOfRangeException("Coordinates are not part of hex grid");
        return new Vector2(cube.x, cube.y);
    }

    public HexCellBehaviour GetCell(int x, int y)
    {
        return _grid[size + x, size + y];
    }

    public void SetCell(int x, int y, HexCellBehaviour cell)
    {
        _grid[size + x, size + y] = cell;
    }

    public HexCellBehaviour GetCell(Vector3 cube)
    {
        return GetCell((int)cube.x, (int)cube.y);
    }

    public void SetCell(Vector3 cube, HexCellBehaviour c)
    {
        _grid[size + (int)cube.x, size + (int)cube.y] = c;
    }

    public int DistanceBetween(Vector3 a, Vector3 b)
    {
        int dx = Math.Abs(Mathf.RoundToInt(a.x) - Mathf.RoundToInt(b.x));
        int dy = Math.Abs(Mathf.RoundToInt(a.y) - Mathf.RoundToInt(b.y));
        int dz = Math.Abs(Mathf.RoundToInt(a.z) - Mathf.RoundToInt(b.z));
        return Math.Max(Math.Max(dx, dy), Math.Max(dx, dz));
    }

    public int DistanceFromCenter(Vector3 cube)
    {
        int x = Math.Abs(Mathf.RoundToInt(cube.x));
        int y = Math.Abs(Mathf.RoundToInt(cube.y));
        int z = Math.Abs(Mathf.RoundToInt(cube.z));
        return ((x + y + z) / 2);
    }
    public bool IsOutOfBounds(Vector3 cube)
    {
        int dist = DistanceFromCenter(cube);
        Debug.Log("Distance = " + dist);
        return dist > size;
    }
}
