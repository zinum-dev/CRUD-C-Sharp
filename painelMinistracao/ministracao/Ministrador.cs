namespace painelMinistracao;

public class Ministrador
{
    public Guid Id { get; init; }
    public string Nome { get; private set; }
    public string Foto { get; private set; }

    public Ministrador(string nome, string foto){
        Nome = nome;
        Foto = foto;
        Id = Guid.NewGuid();
    }

    public void AtualizaNome(string nome)
    {
        Nome = nome;
    }
}
