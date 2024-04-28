using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.RegularExpressions;

public class APIManager : MonoBehaviour
{
    [Space(15)]
    [Header("[Visualizar]")]
    [SerializeField] private string urlLogin = "https://rd3space.com/becoapi/login";
    [SerializeField] private string urlGetDados = "https://rd3space.com/becoapi/creature.php?key=";

    [SerializeField] private string username = "testedev@rd3.digital";
    [SerializeField] private string password = "rd3digital";

    [SerializeField] private string token;

    void Start()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Sem conexão com a Internet.");
        }
        else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork ||
                 Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            StartCoroutine(Login());
        }
    }

    public void GetCreature(string key, CanvasManager canvasManager)
    {
        StartCoroutine(GetAPI(token, key, canvasManager.SetInfoCreature));
    }

    private IEnumerator Login()
    {
        WWWForm form = new WWWForm();
        form.AddField("email", username);
        form.AddField("password", password);

        using (UnityWebRequest request = UnityWebRequest.Post(urlLogin, form))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);

            }
            else
            {
                string jsonDowLoaded = request.downloadHandler.text;

                string pattern = "{\"token\":\"Bearer ([\\w.-]+)\"";

                Match match = Regex.Match(jsonDowLoaded, pattern);

                if (match.Success)
                {
                    token = match.Groups[1].Value;

                }
                else
                {
                    Debug.Log("Token não encontrado na string.");
                }

            }
        }
    }

    private IEnumerator GetAPI(string token, string key, System.Action<CreatureData> callback)
    {
        if (!string.IsNullOrEmpty(token))
        {

            string url = urlGetDados + key;
            UnityWebRequest request = UnityWebRequest.Get(url);
            request.SetRequestHeader("Authorization", "Bearer " + token);


            yield return request.SendWebRequest();


            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
            }
            else
            {
                string jsonDowLoaded = request.downloadHandler.text;
                CreatureData creatureData = JsonUtility.FromJson<CreatureData>(jsonDowLoaded);

                callback.Invoke(creatureData);
            }
        }
        else
        {
            Debug.Log("Sem conexão com a Internet.");
        }
    }
}