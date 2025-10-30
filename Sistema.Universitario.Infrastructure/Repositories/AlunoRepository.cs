using System;
using Sistema.Universitario.Domain.Entities;
using Sistema.Universitario.Infrastructure.Data;

namespace Sistema.Universitario.Infrastructure.Repositories;

public class AlunoRepository: BaseRepository<Aluno>, IAlunoRepository
{
    public AlunoRepository(SistemaUniversitarioDbContext context) : base(context){}
}
