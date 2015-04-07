using System;
using System.Collections.Generic;
using UnityEngine;

namespace eDriven.Mesh
{
    /// <summary>
    /// Generates a mesh
    /// </summary>
    public class MeshGenerator
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Global
        public float Width = 1000;
        public float Height = 1000;
        public int XSegments = 1;
        public int YSegments = 1;
        public UvMode UvMode = UvMode.Normal;
        // ReSharper restore FieldCanBeMadeReadOnly.Global

        private readonly List<Vector3> _vertices = new List<Vector3>();
        public List<Vector3> Vertices
        {
            get { return _vertices; }
        }
        
        private readonly List<int> _triangles = new List<int>();
        public List<int> Triangles
        {
            get { return _triangles; }
        }
        
        private readonly List<Vector3> _normals = new List<Vector3>();
        public List<Vector3> Normals
        {
            get { return _normals; }
        }

        private readonly List<Vector2> _uv = new List<Vector2>();
        public List<Vector2> Uv
        {
            get { return _uv; }
        }

        public UnityEngine.Mesh Mesh
        {
            get
            {
                UnityEngine.Mesh m = new UnityEngine.Mesh();
                m.vertices = _vertices.ToArray();
                m.triangles = _triangles.ToArray();
                m.normals = _normals.ToArray();
                m.uv = _uv.ToArray();
                return m;
            }
        }

        public MeshGenerator()
        {
        }

        public MeshGenerator(float width, float height, int xSegments, int ySegments)
        {
            Width = width;
            Height = height;
            XSegments = xSegments;
            YSegments = ySegments;
        }

        private int _lastVertexCount;

        /// <summary>
        /// Creates 2D mesh in a plane
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="cameraUpVector"></param>
        public void Tessalate2DMesh(Plane plane, Vector3 cameraUpVector)
        {
            int xVertices = XSegments + 1;
            int yVertices = YSegments + 1;

            Vector3[] vertices = new Vector3[xVertices * yVertices];
            Vector3[] normals = new Vector3[vertices.Length];

            Vector3 meshZAxis = plane.normal;
            Vector3 meshXAxis = Vector3.Cross(cameraUpVector, meshZAxis);

            // check just in case that cameraUpVector and planeNormal are not parallel
            if (meshXAxis.magnitude == 0)
                throw new Exception("The mesh up vector cannot be parallel to the plane normal.");

            float xSpace = Width / /*(float)*/XSegments;
            float ySpace = Height / /*(float)*/YSegments;
            float halfWidth = Width / 2;
            float halfHeight = Height / 2;

            int count = 0;

            /**
             * GetEaser vertices in local space
             * */

            for (int y = 0; y < yVertices; y++)
            {
                for (int x = 0; x < xVertices; x++)
                {
                    float xCoord = x * xSpace;
                    float yCoord = y * ySpace;
                    vertices[count] = new Vector3(xCoord - halfWidth, yCoord - halfHeight);
                    count++;
                }
            }

            /**
             * Transform vertices to global space
             * */

            // set transformation matrix
            Matrix4x4 transform = Matrix4x4.identity;
            transform.m00 = meshXAxis.x;
            transform.m01 = meshXAxis.y;
            transform.m02 = meshXAxis.z;
            //transform.m03 = 1;
            transform.m10 = cameraUpVector.x;
            transform.m11 = cameraUpVector.y;
            transform.m12 = cameraUpVector.z;
            //transform.m13 = 1;
            transform.m20 = meshZAxis.x;
            transform.m21 = meshZAxis.y;
            transform.m22 = meshZAxis.z;
            //transform.m23 = 1;

            // add translation by plane distance (in the opoposite direction of plane normal)
            transform = Matrix4x4.TRS(-plane.normal*plane.distance, Quaternion.identity, Vector3.one) * transform;

            // transform vertices
            int vertCount = vertices.Length;
            for (int i = 0; i < vertCount; i++)
            {
                vertices[i] = transform.MultiplyPoint(vertices[i]);
                normals[i] = plane.normal;
            }

            //Debug.Log("XSegments: " + XSegments + "; YSegments: " + YSegments);
            //Debug.Log("vertices.Length: " + vertices.Length);

            // indices (triangles)

            int[] triangles = new int[6 * XSegments * YSegments];

            //Debug.Log("triangles.Length: " + triangles.Length);

            int vInc, uInc, v, u; //, iterations;

            vInc = 1;
            v = 0;

            //iterations = doubleSided ? 2 : 1;

            // make tris in a zigzag pattern (strip compatible)
            u = 0;
            uInc = 1;

            int vCount = YSegments;

            // reset count
            count = 0;

            while (0 < vCount--)
            {
                int uCount = XSegments;

                while (0 < uCount--)
                {
                    // first triangle in the cell
                    // -----------------
                    triangles[count++] = (short)(_lastVertexCount + ((v + vInc) * xVertices) + u);
                    triangles[count++] = (short)(_lastVertexCount + (v * xVertices) + u);
                    triangles[count++] = (short)(_lastVertexCount + ((v + vInc) * xVertices) + (u + uInc));
                    // second triangle in the cell
                    // ------------------
                    triangles[count++] = (short)(_lastVertexCount + ((v + vInc) * xVertices) + (u + uInc));
                    triangles[count++] = (short)(_lastVertexCount + (v * xVertices) + u);
                    triangles[count++] = (short)(_lastVertexCount + (v * xVertices) + (u + uInc));

                    // Next column
                    u += uInc;

                    //Debug.Log("***2. Count: " + count);

                } // while uCount

                v += vInc;
                u = 0;

            } // while vCount

            //v = YSegments - 1;
            //vInc = -vInc;

            _vertices.AddRange(vertices);
            _triangles.AddRange(triangles);
            _normals.AddRange(normals);
            
            /**
             * GetEaser right UV generator
             * */

            IUvGenerator gene;

            switch (UvMode)
            {
                case UvMode.Normal:
                    gene = new NormalUvGenerator();
                    break;
                //case UvMode.Curved:
                default:
                    gene = new CurvedIllusionPlaneUvGenerator();
                    break;
            }

            /**
             * Calculate UV for each vertex
             * */
            //CurvedIllusionPlaneUvGenerator gene = new CurvedIllusionPlaneUvGenerator();
            //NormalUvGenerator gene = new NormalUvGenerator();
            _uv.AddRange(gene.CalculateUv(Width, Height, xVertices, yVertices));
            //mesh.RecalculateNormals();

            _lastVertexCount += vertices.Length;

            //return mesh;
        }

        public void Clear()
        {
            _vertices.Clear();
            _triangles.Clear();
            _lastVertexCount = 0;
            _normals.Clear();
            _uv.Clear();
        }
    }

    public enum UvMode
    {
        Normal, Curved
    }
}
