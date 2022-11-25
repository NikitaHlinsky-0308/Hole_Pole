using UnityEngine;

public class LevelIndicator : MonoBehaviour
{
    [SerializeField] private Transform target;
    // [SerializeField] private GameObject targetGameObject;
    [SerializeField] private GameObject[] numberPrefabs;
    [SerializeField] private AnimationCurve moveCurve;

    private float _currentTime, _totalTime;
    

    private void Start()
    {
        _totalTime = moveCurve.keys[moveCurve.keys.Length - 1].time;
    }

    void FixedUpdate()
    {
        transform.position = new Vector3(target.position.x, moveCurve.Evaluate(_currentTime), target.position.z);

        _currentTime += Time.deltaTime;

        if (_currentTime >= _totalTime)
        {
            _currentTime = 0;
        }
        
    }

    public void ChangePrefab(int lvlNumber)
    {
        switch (lvlNumber)
        {
            case 0:
                numberPrefabs[0].SetActive(true);
                numberPrefabs[1].SetActive(false);
                numberPrefabs[2].SetActive(false);
                break;
            
            case 1:
                numberPrefabs[0].SetActive(false);
                numberPrefabs[1].SetActive(true);
                numberPrefabs[2].SetActive(false);
                break;
            
            case 2:
                numberPrefabs[0].SetActive(false);
                numberPrefabs[1].SetActive(false);
                numberPrefabs[2].SetActive(true);
                break;
        }

    }
    
}
