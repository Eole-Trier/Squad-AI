using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class FormationHandler : MonoBehaviour
{
    enum FormationType
    {
        Circle,
        Line,
    }

    [SerializeField] private bool faceDirection = true;

    [Header("circle formation")]
    //[SerializeField] private float rotationSpeed = 0.5f;
    [SerializeField] private float baseRadius = 1.5f;
    [SerializeField] private float radiusStep = 0.5f;

    [Header("Line formation")]
    [SerializeField] private float spaceBetweenRows = 0.5f;
    [SerializeField] private float spaceBetweenColumns = 0.5f;
    [SerializeField, Min(1)] private int columns = 1;
    [SerializeField] private FormationType _formationType = FormationType.Circle;

    private bool _isDirty = false;

    public void SetTransform(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
        _isDirty = true;
    }
    public void AddUnit(Unit unit)
    {
        units.Add(unit);
        _isDirty = true;
    }
    public void RemoveUnit(Unit unit)
    {
        units.Remove(unit);
        _isDirty = true;
    }

    private List<Unit> units = new();
    private List<Vector3> unitsTargetPos = new();
    private List<Quaternion> unitsTargetRot = new();

    private void ComputeTargetsPos()
    {
        unitsTargetPos.Clear();

        switch (_formationType)
        {
            case FormationType.Circle:
                ComputeTargetsPosCircle();
                break;

            case FormationType.Line:
                ComputeTargetsPosLine();
                break;
        }
    }

    #region Compute Position Methods

    private void ComputeTargetsPosCircle()
    {
        int unitCount = units.Count;
        float radius = baseRadius + radiusStep * unitCount;
        float angleStep = 360f / unitCount;

        for (int i = 0; i < unitCount; i++)
        {
            Vector3 direction = Quaternion.AngleAxis(angleStep * i, Vector3.up) * transform.forward;
            direction.Normalize();

            unitsTargetPos.Add(transform.position + direction * radius);
        }
    }

    private void ComputeTargetsPosLine()
    {
        int unitCount = units.Count;
        int lastRowUnitsQuantity = unitCount % columns;
        if (lastRowUnitsQuantity == 0) lastRowUnitsQuantity = columns;
        int rows = Mathf.CeilToInt(unitCount / (float)columns);
        Vector3 newTargetPos;
        float forwardPosOffset;

        if (columns % 2 == 1)
        {
            for (int i = 0; i < rows - 1; i++)
            {
                forwardPosOffset = (i - rows / 2f) * spaceBetweenRows;

                for (int j = 0; j < columns; j++)
                {
                    float rightPosOffset = (j - (columns - 1) / 2f) * spaceBetweenColumns;
                    newTargetPos = transform.position;
                    newTargetPos += transform.right * rightPosOffset;
                    newTargetPos += transform.forward * forwardPosOffset;
                    unitsTargetPos.Add(newTargetPos);
                }
            }

            //last row here
            forwardPosOffset = (rows - rows / 2f) * spaceBetweenRows;

            for (int j = 0; j < lastRowUnitsQuantity; j++)
            {
                float rightPosOffset = (j - (lastRowUnitsQuantity - 1) / 2f) * spaceBetweenColumns;
                newTargetPos = transform.position;
                newTargetPos += transform.right * rightPosOffset;
                newTargetPos += transform.forward * forwardPosOffset;
                unitsTargetPos.Add(newTargetPos);
            }

        }
        else
        {
            float middleGap = spaceBetweenColumns / 2f;

            for (int i = 0; i < rows - 1; i++)
            {
                forwardPosOffset = (i - rows / 2f) * spaceBetweenRows;

                for (int j = 0; j < columns; j++)
                {
                    float rightPosOffset = j - columns / 2f;

                    if (rightPosOffset >= 0)
                        rightPosOffset = rightPosOffset * spaceBetweenColumns + middleGap;
                    else
                        rightPosOffset = (rightPosOffset + 1) * spaceBetweenColumns - middleGap;

                    newTargetPos = transform.position;
                    newTargetPos += transform.right * rightPosOffset;
                    newTargetPos += transform.forward * forwardPosOffset;
                    unitsTargetPos.Add(newTargetPos);
                }
            }

            //last row here
            forwardPosOffset = (rows - rows / 2f) * spaceBetweenRows;

            for (int j = 0; j < lastRowUnitsQuantity; j++)
            {
                float rightPosOffset = j - lastRowUnitsQuantity / 2f;

                if (rightPosOffset >= 0)
                    rightPosOffset = rightPosOffset * spaceBetweenColumns + middleGap;
                else
                    rightPosOffset = (rightPosOffset + 1) * spaceBetweenColumns - middleGap;

                newTargetPos = transform.position;
                newTargetPos += transform.right * rightPosOffset;
                newTargetPos += transform.forward * forwardPosOffset;
                unitsTargetPos.Add(newTargetPos);
            }
        }

    }

    #endregion

    private void ComputeTargetsRot()
    {
        if (faceDirection) return;

        unitsTargetRot.Clear();
        for (int i = 0; i < unitsTargetPos.Count; i++)
        {
            unitsTargetRot.Add(Quaternion.LookRotation(unitsTargetPos[i] - transform.position));
        }
    }

    private void Update()
    {
        if (_isDirty)
        {
            ComputeTargetsPos();
            ComputeTargetsRot();
            _isDirty = false;
        }

        /*for (int i = 0; i < units.Count; i++)
        {
            units[i].SetTargetPos(unitsTargetPos[i]);

            if (!faceDirection)
                units[i].SetTargetRot(unitsTargetRot[i]);

            else units[i].SetTargetRot(transform.rotation);
        }*/
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        foreach (var targetPos in unitsTargetPos)
        {
            Gizmos.DrawWireSphere(targetPos, 0.5f);
        }
    }
}

