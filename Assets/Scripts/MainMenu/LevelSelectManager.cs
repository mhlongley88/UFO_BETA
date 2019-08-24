using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelSelectManager : MonoBehaviour
{

    public GameObject characterRot;
    public GameObject CameraRot;

    public static int levelStaticInt;

    private int levelInt;

    private bool inLA;
    private bool inSF;
    private bool inLV;
    private bool inHI;
    private bool inMXC;
    private bool inESTI;
    private bool inRIO;
    private bool inWSHDC;
    private bool inNY;
    private bool inLNDN;
    private bool inPRS;
    private bool inROME;
    private bool inEGYPT;
    private bool inTJML;
    private bool inCHINA;
    private bool inTKYO;

    // Start is called before the first frame update
    void Start()
    {
        levelInt = 1;   
    }

    // Update is called once per frame
    void Update()
    {

        levelStaticInt = levelInt;

        Debug.Log(levelStaticInt);

        //levelInt = 0;

        if (levelInt == 1)
        {
            inLA = true;
        }

        if(inLA == true)
        {
            characterRot.GetComponent<DOTweenAnimation>().DOPlayAllById("toLA");
            CameraRot.GetComponent<DOTweenAnimation>().DOPlayAllById("toLA");
        }
        if(inLA == true && Input.GetKeyDown(KeyCode.UpArrow)){

            NextLevel("toSF", "toLA");

            levelInt = 2;

            inLA = false;
            inSF = true;

        }
        else if(inLA == true && Input.GetKeyDown(KeyCode.DownArrow))
        {
            NextLevel("toMXC", "toLA");

            levelInt = 3;

            inLA = false;
            inMXC = true;

        }
        else if(inLA == true && Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextLevel("toLV", "toLA");

            levelInt = 6;
            inLA = false;
            inLV = true;
        }
        else if (inLA == true && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            NextLevel("toHI", "toLA");

            levelInt = 16;
            inLA = false;
            inHI = true;
        }

        if(inSF == true && Input.GetKeyDown(KeyCode.RightArrow))
        {

            NextLevel("toLV", "toSF");

            levelInt = 6;
            inSF = false;
            inLV = true;
        }

            if (inLV == true && Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextLevel("toWSHDC", "toLV");

            levelInt = 7;

            inLV = false;
            inWSHDC = true;
        }

        if (inMXC == true && Input.GetKeyDown(KeyCode.DownArrow))
        {
            NextLevel("toESTI", "toMXC");

            levelInt = 4;

            inMXC = false;
            inESTI = true;

        } else if (inMXC == true && Input.GetKeyDown(KeyCode.UpArrow))
        {
            NextLevel("toLA", "toMXC");

            levelInt = 1;
            inMXC = false;
            inLA = true;
        }

        if (inESTI == true && Input.GetKeyDown(KeyCode.DownArrow) || inESTI == true && Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextLevel("toRIO", "toESTI");

            levelInt = 5;

            inESTI = false;
            inRIO = true;

        } else if(inESTI == true && Input.GetKeyDown(KeyCode.UpArrow))
        {

            NextLevel("toMXC", "toESTI");
            levelInt = 3;
        }

        if(inRIO == true && Input.GetKeyDown(KeyCode.UpArrow))
        {
            NextLevel("toWSHDC", "toRIO");

            levelInt = 7;

            inRIO = false;
            inWSHDC = true;
        }
        else if(inRIO == true && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            NextLevel("toESTI", "toRIO");

            levelInt = 4;
            inRIO = false;
            inESTI = true;
        }
        if(inWSHDC == true && Input.GetKeyDown(KeyCode.UpArrow) || inWSHDC == true && Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextLevel("toNY", "toWSHDC");

            levelInt = 8;
            inWSHDC = false;
            inNY = true;
        } else if(inWSHDC == true && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            NextLevel("toLV", "toWSHDC");
        }

        if(inNY == true && Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextLevel("toLNDN", "toNY");
            levelInt = 9;
            inNY = false;
            inLNDN = true;
        }

        if (inLNDN == true && Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextLevel("toPRS", "toLNDN");
            levelInt = 10;
            inLNDN = false;
            inPRS = true;
        }
        else if (inLNDN == true && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            NextLevel("toNY", "toLNDN");
            levelInt = 8;
            inLNDN = false;
            inNY = true;
        }

            if (inPRS == true && Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextLevel("toROME", "toPRS");
            levelInt = 11;
            inPRS = false;
            inROME = true;
        }
        else if (inPRS == true && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            NextLevel("toLNDN", "toPRS");
            levelInt = 9;
            inPRS = false;
            inLNDN = true;
        }

        if (inROME == true && Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextLevel("toEGYPT", "toRome");
            levelInt = 12;
            inROME = false;
            inEGYPT = true;
        }
        else if (inROME == true && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            NextLevel("toPRS", "toROME");
            levelInt = 10;
            inROME = false;
            inPRS = true;
        }

        if (inEGYPT == true && Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextLevel("toTJML", "toEGYPT");
            levelInt = 13;
            inEGYPT = false;
            inTJML = true;
        }
        else if (inEGYPT == true && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            NextLevel("toROME", "toEGYPT");
            levelInt = 11;
            inEGYPT = false;
            inROME = true;
        }

        if (inTJML == true && Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextLevel("toCHINA", "toTJML");
            levelInt = 14;
            inTJML = false;
            inCHINA = true;
        }
        else if (inTJML == true && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            NextLevel("toEGYPT", "toTJML");
            levelInt = 12;
            inTJML = false;
            inEGYPT = true;
        }

        if (inCHINA == true && Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextLevel("toTKYO", "toCHINA");
            levelInt = 15;
            inCHINA = false;
            inTKYO = true;
        }
        else if (inCHINA == true && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            NextLevel("toTJML", "toCHINA");
            levelInt = 13;
            inCHINA = false;
            inTJML = true;
        }

        if (inTKYO == true && Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextLevel("toHI", "toTKYO");
            levelInt = 16;
            inTKYO = false;
            inHI = true;
        }
        else if (inTKYO == true && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            NextLevel("toCHINA", "toTKYO");
            levelInt = 14;
            inTKYO = false;
            inCHINA = true;
        }

    }

    private void NextLevel(string nextLevel, string prevLevel)

    {

        characterRot.GetComponent<DOTweenAnimation>().DOPlayAllById(nextLevel);
        CameraRot.GetComponent<DOTweenAnimation>().DOPlayAllById(nextLevel);

        characterRot.GetComponent<DOTweenAnimation>().DORewindAllById(prevLevel);
        CameraRot.GetComponent<DOTweenAnimation>().DORewindAllById(prevLevel);
    }

}
