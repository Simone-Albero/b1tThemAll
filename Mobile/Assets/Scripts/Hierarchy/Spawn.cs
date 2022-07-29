using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Spawn : MonoBehaviour
{
    //spawn parameter
    [SerializeField]
    private float spawnRadius = 30;
    private int idProgression = 0;

    //mappa int-int (id, num) --> tiene traccia delle unità generate per categoria 
    private Dictionary<int, int> enemyTracer;
    //mappa int-int (id, num) --> tiene traccia delle unità generate da altre unità
    private Dictionary<int, int> generatedEnemyTracer;

    //player screen radius
    private float outsideScreenRadius = 2;

    //Island dimension islandXMin = -48.3f; islandXMax =  74.6f; islandYMin = -25.5f; islandYMax = 36.3f;
    private Bounds island;

    //map dimension: xMin = -68.4f; xMax = 94.7f; yMin = -37.6f; yMax = 49.1f;
    private Bounds map;

    public GameObject footpath;
    public GameObject border;
    


    private void Start()
    {
        enemyTracer = new Dictionary<int, int>();
        generatedEnemyTracer = new Dictionary<int, int>();
        map = new Bounds();
        map.max = new Vector3(94.7f, 49.1f);
        map.min = new Vector3(-68.4f, -37.6f);

        island = new Bounds();
        island.max = new Vector3(74.6f, 36.3f);
        island.min = new Vector3(-48.3f, -25.5f);
    }

    public IEnumerator SpawnNearEnemy(Animator animator, GameObject Spawner, GameObject spawnSubject, int maxSpawn, float spawnRate, int id)
    {
        float waitingSecond = 1 / spawnRate;
        int spawnCounter = 0;

        if (generatedEnemyTracer.ContainsKey(id))
            spawnCounter = generatedEnemyTracer[id];
        else
            generatedEnemyTracer.Add(id, 0);

        if (spawnCounter < maxSpawn)
        {
            
            
            Vector3 spawnPos = Spawner.GetComponent<Transform>().position;
            float rot = Spawner.GetComponent<Rigidbody2D>().rotation;

            float dis = 1;
            spawnPos.x = spawnPos.x + dis * Mathf.Cos(rot * Mathf.Deg2Rad);
            spawnPos.y = spawnPos.y + dis * Mathf.Sin(rot * Mathf.Deg2Rad);

            if (!footpath.GetComponent<Tilemap>().HasTile(new Vector3Int((int)spawnPos.x, (int)spawnPos.y)) && !border.GetComponent<Tilemap>().HasTile(new Vector3Int((int)spawnPos.x, (int)spawnPos.y)))
            {
                animator.SetTrigger("Open");
                generatedEnemyTracer[id] += 1;
                Instantiate(spawnSubject, spawnPos, new Quaternion(0, 0, rot, 0)).GetComponent<Enemy>().setGenerationId(id);
            }
            else waitingSecond = 0;
           
           
        }
        yield return new WaitForSeconds(waitingSecond);
        yield return SpawnNearEnemy(animator, Spawner, spawnSubject, maxSpawn, spawnRate, id);
    }

    public IEnumerator SpawnNearPlayer(GameObject spawnSubject, int maxSpawn, float spawnRate, GameObject playerBody, int id)
    {
        float waitingSecond = 1 / spawnRate;
        int spawnCounter = 0;

        if (enemyTracer.ContainsKey(id))
            spawnCounter = enemyTracer[id];
        else
            enemyTracer.Add(id, 0);

        if (spawnCounter < maxSpawn)
        {
            //posizione robot
            Vector2 spawnPos = playerBody.transform.position;

            //direzione spawn rispetto al robot
            Vector2 direction = Random.insideUnitCircle.normalized;

            //spostando lo spawn fuori dallo schermo
            float angle = Mathf.Atan2(direction.y, direction.x);
            direction.x = outsideScreenRadius * Mathf.Cos(angle);
            direction.y = outsideScreenRadius * Mathf.Sin(angle);

            spawnPos += direction * spawnRadius;

            if (positionCheck(spawnPos, playerBody, spawnSubject))
            {
                enemyTracer[id] += 1;
                //spawn
                Instantiate(spawnSubject, spawnPos, Quaternion.identity).GetComponent<Enemy>().setProgId(idProgression += 1);
            }
            else waitingSecond = 0;


        }
        

        yield return new WaitForSeconds(waitingSecond);
        yield return SpawnNearPlayer(spawnSubject, maxSpawn, spawnRate, playerBody, id);
    }

    public IEnumerator RandomDrop(GameObject spawnSubject, int maxSpawn, float spawnRate, GameObject playerBody, int id)
    {
        float waitingSecond = 1 / spawnRate;
        int spawnCounter = 0;

        if (enemyTracer.ContainsKey(id))
            spawnCounter = enemyTracer[id];
        else
            enemyTracer.Add(id, 0);

        if (spawnCounter < maxSpawn)
        {
            Vector2 spawnPos = getPositionInRange(map.min.x, map.max.x, map.min.y, map.max.y);

            if (outsidePlayerView(spawnPos, playerBody))
            {
                Instantiate(spawnSubject, spawnPos, Quaternion.identity).GetComponent<Enemy>().setProgId(idProgression += 1);
                enemyTracer[id] += 1;
            }
            else waitingSecond = 0;
            
        }

        yield return new WaitForSeconds(waitingSecond);
        yield return RandomDrop(spawnSubject, maxSpawn, spawnRate, playerBody, id);
    }


    public IEnumerator dropInsideMap(GameObject spawnSubject, int maxSpawn, float spawnRate, GameObject playerBody, int id)
    {
        float waitingSecond = 1 / spawnRate;
        int spawnCounter = 0;

        if (enemyTracer.ContainsKey(id))
            spawnCounter = enemyTracer[id];
        else
            enemyTracer.Add(id, 0);

        if (spawnCounter < maxSpawn)
        {
            Vector2 spawnPos = getPositionInRange(island.min.x, island.max.x, island.min.y, island.max.y);

            if (positionCheck(spawnPos, playerBody, spawnSubject))
            {
                enemyTracer[id] += 1;

                GameObject obj = Instantiate(spawnSubject, spawnPos, Quaternion.identity);
                if (obj.GetComponent<Enemy>() != null) obj.GetComponent<Enemy>().setProgId(idProgression += 1);
            }
            else waitingSecond = 0;
                        
        }
        
        yield return new WaitForSeconds(waitingSecond);
        yield return dropInsideMap(spawnSubject, maxSpawn, spawnRate, playerBody, id);
    }

    public void UpdetEnemyTracer(int key) { if (enemyTracer.ContainsKey(key)) enemyTracer[key] -= 1; }

    public void UpdetGeneratedEnemyTracer(int key) { if (generatedEnemyTracer.ContainsKey(key)) generatedEnemyTracer[key] -= 1; }

    public void RemoveGenerationUnit(int key) { if (generatedEnemyTracer.ContainsKey(key)) generatedEnemyTracer.Remove(key); }


    public Vector2 getPositionInRange(float xMin, float xMax, float yMin, float yMax)
    {
        float xPos = Random.Range(xMin, xMax);
        float yPos = Random.Range(yMin, yMax);
        return new Vector2(xPos, yPos);
    }

    private bool positionCheck(Vector2 spawnPos, GameObject playerBody, GameObject spawnSubject)
    {
        
        // posApprox = new Bounds(new Vector3((spawnPos.x-0.5f), (spawnPos.y-0.5f)), new Vector3(1f,1f));

        Bounds posApprox = spawnSubject.GetComponent<Collider2D>().bounds;
        posApprox.center = spawnPos;

        bool isFarFromFootpath = footpath.GetComponent<Tilemap>().GetTilesRangeCount(new Vector3Int((int)posApprox.min.x, (int)posApprox.min.y), new Vector3Int((int)posApprox.max.x, (int)posApprox.max.y)) == 0;
        bool isInsideBorder = border.GetComponent<Tilemap>().GetTilesRangeCount(new Vector3Int((int)posApprox.min.x, (int)posApprox.min.y), new Vector3Int((int)posApprox.max.x, (int)posApprox.max.y)) == 0;
        

        return isFarFromFootpath && isInsideBorder && outsidePlayerView(spawnPos, playerBody);
    }

    private bool outsidePlayerView(Vector2 spawnPos, GameObject playerBody)
    {
        Rect playerView = new Rect(playerBody.GetComponent<Transform>().position.x - 11, playerBody.GetComponent<Transform>().position.y - 5, 22, 10);
        return !playerView.Contains(spawnPos);
    }

    }
