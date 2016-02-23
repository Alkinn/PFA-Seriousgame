﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;




public class Validation : MonoBehaviour {

    public FicheXml fiche;

    public menu menu;
    public CanvasGroup canvasGConfirmation;
    public AjoutImage ajt;

    public InputField nomFiche;
    public InputField inputExemple;
    public InputField inputReponse1;
    public InputField inputReponse2;
    public InputField inputReponse3;


    public GameObject saveButton;

    public Toggle toggleRep1;
    public Toggle toggleRep2;
    public Toggle toggleRep3;

    /* Pour le placement de la fenêtre de confirmation */
    Rect windowRect = new Rect((Screen.width / 2) - 100, (Screen.height / 2) - 50, 200, 100);

    bool showConfirmation = false;
    private int topLayerNumber = 0;

    string targetPath;
    string destFile;

    public void validation()
    {

        if (isEverythingFilled())
        {
            fiche.creerDossierFiche();
            fiche.copierImages();
            fiche.genererFiche();
            showConfirmation = true;
        }
        else
            menu.showIncompleteSheetError();

    }

    public void confirmation()
    {
        showConfirmation = false;
    }



	// Use this for initialization
	void Start ()
    {
        targetPath = Application.dataPath + "/../Fiches";

        if (!System.IO.Directory.Exists(targetPath))
            System.IO.Directory.CreateDirectory(targetPath);
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        
        if (ajt.imagePathIndic != null && ajt.imagePathQues!= null)
        {
            saveButton.SetActive(true);
            
        }
        else
        {
            saveButton.SetActive(false);
        }
        
    }

    void DoMyWindow(int windowID)
    {
        if (GUI.Button(new Rect(50, 50, 100, 20), "Ok"))
            this.confirmation();

    }
    bool isEverythingFilled()
    {
        if (nomFiche.text != "" &&
            inputExemple.text != "" &&
            inputReponse1.text != "" &&
            inputReponse2.text != "" &&
            inputReponse3.text != ""
            )
            return true;
        return false;
    }
    
    void OnGUI()
    {
        GUI.depth = topLayerNumber;
        if(showConfirmation)
            windowRect = GUI.Window(0, windowRect, DoMyWindow, "Fiche générée !");
    }
}
