using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    // Use this for initialization
    public Transform inventory;
    public GameObject player;
    public static GameManager gm;
    public static Transform pmenu;
    public GameObject ui;
    public GameObject mobileControls;
    public GameObject pcControls;
    public GameObject menu;
    public GameObject playerSpawnPoint;
    public GameObject ccmenu;
    public GameObject gameOverScreen;
    public GameObject playerCloneInstance;
    public int MOBILEVERSION;

    public int selectedControls;
    //Data for saving
    public int playerHealth;
    public int playerMainWeapon;
    public int playerOffWeapon;
    public bool playerHasPotion;
    public float playerGold;
    public String playername;
    public int playergender;
    void Awake()
    {
        if(gm == null)
        {
            DontDestroyOnLoad(gameObject);
            gm = this;
            inventory = transform.Find("PlayerManager");
            LoadData();
        }
        else if(gm != this)
        {
            inventory = transform.Find("PlayerManager");
            Destroy(gameObject);
        }
        LoadLevel1_2(true);


    }

    public void LoadPlayer()
    {

        Debug.Log("PlayerGender" + playergender);
        playerSpawnPoint = GameObject.Find("PlayerSpawnPoint");
        ui = GameObject.Find("UI");
        GameObject playerClone = Instantiate(player, playerSpawnPoint.transform.position, playerSpawnPoint.transform
            .rotation);
        playerClone.GetComponent<PlayerController>().SetHealth(20);
        playerClone.GetComponent<PlayerController>().SetSelectedControls(selectedControls);
        playerClone.GetComponent<PlayerController>().SetGender(playergender);
        playerCloneInstance = playerClone;
        inventory.gameObject.GetComponent<PlayerInventory>().SetPlayer(playerClone);
        ui.GetComponent<HudManager>().SetPlayerStats(playerClone, inventory.gameObject);
        ui.GetComponent<HudManager>().SetControlState(selectedControls);
        mobileControls = GameObject.Find("MobileControls");
        pcControls = GameObject.Find("PcControls");
        if (MOBILEVERSION == 1)
        {

            mobileControls.SetActive(true);
            pcControls.SetActive(false);
            ui.GetComponent<HudManager>().SetMobileControls(true);
            mobileControls = ui.GetComponentInChildren<MobileControlsScript>().gameObject;
            playerClone.GetComponent<PlayerController>().SetMobileVersion(true);
            playerClone.GetComponent<PlayerController>().SetJoystick(mobileControls.GetComponent<MobileControlsScript>().joystick.GetComponent<VirtualJoystick>().gameObject);

        }
        else
        {
            ui.GetComponent<HudManager>().SetMobileControls(false);
            playerClone.GetComponent<PlayerController>().SetMobileVersion(false);
            mobileControls.SetActive(false);
            pcControls.SetActive(true);
        }


    }

    public void SetControls(int i)
    {
        selectedControls = i;
    }
    private string SaveFilePath
    {
        get { return Application.persistentDataPath + "/playerInfo.dat"; }
    }

    public void ResetSaveData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        PlayerData data = new PlayerData();

        data.playerMainWeapon = 0;
        data.playerOffWeapon = 0;
        data.playerGold = 0;
        data.playergender = 0;
        data.playerhaspotion = false;
        data.playername = " ";

        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Playerdata reset");

        LoadData();
        StartCoroutine(BackToCharacterGeneration());
    }
    public void SaveData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        PlayerData data = new PlayerData();

        data.playerMainWeapon = inventory.gameObject.GetComponent<PlayerInventory>().GetCurrentMainWeapon();
        data.playerOffWeapon = inventory.gameObject.GetComponent<PlayerInventory>().GetCurrentOffWeapon();
        data.playerGold = inventory.gameObject.GetComponent<PlayerInventory>().GetCurrentGold();
        data.playername = playername;
        data.playergender = playergender;
        data.playerhaspotion = inventory.gameObject.GetComponent<PlayerInventory>().getHasPotion();

        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Playerdata saved " + playerMainWeapon + " " + playerOffWeapon + " " + playerGold);
    }
    public void LoadData()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            playerMainWeapon = data.playerMainWeapon;
            playerOffWeapon = data.playerOffWeapon;
            playerGold = data.playerGold;
            playername = data.playername;
            playergender = data.playergender;
            playerHasPotion = data.playerhaspotion;

            inventory.gameObject.GetComponent<PlayerInventory>().SetCurrentMainWeapon(playerMainWeapon);
            inventory.gameObject.GetComponent<PlayerInventory>().SetCurrentOffWeapon(playerOffWeapon);
            inventory.gameObject.GetComponent<PlayerInventory>().SetCurrentGold(playerGold);
            inventory.gameObject.GetComponent<PlayerInventory>().SetHasPotion(playerHasPotion);
            Debug.Log("Playerdata loaded "+ playerMainWeapon + " " + playerOffWeapon+ " " +playerGold);
        }
    }

    public void LoadGameOverScreen()
    {
        gameOverScreen = GameObject.Find("GameOverPrefab");
        gameOverScreen.GetComponent<GameOver>().canvas.SetActive(true);

    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    //Wait on scene change and initialize different scenes
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "CharacterCreation")
        {
            ccmenu = GameObject.Find("CCCanvas");
            ccmenu.GetComponent<CharacterCreatorScript>().SetGameManager(this);
        }
        else if(scene.name == "GameMenu")
        {
            menu = GameObject.Find("GameMenuCanvas");
            menu.GetComponent<ButtonManage>().SetGameManager(this);
            SaveData();
        }
        else if(scene.name != "GameMenu" && scene.name != "GameBoot" && scene.name != "CharacterCreation")
        {
            Debug.Log("Trying to load player");
            LoadPlayer();
        }
        else
        {
        }
        Debug.Log("Scene Loaded: " + scene.name + " " + mode);
        inventory = transform.Find("PlayerManager");
        gm = this;
    }

    public void LoadLevel1_2(bool firstLoad)
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat") && firstLoad){
            StartCoroutine(ChangeLevel(2));
        }
        else
        {
            StartCoroutine(ChangeLevel(0));
        }      
 
    }
    public void LoadLevelInt(int num)
    {
        StartCoroutine(ChangeLevelNumber(num));
    }
    IEnumerator ChangeLevelNumber(int num)
    {
        float fadeTime = GetComponent<AutoFade>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        Application.LoadLevel(num);
    }
    IEnumerator ChangeLevel(int skip)
    {
        float fadeTime = GetComponent<AutoFade>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        Application.LoadLevel(Application.loadedLevel + 1 + skip);
    }
    IEnumerator BackToCharacterGeneration()
    {
        float fadeTime = GetComponent<AutoFade>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        Application.LoadLevel(Application.loadedLevel -1);
    }

    public void LoadMenu()
    {
        Application.LoadLevel("GameMenu");
    }

    public void CreateCharacter(int i, String name)
    {
        playergender = i;
        playername = name;
        StartCoroutine(ChangeLevel(0));
    }
    public void MobileJumpDown()
    {
        playerCloneInstance.GetComponent<PlayerController>().MobileJumpDown();
    }
    public void MobileJumpUp()
    {
        playerCloneInstance.GetComponent<PlayerController>().MobileJumpUp();
    }
    public void MobileMainDown()
    {
        playerCloneInstance.GetComponent<PlayerController>().MobileMainDown();
    }
    public void MobileOffDown()
    {
        playerCloneInstance.GetComponent<PlayerController>().MobileOffDown();
    }
    public void MobileOffUp()
    {
        playerCloneInstance.GetComponent<PlayerController>().MobileOffUp();
    }
    public void MobileConsDown()
    {
        playerCloneInstance.GetComponent<PlayerController>().MobileConsumableDown();
    }
}


//Serialized class for saved player data
[Serializable]
class PlayerData
{
    public int playerMainWeapon;
    public int playerOffWeapon;
    public float playerGold;
    public String playername;
    public int playergender;
    public bool playerhaspotion;
    //Cosmetic itemit
}