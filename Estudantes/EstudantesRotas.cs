using ApiCrud.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ApiCrud.Estudantes
{
    public static class EstudantesRotas
    {
        public static void AddRotasEstudantes(this WebApplication app)
        {
            var rotasEstudantes = app.MapGroup(prefix: "estudantes");


            rotasEstudantes.MapPost("", handler:async (AddEstudanteRequest request, AppDBContext context, CancellationToken ct) => 
            {

                var estudanteJaExistente = await context.Estudantes.AnyAsync(estudante => estudante.Nome == request.Nome, ct);

                if (estudanteJaExistente)
                    return Results.Conflict(error: "Estudante já existe!");

                var novoEstudante = new Estudante(request.Nome);
                await context.Estudantes.AddAsync(novoEstudante, ct);
                await context.SaveChangesAsync(ct);

                var estudanteRetorno = new EstudanteDTO(novoEstudante.Id, novoEstudante.Nome);

                return Results.Ok(estudanteRetorno);
            });

            rotasEstudantes.MapGet("", handler:async (AppDBContext context, CancellationToken ct) => 
            {
                var estudantes = await context.Estudantes
                .Where(estudante => estudante.Ativo)
                .Select(estudante => new EstudanteDTO(estudante.Id, estudante.Nome))
                .ToListAsync(ct);
                return estudantes;
            });

            rotasEstudantes.MapPut("{id:guid}", async (
                Guid id, UpdateEstudanteRequest request, AppDBContext context, CancellationToken ct) => 
            {
                var estudante = await context.Estudantes
                    .SingleOrDefaultAsync(estudante => estudante.Id == id, ct);

                if (estudante == null)
                    return Results.NotFound();

                estudante.AtualizarNome(request.Nome);

                await context.SaveChangesAsync();
                return Results.Ok(new EstudanteDTO(estudante.Id, estudante.Nome));
            });

            rotasEstudantes.MapDelete("{id}", async (Guid id, AppDBContext context, CancellationToken ct) =>
            {
                var estudante = await context.Estudantes
                    .SingleOrDefaultAsync(estudante => estudante.Id == id, ct);

                if (estudante == null)
                    return Results.NotFound();

                estudante.Desativar();

                await context.SaveChangesAsync(ct);
                return Results.Ok();
            });
        }
    }
}
