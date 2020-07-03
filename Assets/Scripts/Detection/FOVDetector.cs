using System;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[Serializable]
public class DetectionZone
{
    public float radius = 0f;
    public UnityEvent methods;
    public bool FOVDependable = false;
    public bool IgnoreOthers = false;
    [NonSerialized]
    public bool ignored = false;
    [NonSerialized]
    public bool zoneHit = false;
}
[Serializable]
public class DetectableObject
{
    public GameObject Instance;
    public Transform dObjectTransform { 
        get 
        {
            return Instance.GetComponent<Transform>(); 
        } 
    }
    public UnityEvent methods;
    public float distance;
    [NonSerialized]
    public bool isDetected = false;
}

public class FOVDetector : MonoBehaviour
{
    public List<DetectableObject> DetectableObjects;
    public List<DetectionZone> DetectionZones;
    [Range(0f,360f)]public float FOVAngle = 60f;
    private void OnDrawGizmos()
    {
        float maxRadius = 0f;
        if (DetectionZones != null)
        {
            foreach (DetectionZone dZone in DetectionZones)
            {
                Gizmos.color = Color.grey;
                Gizmos.DrawWireSphere(transform.position, dZone.radius);

                if (dZone.radius > maxRadius)
                {
                    maxRadius = dZone.radius;
                }
            }
        }
        Vector3 fovLine1 = Quaternion.AngleAxis(FOVAngle/2, transform.up) * transform.forward * maxRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-FOVAngle/2, transform.up) * transform.forward * maxRadius;
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);
        
        if (DetectableObjects != null)
        {
            foreach (DetectableObject dObject in DetectableObjects)
            {
                if(dObject.isDetected) Gizmos.color = Color.red;
                else Gizmos.color = Color.green;
                Gizmos.DrawRay(transform.position, (dObject.dObjectTransform.position - transform.position).normalized * maxRadius);
            }
        }
    }
    public bool isVisible(DetectionZone detectionZone, DetectableObject target)
    {
        Collider[] overlaps = Physics.OverlapSphere(transform.position, detectionZone.radius);
        foreach (Collider overlap in overlaps)
        {
            if(overlap != null)
            {
                if(overlap.transform == target.dObjectTransform)
                {
                    if (detectionZone.FOVDependable)
                    {
                        Vector3 directionBetween = (target.dObjectTransform.position - transform.position).normalized;
                        directionBetween.y *= 0;
                        float angle = Vector3.Angle(transform.forward, directionBetween);
                        if (angle <= FOVAngle / 2)
                        {
                            Ray ray = new Ray(transform.position, target.dObjectTransform.position - transform.position);
                            RaycastHit hit;
                            if (Physics.Raycast(ray, out hit, detectionZone.radius))
                            {
                                if (hit.transform == target.dObjectTransform)
                                {
                                    detectionZone.zoneHit = true;
                                    target.isDetected = true;
                                    return true;
                                }
                            }
                        }
                    }
                    else
                    {
                        Ray ray = new Ray(transform.position, target.dObjectTransform.position - transform.position);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit, detectionZone.radius))
                        {
                            if (hit.transform == target.dObjectTransform)
                            {
                                detectionZone.zoneHit = true;
                                target.isDetected = true;
                                return true;
                            }
                        }
                    }
                }
            }
        }
        target.isDetected = false;
        return false;
    }

    private void DetectExample()
    {

        foreach (DetectionZone detectionZone in DetectionZones)
        {
            foreach (DetectableObject detectableObject in DetectableObjects)
            {
                detectableObject.distance = Vector3.Distance(transform.position, detectableObject.Instance.transform.position);
                isVisible(detectionZone, detectableObject);
                if (detectableObject.isDetected) detectableObject.methods.Invoke();
            }
            
        }
        foreach (DetectionZone detectionZone in DetectionZones)
        {
            if(detectionZone.zoneHit)
            {
                detectionZone.methods.Invoke();
                if (detectionZone.IgnoreOthers)
                {
                    break;
                }
            }
        }
        foreach(DetectionZone detectionZone in DetectionZones) detectionZone.zoneHit = false;

    }
    private void Start()
    {
        DetectionZones.OrderBy(x => x.radius);
    }
    void Update()
    {
        DetectExample();
    }
}
