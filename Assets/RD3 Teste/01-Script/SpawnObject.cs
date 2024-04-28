using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SpawnObject : MonoBehaviour
{
    [Space(15)]
    [Header("[Definir]")]
    [SerializeField] private ARRaycastManager aRRaycastManager;
    [SerializeField] private List<GameObject> objetoSpawn;


    [Space(15)]
    [Header("[Visualizar]")]
    [SerializeField] private int idCreature = -1;
    [SerializeField] private List<GameObject> objetoChild;

    void Awake()
    {
        for (int i = 0; i < objetoSpawn.Count; i++)
        {
            objetoChild.Add(null);
        }

    }

    void Update()
    {

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && idCreature >= 0)
        {
            List<ARRaycastHit> touches = new List<ARRaycastHit>();

            aRRaycastManager.Raycast(Input.GetTouch(0).position, touches, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

            if (touches.Count > 0)
            {

                if (objetoChild[idCreature] != null)
                {
                    objetoChild[idCreature].transform.position = touches[0].pose.position;

                }
                else
                {
                    GameObject newGameObject = Instantiate(objetoSpawn[idCreature], touches[0].pose.position, touches[0].pose.rotation);
                    newGameObject.transform.LookAt(Camera.main.transform);
                    objetoChild[idCreature] = newGameObject;
                    
                    Vector3 newRotation = newGameObject.transform.eulerAngles;
                    newRotation.x = 0;
                    newGameObject.transform.eulerAngles = newRotation;
                    
                }

                idCreature = -1;


            }
        }


    }

    public void SetIDCreature(int id)
    {
        idCreature = id;
    }

}
