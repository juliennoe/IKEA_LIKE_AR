using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlacementIndicator : MonoBehaviour
{
    private ARRaycastManager rayManager;
    private GameObject visual;
    
    void Start()
    {

        // charge le composant raycast
        rayManager = FindObjectOfType<ARRaycastManager>();
        visual = transform.GetChild(0).gameObject;

        // masque l'indicateur
        visual.SetActive(false);
    }

    void Update()
    {
        // lance un raycast sur les plans trouvés
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        rayManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);

        // actualise la position de l'objet
        if (hits.Count > 0)
        {
            transform.position = hits[0].pose.position;
            transform.rotation = hits[0].pose.rotation;

            // re active l'objet tracker si celui ci est desactivé dans la herarchie
            if (!visual.activeInHierarchy)
                visual.SetActive(true);
        }
    }

}