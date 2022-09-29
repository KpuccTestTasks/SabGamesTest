using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Field : MonoBehaviour
{
    public static Field Instance { get; private set; }
    
    [SerializeField] private float width;
    [SerializeField] private float length;

    [SerializeField] private Unit unitPrefab;
    [SerializeField] private int unitsOnFieldAmount;

    private List<Unit> _units;

    private void OnEnable()
    {
        Unit.OnUnitDied += RemoveUnit;
    }

    private void OnDisable()
    {
        Unit.OnUnitDied -= RemoveUnit;
    }

    private void Awake()
    {
        Instance = this;
        
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        transform.localScale = new Vector3(width, length);

        GenerateUnits();
    }

    private void GenerateUnits()
    {
        _units = new List<Unit>(unitsOnFieldAmount);

        float fieldHalfWidth = width / 2;
        float fieldHalfLength = length / 2;
        
        for (int i = 0; i < unitsOnFieldAmount; i++)
        {
            float unitPosX = Random.Range(-fieldHalfWidth, fieldHalfWidth);
            float unitPosZ = Random.Range(-fieldHalfLength, fieldHalfLength);
            
            Unit newUnit = Instantiate(unitPrefab, new Vector3(unitPosX, 0, unitPosZ), Quaternion.identity);
            newUnit.Setup(i);

            _units.Add(newUnit);
        }
    }

    public Unit GetClosestTargetToUnit(Unit requestingUnit)
    {
        Unit targetUnit = null;

        foreach (var unit in _units)
        {
            if (unit == requestingUnit)
                continue;
            
            if (unit.TeamId == requestingUnit.TeamId)
                continue;
            
            if (targetUnit == null)
            {
                targetUnit = unit;
            }
            else
            {
                var requestingUnitPosition = requestingUnit.transform.position;
                
                var vectorToCurrentTargetUnit = targetUnit.transform.position - requestingUnitPosition;
                var vectorToNewTargetUnit = unit.transform.position - requestingUnitPosition;

                if (vectorToNewTargetUnit.sqrMagnitude < vectorToCurrentTargetUnit.sqrMagnitude)
                    targetUnit = unit;
            }
        }

        return targetUnit;
    }

    private void RemoveUnit(Unit unit)
    {
        _units.Remove(unit);
    }
    
    // =========== Debug ============
    public void RegenerateUnits()
    {
        foreach (var unit in _units)
        {
            Destroy(unit.gameObject);
        }
        
        _units.Clear();

        GenerateUnits();
    }
}
