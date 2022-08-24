using UnityEngine;

public class Quad : MonoBehaviour, IPosition
{
    [SerializeField] private QuadType quadType;

    public int position;
    public QuadType GetQuadType => quadType;

    public int Take()   
    {
        return this.position;
    }

    public void ReturnToPool()
    {
        gameObject.SetActive(false);
    }
}
