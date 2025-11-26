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
    }
}
