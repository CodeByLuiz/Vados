using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.Sqlite;

namespace Vados
{
    public class BancoDeDados
    {

        public static readonly string UUIDComando = "computador_id.txt";
        public class HistoryEntry
        {
            [Key] public int Id { get; set; }
            public Guid ComputadorId { get; set; }  
            public DateTime Data { get; set; }
            public string Comandotitle { get; set; }
            public string Comando { get; set; }

            
            public HistoryEntry() // pega automaticamente a data e o guid
            {
                Data = DateTime.Now; 
                ComputadorId = ObterComputadorId();
            }
        }

        public class DbConnection : DbContext
        {
            public DbSet<HistoryEntry> Historico { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) // define que sqlite sera usado para criar o banco
            {
                optionsBuilder.UseSqlite("Data Source=historico.db");
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder) // "modela" o banco de dados
            {
                modelBuilder.Entity<HistoryEntry>()
                    .HasKey(h => h.Id); //define id como chave primaria

                modelBuilder.Entity<HistoryEntry>()
                    .Property(h => h.Id)
                    .ValueGeneratedOnAdd(); // gera valor automaticamente

                modelBuilder.Entity<HistoryEntry>()
                    .Property(h => h.ComputadorId)
                    .HasConversion(
                        v => v.ToString(), // salvando banco como string
                        v => Guid.Parse(v) // converte de volta para guid
                    );
            }

        }
        public static Guid ObterComputadorId()
        {
            if (File.Exists(UUIDComando))
            {
                var guidString = File.ReadAllText(UUIDComando);
                if (Guid.TryParse(guidString, out Guid guid))
                    return guid;
            }

            var novoGuid = Guid.NewGuid();
            File.WriteAllText(UUIDComando, novoGuid.ToString());
            return novoGuid;
        }

        public static void AdicionarEntrada(string comando, string titulo)
        {
            using (var db = new DbConnection())
            {
                db.Database.EnsureCreated();

                

                var novaEntrada = new HistoryEntry
                {
                    //Data = data,
                    Comando = comando,
                    Comandotitle = titulo
                    
                    
                };

                db.Historico.Add(novaEntrada);
                db.SaveChanges();

                Console.WriteLine($"Entrada adicionada com id={novaEntrada.Id} e ComputadorId={novaEntrada.ComputadorId}");



                //BancoDeDados.AdicionarEntrada(
                //comando: "mkdir novaPasta",
                //pastas: new List<string> { "C:\\Projetos", "D:\\Backup" }
                // negocio pra colocar dentro do treco de executar comandos
            }
        }

         
        public static void ListarTodasEntradas() //pra mostrar todas as tabelase os bglh dentro se dar certo, dps pode tirar
        {
            using (var db = new DbConnection())
            {
                db.Database.EnsureCreated();

                var entradas = db.Historico.OrderBy(e => e.Data).ToList();

                if (entradas.Count == 0)
                {
                    //MessageBox.Show("Nenhuma entrada encontrada.");
                    return;
                }

                foreach (var entrada in entradas)
                {
                    //MessageBox.Show($"-----------\n ID: {entrada.Id}\n ComputadorId: {entrada.ComputadorId}\n Data: {entrada.Data}\n Comando: {entrada.Comando}\n Pastas:");
                    //MessageBox.Show($"ID: {entrada.Id}");
                   // MessageBox.Show($"ComputadorId: {entrada.ComputadorId}");
                    //MessageBox.Show($"Data: {entrada.Data}");
                    //MessageBox.Show($"Comando: {entrada.Comando}");
                   
                }
            }
        }

        public static void DeleteEntryID(int id)
        {
            using (var db = new DbConnection())
            {
                db.Database.EnsureCreated();

                var entrada = db.Historico.FirstOrDefault(e => e.Id == id);
                if (entrada == null)
                {                    return;
                }

                db.Historico.Remove(entrada);
                db.SaveChanges();
            }
        }


    }
}
