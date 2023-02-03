using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefab;

    [SerializeField]
    private float _interval = 2f;

    [SerializeField]
    private int _limit = 10;

    private float _lastSpawnTime;
    private int _count = 0;

    public void Update()
    {
        if (_lastSpawnTime + _interval < Time.time)
            Spawn();
    }

    public void Spawn()
    {
        if (_count >= _limit) return;

        _lastSpawnTime = Time.time;
        _count++;

        GameObject.Instantiate(_prefab);
    }
}