using UnityEngine;

namespace eDriven.Mesh
{
    public interface IUvGenerator
    {
        Vector2[] CalculateUv(float width, float height, int xVertices, int yVertices);
    }
}