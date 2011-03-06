using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Learning.Physics
{
    struct AABB
    {
        private BoundingBox _box;

        public AABB(Vector3 min, Vector3 max)
        {
            _box.Min = min;
            _box.Max = max;
        }

        public Vector3 Max
        {
            get { return _box.Max; }
            set { _box.Max = value; }
        }

        public Vector3 Min
        {
            get { return _box.Min; }
            set { _box.Min = value; }
        }

        public Vector3 Extents
        {
            get
            {
                return _box.Max - _box.Min;
            }
        }

        public List<Vector3> Vertices
        {
            get
            {
                Vector3 dX = new Vector3(Extents.X, 0, 0);
                Vector3 dY = new Vector3(0, Extents.Y, 0);
                Vector3 dZ = new Vector3(0, 0, Extents.Z);
                List<Vector3> Vertices = new List<Vector3>();

                Vertices.Add(_box.Max);
                Vertices.Add(_box.Min);
                Vertices.Add(_box.Min + dX);
                Vertices.Add(_box.Min + dY);
                Vertices.Add(_box.Min + dZ);
                Vertices.Add(_box.Max - dX);
                Vertices.Add(_box.Max - dY);
                Vertices.Add(_box.Max - dZ);

                return Vertices;
            }
        }

        public bool Intersects(AABB other)
        {
            return _box.Intersects(other._box);
        }
        public float? Intersects(Ray ray)
        {
            return _box.Intersects(ray);
        }
        public ContainmentType Contains(AABB box)
        {
            return _box.Contains(box._box);
        }
        public ContainmentType Contains(Vector3 point)
        {
            return _box.Contains(point);
        }
    }
}
