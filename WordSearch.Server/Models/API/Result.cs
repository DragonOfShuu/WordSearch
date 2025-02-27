namespace WordSearch.Server.Models.API
{
#pragma warning disable CS8604 // Possible null reference argument.
    /// <summary>
    /// Credit to: All credit goes to: https://dev.to/ephilips/better-error-handling-in-c-with-result-types-4aan
    /// 
    /// Store a potential sucessful type, or an error type.
    /// 
    /// This class allows for both to be returned, and then
    /// for you to run a method depending on what the result is.
    /// </summary>
    /// <typeparam name="T">The sucessful type</typeparam>
    /// <typeparam name="E">The error type</typeparam>
    public readonly struct Result<T, E>
    {
        private readonly bool _success;
        public readonly T Value;
        public readonly E Error;

        private Result(T v, E e, bool success)
        {
            Value = v;
            Error = e;
            _success = success;
        }

        public bool IsOk => _success;

        public static Result<T, E> Ok(T v)
        {
            return new(v, default, true);
        }

        public static Result<T, E> Err(E e)
        {
            return new(default, e, false);
        }

        public static implicit operator Result<T, E>(T v) => new(v, default, true);
        public static implicit operator Result<T, E>(E e) => new(default, e, false);

        public R Match<R>(
                Func<T, R> success,
                Func<E, R> failure) =>
            _success ? success(Value) : failure(Error);
    }
#pragma warning restore CS8604 // Possible null reference argument.
}
