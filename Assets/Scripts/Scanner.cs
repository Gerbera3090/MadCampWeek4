using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange = 10f;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public GameObject nearestTarget;
    public string type;

    private Dictionary<string, Vector2> scanVectorDictionary = new Dictionary<string, Vector2>()
    {
        { "vertical", Vector2.right },
        { "horizontal", Vector2.up },
        { "circle", Vector2.zero }
    };
    
    // ReSharper disable Unity.PerformanceAnalysis
    public GameObject GetTarget(bool isPositiveDir)
    {
        int dir = isPositiveDir ? 1 : -1;
        if (!scanVectorDictionary.ContainsKey(type))
        {
            Debug.Log("Error : UnDefined type");
            return null;
        }
        targets = type.Equals("circle") ? Physics2D.CircleCastAll(transform.position, scanRange,Vector2.zero,0,targetLayer) 
                                        : Physics2D.RaycastAll(transform.position,dir * scanVectorDictionary[type], scanRange, targetLayer);
        
        nearestTarget = GetNearest();
        return nearestTarget;
    }

    GameObject GetNearest()
    {
        GameObject result = null;
        float diff = 100;
    
        foreach (RaycastHit2D target in targets){
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos,targetPos);
    
            if (curDiff < diff) {
                diff = curDiff;
                result = target.collider.gameObject;
            }
        }
    
        return result;
    }

    
}