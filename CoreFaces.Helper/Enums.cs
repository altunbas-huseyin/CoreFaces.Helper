using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CoreFaces.Helper
{
    public class Enums
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum Currency
        {
            [Description("Türk lirası.")]
            TL = 1,
            [Description("Dolar")]
            USD = 2,
            [Description("Euro")]
            EURO = 3,
            [Description("Puan")]
            POINT = 4
        }
        [JsonConverter(typeof(StringEnumConverter))]
        public enum Language
        {
            [Description("Türkçe")]
            Turkish = 1,
            [Description("İngilizce")]
            English = 2
        }
        [JsonConverter(typeof(StringEnumConverter))]
        public enum TransactionType
        {
            [Description("Order")]
            Order = 1,
            [Description("Other")]
            Other = 2
        }

    }
}
