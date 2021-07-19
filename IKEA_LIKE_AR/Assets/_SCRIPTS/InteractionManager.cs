using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public GameObject m_mainObject;
    public int m_speed;
    public bool m_switch;

    public GameObject m_sphere;
    public GameObject m_cylindre;
    public GameObject m_cube;
    // Start is called before the first frame update
    void Start()
    {
        m_switch = false;
        m_mainObject = m_cube;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_switch == true)
        {
            m_mainObject.transform.Rotate(0, 15 * Time.deltaTime * m_speed, 0);
        }    
    }

    public void InstantiateGameobject()
    {
      Destroy(GameObject.FindGameObjectWithTag("clone"));
      var _ref =  Instantiate(m_mainObject, new Vector3(0, 0, 0), Quaternion.identity);
      m_mainObject = _ref;
    }

    public void UpScale()
    {
        if(m_mainObject.transform.localScale.x < 5)
        {
            m_mainObject.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
        }
        
    }

    public void DownScale()
    {
        if(m_mainObject.transform.localScale.x > 1)
        {
            m_mainObject.transform.localScale += new Vector3(-0.5f, -0.5f, -0.5f);
        }
        
    }

    public void UpRotate()
    {
        m_mainObject.transform.Rotate(0, 15, 0);
    }

    public void DownRotate()
    {
        m_mainObject.transform.Rotate(0, -15, 0);
    }

    public void OnOffAutoRotation()
    {
        m_switch = !m_switch; 
    }

    public void SetObject(GameObject _refGameObject)
    {
        m_mainObject = _refGameObject;
    }
}
