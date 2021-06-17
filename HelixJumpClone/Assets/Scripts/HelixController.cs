using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelixController : MonoBehaviour
{
    public Transform topTransform;
    public Transform goalTransform;

    private Vector2 lastTapPos;
    private Vector3 startRotation;
    public GameObject helixLevelPrefab;

    public List<Stage> allStages = new List<Stage>();
    private float helixDistance;
    private List<GameObject> spawnedLevels = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        helixDistance = topTransform.localPosition.y - (goalTransform.localPosition.y + 0.1f);
        LoadStage(0);

        startRotation = transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 curTapPos = Input.mousePosition;

            if (lastTapPos == Vector2.zero)
            {
                lastTapPos = curTapPos;
            }

            float delta = lastTapPos.x - curTapPos.x;
            lastTapPos = curTapPos;

            transform.Rotate(Vector3.up * delta);
        }

        if (Input.GetMouseButtonUp(0))
        {
            lastTapPos = Vector2.zero;
        }
    }

    public void LoadStage(int stageNumber)
    {
        Stage stage = allStages[Mathf.Clamp(stageNumber, 0, allStages.Count - 1)];  //son stage e geldiginde basa donmeyi saglar

        if (stage == null)
        {
            Debug.LogError("No stage " + "found in allStages list. Are all stages assigned in the list?");
            return;
        }

        Camera.main.backgroundColor = allStages[stageNumber].stageBackgroundColor;
        FindObjectOfType<BallController>().GetComponent<Renderer>().material.color = allStages[stageNumber].stageBallColor;

        transform.localEulerAngles = startRotation;

        //destroy the old levels if there are any
        foreach (GameObject go in spawnedLevels)
        {
            Destroy(go);
        }

        //create new level/platforms
        float levelDistance = helixDistance / stage.levels.Count;
        float spawnPosY = topTransform.localPosition.y;

        for (int i = 0; i < stage.levels.Count; i++)
        {
            spawnPosY -= levelDistance;
            GameObject level = Instantiate(helixLevelPrefab, transform);
            Debug.Log("Levels spawned");
            level.transform.localPosition = new Vector3(0, spawnPosY, 0);
            spawnedLevels.Add(level);

            //creating the gaps

            int partsToDisable = 12 - stage.levels[i].partCount;
            List<GameObject> disabledParts = new List<GameObject>();

            while (disabledParts.Count < partsToDisable)
            {
                GameObject randomPart = level.transform.GetChild(Random.Range(0, level.transform.childCount)).gameObject;
                if (!disabledParts.Contains(randomPart))
                {
                    randomPart.SetActive(false);
                    disabledParts.Add(randomPart);
                }
            }


            List<GameObject> leftParts = new List<GameObject>();

            foreach (Transform t in level.transform)
            {
                t.GetComponent<Renderer>().material.color = allStages[stageNumber].stageLevelPartColor;
                if (t.gameObject.activeInHierarchy)
                {
                    leftParts.Add(t.gameObject);
                }
            }

            //creating game over parts
            List<GameObject> gameOverParts = new List<GameObject>();

            while (gameOverParts.Count < stage.levels[i].gameOverPartCount)
            {
                GameObject randomPart = leftParts[(Random.Range(0, leftParts.Count))];
                if (!gameOverParts.Contains(randomPart))
                {
                    randomPart.gameObject.AddComponent<GameOverPart>();
                    gameOverParts.Add(randomPart);
                }
            }

        }


    }
}
