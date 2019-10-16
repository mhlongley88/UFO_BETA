using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossAttackActions : MonoBehaviour
{
    public Transform rightHandPoint;
    public Transform leftHandPoint;
    public Transform spinnigProjectilePoint;
    public MommaBullet fireballPrefab;
    public MommaBullet spinningBulletsPrefab;
    public LineRenderer beamLine;
    public BoxCollider beamCollider;
    public float bulletDamage = -25.0f;

    MommaBullet currentFireball;

    void StartFireballModelling()
    {
        currentFireball = Instantiate(fireballPrefab, rightHandPoint);
        currentFireball.myCollider.enabled = false;
       
        currentFireball.transform.localPosition = Vector3.zero;
    }

    void ThrowFireball()
    {
        Debug.Log("Throwing Fireball!");

        currentFireball.transform.parent = null;

        DOVirtual.DelayedCall(0.15f, () => currentFireball.myCollider.enabled = true);

        currentFireball.FireBullet((transform.position + transform.forward * 5) - transform.position, null, bulletDamage, 1.0f, 120.0f);

        currentFireball = null;
    }

    void ShootProjectile()
    {
        StartCoroutine(shootProjectiles());
    }

    IEnumerator shootProjectiles()
    {
        int randProjectilesCount = Random.Range(4, 8);
        for (int i = 0; i < randProjectilesCount; i++)
        {
            Vector3 point = RandomCircle(transform.position, 9.0f);
            Vector3 originP = transform.position;

            point.y += 6.0f;
            originP.y += 6.0f;

            var projectile = Instantiate(spinningBulletsPrefab, point, Quaternion.identity);
            projectile.myCollider.enabled = false;

            projectile.FireBullet(point - originP, null, bulletDamage, 1.0f, 110.0f);
            DOVirtual.DelayedCall(0.3f, () => projectile.myCollider.enabled = true);
            yield return new WaitForSeconds(0.1f);
        }
    }

    Vector3 RandomCircle(Vector3 center, float radius)
    {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y;
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        return pos;
    }

    Vector3[] positions = new Vector3[2];
    void DischargeBeam()
    {
        beamLine.enabled = true;

        StartCoroutine(handleBeam());
    }

    IEnumerator handleBeam()
    {
        // Start
        Vector3 originPoint = GameUtils.MidPoint(leftHandPoint.position, rightHandPoint.position);
        positions[0] = originPoint + transform.forward;
        positions[1] = positions[0];
        positions[0].y = positions[1].y = 0;

        beamLine.SetPositions(positions);

        //Update
        while (beamLine.enabled)
        {
            originPoint = GameUtils.MidPoint(leftHandPoint.position, rightHandPoint.position);

            positions[0] = originPoint + transform.forward;
            positions[1] = Vector3.Lerp(positions[1], positions[0] + transform.forward * 100, Time.deltaTime);

            positions[0].y = positions[1].y = 0;

            beamLine.SetPositions(positions);

            UpdateColliderForBeam(positions[0], positions[1], beamLine);

            yield return null;
        }
    }

    void UpdateColliderForBeam(Vector3 startPos, Vector3 endPos, LineRenderer line)
    {
        BoxCollider col = beamCollider;

        float lineLength = Vector3.Distance(startPos, endPos); // length of line
        col.size = new Vector3(1f, 0.1f, lineLength); // size of collider is set where X is length of line, Y is width of line, Z will be set as per requirement
        Vector3 midPoint = (startPos + endPos) / 2;
        col.transform.position = midPoint; // setting position of collider object

        // Following lines calculate the angle between startPos and endPos
        //float angle = (Mathf.Abs(startPos.y - endPos.y) / Mathf.Abs(startPos.x - endPos.x));
        //if ((startPos.y < endPos.y && startPos.x > endPos.x) || (endPos.y < startPos.y && endPos.x > startPos.x))
        //{
        //    angle *= -1;
        //}
        //angle = Mathf.Rad2Deg * Mathf.Atan(angle);
        //col.transform.Rotate(0, 0, angle);
        
        col.transform.LookAt(endPos);

        col.enabled = true;
    }

    void StopBeam()
    {
        beamLine.enabled = false;
        beamCollider.enabled = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5);

        //for (int i = 0; i < 4; i++)
        //{
        //    Vector3 point = RandomCircle(transform.position, 9.0f);
        //    point.y += 6.0f;
        //    Gizmos.DrawLine(transform.position + Vector3.up * 6, point);
        //}
    }
}
