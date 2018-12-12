using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

public class QRCodeGen : MonoBehaviour {

    public Image privateKeyQR;
    public Text privateKeyText;

    public Text publicKeyText;
    public Image publicAddressQR;

    public Text publicKeyText2;
    public Image publicAddressQR2;

    public Material mater;

    public void Start ()
    {
            string priv = GetComponent<Account>().PrivKey; // Grab public variable from account script (runs on Awake();)
            string pub = GetComponent<Account>().PubAddress;
            publicKeyText.text = pub;
            publicKeyText2.text = pub;
        privateKeyText.text = priv;

            Texture2D myQR;

            myQR = generateQR(priv);

            Material mater0 = new Material(mater);
            mater0.SetTexture("_MainTex", myQR);

            privateKeyQR.material = mater0;

            myQR = generateQR(pub);

            ////mat.SetTexture("_MainTex", myQR);
            Material mater1 = new Material(mater);

            mater1.SetTexture("_MainTex", myQR);
            publicAddressQR.material = mater1;
            publicAddressQR2.material = mater1;

    }



    private static Color32[] Encode(string textForEncoding, int width, int height)
    {
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
    }

    public Texture2D generateQR(string text)
    {
        var encoded = new Texture2D(256, 256); // For some reason it only works with 256x256, idk why
        var color32 = Encode(text, encoded.width, encoded.height);
        encoded.SetPixels32(color32);
        encoded.Apply();
        return encoded;
    }

}
