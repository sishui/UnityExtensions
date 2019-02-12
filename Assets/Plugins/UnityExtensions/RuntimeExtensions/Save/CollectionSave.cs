using System;
using System.IO;
using System.Collections.Generic;

namespace UnityExtensions
{
    /// <summary>
    /// 代表一个二进制存档字段，用于简化存档实现
    /// </summary>
    public struct BinarySaveField
    {
        public Action<BinaryReader> read;
        public Action<BinaryWriter> write;

        public BinarySaveField(Action<BinaryReader> read, Action<BinaryWriter> write)
        {
            this.read = read;
            this.write = write;
        }
    }


    /// <summary>
    /// 代表一个文本存档字段，用于简化存档实现
    /// </summary>
    public struct TextSaveField
    {
        public Action<string> read;
        public Action<StreamWriter> write;

        public TextSaveField(Action<string> read, Action<StreamWriter> write)
        {
            this.read = read;
            this.write = write;
        }
    }


    public abstract class ListBinarySave : BinarySave
    {
        IList<BinarySaveField> _fields;


        public ListBinarySave()
        {
            _fields = CreateFields();
        }


        protected abstract IList<BinarySaveField> CreateFields();


        protected sealed override void Read(BinaryReader reader)
        {
            foreach (var field in _fields)
            {
                field.read(reader);
            }
        }


        protected sealed override void Write(BinaryWriter writer)
        {
            foreach (var field in _fields)
            {
                field.write(writer);
            }
        }
    }


    public abstract class DictionaryTextSave : TextSave
    {
        IDictionary<string, TextSaveField> _fields;


        public DictionaryTextSave()
        {
            _fields = CreateFields();
        }


        protected abstract IDictionary<string, TextSaveField> CreateFields();


        protected sealed override void Read(StreamReader reader)
        {
            var texts = reader.ReadToEnd().Split(new string[] { ": ", ":", "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < texts.Length; i++)
            {
                if (_fields.TryGetValue(texts[i++].Trim(), out TextSaveField field))
                {
                    field.read(texts[i].Trim());
                }
            }
        }


        protected sealed override void Write(StreamWriter writer)
        {
            foreach (var field in _fields)
            {
                writer.Write(field.Key);
                writer.Write(": ");
                field.Value.write(writer);
                writer.WriteLine();
            }
        }
    }

} // namespace UnityExtensions