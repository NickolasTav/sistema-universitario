using System;
using Mapster;
using Sistema.Universitario.Application.ViewModels;
using Sistema.Universitario.Domain.Entities;

namespace Sistema.Universitario.Application.Mapping;

public static class MapsterConfig
{
    public static void Register()
    {
        var config = TypeAdapterConfig.GlobalSettings;

        config.NewConfig<Aluno, AlunoViewModel>()
            .Map(dest => dest.CursoId, src => src.CursoId)
            .IgnoreNullValues(true);

        config.NewConfig<AlunoViewModel, Aluno>()
            .Map(dest => dest.Id, src => src.Id == Guid.Empty ? Guid.NewGuid() : src.Id)
            .IgnoreNullValues(true);

        // Curso
        config.NewConfig<Curso, CursoViewModel>()
            .IgnoreNullValues(true);

        config.NewConfig<CursoViewModel, Curso>()
            .Map(dest => dest.Id, src => src.Id == Guid.Empty ? Guid.NewGuid() : src.Id)
            .IgnoreNullValues(true);

        // Professor
        config.NewConfig<Professor, ProfessorViewModel>()
            .IgnoreNullValues(true);

        config.NewConfig<ProfessorViewModel, Professor>()
            .Map(dest => dest.Id, src => src.Id == Guid.Empty ? Guid.NewGuid() : src.Id)
            .IgnoreNullValues(true);

        // Materia
        config.NewConfig<Materia, MateriaViewModel>()
            .Map(dest => dest.CursoId, src => src.CursoId)
            .Map(dest => dest.ProfessorId, src => src.ProfessorId)
            .IgnoreNullValues(true);

        config.NewConfig<MateriaViewModel, Materia>()
            .Map(dest => dest.Id, src => src.Id == Guid.Empty ? Guid.NewGuid() : src.Id)
            .Map(dest => dest.CursoId, src => src.CursoId ?? Guid.Empty)
            .Map(dest => dest.ProfessorId, src => src.ProfessorId)
            .IgnoreNullValues(true);
    }
}
