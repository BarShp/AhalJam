

using AHL.Core.Events;

namespace AHL.Core.Screen
{
    public class AHLScreenChangeStartEvent : IAHLEvent
    {
        public readonly ScreenTypes NewScreenType;

        public AHLScreenChangeStartEvent(ScreenTypes newScreenType)
        {
            NewScreenType = newScreenType;
        }

    }
}