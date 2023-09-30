[System.Serializable]
public class JsonReader 
{
    public JsonIndividualData Android;
    public JsonIndividualData IOS;
}

[System.Serializable]
public class JsonIndividualData
{
    public string appId;
    public string bannerId;
    public string interId;
    public string rewardedId;
    public string nativeId;
}