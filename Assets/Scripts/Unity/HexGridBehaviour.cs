using UnityEngine;
using System.Collections.Generic;
using System;

public class HexGridBehaviour : MonoBehaviour
{
    public int size = 5;
    public float cellsize = 1;
    public HexCellBehaviour _prefab;
    public const int HEXMASK = 1 >> 8;

    [SerializeField]
    private HexCellBehaviour[,] _grid;
    private const float SQRT3 = 1.7320508075688772935274463415058723669428052538103806280f;
    private HexCellBehaviour lastHex = null;

    private void Start()
    {
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
                }
            }
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
                _grid[size+gx, size+gy] = cell;
                cube.x = ax.x = gx;
                cube.y = ax.y = gy;
                cube.z = -gx - gy;
                cell.cubeCoordinates = cube;
                cell.transform.position = hex_to_pixel(ax);
                cell.transform.SetParent(this.transform);
            }
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

    public HexCellBehaviour GetCell(Vector3 cube)
    {
        return GetCell((int)cube.x, (int)cube.y);
    }

    public void SetCell(Vector3 cube, HexCellBehaviour c)
    {
        _grid[size + (int)cube.x, size + (int)cube.y] = c;
    }

}
