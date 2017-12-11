using UnityEngine;
using System.Collections;
using System;

public class HexGridBehaviour : MonoBehaviour
{
    public int size = 5;
    public HexCellBehaviour _prefab;

    [SerializeField]
    private HexCellBehaviour[,] _grid;

    [ContextMenu("Create Grid")]
    public void CreateGrid()
    {
        _grid = new HexCellBehaviour[2 * size + 1, 2 * size + 1];
        for (int gx=-size; gx < size; ++gx)
        {
            for (int gy = Math.Max(-size, -gx-size); gy < Math.Min(size, -gx+size))
            {
                HexCellBehaviour cell = Instantiate(_prefab);
                cell.transform.position = 
            }
        }
    }


    public Vector2 Convert3DCoordinates(Vector3 v)
    {
        if (v.x + v.y + v.z != 0) throw new IndexOutOfRangeException("Coordinates are not part of hex grid");
        return new Vector2(v.x, v.y);
    }

    public Vector3 Convert2DCoordinates(Vector2 v)
    {
        return new Vector3(v.x, v.y, -v.x - v.y);
    }

    public Vector3 WorldCoordinates(Vector3 v)
    {
        return new Vector3(v.x, 0, v.z + (v.x - ((int)v.x) % 1) / 2);
    }

    public Vector3 WorldCoordinates(Vector2 v)
    {
        return Vector3.zero;
    }
}
