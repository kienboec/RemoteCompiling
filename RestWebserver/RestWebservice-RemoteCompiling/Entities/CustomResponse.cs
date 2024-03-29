﻿namespace RestWebservice_RemoteCompiling.Entities
{
    public class CustomResponse
    {
        public int StatusCode
        {
            get;
            set;
        }

        public string ErrorMessage
        {
            get;
            set;
        }

        public bool IsSuccess => StatusCode >= 200 && StatusCode < 300;
        public virtual bool HasData { get; init; } = false;

        public virtual object GetData()
        {
            return null;
        }
        public static CustomResponse<T> Success<T>(T data)
        {
            return new CustomResponse<T>
                   { StatusCode = 200, Data = data };
        }

        public static CustomResponse<T> Success<T>(int status)
        {
            return new CustomResponse<T>
                   { StatusCode = status, HasData = false };
        }

        public static CustomResponse<T> Error<T>(int statusCode, string errorMessage = "")
        {
            return new() { ErrorMessage = errorMessage, StatusCode = statusCode };
        }

        public void BadRequest(string errorMessage)
        {
            StatusCode = 400;
            ErrorMessage = errorMessage;
        }
    }

    public class CustomResponse<T> : CustomResponse
    {
        public T Data
        {
            get;
            init;
        }

        public override bool HasData { get; init; } = true;
        public override object GetData()
        {
            return Data;
        }
    }
}