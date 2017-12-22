namespace LHRP.Api.Common
{
    public class Result
    {
        public bool IsSuccess { get; protected set; }
        public bool IsFailure => !IsSuccess;
        public string ErrorMessage { get; protected set; }

        public static Result Fail(string errorMessage)
        {
            return new Result()
            {
                IsSuccess = false,
                ErrorMessage = errorMessage
            };
        }

        public static Result Ok()
        {
            return new Result() { IsSuccess = true };
        }
    }

    public class Result<T> : Result
    {
        public T Value { get; private set; }
        public new static Result<T> Fail(string errorMessage)
        {
            return new Result<T>()
            {
                IsSuccess = false,
                ErrorMessage = errorMessage
            };
        }

        public static Result<T> Ok(T obj)
        {
            return new Result<T>()
            {
                IsSuccess = true,
                Value = obj
            };
        }
    }
}