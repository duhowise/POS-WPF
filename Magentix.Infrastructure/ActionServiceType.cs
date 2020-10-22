using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magentix.Infrastructure
{
    public enum ActionServiceType
    {
        Migration,
        PreMigration,
        PostMigration,
        PostDatabaseInitialization
    }
}
