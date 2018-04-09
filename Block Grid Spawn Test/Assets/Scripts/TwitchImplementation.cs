using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TwitchUser
{
    public string streamerInQuestion { get; set; }

    public string Username { get; set; }
    public string AccessToken { get; set; }
    public bool Follows = false;
    public bool Subs = false;
    public bool Mod = false;
    public List<string> StreamersFollowed;

    public TwitchUser()
    {

    }

    public TwitchUser(string username, string accesstoken, bool follows, bool subs, bool mod, List<string> streamersFollowed)
    {
        this.Username = username;
        this.AccessToken = accesstoken;
        this.Follows = follows;
        this.Subs = subs;
        this.Mod = mod;
        StreamersFollowed.AddRange(streamersFollowed);
    }
}

public class TwitchImplementation : MonoBehaviour 
{
    #region Global Variable Declaration

    public string streamer = "xxxxxxxx";

	#endregion

	void Awake () 
	{
		
	}
	void Start () 
	{
		
	}	

    public void OnButtonClicked()
    {
        TwitchUser user = GetTwitchUserLogin();

        if (user.AccessToken != string.Empty)
        {
            // next check to see if this user
            // has "x streamer" in their StreamersFollowed List

            if (user.Follows)
            {
                SceneManager.LoadSceneAsync("");
            }
            else
            {
                // you need to be a follower of "x streamer" to continue
                // would you like to follow "x streamer to continue"
            }
        }
        else
        {
            // sorry, we couldn't get you authenticated
        }
    }

    // takes in the streamer we want to check for after we authenticate
    TwitchUser GetTwitchUserLogin()
    {
        // implement twitch api authentication calls here 
        bool hasAccount = false;

        // checks whether or not they have an account already

        if(hasAccount)
        {
            // use the one with all of the information necessary user account information
            return new TwitchUser();
        }
        else
        {
            return new TwitchUser();
            // display a message saying you need a twitch account to continue
        }
    }

}
