using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TailGeneration))]
[RequireComponent(typeof(SnakeInput))]
public class Snake : MonoBehaviour
{
    [SerializeField] private SnakeHead _head;
    [SerializeField] private int _tailSize;
    [SerializeField] private float _speed;
    [SerializeField] private float _tailSpringness;

    private SnakeInput _input;
    private List<Segment> _tail;
    private TailGeneration _tailGeneration;

    public UnityAction<int> SizeUpdate;

    private void Awake()
    {
        _tailGeneration = GetComponent<TailGeneration>();
        _input = GetComponent<SnakeInput>();

        _tail = _tailGeneration.Generate(_tailSize);

        SizeUpdate?.Invoke(_tail.Count);
    }

    private void OnEnable()
    {
        _head.BlockCollided += OnBlockCollided;
        _head.BonusCollected += OnBonusColleted;
    }

    private void OnDisable()
    {
        _head.BlockCollided -= OnBlockCollided;
        _head.BonusCollected -= OnBonusColleted;
        
    }

    private void FixedUpdate()
    {
        Move(_head.transform.position + _head.transform.up * _speed * Time.fixedDeltaTime);

        _head.transform.up = _input.GetDirectionToClick(_head.transform.position);
    }

    private void Move(Vector2 nextPosition)
    {
        Vector2 previousPosition = _head.transform.position;

        foreach(var segment in _tail)
        {
            Vector2 tempPosition = segment.transform.position;
            segment.transform.position = Vector2.Lerp(segment.transform.position, previousPosition, _tailSpringness * Time.fixedDeltaTime);
            previousPosition = tempPosition;
        }

        _head.Move(nextPosition);
    }

    private void OnBlockCollided()
    {
        Segment deletedSegment = _tail[_tail.Count - 1];
        _tail.Remove(deletedSegment);
        Destroy(deletedSegment.gameObject);
        SizeUpdate?.Invoke(_tail.Count);
    }

    private void OnBonusColleted(int bonusSize)
    {
        _tail.AddRange(_tailGeneration.Generate(bonusSize));
        SizeUpdate?.Invoke(_tail.Count);
    }
}
