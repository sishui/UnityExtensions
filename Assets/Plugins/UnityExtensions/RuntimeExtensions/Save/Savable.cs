using System.IO;
using System.Collections.Generic;
using System;

namespace UnityExtensions
{
    public interface IBinarySavable
    {
        void Read(BinaryReader reader);
        void Write(BinaryWriter writer);
        void Reset();
    }


    public interface ITextSavable
    {
        void Read(string text);
        void Write(StreamWriter writer);
        void Reset();
    }


    /// <summary>
    /// 代表一个二进制存档字段，用于简化存档实现
    /// </summary>
    public class BinarySavableField : IBinarySavable
    {
        Action<BinaryReader> _read;
        Action<BinaryWriter> _write;
        Action _reset;

        public BinarySavableField(Action<BinaryReader> read, Action<BinaryWriter> write, Action reset)
        {
            _read = read;
            _write = write;
            _reset = reset;
        }

        void IBinarySavable.Read(BinaryReader reader)
        {
            _read(reader);
        }

        void IBinarySavable.Write(BinaryWriter writer)
        {
            _write(writer);
        }

        void IBinarySavable.Reset()
        {
            _reset();
        }
    }


    /// <summary>
    /// 代表一个文本存档字段，用于简化存档实现
    /// </summary>
    public class TextSavableField : ITextSavable
    {
        Action<string> _read;
        Action<StreamWriter> _write;
        Action _reset;

        public TextSavableField(Action<string> read, Action<StreamWriter> write, Action reset)
        {
            _read = read;
            _write = write;
            _reset = reset;
        }

        void ITextSavable.Read(string text)
        {
            _read(text);
        }

        void ITextSavable.Write(StreamWriter writer)
        {
            _write(writer);
        }

        void ITextSavable.Reset()
        {
            _reset();
        }
    }


    public sealed class BinarySavableCollection : BinarySave
    {
        IList<IBinarySavable> _savables;


        public BinarySavableCollection(IList<IBinarySavable> savables)
        {
            _savables = savables;
        }


        protected sealed override void Read(BinaryReader reader)
        {
            foreach (var s in _savables)
            {
                s.Read(reader);
            }
        }


        protected sealed override void Write(BinaryWriter writer)
        {
            foreach (var s in _savables)
            {
                s.Write(writer);
            }
        }


        public sealed override void Reset()
        {
            foreach (var s in _savables)
            {
                s.Reset();
            }
        }
    }


    public sealed class TextSavableCollection : TextSave
    {
        IDictionary<string, ITextSavable> _savables;


        public TextSavableCollection(IDictionary<string, ITextSavable> savables)
        {
            _savables = savables;
        }


        protected sealed override void Read(StreamReader reader)
        {
            var texts = reader.ReadToEnd().Split(new string[] { ": ", ":", "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            var info = new Dictionary<string, string>(texts.Length / 2);

            for (int i = 0; i < texts.Length; i+=2)
            {
                info.Add(texts[i].Trim(), texts[i+1].Trim());
            }

            foreach (var s in _savables)
            {
                if (info.TryGetValue(s.Key, out string text))
                {
                    s.Value.Read(text);
                }
                else
                {
                    s.Value.Reset();
                }
            }
        }


        protected sealed override void Write(StreamWriter writer)
        {
            foreach (var s in _savables)
            {
                writer.Write(s.Key);
                writer.Write(": ");
                s.Value.Write(writer);
                writer.WriteLine();
            }
        }


        public sealed override void Reset()
        {
            foreach (var s in _savables)
            {
                s.Value.Reset();
            }
        }
    }

} // namespace UnityExtensions