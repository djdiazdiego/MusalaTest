using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace DoItFast.Infrastructure.Shared.Extensions
{
    public static class ConverterExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <returns></returns>
        public static ValueConverter<TObject, string> ConverterAnyObject<TObject>() =>
           new(v => JsonConvert.SerializeObject(v), v => JsonConvert.DeserializeObject<TObject>(v));
    }
}
