using System;

namespace Hex
{
    [Serializable]
    public class AxialCoordinates
    {
        public int q { get; private set; }
        public int r { get; private set; }

        public AxialCoordinates()
        {
            q = r = 0;
        }

        public AxialCoordinates(CubeCoordinates cube)
        {
            if (cube.x + cube.y + cube.z != 0) throw new IndexOutOfRangeException("Coordinates are not part of hex grid");
            q = cube.x;
            r = cube.y;
        }
    }
}
