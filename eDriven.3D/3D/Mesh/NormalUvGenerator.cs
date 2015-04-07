using UnityEngine;

namespace eDriven.Mesh
{
    public class NormalUvGenerator : IUvGenerator
    {
        public Vector2[] CalculateUv(float width, float height, int xVertices, int yVertices)
        {
            float xSpace = width / (xVertices - 1);
            float ySpace = height / (yVertices - 1); 
            Vector2[] uv = new Vector2[xVertices * yVertices];
            
            int count = 0;
            for (int y = 0; y < yVertices; y++)
            {
                for (int x = 0; x < xVertices; x++)
                {
                    float xCoord = x * xSpace;
                    float yCoord = y * ySpace;
                    uv[count] = new Vector2(xCoord / width, yCoord / height);
                    count++;
                }
            }

            return uv;
        }
    }
}
