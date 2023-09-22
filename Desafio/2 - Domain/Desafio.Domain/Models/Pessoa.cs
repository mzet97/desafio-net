namespace Desafio.Domain.Models
{
    public class Pessoa : Entity
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string CEP { get; set; }
        public string CPFCNPJ { get; set; }

        public Pessoa()
        {
            
        }

        public Pessoa(
            int id,
            string nome, 
            string sobrenome, 
            DateTime dataNascimento, 
            string email, 
            string telefone, 
            string endereco, 
            string cidade, 
            string estado, 
            string cEP, 
            string cPFCNPJ)
        {
            Id = id;
            Nome = nome;
            Sobrenome = sobrenome;
            DataNascimento = dataNascimento;
            Email = email;
            Telefone = telefone;
            Endereco = endereco;
            Cidade = cidade;
            Estado = estado;
            CEP = cEP;
            CPFCNPJ = cPFCNPJ;
        }
    }
}
