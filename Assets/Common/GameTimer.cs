public sealed class GameTimer 
{
    readonly float _begin = 0.0f;
    readonly float _end = 1.0f;
    float _current;

	public float CompletedFactor { get => _current / _end; }
    public float RemainingTime { get => _end - _current; }
	public float Current { get => _current; }

	public GameTimer(float begin, float end)
	{
		_begin = begin;
		_end = end;
		_current = begin;
	}

    public GameTimer(float end)
    {
        _begin = 0.0f;
        _end = end;
        _current = _begin;
    }

    public void Tick(float amount)
	{
		_current += amount;
		if (_current > _end)
			_current = _end;
	}

	public bool HasReachedEnd()
	{
		return _end < _current;
	}

	public void ResetTime()
	{
		_current = _begin;
	}

    public bool HasStarted()
    {
		return CompletedFactor != 0.0f;
    }
}
