using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpawnableManager : MonoBehaviour
{
    [Header("TEXTE INDIQUANT LES ETATS DE L'APPLICATION")]
    [SerializeField] // texte de debug
    private Text m_text; 
    
    [Header("SET L'ARCAMERA POUR POUVOIR ENVOYER DES RAYONS")]
    [SerializeField] // recuperation de l'ARCamera
    private Camera m_arCamera;

    [Header("GLISSER LE AR RAYCAST MANAGER SUR l'AR SESSION ORIGIN")]
    [SerializeField] // recuperation de l'ARRaycastManager
    private ARRaycastManager m_raycastManger;
    
    [Header("OBJET ACTUELLEMENT INSTANTIE")]
    [SerializeField] // Prefab actuellement selectionn�
    private GameObject m_spawnablePrefab;
    
    [Header("OBJET ACTUELLEMENT UTILISE")]
    [SerializeField] // prefab actuellement instanti�
    private GameObject m_spawnedObject;

    [Header("NOMBRE MAXIMUM D'OBJETS DEPOSABLE DANS LA SCENE")]
    [SerializeField] // nombre maximum d'objet dans la scene
    private int m_maxPrefabSpawnCount = 0;

    // compteur du nombre d'objets dans la scene
    private int m_placedPrefabCount;
        
    // Stockage des points d'impacts
    private List<ARRaycastHit> m_hits = new List<ARRaycastHit>();
   // Stockage des prefabs instanti� dans la scene
    private List<GameObject> m_placedPrefabList = new List<GameObject>();
    // Activation et desactivation du raycast sur les objets
    public static bool m_switchRaycast;
    public static bool m_switchDestroyOrCreate;

    // initialisation des variables
    void Start()
    {
        m_switchRaycast = true;
        m_switchDestroyOrCreate = true;
        m_spawnedObject = null;
        m_arCamera = GameObject.Find("ARCamera").GetComponent<Camera>();
    }

    // fonction update ou se g�re le rayon
    void Update()
    {
        // si le boolean du raycast est vrai, alors j'instancie un Gameobject
        if(m_switchDestroyOrCreate)
        {
            InstantiateGameObject();
        }
        else
        {
            DestroyGameObject();
        }
    }

    // si mon rayon tombe sur le plan et qu'il ne tombe pas sur un objet "Spawnable" il en cr�er un
    private void InstantiateGameObject()
    {
        RaycastHit m_hit;
        Ray m_ray = m_arCamera.ScreenPointToRay(Input.GetTouch(0).position);
        
        // si je touche l'�cran j'envoi un rayon sur le sol
        if (m_raycastManger.Raycast(Input.GetTouch(0).position, m_hits))
        {
            // si je suis au d�but du touch et que mon prefab est vide
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && m_spawnedObject == null)
            {
                // si je ne touche pas de l'UI...
                if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    // si je touche un objet existant, je peux le d�placer
                    if (Physics.Raycast(m_ray, out m_hit))
                    {
                        if (m_hit.collider.gameObject.CompareTag("Spawnable"))
                        {
                            m_spawnedObject = m_hit.collider.gameObject;
                        }
                        // sinon si je ne touche pas de prefab, je cr�er un prefab � l'endroit de l'impact (si le compteur est inferieur � la valeur donn�e)
                        else if (m_placedPrefabCount < m_maxPrefabSpawnCount)
                        {
                            SpawnPrefab(m_hits[0].pose.position);
                        }
                    }
                }
                else
                {
                    m_text.text = "toucher l'UI";
                }
            }

            // sinon si je touch l'�cran et que ma variable prefab n'est pas nulle, je d�place l'objet legerement au dessus du sol
            else if (Input.GetTouch(0).phase == TouchPhase.Moved && m_spawnedObject != null)
            {
                m_spawnedObject.transform.position = m_hits[0].pose.position;
                m_spawnedObject.transform.position = new Vector3(m_spawnedObject.transform.position.x, m_spawnedObject.transform.position.y + 0.3f, m_spawnedObject.transform.position.z);

            }

            // si je fini de toucher l'�cran, j'abaisse mon prefab sur le plan et la variable prefab est vid�e
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                m_spawnedObject.transform.position = m_hits[0].pose.position;
                m_spawnedObject = null;

            }
        }
    }

    // je detruis les objets lorsque que je tombe sur un raycast taggu� "Spawnable"
    private void DestroyGameObject()
    {
        RaycastHit m_hit;
        Ray m_ray = m_arCamera.ScreenPointToRay(Input.GetTouch(0).position);

        // si je touche l'�cran j'envoi un rayon sur le sol
        if (m_raycastManger.Raycast(Input.GetTouch(0).position, m_hits))
        {
            // si je suis au d�but du touch et que mon prefab est vide
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && m_spawnedObject == null)
            {
                // si je ne touche pas de l'UI...
                if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    // si je touche un objet existant, je peux le d�placer
                    if (Physics.Raycast(m_ray, out m_hit))
                    {
                        if (m_hit.collider.gameObject.CompareTag("Spawnable"))
                        {
                            m_spawnedObject = m_hit.collider.gameObject;
                            Destroy(m_spawnedObject);
                            m_placedPrefabList.Remove(m_spawnedObject);
                            m_placedPrefabCount--;
                        }                   
                    }
                }
                else
                {
                    m_text.text = "toucher l'UI";
                }
            }
        }
    }

    // je positionne mon prefab sur la position de "hit" du rayon
    private void SpawnPrefab(Vector3 _spawnPosition)
    {
        m_spawnedObject = Instantiate(m_spawnablePrefab, _spawnPosition, Quaternion.identity);
        m_placedPrefabList.Add(m_spawnedObject);
        m_placedPrefabCount++;
    }

    // fonction permettant de d�finir quel objet je souhaites instancier
    public void SwitchArPrefab(GameObject _prefabType)
    {
        m_spawnablePrefab = _prefabType;
    }

    // Changement de mode pour detruire les prefabs
    public void CreateOrDestroyGameObject()
    {
        m_switchDestroyOrCreate = !m_switchDestroyOrCreate;
    }
}
