using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectMovement : MonoBehaviour
{
    public int playersAmount = 4;

    public float radius = 0.6f;
    public float translateSpeed = 180.0f;
    public float rotateSpeed = 360.0f;

    float angle = 0.0f;
    Vector3 direction = Vector3.one;
    Quaternion rotation = Quaternion.identity;

    private void Start()
    {
        transform.position = this.gameObject.transform.position;
    }

    void Update()
    {
        direction = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle));

        for (int i = 0; i < playersAmount; i++)
        {
            float h = Input.GetAxisRaw("P" + (i + 1) + "_Horizontal");
            float v = Input.GetAxisRaw("P" + (i + 1) + "_Vertical");

            bool right = h > .2f;
            bool left = h < -.2f;

            bool up = v > .6f;
            bool down = v < -.6f;

            if (left) Translate(translateSpeed, 0);
            if (right) Translate(-translateSpeed, 0);

            if (up) Translate(0, translateSpeed);
            if (down) Translate(0, -translateSpeed);
        }

        // Rotate with left/right arrows
        if (Input.GetKey(KeyCode.LeftArrow)) Translate(translateSpeed, 0);
        if (Input.GetKey(KeyCode.RightArrow)) Translate(-translateSpeed, 0);

        // Translate forward/backward with up/down arrows
        if (Input.GetKey(KeyCode.UpArrow)) Translate(0, translateSpeed);
        if (Input.GetKey(KeyCode.DownArrow)) Translate(0, -translateSpeed);

        // Translate left/right with A/D. Bad keys but quick test.
        if (Input.GetKey(KeyCode.A)) Translate(translateSpeed, 0);
        if (Input.GetKey(KeyCode.D)) Translate(-translateSpeed, 0);

        UpdatePositionRotation();
    }

    void Rotate(float amount)
    {
        angle += amount * Mathf.Deg2Rad * Time.deltaTime;
    }

    void Translate(float x, float y)
    {
        var perpendicular = new Vector3(-direction.y, direction.x);
        var verticalRotation = Quaternion.AngleAxis(y * Time.deltaTime, perpendicular);
        var horizontalRotation = Quaternion.AngleAxis(x * Time.deltaTime, direction);
        rotation *= horizontalRotation * verticalRotation;
    }

    void UpdatePositionRotation()
    {
        transform.localPosition = rotation * Vector3.forward * radius;
        transform.rotation = rotation * Quaternion.LookRotation(direction, Vector3.forward);
    }
}