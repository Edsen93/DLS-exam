using System;
using System.Collections.Generic;
using System.Text;

namespace Neo4jTests
{
    class Settings
    {
        public static string FluentlyConfigString { get; private set; } = "server=http://localhost:7474/db/data/;User Id=neo4j;password=password";
    }
}
