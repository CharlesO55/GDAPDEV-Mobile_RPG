using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieScript : MonoBehaviour
{
    [SerializeField] private DieValues[] DieValuesList;

    public int GetDieRollValue()
    {
        //Checks developer options if the status is set to always win/lose
        if (GameSettings.IS_DIEROLL_ALWAYS_WIN)
            return 20;

        if (GameSettings.IS_DIEROLL_ALWAYS_FAIL)
            return 1;

        //PREPARE TO STORE THE FACE WITH THE HIGHEST VALUE
        DieValues _rolledFace = null;
        float _greatestAngleMatch = 0.5f;

        //Vector3 dieRot = this.transform.rotation.eulerAngles;


        //LIST ALL VALID VECTORS POITING CLOSE TO THE SAME DIRECTION
        foreach (DieValues d in DieValuesList)
        {
            Vector3 currFacing = this.transform.rotation * d.FaceAngle;

            //Debugging...
            {
                Color rayColor = Color.red;
                if (d == DieValuesList[DieValuesList.Length - 1])
                {
                    rayColor = Color.green;
                }
                Debug.DrawRay(this.transform.position, this.transform.rotation * d.FaceAngle, rayColor);
            }


            //COMPARE CURR ANGLE WITH THAT TO UP
            float angleToCam = Vector3.Dot(currFacing.normalized, Vector3.up);

            //IF POITNING UP , THEN IT IS THE FACE THAT CAMERA SEES
            if (_rolledFace == null || angleToCam > _greatestAngleMatch)
            {
                _greatestAngleMatch = angleToCam;
                _rolledFace = d;
            }
        }

        return _rolledFace.FaceValue;
    }
}
