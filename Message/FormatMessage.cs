using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Message
{
    [Serializable]
    public class FormatMessage
    {
        public byte HeaderLength { get; private set; }
        public string Command { get; private set; }

        public FormatMessage(string cmd)
        {
            if (cmd.Length >= 256)
            {
                throw new ArgumentException("Length of input command bigger than 256 bytes");
            }
            else if (cmd.Length == 0)
            {
                throw new ArgumentException("Input command is blank");
            }
            else
            {
                HeaderLength = Convert.ToByte(cmd.Length);
                Command = cmd;
            }
        }

        public byte[] Serialize()
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, this);
            return ms.ToArray();
        }

        public static FormatMessage Desserialize(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            FormatMessage newFM = (FormatMessage)binForm.Deserialize(memStream);
            return newFM;
        }
    }
}
