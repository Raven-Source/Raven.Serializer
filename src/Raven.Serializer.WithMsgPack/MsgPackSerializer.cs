﻿using MsgPack.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Serializer.WithMsgPack
{
    /// <summary>
    /// 
    /// </summary>
    public class MsgPackSerializer : IDataSerializer
    {
        //private static Dictionary<string, IMessagePackSingleObjectSerializer> msgPackserializers = new Dictionary<string, IMessagePackSingleObjectSerializer>();
        //private static object _obj = new object();
        //private static readonly Encoding encoding = Encoding.UTF8;
        private SerializationContext _context;

        /// <summary>
        /// 
        /// </summary>
        public MsgPackSerializer()
        {
            _context = new SerializationContext() { SerializationMethod = SerializationMethod.Map };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializationMethod"></param>
        public MsgPackSerializer(int serializationMethod)
        {
            _context = new SerializationContext() { SerializationMethod = (SerializationMethod)serializationMethod };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public byte[] Serialize(object obj)
        {
            Type t = obj.GetType();
            IMessagePackSingleObjectSerializer serializer = GetSerializer(t);

            return serializer.PackSingleObject(obj);
            //using (var byteStream = new MemoryStream())
            //{
            //    serializer.Pack(byteStream, obj);
            //    return byteStream.ToArray();
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="stream"></param>
        public void Serialize(object obj, Stream stream)
        {
            Type t = obj.GetType();
            IMessagePackSingleObjectSerializer serializer = GetSerializer(t);

            serializer.Pack(stream, obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public T Deserialize<T>(byte[] data)
        {
            Type t = typeof(T);
            IMessagePackSingleObjectSerializer serializer = GetSerializer(t);

            return (T)serializer.UnpackSingleObject(data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public object Deserialize(Type type, byte[] data)
        {
            IMessagePackSingleObjectSerializer serializer = GetSerializer(type);
            return serializer.UnpackSingleObject(data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="buffer"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public T Deserialize<T>(byte[] buffer, int index, int count)
        {
            Type t = typeof(T);
            IMessagePackSingleObjectSerializer serializer = GetSerializer(t);

            using (var byteStream = new MemoryStream(buffer, index, count))
            {
                return (T)serializer.Unpack(byteStream);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="buffer"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public object Deserialize(Type type, byte[] buffer, int index, int count)
        {
            IMessagePackSingleObjectSerializer serializer = GetSerializer(type);

            using (var byteStream = new MemoryStream(buffer, index, count))
            {
                return serializer.Unpack(byteStream);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public T Deserialize<T>(Stream stream)
        {
            Type t = typeof(T);
            IMessagePackSingleObjectSerializer serializer = GetSerializer(t);
            return (T)serializer.Unpack(stream);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public object Deserialize(Type type, Stream stream)
        {
            IMessagePackSingleObjectSerializer serializer = GetSerializer(type);
            return serializer.Unpack(stream);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private IMessagePackSingleObjectSerializer GetSerializer(Type t)
        {
            return MessagePackSerializer.Get(t, _context);
        }
        
    }
}
