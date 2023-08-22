using AHL.Core.Events;
using AHL.Core.General.Utils;
using AHL.Core.Main;
using AHL.Core.Screen;
using UnityEngine;

public class MainSceneRoot : AHLRootScreenMonoBehaviour
{
    void Start()
    {
        var test =  (AHLManager) injectedData.AHLManager;
        
        test.Events.AddEventListener<TestEvent>(DoSomething);
        
        test.Events.InvokeAHLEvent(new TestEvent());
    }

    private void DoSomething(TestEvent obj)
    {
        Debug.Log("Meow");
    }
}

public class TestEvent : IAHLEvent
{
    
}
