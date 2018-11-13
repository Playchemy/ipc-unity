using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Nethereum.JsonRpc.UnityClient;

public class Account : MonoBehaviour {

    public string PrivKey;
    public string PubAddress;

    public Nethereum.Signer.EthECKey jkfsg;

    // Use this for initialization
    public void Awake ()
    {
        var ecKey = Nethereum.Signer.EthECKey.GenerateKey();
        PrivKey = ecKey.GetPrivateKey();
        PubAddress = ecKey.GetPublicAddress();
	}

    public void GenerateFromPrivateKey(InputField field)
    {
        PrivKey = field.text;
        PubAddress = Nethereum.Signer.EthECKey.GetPublicAddress(PrivKey);
    }



}