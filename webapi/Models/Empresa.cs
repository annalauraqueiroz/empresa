namespace webapi.Models
{
    public class Empresa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public Empresa(int id, string nome)
        {
            Id = id;
            Nome = nome;
        }
    }
}
