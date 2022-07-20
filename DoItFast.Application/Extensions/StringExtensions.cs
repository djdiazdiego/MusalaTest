using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoItFast.Application.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Remove accent.
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string RemoveAccent(this string texto) =>
            new String(
                texto.Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray()
            )
            .Normalize(NormalizationForm.FormC);
    }
}
