using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class levelSelectButton : MonoBehaviour
{
    // Start is called before the first frame update
    // Start is called before the first frame update
    public int levelNumber=1;
    public Shader shader;
    private Material material;
    private TextMeshProUGUI  textMesh;
    public string levelName;
    public int levelStar=-1;
    public Texture _locked;
    public Texture _0Star;
    public Texture _1Star;
    public Texture _2Star;
    public Texture _3Star;
    public Texture alpha;

    void Start()
    {
        material = new Material(shader);
        GetComponent<Image>().material = material;
        material.SetTexture("_MainTex", GetComponent<Image>().sprite.texture);
        material.SetTexture("_alpha",alpha);
        
        textMesh=GetComponentInChildren<TextMeshProUGUI>();//¹Ø¿¨±àºÅ
        if(textMesh!=null)
            textMesh.text = levelNumber.ToString();

        this.GetComponent<Button>().onClick.AddListener(ChangeScene);
        
        ChangeLeveTexture(levelStar);
    }

    // Update is called once per frame
    void Update()
    {
        if (material != null)
        {
            Vector4 mousePosition = Input.mousePosition;
            material.SetVector("_MousePos", mousePosition);
            //Debug.Log(mousePosition);
        }
        ChangeLeveTexture(levelStar);
        if(levelStar==-1)
        {
            GetComponent<Button>().interactable=false;
        }
        else
        {
            GetComponent<Button>().interactable=true;
        }
    }

    void ChangeScene()
    {
        SceneManager.LoadScene( levelName);
    }

    void ChangeLeveTexture(int levelStar)
    {
        switch (levelStar)
        {
            case -1:
                material.SetTexture("_LevelStatusTex", _locked);
                material.SetInt("_levelStar", -1);
                break;
            case 0:
                material.SetTexture("_LevelStatusTex", _0Star);
                material.SetInt("_levelStar", 0);
                break;
            case 1:
                material.SetTexture("_LevelStatusTex", _1Star);
                material.SetInt("_levelStar", 1);
                break;
            case 2:
                material.SetTexture("_LevelStatusTex", _2Star); 
                material.SetInt("_levelStar", 2);
                break;
            case 3:
                material.SetTexture("_LevelStatusTex", _3Star);
                material.SetInt("_levelStar", 3);
                break;
        }
    }

   
    
}
