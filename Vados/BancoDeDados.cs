using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.Sqlite;

namespace Vados
{
    internal class BancoDeDados
    {

        private static readonly string UUIDComando = "computador_id.txt";
        public class HistoryEntry
        {
            [Key] public int Id { get; set; }
            public Guid ComputadorId { get; set; }  
            public DateTime Data { get; set; }
             
            public string Comando { get; set; }

            public string PastasJson { get; set; }  // Armazenado no BD pq o sqlite n aceita lista normal :(
            [NotMapped]
            public List<string> Pastas
            {
                get => JsonSerializer.Deserialize<List<string>>(PastasJson ?? "[]")!;
                set => PastasJson = JsonSerializer.Serialize(value);
            }
            public HistoryEntry()
            {
                Data = DateTime.Now;
                ComputadorId = ObterComputadorId();
            }
        }

        public class DbConnection : DbContext
        {
            public DbSet<HistoryEntry> Historico { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlite("Data Source=historico.db");
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<HistoryEntry>()
                    .HasKey(h => h.Id);

                modelBuilder.Entity<HistoryEntry>()
                    .Property(h => h.Id)
                    .ValueGeneratedOnAdd();

                modelBuilder.Entity<HistoryEntry>()
                    .Property(h => h.ComputadorId)
                    .HasConversion(
                        v => v.ToString(),
                        v => Guid.Parse(v)
                    );
            }

        }
        private static Guid ObterComputadorId()
        {
            if (File.Exists(UUIDComando))
            {
                var guidString = File.ReadAllText(UUIDComando);
                if (Guid.TryParse(guidString, out Guid guid))
                    return guid;
            }

            // Se não existe ou é inválido, cria novo e salva
            var novoGuid = Guid.NewGuid();
            File.WriteAllText(UUIDComando, novoGuid.ToString());
            return novoGuid;
        }

        public static void AdicionarEntrada(string comando, List<string> pastas)
        {
            using (var db = new DbConnection())
            {
                db.Database.EnsureCreated();

                

                var novaEntrada = new HistoryEntry
                {
                    //Data = data,
                    Comando = comando,
                    Pastas = pastas,
                    
                };

                db.Historico.Add(novaEntrada);
                db.SaveChanges();

                MessageBox.Show($"Entrada adicionada com id={novaEntrada.Id} e ComputadorId={novaEntrada.ComputadorId}");



                //BancoDeDados.AdicionarEntrada(
                //comando: "mkdir novaPasta",
                //pastas: new List<string> { "C:\\Projetos", "D:\\Backup" }
                // negocio pra colocar dentro do treco de executar comandos
            }
        }

         
        public static void ListarEntradas() //pra mostrar todas as tabelase os bglh dentro se dar certo, dps pode tirar
        {
            using (var db = new DbConnection())
            {
                db.Database.EnsureCreated();

                var entradas = db.Historico.OrderBy(e => e.Data).ToList();

                if (entradas.Count == 0)
                {
                    MessageBox.Show("Nenhuma entrada encontrada.");
                    return;
                }

                foreach (var entrada in entradas)
                {
                    MessageBox.Show($"-----------\n ID: {entrada.Id}\n ComputadorId: {entrada.ComputadorId}\n Data: {entrada.Data}\n Comando: {entrada.Comando}\n Pastas:");
                    //MessageBox.Show($"ID: {entrada.Id}");
                   // MessageBox.Show($"ComputadorId: {entrada.ComputadorId}");
                    //MessageBox.Show($"Data: {entrada.Data}");
                    //MessageBox.Show($"Comando: {entrada.Comando}");
                    //MessageBox.Show($"Pastas:");
                    foreach (var pasta in entrada.Pastas)
                    {
                        MessageBox.Show($"  - {pasta}\n");
                    }
                }
            }
        }

    }
}
