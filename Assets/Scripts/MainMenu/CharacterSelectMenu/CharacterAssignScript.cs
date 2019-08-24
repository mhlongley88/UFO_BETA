using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

public class CharacterAssignScript : MonoBehaviour
{

    /*  public Texture ch0_1_Tex;
      public Texture ch0_2_Tex;
      public Texture ch0_3_Tex;
      public Texture ch0_4_Tex;*/

    public Texture ch1_Tex;
    public Texture ch2_Tex;
    public Texture ch3_Tex;
    public Texture ch4_Tex;
    public Texture ch5_Tex;
    public Texture ch6_Tex;
    public Texture ch7_Tex;
    public Texture ch8_Tex;
    public Texture ch9_Tex;
    public Texture ch10_Tex;
    public Texture ch11_Tex;
    public Texture ch12_Tex;
    public Texture ch13_Tex;
    public Texture ch14_Tex;
    public Texture ch15_Tex;

    public Texture ch1_BodyTex;
    public Texture ch2_BodyTex;

    /* public GameObject character0_1;
     public GameObject character0_2;
     public GameObject character0_3;
     public GameObject character0_4;*/
    public GameObject character1;
    public GameObject character2;
    public GameObject character3;
    public GameObject character4;
    public GameObject character5;
    public GameObject character6;
    public GameObject character7;
    public GameObject character8;
    public GameObject character9;
    public GameObject character10;
    public GameObject character11;
    public GameObject character12;
    public GameObject character13;
    public GameObject character14;
    public GameObject character15;

    public Material myMaterial;
    public Material myOtherMaterial;

    public Material bodyMaterial;

    public GameObject Weapon;
    public GameObject SuperWeapon;

    public Renderer currRenderer;

    public Player PlayerNum;

    //  public bool forInGame;

    private int currChar;
    private int currWeapon;

    // Start is called before the first frame update
    private void Awake()
    {
        /* if(forInGame == true)
         {
             CharacterAssign();
         }*/
    }
    //     void Update()
    //     {

    // {
    //             currChar = GameManagerScript.Instance.;
    //             currWeapon = CharacterSelect.p1CharStaticInt;
    //         }
    //         else if (PlayerNum == 2)
    //         {
    //             currChar = CharacterSelect.p2CharStaticInt;
    //             currWeapon = CharacterSelect.p2CharStaticInt;
    //         }
    //         else if (PlayerNum == 3)
    //         {
    //             currChar = CharacterSelect.p3CharStaticInt;
    //             currWeapon = CharacterSelect.p3CharStaticInt;
    //         }
    //         else if (PlayerNum == 4)
    //         {
    //             currChar = CharacterSelect.p4CharStaticInt;
    //             currWeapon = CharacterSelect.p4CharStaticInt;
    //         }

    //         if (currChar == 1)
    //         {

    //             /*  character0_1.SetActive(false);
    //               character0_2.SetActive(false);
    //               character0_3.SetActive(false);
    //               character0_4.SetActive(false);*/

    //             character1.SetActive(true);
    //             currRenderer.material.SetTexture("_MainTex", ch1_Tex);
    //             Weapon.GetComponent<NormalWeapon>().ChangeWeapon(global::Weapon.WeaponTypes.Canon);
    //             SuperWeapon.GetComponent<SuperWeapon>().ChangeWeapon(global::Weapon.WeaponTypes.Canon);

    //             character2.SetActive(false);
    //             character3.SetActive(false);
    //             character4.SetActive(false);
    //             character5.SetActive(false);
    //             character6.SetActive(false);
    //             character7.SetActive(false);
    //             character8.SetActive(false);
    //             character9.SetActive(false);
    //             character10.SetActive(false);
    //             character11.SetActive(false);
    //             character12.SetActive(false);
    //             character13.SetActive(false);
    //             character14.SetActive(false);
    //             character15.SetActive(false);

    //             myMaterial.SetTexture("_MainTex", ch1_Tex);
    //             myOtherMaterial.SetTexture("_MainTex", ch1_Tex);

    //             bodyMaterial.SetTexture("_MainTex", ch1_BodyTex);

    //             // Debug.Log(p1CharStaticInt);

    //         }

    //         if (currChar == 2)
    //         {

    //             /*  character0_1.SetActive(false);
    //   character0_2.SetActive(false);
    //   character0_3.SetActive(false);
    //   character0_4.SetActive(false);*/

    //             character1.SetActive(false);

    //             character2.SetActive(true);
    //             currRenderer.material.SetTexture("_MainTex", ch2_Tex);
    //             Weapon.GetComponent<NormalWeapon>().ChangeWeapon(global::Weapon.WeaponTypes.Shotgun);
    //             SuperWeapon.GetComponent<SuperWeapon>().ChangeWeapon(global::Weapon.WeaponTypes.Shotgun);

    //             character3.SetActive(false);
    //             character4.SetActive(false);
    //             character5.SetActive(false);
    //             character6.SetActive(false);
    //             character7.SetActive(false);
    //             character8.SetActive(false);
    //             character9.SetActive(false);
    //             character10.SetActive(false);
    //             character11.SetActive(false);
    //             character12.SetActive(false);
    //             character13.SetActive(false);
    //             character14.SetActive(false);
    //             character15.SetActive(false);

    //             myMaterial.SetTexture("_MainTex", ch2_Tex);
    //             myOtherMaterial.SetTexture("_MainTex", ch2_Tex);

    //             bodyMaterial.SetTexture("_MainTex", ch2_BodyTex);

    //             //Debug.Log(p1CharStaticInt);

    //         }

    //         if (currChar == 3)
    //         {


    //             /*  character0_1.SetActive(false);
    //   character0_2.SetActive(false);
    //   character0_3.SetActive(false);
    //   character0_4.SetActive(false);*/

    //             character1.SetActive(false);

    //             character2.SetActive(false);

    //             character3.SetActive(true);
    //             currRenderer.material.SetTexture("_MainTex", ch3_Tex);
    //             Weapon.GetComponent<NormalWeapon>().ChangeWeapon(global::Weapon.WeaponTypes.DualPistols);
    //             SuperWeapon.GetComponent<SuperWeapon>().ChangeWeapon(global::Weapon.WeaponTypes.DualPistols);


    //             character4.SetActive(false);
    //             character5.SetActive(false);
    //             character6.SetActive(false);
    //             character7.SetActive(false);
    //             character8.SetActive(false);
    //             character9.SetActive(false);
    //             character10.SetActive(false);
    //             character11.SetActive(false);
    //             character12.SetActive(false);
    //             character13.SetActive(false);
    //             character14.SetActive(false);
    //             character15.SetActive(false);

    //             myMaterial.SetTexture("_MainTex", ch3_Tex);
    //             myOtherMaterial.SetTexture("_MainTex", ch3_Tex);

    //             //Debug.Log("Character 3");

    //         }

    //         if (currChar == 4)
    //         {

    //             /*  character0_1.SetActive(false);
    //   character0_2.SetActive(false);
    //   character0_3.SetActive(false);
    //   character0_4.SetActive(false);*/

    //             character1.SetActive(false);

    //             character2.SetActive(false);

    //             character3.SetActive(false);


    //             character4.SetActive(true);
    //             currRenderer.material.SetTexture("_MainTex", ch4_Tex);
    //             Weapon.GetComponent<NormalWeapon>().ChangeWeapon(global::Weapon.WeaponTypes.SemiAuto);
    //             SuperWeapon.GetComponent<SuperWeapon>().ChangeWeapon(global::Weapon.WeaponTypes.SemiAuto);

    //             character5.SetActive(false);
    //             character6.SetActive(false);
    //             character7.SetActive(false);
    //             character8.SetActive(false);
    //             character9.SetActive(false);
    //             character10.SetActive(false);
    //             character11.SetActive(false);
    //             character12.SetActive(false);
    //             character13.SetActive(false);
    //             character14.SetActive(false);
    //             character15.SetActive(false);

    //             myMaterial.SetTexture("_MainTex", ch4_Tex);
    //             myOtherMaterial.SetTexture("_MainTex", ch4_Tex);

    //             //Debug.Log("Character 4");

    //         }

    //         if (currChar == 5)
    //         {

    //             /*  character0_1.SetActive(false);
    //   character0_2.SetActive(false);
    //   character0_3.SetActive(false);
    //   character0_4.SetActive(false);*/

    //             character1.SetActive(false);

    //             character2.SetActive(false);

    //             character3.SetActive(false);


    //             character4.SetActive(false);
    //             currRenderer = character5.GetComponentInChildren<Renderer>();
    //             currRenderer.material.SetTexture("_MainTex", ch5_Tex);
    //             Weapon.GetComponent<NormalWeapon>().ChangeWeapon(global::Weapon.WeaponTypes.LaserSpear);

    //             character5.SetActive(true);
    //             character6.SetActive(false);
    //             character7.SetActive(false);
    //             character8.SetActive(false);
    //             character9.SetActive(false);
    //             character10.SetActive(false);
    //             character11.SetActive(false);
    //             character12.SetActive(false);
    //             character13.SetActive(false);
    //             character14.SetActive(false);
    //             character15.SetActive(false);

    //             myMaterial.SetTexture("_MainTex", ch5_Tex);
    //             myOtherMaterial.SetTexture("_MainTex", ch5_Tex);

    //             //Debug.Log("Character 5");

    //         }

    //         if (currChar == 6)
    //         {

    //             /*  character0_1.SetActive(false);
    //   character0_2.SetActive(false);
    //   character0_3.SetActive(false);
    //   character0_4.SetActive(false);*/

    //             character1.SetActive(false);

    //             character2.SetActive(false);

    //             character3.SetActive(false);


    //             character4.SetActive(false);
    //             currRenderer = character6.GetComponentInChildren<Renderer>();
    //             currRenderer.material.SetTexture("_MainTex", ch6_Tex);
    //             Weapon.GetComponent<NormalWeapon>().ChangeWeapon(global::Weapon.WeaponTypes.LaserSpear);

    //             character5.SetActive(false);
    //             character6.SetActive(true);
    //             character7.SetActive(false);
    //             character8.SetActive(false);
    //             character9.SetActive(false);
    //             character10.SetActive(false);
    //             character11.SetActive(false);
    //             character12.SetActive(false);
    //             character13.SetActive(false);
    //             character14.SetActive(false);
    //             character15.SetActive(false);

    //             myMaterial.SetTexture("_MainTex", ch6_Tex);
    //             myOtherMaterial.SetTexture("_MainTex", ch6_Tex);

    //             //Debug.Log("Character 6");

    //         }

    //         if (currChar == 7)
    //         {
    //             /*  character0_1.SetActive(false);
    //   character0_2.SetActive(false);
    //   character0_3.SetActive(false);
    //   character0_4.SetActive(false);*/

    //             character1.SetActive(false);

    //             character2.SetActive(false);

    //             character3.SetActive(false);


    //             character4.SetActive(false);
    //             currRenderer = character7.GetComponentInChildren<Renderer>();
    //             currRenderer.material.SetTexture("_MainTex", ch7_Tex);

    //             character5.SetActive(false);
    //             character6.SetActive(false);
    //             character7.SetActive(true);
    //             character8.SetActive(false);
    //             character9.SetActive(false);
    //             character10.SetActive(false);
    //             character11.SetActive(false);
    //             character12.SetActive(false);
    //             character13.SetActive(false);
    //             character14.SetActive(false);
    //             character15.SetActive(false);

    //             myMaterial.SetTexture("_MainTex", ch7_Tex);
    //             myOtherMaterial.SetTexture("_MainTex", ch7_Tex);

    //             //Debug.Log("Character 7");

    //         }

    //         if (currChar == 8)
    //         {

    //             /*  character0_1.SetActive(false);
    //   character0_2.SetActive(false);
    //   character0_3.SetActive(false);
    //   character0_4.SetActive(false);*/

    //             character1.SetActive(false);

    //             character2.SetActive(false);

    //             character3.SetActive(false);


    //             character4.SetActive(false);
    //             currRenderer = character8.GetComponentInChildren<Renderer>();
    //             currRenderer.material.SetTexture("_MainTex", ch8_Tex);

    //             character5.SetActive(false);
    //             character6.SetActive(false);
    //             character7.SetActive(false);
    //             character8.SetActive(true);
    //             character9.SetActive(false);
    //             character10.SetActive(false);
    //             character11.SetActive(false);
    //             character12.SetActive(false);
    //             character13.SetActive(false);
    //             character14.SetActive(false);
    //             character15.SetActive(false);

    //             myMaterial.SetTexture("_MainTex", ch8_Tex);
    //             myOtherMaterial.SetTexture("_MainTex", ch8_Tex);

    //             //Debug.Log("Character 8");

    //         }

    //         if (currChar == 9)
    //         {

    //             /*  character0_1.SetActive(false);
    //   character0_2.SetActive(false);
    //   character0_3.SetActive(false);
    //   character0_4.SetActive(false);*/

    //             character1.SetActive(false);

    //             character2.SetActive(false);

    //             character3.SetActive(false);


    //             character4.SetActive(false);
    //             currRenderer = character9.GetComponentInChildren<Renderer>();
    //             currRenderer.material.SetTexture("_MainTex", ch9_Tex);

    //             character5.SetActive(false);
    //             character6.SetActive(false);
    //             character7.SetActive(false);
    //             character8.SetActive(false);
    //             character9.SetActive(true);
    //             character10.SetActive(false);
    //             character11.SetActive(false);
    //             character12.SetActive(false);
    //             character13.SetActive(false);
    //             character14.SetActive(false);
    //             character15.SetActive(false);

    //             myMaterial.SetTexture("_MainTex", ch9_Tex);
    //             myOtherMaterial.SetTexture("_MainTex", ch9_Tex);

    //             //Debug.Log("Character 9");

    //         }

    //         if (currChar == 10)
    //         {
    //             /*  character0_1.SetActive(false);
    //   character0_2.SetActive(false);
    //   character0_3.SetActive(false);
    //   character0_4.SetActive(false);*/


    //             character1.SetActive(false);

    //             character2.SetActive(false);

    //             character3.SetActive(false);


    //             character4.SetActive(false);
    //             currRenderer = character10.GetComponentInChildren<Renderer>();
    //             currRenderer.material.SetTexture("_MainTex", ch10_Tex);

    //             character5.SetActive(false);
    //             character6.SetActive(false);
    //             character7.SetActive(false);
    //             character8.SetActive(false);
    //             character9.SetActive(false);
    //             character10.SetActive(true);
    //             character11.SetActive(false);
    //             character12.SetActive(false);
    //             character13.SetActive(false);
    //             character14.SetActive(false);
    //             character15.SetActive(false);

    //             myMaterial.SetTexture("_MainTex", ch10_Tex);
    //             myOtherMaterial.SetTexture("_MainTex", ch10_Tex);

    //             //Debug.Log("Character 10");


    //         }

    //         if (currChar == 11)
    //         {

    //             /*  character0_1.SetActive(false);
    //   character0_2.SetActive(false);
    //   character0_3.SetActive(false);
    //   character0_4.SetActive(false);*/

    //             character1.SetActive(false);

    //             character2.SetActive(false);

    //             character3.SetActive(false);


    //             character4.SetActive(false);
    //             currRenderer = character11.GetComponentInChildren<Renderer>();
    //             currRenderer.material.SetTexture("_MainTex", ch11_Tex);

    //             character5.SetActive(false);
    //             character6.SetActive(false);
    //             character7.SetActive(false);
    //             character8.SetActive(false);
    //             character9.SetActive(false);
    //             character10.SetActive(false);
    //             character11.SetActive(true);
    //             character12.SetActive(false);
    //             character13.SetActive(false);
    //             character14.SetActive(false);
    //             character15.SetActive(false);

    //             myMaterial.SetTexture("_MainTex", ch11_Tex);
    //             myOtherMaterial.SetTexture("_MainTex", ch11_Tex);

    //             //Debug.Log("Character 11");

    //         }

    //         if (currChar == 12)
    //         {

    //             /*  character0_1.SetActive(false);
    //   character0_2.SetActive(false);
    //   character0_3.SetActive(false);
    //   character0_4.SetActive(false);*/

    //             character1.SetActive(false);

    //             character2.SetActive(false);

    //             character3.SetActive(false);


    //             character4.SetActive(false);
    //             currRenderer = character12.GetComponentInChildren<Renderer>();
    //             currRenderer.material.SetTexture("_MainTex", ch12_Tex);

    //             character5.SetActive(false);
    //             character6.SetActive(false);
    //             character7.SetActive(false);
    //             character8.SetActive(false);
    //             character9.SetActive(false);
    //             character10.SetActive(false);
    //             character11.SetActive(false);
    //             character12.SetActive(true);
    //             character13.SetActive(false);
    //             character14.SetActive(false);
    //             character15.SetActive(false);

    //             myMaterial.SetTexture("_MainTex", ch12_Tex);
    //             myOtherMaterial.SetTexture("_MainTex", ch12_Tex);

    //             //Debug.Log("Character 12");

    //         }

    //         if (currChar == 13)
    //         {

    //             /*  character0_1.SetActive(false);
    //               character0_2.SetActive(false);
    //               character0_3.SetActive(false);
    //               character0_4.SetActive(false);*/

    //             character1.SetActive(false);

    //             character2.SetActive(false);

    //             character3.SetActive(false);


    //             character4.SetActive(false);
    //             currRenderer = character13.GetComponentInChildren<Renderer>();
    //             currRenderer.material.SetTexture("_MainTex", ch13_Tex);

    //             character5.SetActive(false);
    //             character6.SetActive(false);
    //             character7.SetActive(false);
    //             character8.SetActive(false);
    //             character9.SetActive(false);
    //             character10.SetActive(false);
    //             character11.SetActive(false);
    //             character12.SetActive(false);
    //             character13.SetActive(true);
    //             character14.SetActive(false);
    //             character15.SetActive(false);

    //             myMaterial.SetTexture("_MainTex", ch13_Tex);
    //             myOtherMaterial.SetTexture("_MainTex", ch13_Tex);

    //             //Debug.Log("Character 13");


    //         }

    //         if (currChar == 14)
    //         {

    //             /*  character0_1.SetActive(false);
    //               character0_2.SetActive(false);
    //               character0_3.SetActive(false);
    //               character0_4.SetActive(false);*/

    //             character1.SetActive(false);

    //             character2.SetActive(false);

    //             character3.SetActive(false);


    //             character4.SetActive(false);
    //             currRenderer = character14.GetComponentInChildren<Renderer>();
    //             currRenderer.material.SetTexture("_MainTex", ch14_Tex);

    //             character5.SetActive(false);
    //             character6.SetActive(false);
    //             character7.SetActive(false);
    //             character8.SetActive(false);
    //             character9.SetActive(false);
    //             character10.SetActive(false);
    //             character11.SetActive(false);
    //             character12.SetActive(false);
    //             character13.SetActive(false);
    //             character14.SetActive(true);
    //             character15.SetActive(false);

    //             myMaterial.SetTexture("_MainTex", ch14_Tex);
    //             myOtherMaterial.SetTexture("_MainTex", ch14_Tex);
    //             //Debug.Log("Character 14");

    //         }

    //         if (currChar == 15)
    //         {

    //             /*  character0_1.SetActive(false);
    //               character0_2.SetActive(false);
    //               character0_3.SetActive(false);
    //               character0_4.SetActive(false);*/

    //             character1.SetActive(false);

    //             character2.SetActive(false);

    //             character3.SetActive(false);


    //             character4.SetActive(false);
    //             currRenderer = character15.GetComponentInChildren<Renderer>();
    //             currRenderer.material.SetTexture("_MainTex", ch15_Tex);

    //             character5.SetActive(false);
    //             character6.SetActive(false);
    //             character7.SetActive(false);
    //             character8.SetActive(false);
    //             character9.SetActive(false);
    //             character10.SetActive(false);
    //             character11.SetActive(false);
    //             character12.SetActive(false);
    //             character13.SetActive(false);
    //             character14.SetActive(false);
    //             character15.SetActive(true);

    //             myMaterial.SetTexture("_MainTex", ch15_Tex);
    //             myOtherMaterial.SetTexture("_MainTex", ch15_Tex);

    //             //  Debug.Log("Character 15");

    //         }
    //     }

    void CharacterAssign()
    {



    }
}
