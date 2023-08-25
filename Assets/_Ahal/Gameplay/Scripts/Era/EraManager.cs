using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


public class EraManager : MonoBehaviour
{
    Dictionary<EraType, string> eraTypeToTags;
    Dictionary<EraType, List<EraObjectController>> eraTypeToEraObjects = new ();
    EraType? currentEraType;

    void Start()
    {
        PopulateEraTypeToTag();
        PopulateEraTypeToEraObjects();
        ActivateEra(EraType.Era1);
    }

    public void ActivateEra(EraType newEraType)
    {
        if (currentEraType == newEraType) return;
        
        if (currentEraType != null)
        {
            foreach (var eraObjectController in eraTypeToEraObjects[currentEraType.Value]) 
            {
                eraObjectController.DisableObject();
            }
        }

        foreach (var eraObjectController in eraTypeToEraObjects[newEraType]) 
        {
            eraObjectController.EnableObject();
        }

        currentEraType = newEraType;
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
