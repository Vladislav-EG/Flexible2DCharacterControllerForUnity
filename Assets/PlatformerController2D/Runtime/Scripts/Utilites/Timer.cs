
using System;

public abstract class Timer
{
	protected float initialTime;
	protected float Time { get; set; }
	public bool IsRunning { get; protected set; }

	public float Progress => Time / initialTime;

	public Action OnTimerStart = delegate { };
	public Action OnTimerStop = delegate { };
	
	protected Timer(float value)
	{
		initialTime = value;
		IsRunning = false;
	}

	public void Start()
	{

		Time = initialTime;
		if (!IsRunning)
		{
			IsRunning = true;
			OnTimerStart.Invoke();
		}
	}

	public void Stop()
	{
		if (IsRunning)
		{
			IsRunning = false;
			OnTimerStop.Invoke();
		}
	}

	public void Resume() => IsRunning = true;
	public void Pause() => IsRunning = false;

	public abstract void Tick(float deltaTime);
}

public class CountdownTimer : Timer
{
	private System.Func<float> getInitialTime; // Делегат для получения актуального времени

	public Action OnTimerFinished = delegate { };

	// public CountdownTimer(float value) : base(value)
	// {
	// }
	
	public CountdownTimer(System.Func<float> getTime) : base(0)
	{
		getInitialTime = getTime;
		initialTime = getTime();
	}
	
	public new void Start()
	{
		if (getInitialTime != null)
		{
			initialTime = getInitialTime();
		}
		Time = initialTime;
		if (!IsRunning)
		{
			IsRunning = true;
			OnTimerStart.Invoke();
		}
	}

	public override void Tick(float deltaTime)
	{
		if (IsRunning && Time > 0)
		{
			Time -= deltaTime;
		}

		if (IsRunning && Time <= 0)
		{
			Stop();
		}

		if (IsFinished)
		{
			OnTimerFinished?.Invoke();
		}
	}

	public bool IsFinished => Time <= 0;

	public void Reset() => Time = initialTime;

	public void Reset(float newTime)
	{
		initialTime = newTime;
		Reset();
	}
	
	
	
}

public class StopwatchTimer : Timer
{
	public StopwatchTimer() : base(0) { }

	public override void Tick(float deltaTime)
	{
		if (IsRunning)
		{
			Time += deltaTime;
		}
	}

	public void Reset() => Time = 0;

	public float GetTime() => Time;
}