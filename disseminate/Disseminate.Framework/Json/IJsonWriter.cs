using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disseminate.Json
{
    public enum JsonTypes : int
    {
        Array,
        Date,
        //False,
        Null,
        Number,
        Object,
        String,
        Bool,
        //True,
        Unknown,
        Instance
    }

    public interface IJsonObject: IJsonWriter
    {
        void Add(string field, object value);
    }

    public interface IJsonArray: IJsonWriter
    {
        void Add(object value);
    }

    public interface IJsonWriter
    {
        JsonTypes Type { get; }
        void Write(StringBuilder sb);
    }
}
