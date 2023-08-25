using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EraManager : MonoBehaviour
{
    Dictionary<EraType, string> eraTypeToTags;
    Dictionary<EraType, List<EraObjectController>> eraTypeToEraObjects = new ();
    EraType? currentEraType;

    void Start()
    {
        PopulateEraTypeToTag();
        PopulateEraTypeToEraObjects();
        DisableAllEraObjectControllers();
        ActivateEra(EraType.Era1);
    }

    public void ActivateEra(EraType newEraType)
    {
        if (currentEraType == newEraType) return;
        
        if (currentEraType != null)
        {
            SetEraObjectControllersState(currentEraType.Value, false);
        }

        SetEraObjectControllersState(newEraType, true);
        currentEraType = newEraType;
    }    

    private void DisableAllEraObjectControllers()
    {
        eraTypeToEraObjects.Keys.ToList().ForEach(eraTypeToDisable => SetEraObjectControllersState(eraTypeToDisable, false));
    }

    private void SetEraObjectControllersState(EraType eraType, bool isEnabled)
    {
        foreach (var eraObjectController in eraTypeToEraObjects[eraType]) 
        {
            if (isEnabled)
            {
                eraObjectController.EnableObject();
            }
            else
            {
                eraObjectController.DisableObject();
            }
        }
    }

    private void PopulateEraTypeToTag()
    {
        eraTypeToTags = new Dictionary<EraType, string>{
            {EraType.Era1, "Era1"},
            {EraType.Era2, "Era2"},
        };
    }

    private void PopulateEraTypeToEraObjects()
    {
        foreach (var eraTypeToTag in eraTypeToTags)
        {
            var eraObjects = GameObject.FindGameObjectsWithTag(eraTypeToTag.Value);
            var eraObjectControllers = eraObjects.Select(eraObject => eraObject.GetComponent<EraObjectController>());
            eraTypeToEraObjects[eraTypeToTag.Key] = eraObjectControllers.ToList();
        }
    }
}
