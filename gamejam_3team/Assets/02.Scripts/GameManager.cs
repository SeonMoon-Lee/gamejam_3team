using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public EndingType endingType = EndingType.Bad;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        { 
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this);

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string name)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    }
}
