using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class TrackingImage : MonoBehaviour
{
    private ARTrackedImageManager m_arTrackedImageManager;
    // Start is called before the first frame update

    private void Awake()
    {
        m_arTrackedImageManager = FindObjectOfType<ARTrackedImageManager>();
            
    }

    public void OnEnable()
    {
        m_arTrackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    public void OnDisable()
    {
        m_arTrackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    public void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach(var trackedImage in args.added)
        {
            Debug.Log(trackedImage.name);
        }
    }
}
