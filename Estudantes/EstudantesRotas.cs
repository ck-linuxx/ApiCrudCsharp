using ApiCrud.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiCrud.Estudantes
{
    public static class EstudantesRotas
    {
        public static void AddRotasEstudantes(this WebApplication app)
        {
            var rotasEstudantes = app.MapGroup(prefix: "estudantes");


            rotasEstudantes.MapPost("", handler:async (AddEstudanteRequest request, AppDBContext context) => 
            {

                var estudanteJaExistente = await context.Estudantes.AnyAsync(estudante => estudante.Nome == request.Nome);

                if (estudanteJaExistente)
                    return Results.Conflict(error: "Estudante já existe!");

                var novoEstudante = new Estudante(request.Nome);
                await context.Estudantes.AddAsync(novoEstudante);
                await context.SaveChangesAsync();

                return Results.Ok(novoEstudante);
            });

            rotasEstudantes.MapGet("", handler:async (AppDBContext context) => 
            {
                var estudantes = await context.Estudantes
                .Where(estudante => estudante.Ativo)
                .ToListAsync();
                return estudantes;
            });
        }
    }
}
