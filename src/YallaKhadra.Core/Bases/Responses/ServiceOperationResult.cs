using YallaKhadra.Core.Enums;

namespace YallaKhadra.Core.Bases.Responses;

public class ServiceOperationResult {
    public ServiceOperationStatus Status { get; }
    public string Message { get; }
    public string? ErrorCode { get; }
    public object? Meta { get; }
    public bool IsSuccess => Status == ServiceOperationStatus.Succeeded ||
                             Status == ServiceOperationStatus.Created ||
                             Status == ServiceOperationStatus.Updated ||
                             Status == ServiceOperationStatus.Deleted;

    protected ServiceOperationResult(ServiceOperationStatus status, string? message = null, string? errorCode = null, object? meta = null) {
        var isSuccessStatus = status == ServiceOperationStatus.Succeeded ||
                              status == ServiceOperationStatus.Created ||
                              status == ServiceOperationStatus.Updated ||
                              status == ServiceOperationStatus.Deleted;

        if (isSuccessStatus && !string.IsNullOrEmpty(errorCode))
            throw new ArgumentException("Success result cannot have an ErrorCode.", nameof(errorCode));

        Status = status; ErrorCode = errorCode; Meta = meta; Message = message ?? GetDefaultMessage(status);
    }

    public static ServiceOperationResult Success(ServiceOperationStatus status = ServiceOperationStatus.Succeeded, string? message = null, object? meta = null) {
        if (status != ServiceOperationStatus.Succeeded &&
            status != ServiceOperationStatus.Created &&
            status != ServiceOperationStatus.Updated &&
            status != ServiceOperationStatus.Deleted)
            throw new ArgumentException("Success method can only be called with success status.", nameof(status));

        return new(status, message, null, meta);
    }

    public static ServiceOperationResult Failure(ServiceOperationStatus status, string? message = null, string? errorCode = null, object? meta = null) {
        if (status == ServiceOperationStatus.Succeeded || status == ServiceOperationStatus.Created || status == ServiceOperationStatus.Updated || status == ServiceOperationStatus.Deleted)
            throw new ArgumentException("Failure method cannot be called with success status.", nameof(status));

        return new(status, message, errorCode, meta);
    }

    private static string GetDefaultMessage(ServiceOperationStatus status) => status switch {
        ServiceOperationStatus.Succeeded => "Operation completed successfully.",
        ServiceOperationStatus.Created => "Resource created successfully.",
        ServiceOperationStatus.Updated => "Resource updated successfully.",
        ServiceOperationStatus.Deleted => "Resource deleted successfully.",
        ServiceOperationStatus.NotFound => "The requested resource was not found.",
        ServiceOperationStatus.AlreadyExists => "This record already exists.",
        ServiceOperationStatus.Unauthorized => "Unauthorized access. Please login.",
        ServiceOperationStatus.Forbidden => "You do not have permission to perform this action.",
        ServiceOperationStatus.InvalidParameters => "The provided parameters are invalid.",
        _ => "An unexpected error occurred during the operation."
    };
}

public class ServiceOperationResult<T> : ServiceOperationResult {
    public T? Data { get; }

    private ServiceOperationResult(ServiceOperationStatus status, T? data = default, string? message = null, string? errorCode = null, object? meta = null)
        : base(status, message, errorCode, meta) {
        var isSuccessStatus = status == ServiceOperationStatus.Succeeded ||
                              status == ServiceOperationStatus.Created ||
                              status == ServiceOperationStatus.Updated ||
                              status == ServiceOperationStatus.Deleted;

        if (isSuccessStatus && data == null && typeof(T).IsValueType && Nullable.GetUnderlyingType(typeof(T)) == null)
            throw new ArgumentNullException(nameof(data), "Success result for a non-nullable value type must have data.");

        Data = data;
    }

    public static ServiceOperationResult<T> Success(T? data, ServiceOperationStatus status = ServiceOperationStatus.Succeeded, string? message = null, object? meta = null) {
        if (status != ServiceOperationStatus.Succeeded &&
            status != ServiceOperationStatus.Created &&
            status != ServiceOperationStatus.Updated &&
            status != ServiceOperationStatus.Deleted)
            throw new ArgumentException("Success method can only be called with success status.", nameof(status));

        if (data == null && typeof(T).IsValueType && Nullable.GetUnderlyingType(typeof(T)) == null)
            throw new ArgumentNullException(nameof(data), "Cannot create a successful result with null data for non-nullable value types.");

        return new(status, data, message, null, meta);
    }

    public new static ServiceOperationResult<T> Failure(ServiceOperationStatus status, string? message = null, string? errorCode = null, object? meta = null) {
        if (status == ServiceOperationStatus.Succeeded || status == ServiceOperationStatus.Created || status == ServiceOperationStatus.Updated || status == ServiceOperationStatus.Deleted)
            throw new ArgumentException("Failure method cannot be called with success status.", nameof(status));
        return new(status, default, message, errorCode, meta);
    }
}
