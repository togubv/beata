using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] private Core core;

    [SerializeField] private Quad prefabQuad, prefabQuadFinish, prefabQuadEmpty;

    private List<Quad> poolQuad;
    private List<Quad> poolQuadEmpty;
    private List<Quad> poolQuadFinish;
    private List<Quad> quads;

    private int prefabsStart = 20;
    private int prefabsDistance = 10;

    private void Start()
    {
        poolQuad = new List<Quad>();
        poolQuadEmpty = new List<Quad>();
        poolQuadFinish = new List<Quad>();
        quads = new List<Quad>();
    }
    public void GenerateLevel(int start, int distance, int lvl)
    {
        StartCoroutine(GeneratingLevel(start, distance, lvl));
    }

    private IEnumerator GeneratingLevel(int start, int distance, int lvl)
    {
        prefabsStart = start;
        prefabsDistance = distance;
        core.goPlayer.transform.position = new Vector3(0, 0, 0);
        for (int i = 0; i < (lvl * 15) * 4; i += 4)
        {
            GenerateFigure(i, Random.Range(0, 10));
            if (i > 5)
            {
                float cooldown = 0.1f; // 1.0f - 0.05f * lvl;
                if (cooldown < 0) cooldown = 0;
                yield return new WaitForSeconds(cooldown);
            }
        }
        RefreshQuad(lvl * 15 * 4, lvl * 15 * 4, GetQuadFromPool(QuadType.Finish), 0.5f, 0.5f);
    }

    public Quad InstantiateQuad(QuadType type)
    {
        switch (type)
        {
            case QuadType.None:
                Quad quad = Instantiate(prefabQuad, transform);
                poolQuad.Add(quad);
                return quad;

            case QuadType.Empty:
                Quad quadEmpty = Instantiate(prefabQuadEmpty, transform);
                poolQuadEmpty.Add(quadEmpty);
                return quadEmpty;

            case QuadType.Finish:
                Quad quadFinish = Instantiate(prefabQuadFinish, transform);
                poolQuadFinish.Add(quadFinish);
                return quadFinish;
        }
        return null;
    }

    private Quad GetQuadFromPool(QuadType type)
    {
        switch (type)
        {
            case QuadType.None:
                foreach (Quad quad in poolQuad)
                {
                    if (quad.gameObject.activeInHierarchy == false)
                    {
                        return quad;
                    }
                }
                break;

            case QuadType.Empty:
                foreach (Quad quad in poolQuadEmpty)
                {
                    if (quad.gameObject.activeInHierarchy == false)
                    {
                        return quad;
                    }
                }
                break;

            case QuadType.Finish:

                foreach (Quad quad in poolQuadFinish)
                {
                    if (quad.gameObject.activeInHierarchy == false)
                    {
                        return quad;
                    }
                }
                break;
        }
        return InstantiateQuad(type);
    }

    public void ReturnQuads(int j)
    {
        DelayedReturnQuads(j);
    }

    private void DelayedReturnQuads(int j)
    {
        if (j <= 0)
        {
            return;
        }

        for (int i = j - 4; i < j; i++)
        {
            quads[i].ReturnToPool();
        }
    }

    public void ReturnToPoolOtherPrefabs()
    {
        StopAllCoroutines();
        foreach (Quad quad in quads)
        {
            if (quad.gameObject.activeInHierarchy) 
                quad.ReturnToPool();
        }

        quads.Clear();
    }

    public void RefreshQuad(int i, float position, Quad quad, float x, float y)
    {
        quad.gameObject.SetActive(true);
        quad.gameObject.transform.position = new Vector3(x, y, prefabsStart + (position / 4 * prefabsDistance));
        quad.position = ((int)position);
        quads.Add(quad);
    }

    public void GenerateFigure(int i, int figure)
    {
        switch (figure)
        {
            case 0:
                RefreshQuad(i, i, GetQuadFromPool(QuadType.None), 0, 0);
                RefreshQuad(i + 1, i, GetQuadFromPool(QuadType.None), 0, 1);
                RefreshQuad(i + 2, i, GetQuadFromPool(QuadType.Empty), 1, 0);    //  XO
                RefreshQuad(i + 3, i, GetQuadFromPool(QuadType.Empty), 1, 1);    //  XO
                break;
            case 1:
                RefreshQuad(i, i, GetQuadFromPool(QuadType.None), 1, 0);
                RefreshQuad(i + 1, i, GetQuadFromPool(QuadType.None), 1, 1);
                RefreshQuad(i + 2, i, GetQuadFromPool(QuadType.Empty), 0, 0);    //  OX
                RefreshQuad(i + 3, i, GetQuadFromPool(QuadType.Empty), 0, 1);    //  OX
                break;
            case 2:
                RefreshQuad(i, i, GetQuadFromPool(QuadType.None), 0, 1);
                RefreshQuad(i + 1, i, GetQuadFromPool(QuadType.None), 1, 1);
                RefreshQuad(i + 2, i, GetQuadFromPool(QuadType.Empty), 0, 0);    //  XX
                RefreshQuad(i + 3, i, GetQuadFromPool(QuadType.Empty), 1, 0);    //  OO
                break;
            case 3:
                RefreshQuad(i, i, GetQuadFromPool(QuadType.None), 0, 0);
                RefreshQuad(i + 1, i, GetQuadFromPool(QuadType.None), 1, 0);
                RefreshQuad(i + 2, i, GetQuadFromPool(QuadType.Empty), 0, 1);    //  OO
                RefreshQuad(i + 3, i, GetQuadFromPool(QuadType.Empty), 1, 1);    //  XX
                break;
            case 4:
                RefreshQuad(i, i, GetQuadFromPool(QuadType.None), 0, 1);
                RefreshQuad(i + 1, i, GetQuadFromPool(QuadType.None), 1, 0);
                RefreshQuad(i + 2, i, GetQuadFromPool(QuadType.Empty), 0, 0);    //  XO
                RefreshQuad(i + 3, i, GetQuadFromPool(QuadType.Empty), 1, 1);    //  OX
                break;
            case 5:
                RefreshQuad(i, i, GetQuadFromPool(QuadType.None), 1, 1);
                RefreshQuad(i + 1, i, GetQuadFromPool(QuadType.None), 0, 0);
                RefreshQuad(i + 2, i, GetQuadFromPool(QuadType.Empty), 1, 0);    //  OX
                RefreshQuad(i + 3, i, GetQuadFromPool(QuadType.Empty), 0, 1);    //  XO
                break;
            case 6:
                RefreshQuad(i, i, GetQuadFromPool(QuadType.None), 0, 0);
                RefreshQuad(i + 1, i, GetQuadFromPool(QuadType.None), 1, 0);
                RefreshQuad(i + 2, i, GetQuadFromPool(QuadType.None), 0, 1);    //     XO
                RefreshQuad(i + 3, i, GetQuadFromPool(QuadType.Empty), 1, 1);   //     XX
                break;
            case 7:
                RefreshQuad(i, i, GetQuadFromPool(QuadType.None), 0, 0);
                RefreshQuad(i + 1, i, GetQuadFromPool(QuadType.None), 0, 1);
                RefreshQuad(i + 2, i, GetQuadFromPool(QuadType.None), 1, 1);    //     XX
                RefreshQuad(i + 3, i, GetQuadFromPool(QuadType.Empty), 1, 0);   //     XO
                break;
            case 8:
                RefreshQuad(i, i, GetQuadFromPool(QuadType.None), 0, 1);
                RefreshQuad(i + 1, i, GetQuadFromPool(QuadType.None), 1, 1);
                RefreshQuad(i + 2, i, GetQuadFromPool(QuadType.None), 1, 0);    //     XX
                RefreshQuad(i + 3, i, GetQuadFromPool(QuadType.Empty), 0, 0);   //     OX
                break;
            case 9:
                RefreshQuad(i, i, GetQuadFromPool(QuadType.None), 1, 1);
                RefreshQuad(i + 1, i, GetQuadFromPool(QuadType.None), 1, 0);
                RefreshQuad(i + 2, i, GetQuadFromPool(QuadType.None), 0, 0);    //     OX
                RefreshQuad(i + 3, i, GetQuadFromPool(QuadType.Empty), 0, 1);   //     XX
                break;
        }
    }
}
