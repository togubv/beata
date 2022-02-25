using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public Game game;

    [SerializeField] private GameObject prefabQuad, prefabQuadFinish, prefabEmpty;

    [SerializeField] private List<GameObject> quad;
    [SerializeField] private Transform quadPrefabs;
    private int prefabsStart = 20;
    private int prefabsDistance = 10;

    public void GenerateLevel(int start, int distance, int lvl)
    {
        StartCoroutine(GeneratingLevel(start, distance, lvl));
    }

    public IEnumerator GeneratingLevel(int start, int distance, int lvl)
    {
        game.GameStateGame();
        prefabsStart = start;
        prefabsDistance = distance;
        game.goPlayer.transform.position = new Vector3(0, 0, 0);
        for (int i = 0; i < (lvl * 15) * 4; i += 4)
        {
            GenerateFigure(i, Random.Range(0, 10));
            if (i > 5)
            {
                float cooldown = 1.0f - 0.05f * lvl;
                if (cooldown < 0) cooldown = 0;
                yield return new WaitForSeconds(cooldown);
            }
        }
        Instantiate(prefabQuadFinish, new Vector3(0.5f, 0.5f, prefabsStart + ((lvl * 15) * prefabsDistance)), Quaternion.identity, quadPrefabs);
    }

    public void GenerateFigure(int i, int figure)
    {
        switch (figure)
        {
            case 0:
                InstantiatePrefab(i, i, prefabQuad, 0, 0);
                InstantiatePrefab(i + 1, i, prefabQuad, 0, 1);

                InstantiatePrefab(i + 2, i, prefabEmpty, 1, 0);    //  XO
                InstantiatePrefab(i + 3, i, prefabEmpty, 1, 1);    //  XO
                break;
            case 1:
                InstantiatePrefab(i, i, prefabQuad, 1, 0);
                InstantiatePrefab(i + 1, i, prefabQuad, 1, 1);

                InstantiatePrefab(i + 2, i, prefabEmpty, 0, 0);    //  OX
                InstantiatePrefab(i + 3, i, prefabEmpty, 0, 1);    //  OX
                break;
            case 2:
                InstantiatePrefab(i, i, prefabQuad, 0, 1);
                InstantiatePrefab(i + 1, i, prefabQuad, 1, 1);

                InstantiatePrefab(i + 2, i, prefabEmpty, 0, 0);    //  XX
                InstantiatePrefab(i + 3, i, prefabEmpty, 1, 0);    //  OO
                break;
            case 3:
                InstantiatePrefab(i, i, prefabQuad, 0, 0);
                InstantiatePrefab(i + 1, i, prefabQuad, 1, 0);
                
                InstantiatePrefab(i + 2, i, prefabEmpty, 0, 1);    //  OO
                InstantiatePrefab(i + 3, i, prefabEmpty, 1, 1);    //  XX
                break;
            case 4:
                InstantiatePrefab(i, i, prefabQuad, 0, 1);
                InstantiatePrefab(i + 1, i, prefabQuad, 1, 0);
                
                InstantiatePrefab(i + 2, i, prefabEmpty, 0, 0);    //  XO
                InstantiatePrefab(i + 3, i, prefabEmpty, 1, 1);    //  OX
                break;
            case 5:
                InstantiatePrefab(i, i, prefabQuad, 1, 1);
                InstantiatePrefab(i + 1, i, prefabQuad, 0, 0);
                
                InstantiatePrefab(i + 2, i, prefabEmpty, 1, 0);    //  OX
                InstantiatePrefab(i + 3, i, prefabEmpty, 0, 1);    //  XO
                break;
            case 6:
                InstantiatePrefab(i, i, prefabQuad, 0, 0);
                InstantiatePrefab(i + 1, i, prefabQuad, 1, 0);              //  XO
                InstantiatePrefab(i + 2, i, prefabQuad, 0, 1);              //  XX
               
                InstantiatePrefab(i + 3, i, prefabEmpty, 1, 1);
                break;
            case 7:
                InstantiatePrefab(i, i, prefabQuad, 0, 0);
                InstantiatePrefab(i + 1, i, prefabQuad, 0, 1);              //  XX
                InstantiatePrefab(i + 2, i, prefabQuad, 1, 1);              //  XO
                
                InstantiatePrefab(i + 3, i, prefabEmpty, 1, 0);
                break;
            case 8:
                InstantiatePrefab(i, i, prefabQuad, 0, 1);
                InstantiatePrefab(i + 1, i, prefabQuad, 1, 1);              //  XX
                InstantiatePrefab(i + 2, i, prefabQuad, 1, 0);              //  OX
                
                InstantiatePrefab(i + 3, i, prefabEmpty, 0, 0);
                break;
            case 9:
                InstantiatePrefab(i, i, prefabQuad, 1, 1);
                InstantiatePrefab(i + 1, i, prefabQuad, 1, 0);              //  OX
                InstantiatePrefab(i + 2, i, prefabQuad, 0, 0);              //  XX
                
                InstantiatePrefab(i + 3, i, prefabEmpty, 0, 1);
                break;
        }
    }

    public void InstantiatePrefab(int i, float position, GameObject go, float x, float y)
    {
        GameObject newquad = Instantiate(go, new Vector3(x, y, prefabsStart + (position / 4 * prefabsDistance)), Quaternion.identity, quadPrefabs);
        newquad.GetComponent<Quad>().position = ((int)position);
        quad.Add(newquad);
    }

    public void DeleteQuads(int j)
    {
        for (int i = j; i < j + 4; i++)
        {
            Destroy(quad[i], 2.0f);
        }
    }

    public void DeletePrefabs()
    {
        foreach (GameObject go in quad)
        {
            if (go != null) Destroy(go);
        }

        quad.Clear();
    }
}
