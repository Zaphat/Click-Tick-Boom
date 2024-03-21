using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Director : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameObject jewelPrefab;
    [SerializeField] private AudioClip[] coinSounds;
    [SerializeField] private AudioClip[] bombSounds;
    [SerializeField] private AudioClip[] levelUp;

    private float sizeX, sizeY;
    private int score = 0;
    private bool isGameOver = false;
    private bool isPenaltyTriggered = false;

    private Collider2D safeZoneCollider;
    IEnumerator waitAndSpawnNewCoin(GameObject gameObject, float time = 3.0f)
    {
        yield return new WaitForSeconds(time);
        if (gameObject != null && !isGameOver)
        {
            Destroy(gameObject);
            if (isPenaltyTriggered)
            {
                score -= 2;
                if (score < 0)
                {
                    score = 0;
                }
                scoreText.text = score.ToString();
            }

            SpawnCoin(1);
        }
    }
    IEnumerator scoreReachedZero()
    {
        yield return new WaitUntil(() => score <= 0 && isPenaltyTriggered && !isGameOver);
        isGameOver = true;
        StartCoroutine(GameOver(0.0f));
    }
    IEnumerator waitAndSpawnNewBomb(GameObject gameObject, float time = 2.0f)
    {
        yield return new WaitForSeconds(time);
        if (gameObject != null && !isGameOver)
        {
            Destroy(gameObject);
            int seed = Random.Range(0, 100);
            if (seed < 94)
            {
                StartCoroutine(waitAndSpawnNewBomb(Instantiate(bombPrefab, getRandomPosition(), Quaternion.Euler(0, 180, 0))));
            }
            else
            {
                StartCoroutine(waitAndSpawnNewBomb(Instantiate(bombPrefab, getRandomPosition(), Quaternion.Euler(0, 180, 0))));
                StartCoroutine(waitAndSpawnNewBomb(Instantiate(bombPrefab, getRandomPosition(), Quaternion.Euler(0, 180, 0))));
            }
        }
    }
    IEnumerator GameOver(float time = 4.0f)
    {
        yield return new WaitForSeconds(time);
        SFX.instance.PlayOnGameOver();
        Time.timeScale = 0;
        DestroyAllObjects();
        Settings.instance.GameOverNotify();
    }
    private void SpawnCoin(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector2 spawnPosition = getRandomPosition();
            StartCoroutine(waitAndSpawnNewCoin(Instantiate(coinPrefab, spawnPosition, Quaternion.identity)));
        }
    }
    private void JewelClicked(GameObject jewel)
    {
        if (isGameOver)
            return;
        score += 10;
        scoreText.text = score.ToString();
        SFX.instance.PlayOnPickup(levelUp, jewel.transform, 5.0f);
        Destroy(jewel);

    }
    private void CoinClicked(GameObject coin)
    {
        if (isGameOver)
            return;

        score++;

        if (score >= 100 && !isPenaltyTriggered)
        {
            isPenaltyTriggered = true;
            StartCoroutine(waitAndSpawnNewBomb(Instantiate(bombPrefab, getRandomPosition(), Quaternion.Euler(0, 180, 0))));
        }

        scoreText.text = score.ToString();
        SFX.instance.PlayOnPickup(coinSounds, coin.transform);
        Destroy(coin);
        int seed = Random.Range(0, 100);
        if (seed < 94)
        {
            SpawnCoin(1);
        }
        else if (seed < 99)
        {
            SpawnCoin(2);
        }
        else
        {
            SpawnCoin(2);
            StartCoroutine(waitAndSpawnNewCoin(Instantiate(jewelPrefab, getRandomPosition(), Quaternion.identity), 2.0f));
        }
    }
    private void BombClicked(GameObject bomb)
    {
        if (isGameOver)
            return;
        isGameOver = true;
        var animator = bomb.GetComponent<Animator>();
        animator.SetTrigger("explode");
        SFX.instance.PlayOnPickup(bombSounds, bomb.transform, 2.5f);
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        Destroy(bomb, animationLength);
        StartCoroutine(GameOver(animationLength));

    }
    private Vector2 getRandomPosition()
    {

        float spawnY = Random.Range
            (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y + sizeY / 2, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y - sizeY / 2);
        float spawnX = Random.Range
            (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x + sizeX / 2, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x - sizeX / 2);

        Vector2 spawnPosition = new Vector2(spawnX, spawnY);
        if (safeZoneCollider.bounds.Contains(spawnPosition))
        {
            return getRandomPosition();
        }
        return spawnPosition;
    }
    void DestroyAllObjects()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Coin");
        foreach (GameObject target in gameObjects)
        {
            GameObject.Destroy(target);
        }
        gameObjects = GameObject.FindGameObjectsWithTag("Bomb");
        foreach (GameObject target in gameObjects)
        {
            GameObject.Destroy(target);
        }
        gameObjects = GameObject.FindGameObjectsWithTag("Jewel");
        foreach (GameObject target in gameObjects)
        {
            GameObject.Destroy(target);
        }


    }
    void Start()
    {
        sizeX = coinPrefab.GetComponent<SpriteRenderer>().bounds.size.x + 1.25f;
        sizeY = coinPrefab.GetComponent<SpriteRenderer>().bounds.size.y + 1.25f;
        safeZoneCollider = this.gameObject.GetComponent<Collider2D>();


        StartCoroutine(waitAndSpawnNewBomb(Instantiate(bombPrefab, getRandomPosition(), Quaternion.Euler(0, 180, 0))));
        StartCoroutine(waitAndSpawnNewCoin(Instantiate(coinPrefab, new Vector3(0, 0, 0), Quaternion.identity)));

        StartCoroutine(scoreReachedZero());
    }
    void FixedUpdate()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hits = Physics2D.GetRayIntersectionAll(ray, 500f);

            foreach (var hit in hits)
            {
                if (hit.collider != null)
                {

                    if (hit.collider.gameObject.tag == "Coin")
                    {
                        CoinClicked(hit.collider.gameObject);
                    }
                    else if (hit.collider.gameObject.tag == "Bomb")
                    {
                        BombClicked(hit.collider.gameObject);
                    }
                    else if (hit.collider.gameObject.tag == "Jewel")
                    {
                        JewelClicked(hit.collider.gameObject);
                    }
                }
            }
        }
# else
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                var ray = Camera.main.ScreenPointToRay(touch.position);
                var hits = Physics2D.GetRayIntersectionAll(ray, 500f);

                foreach (var hit in hits)
                {
                    if (hit.collider != null)
                    {
                        if (hit.collider.gameObject.tag == "Coin")
                        {
                            CoinClicked(hit.collider.gameObject);
                        }
                        else if (hit.collider.gameObject.tag == "Bomb")
                        {
                            BombClicked(hit.collider.gameObject);
                        }
                        else if (hit.collider.gameObject.tag == "Jewel")
                        {
                            JewelClicked(hit.collider.gameObject);
                        }
                    }
                }
            }
        }

#endif
    }

}
