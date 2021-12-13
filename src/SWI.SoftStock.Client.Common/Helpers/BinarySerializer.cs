using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SWI.SoftStock.Client.Common.Helpers
{
    public class BinarySerializer<T>
    {
        public BinarySerializer()
        {
        }

        public void SerializeObject(string filename, T objectToSerialize)
        {
            Stream stream = File.Open(filename, FileMode.Create);
            var bFormatter = new BinaryFormatter();
            bFormatter.Serialize(stream, objectToSerialize);
            stream.Close();
        }

        public T DeSerializeObject(string filename)
        {
            Stream stream = File.Open(filename, FileMode.Open);
            var bFormatter = new BinaryFormatter();
            var objectToSerialize = (T)bFormatter.Deserialize(stream);
            stream.Close();
            return objectToSerialize;
        }
    }

    public class DataContractSerializer<T>
    {
        public DataContractSerializer()
        {
        }

        public void SerializeObject(string filename, T objectToSerialize)
        {
            Stream stream = File.Open(filename, FileMode.Create);
            var serializer = new DataContractSerializer(typeof(T));
            var writer = new StringWriter();
            serializer.WriteObject(stream, objectToSerialize);
            stream.Close();
        }

        public T DeSerializeObject(string filename)
        {
            if (!File.Exists(filename))
            {
                return default(T);
            }
            Stream stream = File.Open(filename, FileMode.Open);
            var serializer = new DataContractSerializer(typeof(T));
            var objectToSerialize = (T)serializer.ReadObject(stream);
            stream.Close();
            return objectToSerialize;
        }
    }
}
