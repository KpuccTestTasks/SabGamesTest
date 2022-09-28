using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] private float width;
    [SerializeField] private float length;

    [SerializeField] private Unit unitPrefab;
    [SerializeField] private int unitsOnFieldAmount;

    private Unit[] _units;

    private void Start()
    {
        transform.localScale = new Vector3(width, length);

        GenerateUnits();
    }

    private void GenerateUnits()
    {
        _units = new Unit[unitsOnFieldAmount];

        float fieldHalfWidth = width / 2;
        float fieldHalfLength = length / 2;
        
        for (int i = 0; i < unitsOnFieldAmount; i++)
        {
            float unitPosX = Random.Range(-fieldHalfWidth, fieldHalfWidth);
            float unitPosZ = Random.Range(-fieldHalfLength, fieldHalfLength);
            
            Unit newUnit = Instantiate(unitPrefab, new Vector3(unitPosX, 0, unitPosZ), Quaternion.identity);

            _units[i] = newUnit;
            
            newUnit.Move(Vector3.zero);
        }
    }
    
    // =========== Debug ============
    public void RegenerateUnits()
    {
        for (var i = 0; i < _units.Length; i++)
        {
            var unit = _units[i];
            _units[i] = null;
            
            Destroy(unit.gameObject);
        }
        
        GenerateUnits();
    }
}
