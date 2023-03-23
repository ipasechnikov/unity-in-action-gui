using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingAI : MonoBehaviour
{
    [SerializeField] GameObject fireballPrefab;
    private GameObject fireball;

    public const float baseSpeed = 3.0f;

    public float speed = 3.0f;
    public float obstracleRange = 5.0f;

    public bool IsAlive
    {
        get; set;
    }

    // Start is called before the first frame update
    void Start()
    {
        IsAlive = true;
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
        // Don't do anything if enemy is dead
        if (!IsAlive)
            return;

        var deltaZ = speed * Time.deltaTime;
        transform.Translate(0, 0, deltaZ);

        // A ray at the same position and pointing in the same direction at the character
        var ray = new Ray(transform.position, transform.forward);

        if (Physics.SphereCast(ray, 0.75f, out var hit))
        {
            var hitObject = hit.transform.gameObject;
            if (hitObject.GetComponent<PlayerCharacter>())
            {
                if (fireball == null)
                {
                    fireball = Instantiate(fireballPrefab);

                    // Place fireball in from of the enemy and point in the same direction
                    fireball.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
                    fireball.transform.rotation = transform.rotation;
                }
            }
            else if (hit.distance < obstracleRange)
            {
                var angle = Random.Range(-110, 110);
                transform.Rotate(0, angle, 0);
            }
        }
    }

    public void OnSpeedChanged(float value)
    {
        speed = baseSpeed * value;
    }
}
