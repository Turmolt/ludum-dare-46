using UnityEngine;

public static class Ext
{
    public static Vector3 xy(this Vector3 _v)
    {
        return new Vector3(_v.x, _v.y,0);
    }

    public static Vector3 xz(this Vector3 _v)
    {
        return new Vector3(_v.x, 0, _v.z);
    }

    public static Vector3 yz(this Vector3 _v)
    {
        return new Vector3(0, _v.y, _v.z);
    }

    public static Vector3 xy(this Vector3 _v, float z)
    {
        return new Vector3(_v.x, _v.y, z);
    }

    public static Vector3 xz(this Vector3 _v, float y)
    {
        return new Vector3(_v.x, y, _v.z);
    }

    public static Vector3 yz(this Vector3 _v, float x)
    {
        return new Vector3(x, _v.y, _v.z);
    }

    public static Vector3 x(this Vector3 _v)
    {
        return new Vector3(_v.x, 0, 0);
    }

    public static Vector3 y(this Vector3 _v)
    {
        return new Vector3(0, _v.y, 0);
    }

    public static Vector3 z(this Vector3 _v)
    {
        return new Vector3(0f, 0, _v.z);
    }
}
