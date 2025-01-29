using Player;
using UnityEngine;

public class FreezeBullet : MonoBehaviour
{
    [SerializeField] private float _speed;

    [SerializeField] private float _duration;

    private Vector3 _direction;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        _transform.position += _direction * (_speed * Time.deltaTime);

        if (_duration <= 0)
        {
            Destroy(gameObject);
        }

        _duration -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerManager>(out PlayerManager playerManager))
        {
            playerManager.Freeze();
        }

        Destroy(gameObject);
    }

    public Vector3 Direction { get { return _direction; } set { _direction = value; } }
}
