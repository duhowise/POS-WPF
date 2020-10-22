﻿using Magentix.Infrastructure.Data;
using Magentix.Infrastructure.Data.Validation;

namespace Magentix.Persistance.Specification
{
    public class NonDuplicateSaveValidator<T> : SpecificationValidator<T> where T : class, IEntityClass
    {
        private readonly string _errorMessage;

        public NonDuplicateSaveValidator(string errorMessage)
        {
            _errorMessage = errorMessage;
        }

        public override string GetErrorMessage(T model)
        {
            return EntitySpecifications.EntityDuplicates(model).Exists() ? _errorMessage : "";
        }
    }
}