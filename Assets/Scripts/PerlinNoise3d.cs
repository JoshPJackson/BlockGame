using UnityEngine;

public class PerlinNoise3d
{
    public static float Make(float x, float y, float z)
    {
        float xy = Mathf.PerlinNoise(x, y);
        float xz = Mathf.PerlinNoise(x, z);
        float yz = Mathf.PerlinNoise(y, z);
        float yx = Mathf.PerlinNoise(y, x);
        float zx = Mathf.PerlinNoise(z, x);
        float zy = Mathf.PerlinNoise(z, y);

        return (xy + xz + yz + yx + zx + zy) / 6;
    }

    public static float Make(Vector3 position)
    {
        float x = position.x;
        float y = position.y;
        float z = position.z;

        return Make(x, y, z);
    }
}


