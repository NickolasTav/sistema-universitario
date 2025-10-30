using System;
using System.ComponentModel.DataAnnotations;

namespace Sistema.Universitario.Application.ViewModels;

public class ProfessorViewModel
{
    public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }
}
