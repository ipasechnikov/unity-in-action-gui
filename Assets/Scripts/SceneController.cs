using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    // Serialized variable for linking to the prefab object
    [SerializeField] GameObject enemyPrefab;

    // Variable to keep track of the enemy instance in the scene
    private GameObject enemy;
    private float enemySpeedMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        enemySpeedMultiplier = PlayerPrefs.GetFloat("speed", 1.0f);
    }

    void OnEnable()
    {
        Messenger<float>.AddListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    }

    void OnDisable()
    {
        Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy == null)
        {
            enemy = Instantiate(enemyPrefab);
            enemy.transform.position = new Vector3(0, 1, 0);
            var angle = Random.Range(0, 360);
            enemy.transform.Rotate(0, angle, 0);
            enemy.GetComponent<WanderingAI>().OnSpeedChanged(enemySpeedMultiplier);
        }
    }

    private void OnSpeedChanged(float value)
    {
        enemySpeedMultiplier = value;
    }
}
