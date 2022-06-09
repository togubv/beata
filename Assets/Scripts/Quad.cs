using UnityEngine;

public class Quad : MonoBehaviour, IPosition
{
    public int position;

    public int Take()
    {
        return this.position;
    }
}
