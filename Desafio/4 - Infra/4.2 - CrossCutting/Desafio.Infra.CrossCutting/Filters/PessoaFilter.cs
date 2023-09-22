namespace Desafio.Infra.CrossCutting.Filters
{
    public class PessoaFilter : BaseFilter
    {
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
    }
}
