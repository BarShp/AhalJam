using AHL.Core.Events;

namespace AHL.Core.Screen
{
    public class AHLScreenChangeFinishEvent : IAHLEvent
    {
        public readonly ScreenTypes NewScreenType;

        public AHLScreenChangeFinishEvent(ScreenTypes newScreenType, ScreenTypes sourceScreenType, float screenLoadTime)
        {
            NewScreenType = newScreenType;
        }
    }
}