using System.Diagnostics;

namespace Carvana
{   
    [DebuggerNonUserCode]
    public sealed class Result<T> : Result
    {
        public T Content { get; }

        public Result(T content)
        {
            Content = content;
        }

        public Result(ResultStatus status, string errorMessage)
            : base(status, errorMessage) { }

        internal Result(ResultStatus status, string errorMessage, T content)
            : base(status, errorMessage)
        {
            Content = content;
        }

        public static implicit operator Result<T>(T content) => new Result<T>(content);

        public static Result<T> Success(T content) => new Result<T>(content);
        public new static Result<T> Errored(ResultStatus status, string errorMessage) => new Result<T>(status, errorMessage);
        public static Result<T> NotFound(string errorMessage) => new Result<T>(ResultStatus.MissingResource, errorMessage);
        public new static Result<T> InvalidRequest(string errorMessage) => new Result<T>(ResultStatus.InvalidRequest, errorMessage);
    }

    [DebuggerNonUserCode]
    public abstract class Result
    {
        public ResultStatus Status { get; }
        public string ErrorMessage { get; }
        public bool Succeeded() => Status == ResultStatus.Succeeded;
        public bool Failed() => !Succeeded();

        protected Result()
            : this(ResultStatus.Succeeded, "") { }

        protected Result(ResultStatus status, string errorMessage)
        {
            Status = status;
            ErrorMessage = errorMessage;
        }

        public override string ToString() => $"{Status}: {ErrorMessage}";

        public static Result Success() => new Result<string>("No Content");
        public static Result Errored(ResultStatus status, string errorMessage) => new Result<string>(status, errorMessage, "No Content");
        public static Result InvalidRequest(string errorMessage) => new Result<string>(ResultStatus.InvalidRequest, errorMessage, "No Content");
        public static Result MissingResource(string errorMessage) => new Result<string>(ResultStatus.MissingResource, errorMessage, "No Content");
        public static Result<T> Success<T>(T content) => new Result<T>(content);
        public static Result<T> Errored<T>(ResultStatus status, string errorMessage) => new Result<T>(status, errorMessage);
    }
}
