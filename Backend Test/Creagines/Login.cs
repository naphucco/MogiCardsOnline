using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Login : MonoBehaviour {

    public string username;
    public string password;

    string loginURL = "http://128.199.122.20/Login.php";

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.L)) StartCoroutine(LoginToDB(username, password));
	}

    IEnumerator LoginToDB(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("passwordPost", password);

        WWW www = new WWW(loginURL, form);

        yield return www;

        Debug.Log(www.text);
    }
}
