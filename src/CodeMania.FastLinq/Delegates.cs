namespace CodeMania.FastLinq
{
    /// <summary>
    /// Represents predicate with additional argument of type <typeparamref name="TArg"/> and index of item in source sequence.
    /// </summary>
    /// <typeparam name="T">Item type.</typeparam>
    /// <typeparam name="TArg">Additional argument type.</typeparam>
    /// <param name="item">Item to check in predicate.</param>
    /// <param name="argument">Additional argument which can be used in predicate logic.</param>
    /// <param name="index">Index of item in source sequence.</param>
    /// <returns><see langword="true"/> if item satisfies predicate conditions, otherwise <see langword="true"/>.</returns>
    public delegate bool ParametrizedIndexedPredicate<in T, in TArg>(T item, TArg argument, int index);

    /// <summary>
    /// Represents predicate with additional argument of type <typeparamref name="TArg"/>.
    /// </summary>
    /// <typeparam name="T">Item type.</typeparam>
    /// <typeparam name="TArg">Additional argument type.</typeparam>
    /// <param name="item">Item to check in predicate.</param>
    /// <param name="argument">Additional argument which can be used in predicate logic.</param>
    /// <returns><see langword="true"/> if item satisfies predicate conditions, otherwise <see langword="true"/>.</returns>
    public delegate bool ParametrizedPredicate<in T, in TArg>(T item, TArg argument);

    /// <summary>
    /// Represents projection delegate to convert source element of type <typeparamref name="TSource"/> to instance of type <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TSource">Type of item to convert.</typeparam>
    /// <typeparam name="TArg">Additional argument type.</typeparam>
    /// <typeparam name="TResult">Result type.</typeparam>
    /// <param name="item">Item to project.</param>
    /// <param name="argument">Additional argument which can be used in projection logic.</param>
    /// <returns>Instance of result type <typeparamref name="TResult"/>.</returns>
    public delegate TResult ParametrizedProjection<in TSource, in TArg, out TResult>(TSource item, TArg argument);

    /// <summary>
    /// Represents projection delegate to convert source element of type <typeparamref name="TSource"/> to instance of type <typeparamref name="TResult"/>
    /// with index of item in source sequence.
    /// </summary>
    /// <typeparam name="TSource">Type of item to convert.</typeparam>
    /// <typeparam name="TArg">Additional argument type.</typeparam>
    /// <typeparam name="TResult">Result type.</typeparam>
    /// <param name="item">Item to project.</param>
    /// <param name="argument">Additional argument which can be used in projection logic.</param>
    /// <param name="index">Index of item in source sequence.</param>
    /// <returns>Instance of result type <typeparamref name="TResult"/>.</returns>
    public delegate TResult ParametrizedIndexedProjection<in TSource, in TArg, out TResult>(TSource item, TArg argument, int index);
}