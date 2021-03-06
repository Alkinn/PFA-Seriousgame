﻿using UnityEngine;
using System.Collections;
using System.Xml.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class OuvrirFicheXml : MonoBehaviour {

    private int topLayerNumber = 0;

    bool updateFile = true; // pour controler le fait que Update() ne recharge pas la fiche continuellement
    bool showFileBrowser = false;
    string xmlFilePath;

    public AjoutImage ajt;
    public Validation vali;
    public menu menu;
    public CanvasGroup canvasOuvrirFiche;
    string cheminFiche;


    XDocument ffs;
    XElement partieQuestion, partieExemple;
    FileBrowser fileBrowser = new FileBrowser();


    /* Paramètres pour le skin de l'explorateur de fichier, les valeurs a modifier sont dans Unity */
    public GUISkin[] skins;
    public Texture2D file, folder, back, drive;
    string[] layoutTypes = { "Type 0", "Type 1" };


    public void openXmlFile()
    {
        showFileBrowser = true;
        updateFile = true;
    }

    void Start()
    {

        /* Initialisation du File Browser */
        fileBrowser.fileTexture = file;
        fileBrowser.directoryTexture = folder;
        fileBrowser.backTexture = back;
        fileBrowser.driveTexture = drive;
        fileBrowser.showSearch = true;
        fileBrowser.searchRecursively = true;
        fileBrowser.guiSkin = skins[0];
    }

    /* Fonction qui récupère les chemin des images */
    void getImagesPath() {

        cheminFiche = System.IO.Path.GetDirectoryName(xmlFilePath);

        ajt.imagePathIndic = System.IO.Path.Combine(cheminFiche, "image_exemple.jpg");
        ajt.imagePathQues = System.IO.Path.Combine(cheminFiche, "image_question.jpg");
    }

    public void readXmlFile()
    {

        InputField[] reponses = new InputField[3];
        reponses[0] = vali.inputReponse1;
        reponses[1] = vali.inputReponse2;
        reponses[2] = vali.inputReponse3;

        

        vali.nomFiche.text = ffs.Root.Element("title").Value;
        vali.intputNumeroFiche.text = ffs.Root.Element("number").Value;
        vali.inputExemple.text = ffs.Root.Element("ExamplePart").Element("text").Value;
        vali.inputReponse1.text = ffs.Root.Element("QuestionPart").Element("answer1").Value;
        vali.inputReponse2.text = ffs.Root.Element("QuestionPart").Element("answer2").Value;
        vali.inputReponse3.text = ffs.Root.Element("QuestionPart").Element("answer3").Value;

        if (ffs.Root.Element("QuestionPart").Element("answer1").Attribute("value").ToString().Equals("value=\"true\""))
            vali.toggleRep1.isOn = true;
        else if (ffs.Root.Element("QuestionPart").Element("answer2").Attribute("value").ToString() == "value=\"true\"")
            vali.toggleRep2.isOn = true;
        else
            vali.toggleRep3.isOn = true;


        getImagesPath();

        ajt.img_indication = ajt.LoadPNG(ajt.imagePathIndic);
        ajt.rawImageExemple.texture = ajt.img_indication;

        ajt.img_question = ajt.LoadPNG(ajt.imagePathQues);
        ajt.rawImageQuestion.texture = ajt.img_question;

        updateFile = false;
    }

    public bool isSearchingFile()
    {
        return showFileBrowser;
    }

    public void desactiverBouttonOuvrir()
    {
        canvasOuvrirFiche.interactable = false;
    }

    public void activerBouttonOuvrir()
    {
        canvasOuvrirFiche.interactable = true;
    }

    void Update()
    {
        if (xmlFilePath != null && updateFile == true)
        {
            try {
                ffs = XDocument.Load(xmlFilePath);
                this.readXmlFile();
            }
            catch(System.Xml.XmlException)
            {
                menu.showOpenSheetError();
                xmlFilePath = null;
                updateFile = false;
            }
        }

        if (showFileBrowser)
            ajt.desactiverBouttonAjout();
        else
            ajt.activerBouttonAjout();
   
    }


    void OnGUI()
    {
        GUI.depth = topLayerNumber;
        if (showFileBrowser)
        {

            /*
             * Pour gérer les skins/ modes de vues
             */
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            GUILayout.Label("Layout Type");
            fileBrowser.setLayout(GUILayout.SelectionGrid(fileBrowser.layoutType, layoutTypes, 1));
            GUILayout.Space(10);
            //select from available gui skins
            GUILayout.Label("GUISkin");
            foreach (GUISkin s in skins)
            {
                if (GUILayout.Button(s.name))
                {
                    fileBrowser.guiSkin = s;
                }
            }
            GUILayout.Space(10);

            /* Récupération du chemin vers la fiche Xml */
            if (fileBrowser.draw())
            {
                if (fileBrowser.outputFile == null)
                {
                    showFileBrowser = false;
                }

                else
                {
                    xmlFilePath = fileBrowser.outputFile.FullName﻿.ToString();
                    showFileBrowser = false;
                }

            }
        }
    }


}
