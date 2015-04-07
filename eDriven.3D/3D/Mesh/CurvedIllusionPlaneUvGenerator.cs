using System;
using UnityEngine;

namespace eDriven.Mesh
{
    public class CurvedIllusionPlaneUvGenerator : IUvGenerator
    {
        /// <summary>
        /// The curvature parameter controls how much curved will the plane be.
        /// </summary>
        public float Curvature = 5;

        /// <summary>
        /// Number of repeat in X direction
        /// </summary>
        public float UTiles = (float) 1/16; //8; //16f; //16; //(float)1/16;

        /// <summary>
        /// Number of repeat in Y direction
        /// </summary>
        public float VTiles = (float) 1/16; //8; //16; //(float)1 / 161;

        // actual values irrelevant, it's the relation between the sphere's radius and the camera's position which is important
        // ReSharper disable InconsistentNaming
        const float SPHERE_RADIUS = 1000;
        const float CAMERA_DISTANCE = 500;
        // ReSharper restore InconsistentNaming

        public Vector2[] CalculateUv(float width, float height, int xVertices, int yVertices)
        {
            float xSpace = width / (float)(xVertices-1);
            float ySpace = height / (float)(yVertices-1);

            float halfWidth = width / 2;
            float halfHeight = height / 2;

            Vector2[] uvs = new Vector2[xVertices * yVertices];

            // generate vertex data, imagine a large sphere with the camera located near the top,
            // the lower the curvature, the larger the sphere.  use the angle from the viewer to the
            // points on the plane

            // actual values irrelevant, it's the relation between the sphere's radius and the camera's position which is important
            float sphereRadius = SPHERE_RADIUS - Curvature;
            float cameraPosition = sphereRadius - CAMERA_DISTANCE; // camera position relative to the sphere center

            int count = 0;
            for (int y = 0; y < yVertices; y++)
            {
                for (int x = 0; x < xVertices; x++)
                {
                    // centered on origin
                    Vector3 vec = new Vector3();
                    vec.x = (x * xSpace) - halfWidth;
                    vec.y = (y * ySpace) - halfHeight;
                    vec.z = 0;

                    Quaternion q = Quaternion.identity;
                    q = Quaternion.Inverse(q);

                    vec = q*vec;
                    vec.Normalize(); // / vec.magnitude; // normalize

                    float sphereDistance = (float)Math.Sqrt(cameraPosition * cameraPosition * (vec.y * vec.y - 1.0f) + sphereRadius * sphereRadius) - cameraPosition * vec.y;

                    vec.x *= sphereDistance;
                    vec.y *= sphereDistance;

                    // use x and y on sphere as texture coordinates, tiled
                    Vector2 uv = new Vector2();
                    //uv.x = vec.x*UTiles;
                    //uv.y = vec.y*VTiles;
                    uv.x = vec.x * (0.01f * UTiles);
                    uv.y = 1 - vec.y * (0.01f * VTiles);
                    uvs[count] = uv;

                    count++;
                }
            }

            return uvs;
        }
    }
}
