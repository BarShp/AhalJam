using System;
using System.Collections.Generic;
using System.Linq;
using AHL.Core.General;
using AHL.Core.Main;

namespace AHL.Core.Events
{
	public sealed class AHLEventsManager : AHLBaseManager, IAHLEventsManager
	{
	    private const string LOG_TAG = "#AhalEventsManager# ";

	    private Dictionary<Type, List<PriorityAction>> eventDict = new();
	    
	    private Type[] typeIndex;
	    private PriorityAction[][] actioIndex;
	    
	    
		public AHLEventsManager(AHLManager manager, Action<AHLBaseManager> onComplete) : base(manager, onComplete)
		{
			OnInitComplete();
		}
		
		public void InvokeAHLEvent(IAHLEvent evt)
		{
			if(evt == null)
			{
				return;
			}
			
			typeIndex = eventDict.Keys.ToArray();
			actioIndex = eventDict.Select(x => x.Value.ToArray()).ToArray();
			
			for (var i = 0; i < typeIndex.Length; i++)
			{
				if (!typeIndex[i].IsInstanceOfType(evt))
				{
					continue;
				}
				
				var actionList =  actioIndex[i].OrderBy(o => o.Priority).ToArray();
					
				for (var index = 0; index < actionList.Length; index++)
				{
					var priorityAction = actionList[index];
					priorityAction.Action.DynamicInvoke(evt);
				}
			}
		}

		public void AddEventListener<T>(Action<T> action, int priority = 100) where T : IAHLEvent
		{
			var priorityAction = new PriorityAction(action, priority);
			
			if (eventDict.TryGetValue(typeof(T), out var actionList))
			{
				actionList.Add(priorityAction);
			}
			else
			{
				actionList = new List<PriorityAction> {priorityAction};
				eventDict.Add(typeof(T), actionList);
			}
		}

		public void RemoveEventListener<T>(Action<T> action) where T : IAHLEvent
		{
			if (eventDict.TryGetValue(typeof(T), out var actionList))
			{
				actionList.RemoveAll(act => act.Action == (Delegate) action);
				if (actionList.Count == 0)
				{
					eventDict.Remove(typeof(T));
				}
			}
		}
	}
}