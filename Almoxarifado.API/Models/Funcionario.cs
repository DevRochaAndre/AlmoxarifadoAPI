using System.ComponentModel.DataAnnotations;

namespace Almoxarifado.API.Models
{
    public class Funcionario
    {
        public int Id { get; set; } // Interno do MySQL (Primary Key)

        [Required(ErrorMessage = "A matrícula do funcionário é obrigatória.")]
        public int Matricula { get; set; } // Identificador corporativo da pessoa (Ex: 101, 102...)

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve ter 11 dígitos.")]
        public string Cpf { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O cargo é obrigatório.")]
        [StringLength(50)]
        public string Cargo { get; set; } = string.Empty;

        public bool Ativo { get; set; } = true;

        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    }
}