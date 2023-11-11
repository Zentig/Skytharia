namespace Skytharia.SaveManagement.Serializers
{
    public interface ISerializer 
    {
        public string SerializeObject(object toSerialize);
        public string SerializeAndEncrypt(object toSerialize, string encryptionKey);
        public T DeserializeObject<T>(string json);
        public T DecryptAndDeserialize<T>(string encryptedJson, string decryptionKey);
    }
}