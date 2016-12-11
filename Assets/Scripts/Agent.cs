using UnityEngine;
using System.Collections.Generic;

public class Agent : MonoBehaviour
{

    private const float RAY_DISTANCE = 1f;
    private const int RAYS = 3;
    private const int ROTATION_ANGLE = 30;

    private bool _isAlive = true;
    private float _mySize = 0.2f;
    private float _speed = 2f;
    private Rigidbody2D _rigidbody2D;
    private NeuralNetwork _brains;
    private float _score = 0;
    private Vector2 _startPosition;
    private int _frames = 0;

	// Use this for initialization
	void Start ()
	{
        _brains = new NeuralNetwork(RAYS);
	    _rigidbody2D = GetComponent<Rigidbody2D>();
	    _startPosition = transform.position;

	}
	
	// Update is called once per frame
	void Update ()
	{
	    _frames++;
	    if (_isAlive)
	    {
	        
            List<float> distances = new List<float>();
	        int step = 45/(RAYS/2);
	        float angle = transform.rotation.eulerAngles.z*Mathf.Deg2Rad;
	        for (var i = 0; i < RAYS; i++)
	        {
	            Vector2 direction = new Vector2();
	            direction.x = Mathf.Cos(angle + (135 - step*i)*Mathf.Deg2Rad);
	            direction.y = Mathf.Sin(angle + (135 - step*i)*Mathf.Deg2Rad);
	            distances.Add(getDistance(direction));
	        }

	        int move = _brains.Predict(distances);
            Vector3 rotation = transform.rotation.eulerAngles;
            switch (move)
            {
                case 0:
                    rotation.z += ROTATION_ANGLE;
                    break;
                case 1:
                    rotation.z -= ROTATION_ANGLE;
                    break;
            }
            transform.rotation = Quaternion.Euler(rotation);

            _rigidbody2D.velocity = new Vector2(-Mathf.Sin(angle), Mathf.Cos(angle)) * _speed;

	        if (_frames % 30 == 0)
	        {
	            float distance = Vector2.Distance(transform.position, _startPosition);
	            if (distance > 0.5)
	            {
                    _score += Vector2.Distance(transform.position, _startPosition);
                    _startPosition = transform.position;
                }
	            else
	            {
	                Die();
	            }
            }

	    }
	    else
	    {
	        _rigidbody2D.velocity = new Vector2(0, 0);
	    }
	    

	}

    public void Reset(Vector2 pos)
    {
        _isAlive = true;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.7f);
        transform.position = pos;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        _score = 0;
        _frames = 0;
    }

    void Die()
    {
        _isAlive = false;
        GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 0.2f, 0.3f);
    }

    float getDistance(Vector2 direction)
    {
        int layerMask = 9;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, RAY_DISTANCE, layerMask);
        Debug.DrawRay(transform.position, direction, Color.green);
        if (hit.collider != null)
        {
            float distance = Vector2.Distance(transform.position, hit.point);
            if (distance < _mySize)
            {
                Die();
            }
            
            return distance;
        }
        
        return RAY_DISTANCE;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.LogWarning(other.gameObject.name);
    }

    public float Score
    {
        get
        {
            return _score;
        }
    }

    public NeuralNetwork Brains
    {
        get
        {
            return new NeuralNetwork(_brains);
            
        }
        set
        {
            _brains = value;
            
        }
    }

    public bool Alive
    {
        get
        {
            return _isAlive;
        }
    }
}
