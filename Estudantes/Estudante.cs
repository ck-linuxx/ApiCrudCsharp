﻿namespace ApiCrud.Estudantes
{
    public class Estudante
    {
        //init não deixa ser alterado
        public Guid Id { get; init; }
        public string Nome { get; private set; }
        public bool Ativo { get; private set; }

        public Estudante(string nome)
        {
            Nome = nome;
            Id = Guid.NewGuid();
            Ativo = true;
        }


    }
}
