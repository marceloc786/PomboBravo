using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class phase : MonoBehaviour {

    public Text txt;
    public Touch toque;
	
	// Update is called once per frame
	void Update () {
		if(Input.touchCount > 0)
        {
            toque = Input.GetTouch(0);

            switch (toque.phase)
            {
                case TouchPhase.Began:

                    break;
                case TouchPhase.Ended:

                    break;
                case TouchPhase.Moved:

                    break;
                case TouchPhase.Stationary:

                    break;
                case TouchPhase.Canceled:

                    break;
            }
        }
	}
}
