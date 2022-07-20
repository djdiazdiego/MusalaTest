namespace DoItFast.Application.Extensions
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Get all messages in exception
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetAllMessages(this Exception exception) =>
            exception.FromHierarchy(ex => ex.InnerException).Select(ex => ex.Message);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="nextItem"></param>
        /// <param name="canContinue"></param>
        /// <returns></returns>
        private static IEnumerable<TSource> FromHierarchy<TSource>(
            this TSource source,
            Func<TSource, TSource> nextItem,
            Func<TSource, bool> canContinue)
        {
            for (var current = source; canContinue(current); current = nextItem(current))
                yield return current;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="nextItem"></param>
        /// <returns></returns>
        private static IEnumerable<TSource> FromHierarchy<TSource>(
            this TSource source,
            Func<TSource, TSource> nextItem)
            where TSource : class
            => FromHierarchy(source, nextItem, s => s != null);
    }
}
