using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace QAToolKit.Swagger.AspNet.Demo.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BicycleType
    {
        Road,
        Gravel,
        Mountain,
        City
    }
}