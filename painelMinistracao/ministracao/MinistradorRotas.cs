using Microsoft.EntityFrameworkCore;

namespace painelMinistracao;

public static class MinistradorRotas
{
    public static void AddRotasMinistradores(this WebApplication app)
    {
        var rotasMinistradores = app.MapGroup("ministrador");

        rotasMinistradores.MapPost("", async (AddMinistradorRequest request,AppDbContext context, CancellationToken ct) => 
         {
            var novoMinistrador = new Ministrador(request.Nome,request.Foto);

            var cadastrado = await context.Ministradores
                .AnyAsync(ministrador => ministrador.Nome == request.Nome, ct);

            if (cadastrado) return Results.Conflict("Já Existe!");
            await context.Ministradores.AddAsync(novoMinistrador);
            await context.SaveChangesAsync(ct);


            var ministradorRetorno = new MinistradorDto(novoMinistrador.Id,novoMinistrador.Nome);
            return Results.Ok(ministradorRetorno);
         });

         rotasMinistradores.MapGet("", async (AppDbContext context, CancellationToken ct) => 
         {
            var ministradores = await context
            .Ministradores
            .Select(ministrador => new MinistradorDto(ministrador.Id,ministrador.Nome))
            .ToListAsync(ct);

            return ministradores;
         });

         rotasMinistradores.MapPut("{id:guid}", async (Guid id, UpdateMinistradorRequest request, AppDbContext context, CancellationToken ct) => 
         {
            var ministrador = await context.Ministradores.SingleOrDefaultAsync(Ministrador => Ministrador.Id == id, ct);
            if(ministrador == null)
               return Results.NotFound(); 

            ministrador.AtualizaNome(request.Nome);
            await context.SaveChangesAsync(ct);
               
            return Results.Ok(new MinistradorDto(ministrador.Id,ministrador.Nome));
         });

         rotasMinistradores.MapDelete("{id:guid}",async (Guid id, AppDbContext context, CancellationToken ct) =>{
            var ministrador = await context.Ministradores.SingleOrDefaultAsync(ministrador => ministrador.Id == id, ct);
            
            if(ministrador == null)
               return Results.NotFound();

            context.Remove(ministrador); 
            await context.SaveChangesAsync(ct);

            return Results.Ok();
         });
    }
}
