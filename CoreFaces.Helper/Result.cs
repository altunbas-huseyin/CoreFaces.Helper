using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreFaces.Helper
{
    public class Result<TEntity>
    {
        public bool Status { get; set; }
        public string Text { get; set; }
        public TEntity Data { get; set; }
        public string Type { get; set; } = "error";

        public List<ValidationFailure> ErrorList { get; set; }
        public Result()
        {
            ErrorList = new List<ValidationFailure>();
        }

        public Result(TEntity data)
        {
            Data = data;
        }

        public Result(TEntity data, bool status)
        {
            Data = data;
            Status = status;
        }
        public void AddError(string error)
        {
            ErrorList.Add(new ValidationFailure("", error));
        }
    }

    public class ResultByList<TEntity>
    {
        public bool Status { get; set; }
        public string Text { get; set; }
        public List<TEntity> Data { get; set; }
        public List<ValidationFailure> ErrorList { get; set; }
        public ResultByList()
        {
            ErrorList = new List<ValidationFailure>();
        }
        public ResultByList(List<TEntity> data)
        {
            Data = data;
        }
        public ResultByList(List<TEntity> data, bool status)
        {
            Data = data;
            Status = status;
        }
        public void AddError(string error)
        {
            ErrorList.Add(new ValidationFailure("", error));
        }
    }

}
