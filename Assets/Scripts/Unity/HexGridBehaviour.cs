using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

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
    private const float NEARZERO = 0.0001f;
    private bool[,] visited;

    private Dictionary<HexPassable, Vector3> cubeNeighboursCoordinates;

    private HexCellBehaviour from, to;
    private List<HexCellBehaviour> path = new List<HexCellBehaviour>();
    
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
            if (from != null) from.SetHighLight(false);
            foreach (var cell in path) cell.SetHighLight(false);

            if (Physics.Raycast(ray, out hitInfo, 1 << 8))
            {
                Vector3 c = axial_to_cube(pixel_to_hex(hitInfo.point));
                from = GetCell(c);
                if (from != null)
                {
                    from.SetHighLight(true);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (to != null) to.SetHighLight(false);
            foreach (var cell in path) cell.SetHighLight(false);

            if (Physics.Raycast(ray, out hitInfo, 1 << 8))
            {
                Vector3 c = axial_to_cube(pixel_to_hex(hitInfo.point));
                to = GetCell(c);
                if (to != null)
                {
                    to.SetHighLight(true);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(FindPath(from, to));
        }
    }

    private IEnumerator FindPath(HexCellBehaviour from, HexCellBehaviour to)
    {
        this.path.Clear();
        Dictionary<HexCellBehaviour, int> cost = new Dictionary<HexCellBehaviour, int>();
        Dictionary<HexCellBehaviour, HexCellBehaviour> parent = new Dictionary<HexCellBehaviour, HexCellBehaviour>();
        Queue<HexCellBehaviour> queue = new Queue<HexCellBehaviour>();
        List<HexCellBehaviour> unvisitedNeighbours = new List<HexCellBehaviour>();
        cost[from] = 0;
        queue.Enqueue(from);

        bool found = false;
        while (queue.Count > 0 && !found)
        {
            HexCellBehaviour current = queue.Dequeue();
            LightPath(from, current, true, parent);
            Debug.Log("Next node: " + current.cubeCoordinates);
            yield return new WaitForSeconds(.3f);
            if (current == to)
            {
                found = true;
                break;
            }

            //find all (unvisited) neighbours
            unvisitedNeighbours.Clear();
            foreach (HexPassable dir in Enum.GetValues(typeof(HexPassable)))
            {
                if (!current.CanGo(dir)) continue;
                Vector3 cube = current.cubeCoordinates + cubeNeighboursCoordinates[dir];
                if (IsOutOfBounds(cube)) continue;
                HexCellBehaviour neighbour = GetCell(cube);
                if (!cost.ContainsKey(neighbour))
                {
                    queue.Enqueue(neighbour);
                    unvisitedNeighbours.Add(neighbour);
                    cost[neighbour] = cost[current] + 1;
                    parent[neighbour] = current;
                }
                if (cost[neighbour] > (cost[current] + 1))
                {
                    queue.Enqueue(neighbour);
                    unvisitedNeighbours.Add(neighbour);
                    cost[neighbour] = cost[current] + 1;
                    parent[neighbour] = current;
                }
            }

            LightPath(from, current, false, parent);
        }

        if (found)
        {
            Debug.Log("Found a path!");
            for (HexCellBehaviour cell = to; cell != from; cell = parent[cell])
            {
                this.path.Add(cell);
                cell.SetHighLight(true);
            }
            from.SetHighLight(true);
        }
        else
        {
            Debug.Log("Path not found");
        }

    }

    private void LightPath(HexCellBehaviour from, HexCellBehaviour to, bool lit, Dictionary<HexCellBehaviour, HexCellBehaviour> parent)
    {
        for (HexCellBehaviour cell = to; cell != null && cell != from; cell = parent[cell])
        {
            cell.SetHighLight(lit);
        }
        from.SetHighLight(lit);
    }

    private System.Collections.IEnumerator LightUpNeighbours(HexCellBehaviour cell)
    {
        HexCellBehaviour n;

        n = GetNeighbour(cell, HexPassable.N);
        if (n != null)
        {
            Debug.Log("North, direction = " + FindDirection(cell, n));
            n.SetHighLight(true);
            yield return new WaitForSeconds(1f);
            n.SetHighLight(false);
        }
        n = GetNeighbour(cell, HexPassable.NE);
        if (n != null)
        {
            Debug.Log("North East, direction = " + FindDirection(cell, n));
            if (n != null) n.SetHighLight(true);
            yield return new WaitForSeconds(1f);
            n.SetHighLight(false);
        }

        n = GetNeighbour(cell, HexPassable.SE);
        if (n != null)
        {
            Debug.Log("South East, direction = " + FindDirection(cell, n));
            n.SetHighLight(true);
            yield return new WaitForSeconds(1f);
            n.SetHighLight(false);
        }
        n = GetNeighbour(cell, HexPassable.S);
        if (n != null)
        {
            Debug.Log("South, direction = " + FindDirection(cell, n));
            n.SetHighLight(true);
            yield return new WaitForSeconds(1f);
            n.SetHighLight(false);
        }
        n = GetNeighbour(cell, HexPassable.SW);
        if (n != null)
        {
            Debug.Log("South West, direction = " + FindDirection(cell, n));
            n.SetHighLight(true);
            yield return new WaitForSeconds(1f);
            n.SetHighLight(false);
        }
        n = GetNeighbour(cell, HexPassable.NW);
        if (n != null)
        {
            Debug.Log("North West, direction = " + FindDirection(cell, n));
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
        visited = new bool[2 * size + 1, 2 * size + 1];
        for (int q=0; q<2*size+1; ++q)
            for (int r=0; r<2*size+1; ++r)
                visited[q, r] = false;

        //pick a random cell (withing range 'size') to start from:
        int rx = UnityEngine.Random.Range(-size, size);
        int ry = UnityEngine.Random.Range(Math.Max(-size, -rx - size), Math.Min(size, -rx + size));
        HexCellBehaviour randomCell = GetCell(rx, ry);

        //recursively call this function to make passage ways:
        StartCoroutine(MazeRun(randomCell));
    }

    //recursive procedure to create a maze
    private System.Collections.IEnumerator MazeRun(HexCellBehaviour cell)
    {
        cell.SetHighLight(true);
        yield return new WaitForSeconds(.2f);
        cell.SetHighLight(false);

        Vector2 axial =  this.cube_to_axial(cell.cubeCoordinates);
        int aq = Mathf.RoundToInt(axial.x);
        int ar = Mathf.RoundToInt(axial.y);

        visited[size+aq, size+ar] = true; //mark visited

        List<HexCellBehaviour> CandidateNeighbors = new List<HexCellBehaviour>(6);

        foreach (HexPassable d in Enum.GetValues(typeof(HexPassable)))
        {
            Vector3 ncube = cubeNeighboursCoordinates[d];
            ncube += cell.cubeCoordinates; //calculate absolute cube coordinates for each neighbour
            if (!IsOutOfBounds(ncube))
            {
                //we are interested:
                CandidateNeighbors.Add(GetCell(ncube));
            }
        }

        //choose an interesting neighbour and continue path
        while (CandidateNeighbors.Count > 0)
        {
            //from viable candidates, choose 1 direction to continue:
            HexCellBehaviour chosen = CandidateNeighbors[UnityEngine.Random.Range(0, CandidateNeighbors.Count)];

            Vector2 naxial = this.cube_to_axial(chosen.cubeCoordinates);
            int naq = Mathf.RoundToInt(naxial.x);
            int nar = Mathf.RoundToInt(naxial.y);

            if (!visited[size + naq, size + nar])
            {
                CreatePathWay(cell, chosen);
                yield return MazeRun(chosen);
            }
            else if (UnityEngine.Random.Range(0f, 1f) < _chanceForPassage)
            {
                //even though the cell was already visited, we create an exta pathway:
                CreatePathWay(cell, chosen);
            }
            CandidateNeighbors.Remove(chosen);
        }

    }

    //this method sets the HexPassable attribute in both ways between 2 neighbouring cells
    private void CreatePathWay(HexCellBehaviour cell, HexCellBehaviour neighbour)
    {
        HexPassable dir = FindDirection(cell, neighbour);
        cell.AllowGo(dir);
        neighbour.AllowGo(OppositeDirection(dir));
    }

    private HexPassable FindDirection(HexCellBehaviour from, HexCellBehaviour to)
    {
        Vector3 diff = to.cubeCoordinates - from.cubeCoordinates;
        foreach (HexPassable dir in Enum.GetValues(typeof(HexPassable)))
        {
            Vector3 dv = cubeNeighboursCoordinates[dir];
            if ((diff - dv).sqrMagnitude < NEARZERO) return dir;
        }

        //this happens only if you try to find the direction between 2 cells that are no neighbours:
        throw new ArgumentException("can't find direction between non-neighboring cells");
    }

    public HexPassable OppositeDirection(HexPassable dir)
    {
        switch (dir)
        {
            case HexPassable.N: return HexPassable.S;
            case HexPassable.NE: return HexPassable.SW;
            case HexPassable.NW: return HexPassable.SE;
            case HexPassable.S: return HexPassable.N;
            case HexPassable.SE: return HexPassable.NW;
            case HexPassable.SW: return HexPassable.NE;
            default: return HexPassable.N;
        }
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
        return dist > size;
    }
}
