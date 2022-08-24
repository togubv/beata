using UnityEngine;

public class Quad : MonoBehaviour, IPosition
{
    [SerializeField] private QuadType quadType;

    public QuadType GetQuadType => quadType;

    public int position;

    public int Take()   
    {
        return position;
    }

    public void ReturnToPool()
    {
        gameObject.SetActive(false);
    }
}
