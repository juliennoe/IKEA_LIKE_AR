using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaneDetectionToggle : MonoBehaviour
{
    [SerializeField]
    private ARPlaneManager m_planeManager;
    [SerializeField]
    private GameObject m_placementIndicator;


    private void Awake()
    {
        m_planeManager = GetComponent<ARPlaneManager>();
    }

    public void TogglePlaneDetection()
    {
        m_planeManager.enabled = !m_planeManager.enabled;

        if (m_planeManager.enabled)
        {
            SetAllPlanesActive(true);
            SpawnableManager.m_switchRaycast = true;
            m_placementIndicator.SetActive(true);
        }
        else
        {
            SetAllPlanesActive(false);
            SpawnableManager.m_switchRaycast = false;
            m_placementIndicator.SetActive(false);
        }
    }

    private void SetAllPlanesActive(bool _value)
    {
        foreach(var _plane in m_planeManager.trackables)
        {
            _plane.gameObject.SetActive(_value);
        }
    }



}
