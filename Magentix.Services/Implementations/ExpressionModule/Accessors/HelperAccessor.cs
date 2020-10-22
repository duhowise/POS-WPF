﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magentix.Infrastructure.Helpers;

namespace Magentix.Services.Implementations.ExpressionModule.Accessors
{
    public static class HelperAccessor
    {
        public static string GetUniqueString()
        {
           return Utility.GetDateBasedUniqueString();
        }

        public static string GetDateTime()
        {
            var date = DateTime.Now;
            return date.ToShortDateString() + " " + date.ToShortTimeString();
        }
    }
}