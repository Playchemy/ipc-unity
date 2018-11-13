using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.ABI.Encoders;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Transactions;
using Nethereum.Signer;
using UnityEngine;


public class IpcContractService
{
    //public static string topScoreABI = @"[{'constant':false,'inputs':[{'name':'score','type':'int256'},{'name':'v','type':'uint8'},{'name':'r','type':'bytes32'},{'name':'s','type':'bytes32'}],'name':'setTopScore','outputs':[],'payable':false,'type':'function'},{'constant':true,'inputs':[{'name':'','type':'uint256'}],'name':'topScores','outputs':[{'name':'addr','type':'address'},{'name':'score','type':'int256'}],'payable':false,'type':'function'},{'constant':false,'inputs':[],'name':'getCountTopScores','outputs':[{'name':'','type':'uint256'}],'payable':false,'type':'function'},{'constant':true,'inputs':[{'name':'','type':'address'}],'name':'userTopScores','outputs':[{'name':'','type':'int256'}],'payable':false,'type':'function'},{'inputs':[],'payable':false,'type':'constructor'}]";
    public static string ABI = @"[{'constant':false,'inputs':[{'name':'_newGod','type':'address'}],'name':'renounceGodhood','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[{'name':'_interfaceId','type':'bytes4'}],'name':'supportsInterface','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'nameModificationLevelRequirement','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'_ipcId','type':'uint256'}],'name':'randomizeDna','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[],'name':'name','outputs':[{'name':'','type':'string'}],'payable':false,'stateMutability':'pure','type':'function'},{'constant':true,'inputs':[{'name':'_tokenId','type':'uint256'}],'name':'getApproved','outputs':[{'name':'','type':'address'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'_to','type':'address'},{'name':'_tokenId','type':'uint256'}],'name':'approve','outputs':[],'payable':true,'stateMutability':'payable','type':'function'},{'constant':true,'inputs':[],'name':'priceToChangeName','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'_newAddress','type':'address'}],'name':'updateIpcContract','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_newPrice','type':'uint256'}],'name':'setMaxIpcPrice','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_newPrice','type':'uint256'}],'name':'changePriceToModifyDna','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[],'name':'ipcPriceInCents','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'_adminToRemove','type':'address'}],'name':'removeAdmin','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[],'name':'totalSupply','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'getAllPositions','outputs':[{'name':'','type':'address[]'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[{'name':'','type':'uint256'}],'name':'Ipcs','outputs':[{'name':'name','type':'string'},{'name':'attributeSeed','type':'bytes32'},{'name':'dna','type':'bytes32'},{'name':'experience','type':'uint128'},{'name':'timeOfBirth','type':'uint128'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'_newMultiplier','type':'uint256'}],'name':'changeCustomizationMultiplier','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_from','type':'address'},{'name':'_to','type':'address'},{'name':'_tokenId','type':'uint256'}],'name':'transferFrom','outputs':[],'payable':true,'stateMutability':'payable','type':'function'},{'constant':false,'inputs':[{'name':'_newUrl','type':'string'}],'name':'updateIpcUrl','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_ipcIdArray','type':'uint256[]'},{'name':'_xpIdArray','type':'uint256[]'}],'name':'grantBulkXp','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[{'name':'_owner','type':'address'},{'name':'_index','type':'uint256'}],'name':'tokenOfOwnerByIndex','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'_ipcId','type':'uint256'},{'name':'_beneficiaryAddress','type':'address'},{'name':'_beneficiaryPrice','type':'uint256'}],'name':'setSpecialPriceForAddress','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[],'name':'getAdmins','outputs':[{'name':'','type':'address[]'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[{'name':'','type':'uint256'}],'name':'ipcToMarketInfo','outputs':[{'name':'sellPrice','type':'uint32'},{'name':'beneficiaryPrice','type':'uint32'},{'name':'beneficiaryAddress','type':'address'},{'name':'approvalAddress','type':'address'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[{'name':'','type':'uint256'},{'name':'','type':'uint256'}],'name':'ipcIdToExperience','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[],'name':'withdraw','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_ipcId','type':'uint256'},{'name':'_newPrice','type':'uint256'}],'name':'setIpcPrice','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_ipcId','type':'uint256'},{'name':'_newPrice','type':'uint256'}],'name':'buyIpc','outputs':[],'payable':true,'stateMutability':'payable','type':'function'},{'constant':false,'inputs':[{'name':'_from','type':'address'},{'name':'_to','type':'address'},{'name':'_tokenId','type':'uint256'}],'name':'safeTransferFrom','outputs':[],'payable':true,'stateMutability':'payable','type':'function'},{'constant':false,'inputs':[{'name':'_name','type':'string'},{'name':'_price','type':'uint256'}],'name':'createRandomizedIpc','outputs':[],'payable':true,'stateMutability':'payable','type':'function'},{'constant':false,'inputs':[{'name':'_name','type':'string'},{'name':'_price','type':'uint256'},{'name':'_owner','type':'address'}],'name':'createAndAssignRandomizedIpc','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[{'name':'_developer','type':'address'}],'name':'experiencesOfDeveloper','outputs':[{'name':'','type':'uint256[]'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[{'name':'_index','type':'uint256'}],'name':'tokenByIndex','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'mostCurrentIpcAddress','outputs':[{'name':'','type':'address'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'maxIpcPrice','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[{'name':'_tokenId','type':'uint256'}],'name':'ownerOf','outputs':[{'name':'owner','type':'address'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'totalDevelopers','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[],'name':'releaseNewTranche','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_ipcId','type':'uint256'},{'name':'_dna','type':'bytes32'}],'name':'customizeDna','outputs':[],'payable':true,'stateMutability':'payable','type':'function'},{'constant':false,'inputs':[{'name':'value','type':'bool'}],'name':'setAutoTrancheRelease','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_newAdmin','type':'address'}],'name':'addAdmin','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[{'name':'_owner','type':'address'}],'name':'balanceOf','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'customizationPriceMultiplier','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[{'name':'_ipcId','type':'uint256'}],'name':'getIpcPriceInWei','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'totalIpcs','outputs':[{'name':'','type':'uint128'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[{'name':'_ipcId','type':'uint256'}],'name':'getIpc','outputs':[{'name':'name','type':'string'},{'name':'attributeSeed','type':'bytes32'},{'name':'dna','type':'bytes32'},{'name':'experience','type':'uint128'},{'name':'timeOfBirth','type':'uint128'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'_newAmount','type':'uint256'}],'name':'changeXpPrice','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_xpId','type':'uint256'}],'name':'removeExperience','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[{'name':'_owner','type':'address'}],'name':'tokensOfOwner','outputs':[{'name':'','type':'uint256[]'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'_newReq','type':'uint256'}],'name':'changeNameModificationLevelRequirement','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[],'name':'getXpPrice','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'_address','type':'address'},{'name':'_name','type':'string'}],'name':'setDeveloperName','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_newAddress','type':'address'}],'name':'updateMarketPriceContract','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_newInterfaceId','type':'bytes4'}],'name':'addSupportedInterface','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_ipcId','type':'uint256'}],'name':'rollAttributes','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[],'name':'symbol','outputs':[{'name':'','type':'string'}],'payable':false,'stateMutability':'pure','type':'function'},{'constant':false,'inputs':[{'name':'developer','type':'address'},{'name':'value','type':'bool'}],'name':'changeDeveloperStatus','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_ipcId','type':'uint256'},{'name':'_authorization','type':'bool'}],'name':'changeAdminAuthorization','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[],'name':'dnaModificationLevelRequirement','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'_operator','type':'address'},{'name':'_approved','type':'bool'}],'name':'setApprovalForAll','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[{'name':'','type':'uint256'}],'name':'ipcToOwner','outputs':[{'name':'','type':'address'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'priceToModifyDna','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'_name','type':'string'},{'name':'_price','type':'uint256'},{'name':'_owner','type':'address'}],'name':'createAndAssignIpcSeed','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[{'name':'_ipcId','type':'uint256'}],'name':'getIpcName','outputs':[{'name':'result','type':'bytes32'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'_newCashier','type':'address'}],'name':'setCashier','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_newPriceIncrease','type':'uint256'}],'name':'changePriceIncreasePerTranche','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_description','type':'string'}],'name':'registerNewExperience','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_newSize','type':'uint256'}],'name':'changeTrancheSize','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_from','type':'address'},{'name':'_to','type':'address'},{'name':'_tokenId','type':'uint256'},{'name':'data','type':'bytes'}],'name':'safeTransferFrom','outputs':[],'payable':true,'stateMutability':'payable','type':'function'},{'constant':false,'inputs':[{'name':'_interfaceId','type':'bytes4'}],'name':'removeSupportedInterface','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_name','type':'string'},{'name':'_price','type':'uint256'}],'name':'createIpcSeed','outputs':[],'payable':true,'stateMutability':'payable','type':'function'},{'constant':true,'inputs':[{'name':'_tokenId','type':'uint256'}],'name':'tokenURI','outputs':[{'name':'','type':'string'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'getXpBalance','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[{'name':'','type':'uint256'}],'name':'experiences','outputs':[{'name':'developer','type':'address'},{'name':'description','type':'string'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[],'name':'buyXp','outputs':[],'payable':true,'stateMutability':'payable','type':'function'},{'constant':true,'inputs':[{'name':'','type':'address'}],'name':'ownerIpcCount','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'_ipcId','type':'uint256'},{'name':'_byteToModify','type':'uint256'},{'name':'_modifyAmount','type':'int256'}],'name':'modifyDna','outputs':[],'payable':true,'stateMutability':'payable','type':'function'},{'constant':false,'inputs':[{'name':'_newReq','type':'uint256'}],'name':'changeDnaModificationLevelRequirement','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_ipcId','type':'uint256'},{'name':'_newName','type':'string'}],'name':'changeIpcName','outputs':[],'payable':true,'stateMutability':'payable','type':'function'},{'constant':true,'inputs':[{'name':'_owner','type':'address'},{'name':'_operator','type':'address'}],'name':'isApprovedForAll','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'_newExec','type':'address'}],'name':'setExec','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_ipcId','type':'uint256'},{'name':'_xpId','type':'uint256'}],'name':'grantXpToIpc','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'inputs':[],'payable':false,'stateMutability':'nonpayable','type':'constructor'},{'anonymous':false,'inputs':[{'indexed':true,'name':'tokenId','type':'uint256'},{'indexed':true,'name':'developer','type':'address'},{'indexed':true,'name':'xpId','type':'uint256'}],'name':'ExperienceEarned','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'name':'_tokenId','type':'uint256'},{'indexed':false,'name':'_to','type':'string'}],'name':'NameChanged','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'name':'_tokenId','type':'uint256'},{'indexed':false,'name':'_seller','type':'address'},{'indexed':true,'name':'_buyer','type':'address'},{'indexed':false,'name':'price','type':'uint256'}],'name':'Bought','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'name':'_tokenId','type':'uint256'},{'indexed':false,'name':'from','type':'uint256'},{'indexed':false,'name':'to','type':'uint256'}],'name':'PriceChanged','type':'event'},{'anonymous':false,'inputs':[{'indexed':false,'name':'tokenId','type':'uint256'},{'indexed':true,'name':'owner','type':'address'},{'indexed':false,'name':'name','type':'string'}],'name':'Created','type':'event'},{'anonymous':false,'inputs':[{'indexed':false,'name':'tokenId','type':'uint256'},{'indexed':false,'name':'dna','type':'bytes32'},{'indexed':false,'name':'attributes','type':'bytes32'}],'name':'Substantiated','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'name':'tokenId','type':'uint256'},{'indexed':false,'name':'to','type':'bytes32'}],'name':'DnaModified','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'name':'_from','type':'address'},{'indexed':true,'name':'_to','type':'address'},{'indexed':true,'name':'_tokenId','type':'uint256'}],'name':'Transfer','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'name':'_owner','type':'address'},{'indexed':true,'name':'_approved','type':'address'},{'indexed':true,'name':'_tokenId','type':'uint256'}],'name':'Approval','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'name':'_owner','type':'address'},{'indexed':true,'name':'_operator','type':'address'},{'indexed':false,'name':'_approved','type':'bool'}],'name':'ApprovalForAll','type':'event'}]";
    private static string contractAddress = "0x011C77fa577c500dEeDaD364b8af9e8540b808C0";
    private Contract contract;

    public IpcContractService()
    {
        this.contract = new Contract(null, ABI, contractAddress);
    }

    public Function GetFunctionGetIpc()
    {
        return contract.GetFunction("getIpc");
    }

    public Function GetFunctionCountIpcs()
    {
        return contract.GetFunction("totalIpcs");
    }

    public Function GetFunctionIpcToMarketInfo()
    {
        return contract.GetFunction("ipcToMarketInfo");
    }

    public Function GetFunctionBuyIpc()
    {
        return contract.GetFunction("buyIpc");
    }

    public Function GetFunctionOwnerOf()
    {
        return contract.GetFunction("ownerOf");
    }

    public Function GetFunctionTokensOfOwner()
    {
        return contract.GetFunction("tokensOfOwner");
    }

    public CallInput CreateGetIpcCallInput(BigInteger index)
    {
        var function = GetFunctionGetIpc();
        return function.CreateCallInput(index);
    }

    public CallInput CreateCountIpcsCallInput()
    {
        var function = GetFunctionCountIpcs();
        return function.CreateCallInput();
    }

    public CallInput CreateIpcToMarketInfoCallInput(BigInteger index)
    {
        var function = GetFunctionIpcToMarketInfo();
        return function.CreateCallInput(index);
    }

    public CallInput CreateOwnerOfCallInput(BigInteger index)
    {
        var function = GetFunctionOwnerOf();
        return function.CreateCallInput(index);
    }

    public CallInput CreateTokensOfOwnerCallInput(string address)
    {
        var function = GetFunctionTokensOfOwner();
        return function.CreateCallInput(address);
    }

    public GetIpcDto DecodeGetIpcDto(string blockchainQueryResult)
    {
        var function = GetFunctionGetIpc();
        return function.DecodeDTOTypeOutput<GetIpcDto>(blockchainQueryResult);
    }

    public int DecodeIpcCount(string blockchainQueryResult)
    {
        var function = GetFunctionCountIpcs();
        return function.DecodeSimpleTypeOutput<int>(blockchainQueryResult);
    }

    public IpcToMarketInfoDto DecodeIpcToMarketInfoDto(string blockchainQueryResult)
    {
        var function = GetFunctionIpcToMarketInfo();
        return function.DecodeDTOTypeOutput<IpcToMarketInfoDto>(blockchainQueryResult);
    }

    public string DecodeGetOwner(string blockchainQueryResult)
    {
        var function = GetFunctionOwnerOf();
        return function.DecodeSimpleTypeOutput<string>(blockchainQueryResult);
    }

    public List<int> DecodeTokensOfOwner(string blockchainQueryResult)
    {
        var function = GetFunctionTokensOfOwner();
        return function.DecodeSimpleTypeOutput<List<int>>(blockchainQueryResult);
    }

    public TransactionInput CreateBuyIpcTransactionInput(string addressFrom, string addressOwner, string privateKey, BigInteger ipcId, HexBigInteger gas, HexBigInteger valueAmount)
    {
        var numberBytes = new IntTypeEncoder().Encode(ipcId);
        var sha3 = new Nethereum.Util.Sha3Keccack();
        var hash = sha3.CalculateHashFromHex(addressFrom, addressOwner, numberBytes.ToHex());
        var signer = new MessageSigner();
        var signature = signer.Sign(hash.HexToByteArray(), privateKey);
        var ethEcdsa = MessageSigner.ExtractEcdsaSignature(signature);

        var function = GetFunctionBuyIpc();
        return function.CreateTransactionInput(addressFrom, gas, valueAmount, ipcId, ethEcdsa.V, ethEcdsa.R, ethEcdsa.S);
    }
}


[FunctionOutput]
public class GetIpcDto
{
    [Parameter("string", "name", 1)]
    public string Name { get; set; }
    [Parameter("bytes32", "attributeSeed", 2)]
    public byte[] AttributeSeed { get; set; }
    [Parameter("bytes32", "dna", 3)]
    public byte[] Dna { get; set; }
    [Parameter("uint128", "experience", 4)]
    public uint Experience { get; set; }
    [Parameter("uint128", "timeOfBirth", 5)]
    public uint TimeOfBirth { get; set; }
}

[FunctionOutput]
public class TokensOfOwnerDto
{
    [Parameter("int256[]", "name", 1)]
    public List<int> IpcIdArray{ get; set; }

}

[FunctionOutput]
public class IpcToMarketInfoDto
{
    [Parameter("uint32", "price", 1)]
    public uint Price { get; set; }
    /* these parameters are most likely zero which makes them unreadable
     * [Parameter("uint32", "beneficiaryPrice", 2)]
    public uint BeneficiaryPrice { get; set; }
    [Parameter("string", "beneficiaryAddress", 3)]
    public string BeneficiaryAddress { get; set; }
    [Parameter("string", "approvalAddress", 4)]
    public string ApprovalAddress { get; set; }*/
}