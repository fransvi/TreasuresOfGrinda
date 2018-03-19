using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreatorScript : MonoBehaviour {

    private GameManager _gm;
    private int _gender;
    private string _name;
    public Image _cursor;

    public GameObject _inputFieldGo;
    private InputField _inputField;
    public GameObject _mButton;
    public GameObject _fButton;
    public GameObject _confirmButton;

    public Button[] buttons;

    public GameObject[] _characterPreviews;
    private int highlightedButton = 0;



    // Use this for initialization
    void Start () {
        _inputField = _inputFieldGo.GetComponent<InputField>();
        _gender = 0;
	}

    public void SetGender(int i)
    {
        _gender = i;
    }

    public void SetName(string str)
    {
        _name = str;
        highlightedButton += 1;
    }

    void moveCursor(int amount)
    {
        if (highlightedButton + amount < 0)
        {
            highlightedButton = buttons.GetLength(0) - 1;
        }
        else if (highlightedButton + amount > buttons.Length - 1)
        {
            highlightedButton = 0;
        }
        else
        {
            highlightedButton += amount;
        }
    }

    // Update is called once per frame
    void Update () {

        if(_gender == 0)
        {
            _characterPreviews[1].SetActive(false);
            _characterPreviews[0].SetActive(true);
            
        }
        else
        {
            _characterPreviews[0].SetActive(false);
            _characterPreviews[1].SetActive(true);
            
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveCursor(-1);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveCursor(1);
        }

        // kursorin liike
        _cursor.transform.position = new Vector3(buttons[highlightedButton].transform.position.x-50, buttons[highlightedButton].transform.position.y, _cursor.transform.position.z);

        // valinta
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Space))
        {
            buttons[highlightedButton].GetComponent<Button>().onClick.Invoke();
        }

        if(highlightedButton == 0)
        {
            _inputField.ActivateInputField();
        }
        else
        {
            _inputField.DeactivateInputField();
        }




    }

    public void SetGameManager(GameManager g)
    {
        _gm = g;
    }

  
    public void ConfirmSelection()
    {
        _gm.CreateCharacter(_gender, _name);

    }

}
