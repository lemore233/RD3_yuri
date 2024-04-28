using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    [Space(15)]
    [Header("[Definir]")]
    [SerializeField] private float smooth;
    [SerializeField] private string key;

    [SerializeField] private GameObject painel;
    [SerializeField] private TMP_Text name;
    [SerializeField] private TMP_Text tipo;
    [SerializeField] private TMP_Text descricao;

    [Space(15)]
    [Header("[Visualizar]")]
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private APIManager managerAPI;
    [SerializeField] private CreatureData creatureData;

    private void Awake()
    {
        if (managerAPI == null)
        {
            managerAPI = FindAnyObjectByType<APIManager>();
        }
        if (mainCamera == null)
        {
            mainCamera = GameObject.FindWithTag("MainCamera");
        }

    }
    private void Start()
    {
        painel.SetActive(false);
        RequestAPIGetCreature();

    }

    private void LateUpdate()
    {
        if (mainCamera != null)
        {

            Vector3 direcao = mainCamera.transform.position - transform.position;

            Quaternion rotacaoDesejada = Quaternion.LookRotation(direcao);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotacaoDesejada, Time.deltaTime * smooth);
        }

    }

    public void SetInfoCreature(CreatureData creatureDataAPI)
    {
        creatureData = creatureDataAPI;

        name.text = "Nome: " + creatureData.creature[0].name;
        tipo.text = "Tipo: " + creatureData.creature[0].tipo;
        descricao.text = "Descrição: " + creatureData.creature[0].descricao;

        painel.SetActive(true);
    }
    private void RequestAPIGetCreature()
    {
        if (managerAPI != null)
        {
            managerAPI.GetCreature(key, this);
        }
    }

}
