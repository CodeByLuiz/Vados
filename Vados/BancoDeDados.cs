using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Vados
{
    internal class BancoDeDados
    {
        public class HistoryEntry
        {
            public required DateTime data { get; set; }
            public required int id { get; set; }   
            public required string comando { get; set; }
            public required List<string> pastas { get; set; }

        }

        public class DbConnection : DbContext
        {
            public DbSet<HistoryEntry> Historico { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlite("Data Source=historico.db");
            }

        }

    }
}
