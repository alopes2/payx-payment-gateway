using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace PayX.Data.Configurations
{
    public class GuidValueGenerator : ValueGenerator
    {
        public override bool GeneratesTemporaryValues => false;

        protected override object NextValue([NotNullAttribute] EntityEntry entry)
        {
            return Guid.NewGuid();
        }
    }
}