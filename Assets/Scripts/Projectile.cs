using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ProjectileVisuals projectileVisual;

    private Transform target;
    private float moveSpeed;
    private float maxMoveSpeed;
    private float trajectoryRelMaxHeight;
    private float distanceTargetDestroy = 1f;

    private AnimationCurve trajectoryAnimationCurve;
    private AnimationCurve axisCorrection;
    private AnimationCurve speedCurve;

    private Vector3 trajectoryStartPoint;
    private Vector3 ProjectileMoveDir;
    private Vector3 trajectoryRange;

    private float nextYTrajectoryPosition;
    private float nextXTrajectoryPosition;
    private float nextPositionCorrectionAbsolute;
    private float nextPositionXCorrectionAbsolute;

    private void Start()
    {
        trajectoryStartPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        UpdateProjectilePosition();

        if(Vector3.Distance(transform.position, target.position) < distanceTargetDestroy )
        {
            Destroy(gameObject);
        }
    }

    private void UpdateProjectilePosition()
    {
        trajectoryRange = target.position - trajectoryStartPoint;

        if (Mathf.Abs(trajectoryRange.normalized.x) < Mathf.Abs(trajectoryRange.normalized.y) )
        {
            if (trajectoryRange.y < 0f)
            {
                moveSpeed = -moveSpeed;
            }

            UpdatePositionWithXCurve();
        } 
        else
        {
            if (trajectoryRange.x < 0f)
            {
                moveSpeed = -moveSpeed;
            } 

            UpdatePositionWithYCurve();
        }
    }

    private void UpdatePositionWithXCurve()
    {
        float nextPositionY = transform.position.y + moveSpeed * Time.deltaTime;
        float nextPositionYNormal = (nextPositionY - trajectoryStartPoint.y) / trajectoryRange.y;

        float nextPositionXNormal = trajectoryAnimationCurve.Evaluate(nextPositionYNormal);
        nextXTrajectoryPosition = nextPositionXNormal * trajectoryRelMaxHeight;

        float nextPositionXCorrectionNormal = axisCorrection.Evaluate(nextPositionYNormal);
        float nextPositionXCorrectionAbs = nextPositionXCorrectionNormal * trajectoryRange.x;

        if (trajectoryRange.x > 0 && trajectoryRange.y > 0)
        {
            nextXTrajectoryPosition = -nextXTrajectoryPosition;
        }

        if (trajectoryRange.x < 0 && trajectoryRange.y < 0)
        {
            nextXTrajectoryPosition = -nextXTrajectoryPosition;
        }

        float nextPositionX = trajectoryStartPoint.x + nextXTrajectoryPosition + nextPositionXCorrectionAbs;

        Vector3 newPosition = new Vector3(nextPositionX, nextPositionY, 0);

        CalculateNextProjectileSpeed(nextPositionYNormal);
        ProjectileMoveDir = newPosition - transform.position;


        transform.position = newPosition;
    }
    private void UpdatePositionWithYCurve()
    {
        float nextPositionX = transform.position.x + moveSpeed * Time.deltaTime;
        float nextPositionXNormal = (nextPositionX - trajectoryStartPoint.x) / trajectoryRange.x;

        float nextPositionNormal = trajectoryAnimationCurve.Evaluate(nextPositionXNormal);
        nextYTrajectoryPosition = nextPositionNormal * trajectoryRelMaxHeight;

        float nextPositionYCorrectionNormal = axisCorrection.Evaluate(nextPositionXNormal);
        float nextPositionYCorrectionAbs = nextPositionYCorrectionNormal * trajectoryRange.y;

        float nextPositionY = trajectoryStartPoint.y + nextYTrajectoryPosition + nextPositionYCorrectionAbs;

        Vector3 newPosition = new Vector3(nextPositionX, nextPositionY, 0);

        CalculateNextProjectileSpeed(nextPositionXNormal);
        ProjectileMoveDir = newPosition - transform.position;


        transform.position = newPosition;
    }

    private void CalculateNextProjectileSpeed(float nextPositionXNormal)
    {
        float nextMoveSpeedNormal = speedCurve.Evaluate(nextPositionXNormal);

        moveSpeed = nextMoveSpeedNormal * maxMoveSpeed;
    }

    public void InitalizeProjectile(Transform target, float maxMoveSpeed, float trajectorymaxHeight)
    {
        this.target = target;  
        this.maxMoveSpeed = maxMoveSpeed;

        float xDistanceToTareget = target.position.x - transform.position.x;
        this.trajectoryRelMaxHeight = Mathf.Abs(xDistanceToTareget) * trajectorymaxHeight;

        projectileVisual.SetTarget(target);
    }

    public void InitializeAnimationCurves(AnimationCurve trajectoryAnimationCurve, AnimationCurve axisCorrection, AnimationCurve speedCurve)
    {
        this.trajectoryAnimationCurve = trajectoryAnimationCurve;
        this.axisCorrection = axisCorrection;
        this.speedCurve = speedCurve;
    }

    public Vector3 GetProjectileMoveDir()
    {
        return ProjectileMoveDir;
    }

    public float GetNextYTrajectoryPosition()
    {
        return nextYTrajectoryPosition;
    }

    public float GetNextPositionYCorrectionAbs()
    {
        return nextPositionCorrectionAbsolute;
    }

    public float GetNextXTrajectoryPosition()
    {
        return nextXTrajectoryPosition;
    }

    public float GetNextPositionXCorrectionAbs()
    {
        return nextPositionXCorrectionAbsolute;
    }

}
