using Newtonsoft.Json;

namespace Skytharia.SaveManagement.Serializers
{
    public class JSONSerializer : ISerializer
    {
        public string SerializeObject(object toSerialize) 
        {
            return JsonConvert.SerializeObject(toSerialize);
        }
        public string SerializeAndEncrypt(object toSerialize, string encryptionKey)
        {
            string json = SerializeObject(toSerialize);
            string encryptedJson = Encryptor.EncryptString(json, encryptionKey);

            return encryptedJson;
        }
        public T DeserializeObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        public T DecryptAndDeserialize<T>(string encryptedJson, string decryptionKey)
        {
            string unencryptedJson = Encryptor.DecryptString(encryptedJson, decryptionKey);
            return DeserializeObject<T>(unencryptedJson);
        }
    }
}