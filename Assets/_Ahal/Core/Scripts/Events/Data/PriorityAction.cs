using System;

namespace AHL.Core.Events
{
    public readonly struct PriorityAction
    {
        public readonly Delegate Action;
        public readonly int Priority;

        public PriorityAction(Delegate action, int priority)
        {
            Action = action;
            Priority = priority;
        }
    }
}