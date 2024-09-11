using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 이미지에 적용할려면 필요

public class GaugeColor : MonoBehaviour
{
    Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        image.color = Color.HSVToRGB(image.fillAmount / 3,1,1);
    }
}
