using System;
using UnityEngine;

namespace Hex
{

    [Serializable]
    public class CubeCoordinates
    {
        public int x { get; private set; }
        public int y { get; private set; }
        public int z { get; private set; }

        private const float NEARZERO = 0.0001f;

        public CubeCoordinates()
        {
            x = y = z = 0;
        }

        public CubeCoordinates(int nx, int ny, int nz)
        {
            if (x + y + z != 0) throw new ArgumentOutOfRangeException("x+y+z should be zero");
            x = nx;
            y = ny;
            z = nz;
        }

        public CubeCoordinates(AxialCoordinates ax)
        {
            x = ax.q;
            y = ax.r;
            z = -ax.q - ax.r;
        }

        public static CubeCoordinates Round(Vector3 cube)
        {
            if (Mathf.Abs(cube.x + cube.y + cube.z) > NEARZERO) throw new ArgumentOutOfRangeException("x+y+z should be zero");

            CubeCoordinates r = new CubeCoordinates();
            r.x = Mathf.RoundToInt(cube.x);
            r.y = Mathf.RoundToInt(cube.y);
            r.z = Mathf.RoundToInt(cube.z);

            var x_diff = Mathf.Abs(r.x - cube.x);
            var y_diff = Mathf.Abs(r.y - cube.y);
            var z_diff = Mathf.Abs(r.z - cube.z);

            if (x_diff > y_diff && x_diff > z_diff) r.x = -r.y - r.z;
            else if (y_diff > z_diff) r.y = -r.x - r.z;
            else r.z = -r.x - r.y;

            return r;
        }

        public static int operator -(CubeCoordinates a, CubeCoordinates b)
        {
            return (Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y) + Math.Abs(a.z - b.z)) / 2;
        }

        public int Magnitude()
        {
            return Mathf.Max(Math.Abs(x), Math.Abs(y), Math.Abs(z));
        }

    }

}
