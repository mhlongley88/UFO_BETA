using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class TutorialAnimations : MonoBehaviour
{

    public GameObject TutorialController;
    public GameObject LThumbstick;
    public GameObject RThumbstick;
    public GameObject Shoot;
    public GameObject Dash;
    public GameObject Abduct;
    public GameObject city;
    public GameObject specialPt1;
    public GameObject specialPt2;
    public GameObject rotNull;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RunTutorial());
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            StartCoroutine(RunTutorial());
        }
    }

    private IEnumerator RunTutorial()
    {
        yield return new WaitForSeconds(6);
        LThumbstick.SetActive(true);
        yield return new WaitForSeconds(6);
        LThumbstick.SetActive(false);
        RThumbstick.SetActive(true);
        yield return new WaitForSeconds(6);
        RThumbstick.SetActive(false);
        Shoot.SetActive(true);
        yield return new WaitForSeconds(6);
        Shoot.SetActive(false);
        Dash.SetActive(true);
        yield return new WaitForSeconds(6);
        Dash.SetActive(false);
        Abduct.SetActive(true);
        city.SetActive(true);
        yield return new WaitForSeconds(6);
        Abduct.SetActive(false);
        specialPt1.SetActive(true);
        specialPt2.SetActive(true);
        rotNull.transform.Rotate(0, 180, 0);
        yield return new WaitForSeconds(8);
        specialPt1.SetActive(false);
        specialPt2.SetActive(false);
        rotNull.transform.Rotate(0, -180, 0);
        Shoot.SetActive(true);
        yield return new WaitForSeconds(4);
        Shoot.SetActive(false);
        yield return new WaitForSeconds(4);
        TutorialController.SetActive(false);
    }
}
