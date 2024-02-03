using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    private BoxCollider2D collider;
    private Rigidbody2D rb;
    private float width;
    
    [SerializeField] private List<Transform> levelPartList;
    [SerializeField] private Transform previousLevelPart;
    private bool IsVelocityReset = false;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        width = collider.size.x;
        collider.enabled = false;

        rb.velocity = new Vector2(GlobalInfo.GameSpeed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalInfo.IsGameOver)
        {
            if (IsVelocityReset)
            {
                return;
            }
            rb.velocity = new Vector2(0, 0);
            IsVelocityReset = true;
            return;
        }

        if (transform.position.x < -width)
        {
            Vector3 resetPosition = new Vector3(width * 2f - 1, 0, 0);
            transform.position = transform.position + resetPosition;
            SpawnRandomLevel();
        }
        rb.velocity = new Vector2(GlobalInfo.GameSpeed, 0);
    }

    public void SpawnRandomLevel()
    {
        if (previousLevelPart != null)
        {
            Destroy(previousLevelPart.gameObject);
        }

        Transform chosenLevelPart = levelPartList[Random.Range(0, levelPartList.Count)];
        previousLevelPart = Instantiate(chosenLevelPart, transform.position + Vector3.right + new Vector3(0f, 0f, -1f), Quaternion.identity, transform);

        GlobalInfo.NumIterations++;
        if (GlobalInfo.NumIterations == 3)
        {
            GlobalInfo.NumIterations = 0;
            GlobalInfo.MakeGameFaster();
        }
    }
}
