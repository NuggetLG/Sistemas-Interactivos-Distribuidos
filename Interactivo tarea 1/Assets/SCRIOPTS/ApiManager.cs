using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Unity.VisualScripting;

public class ApiManager : MonoBehaviour
{
    [SerializeField]
    private RawImage Primero, Segundo, Tecero, Cuarto, Quinto;
    private int[] OpId;

    [SerializeField]
    private int UserId = 1;
    public void GetUsersClick()
    {
        StartCoroutine(GetPlayerInfo());
    }
    IEnumerator GetPlayerInfo()
    {
        UnityWebRequest
            www = UnityWebRequest.Get("https://my-json-server.typicode.com/NuggetLG/Sistemas-Interactivos-Distribuidos/users/" + UserId);
        yield return www.Send();
        UnityWebRequest
            www2 = UnityWebRequest.Get("https://rickandmortyapi.com/api/character/");
        yield return www2.Send();

        if (www.isNetworkError)
        {
            Debug.Log("NETWORK ERROR" + www.error);

        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            string json = www.downloadHandler.text;
            if (www.responseCode == 200)
            {
                UserData user = JsonUtility.FromJson<UserData>(json);
                OpId = user.deck;

            }
            else
            {
                string mensaje = "Status :" + www.responseCode;
                mensaje += "/ncontent-type:" + www.GetResponseHeader("content-type:");
                mensaje += "/nError :" + www.error;
                Debug.Log(mensaje);
            }



            if (www2.isNetworkError)
            {
                Debug.Log("NETWORK ERROR" + www2.error);
            }
            else
            {
                Debug.Log(www2.downloadHandler.text);
                string json2 = www2.downloadHandler.text;
                if (www2.responseCode == 200)
                {
                    CharactersList characters = JsonUtility.FromJson<CharactersList>(json2);
                    StartCoroutine(Dimage(characters.results[OpId[0]].image, Primero));
                    StartCoroutine(Dimage(characters.results[OpId[1]].image, Segundo));
                    StartCoroutine(Dimage(characters.results[OpId[2]].image, Tecero));
                    StartCoroutine(Dimage(characters.results[OpId[3]].image, Cuarto));
                    StartCoroutine(Dimage(characters.results[OpId[4]].image, Quinto));


                }
                else
                {
                    string mensaje = "Status :" + www2.responseCode;
                    mensaje += "/ncontent-type:" + www2.GetResponseHeader("content-type:");
                    mensaje += "/nError :" + www2.error;
                    Debug.Log(mensaje);
                }


            }
        }
    }

    IEnumerator Dimage(string MediaUrl, RawImage Dimage)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
            Dimage.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
    }
}

public class UserData
{
    public int id;
    public string name;
    public int[] deck;
}

[System.Serializable]
public class CharactersList
{
    public CharactersListInfo info;
    public List<Character> results;
}
[System.Serializable]
public class CharactersListInfo
{
    public int count;
    public int pages;
    public string next;
    public string prev;
}
[System.Serializable]
public class Character
{
    public int id;
    public string name;
    public string species;
    public string image;
}

