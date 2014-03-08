// This a events system for unity better than the built-in message system.
// This system is not hierarchy-dependant
// Credits : http://www.willrmiller.com/?p=87

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Event {

/// <summary>
/// The base classe to send an event.
/// </summary>
public class GameEvent{}

/// <summary>
/// Singleton. Use it to send GameEvent object.
/// </summary>
public class EventManager {
	private static EventManager _sharedManager = null;
	public static EventManager sharedManager
	{
		get
		{
			if(_sharedManager == null)
			{
				_sharedManager = new EventManager();
			}
			
			return _sharedManager;
		}
	}
	
	public delegate void EventDelegate<T> (T e) where T : GameEvent;
	
	private Dictionary<System.Type, System.Delegate> delegates = 
		new Dictionary<System.Type, System.Delegate>();
	
	/// <summary>
	/// Adds the listener.
	/// </summary>
	/// <param name='del'>
	/// The method that will listen to the event.
	/// </param>
	/// <typeparam name='T'>
	/// The GameEvent type parameter.
	/// </typeparam>
	public void AddListener<T> (EventDelegate<T> del) where T : GameEvent
	{
		if(delegates.ContainsKey(typeof(T)))
		{
			System.Delegate tempDel = delegates[typeof(T)];
			delegates[typeof(T)] = System.Delegate.Combine(tempDel, del);
		}
		else {
			delegates[typeof(T)] = del;
		}
	}
	
	/// <summary>
	/// Removes the listener.
	/// </summary>
	/// <param name='del'>
	/// The method that listened to the event.
	/// </param>
	/// <typeparam name='T'>
	/// The GameEvent type parameter.
	/// </typeparam>
	public void RemoveListener<T> (EventDelegate<T> del) where T : GameEvent
	{
		if(delegates.ContainsKey(typeof(T)))
		{
			var currentDel = System.Delegate.Remove(delegates[typeof(T)], del);
			
			if(currentDel == null)
			{
				delegates.Remove(typeof(T));
			}
			else
			{
				delegates[typeof(T)] = currentDel;
			}
		}
	}
	
	/// <summary>
	/// Raise the specified GameEvent.
	/// </summary>
	/// <param name='e'>
	/// A GameEvent base object.
	/// </param>
	public void Raise (GameEvent e)
	{
		if(e == null)
		{
			Debug.Log("Invalid event argument: " + e.GetType().ToString());
			return;
		}
		
		if (delegates.ContainsKey(e.GetType()))
		{
			delegates[e.GetType()].DynamicInvoke(e);
		}
	}
}
	
}