using System;
using System.Collections;
using System.Collections.Generic;

using Csharp_playground.Model;
using Csharp_playground.Validations;
using System.Linq;


//
// Este exemplo foi criado para exemplificar o padrão strategy
//
namespace Csharp_playground
{

    /// <summary>
    /// Classes de modelo.
    /// </summary>
    namespace Model
    {
        /// <summary>
        /// Representa uma pessoa
        /// </summary>
        class Pessoa
        {
            public string Name { get; set; }

            public Pessoa(string name)
            {
                this.Name = name;
            }
        }

        /// <summary>
        /// Representa uma pessoa física
        /// </summary>
        class PessoaFisica : Pessoa
        {
            public string Cpf { get; set; }

            public PessoaFisica(String name, string cpf)
                : base(name)
            {
                this.Cpf = cpf;
            }

        }

        /// <summary>
        /// Representa uma pessoa jurídica
        /// </summary>
        class PessoaJuridica : Pessoa
        {
            public string Cnpj { get; set; }

            public PessoaJuridica(String name, string cnpj)
                : base(name)
            {
                this.Cnpj = cnpj;
            }
        }
    }

    namespace Validations
    {

        /// <summary>
        /// Interface base de validação.
        /// </summary>
        interface IValidatable
        {
            Dictionary<object, string> ValidationMessages { get; set; }
            void Validate(object o);
        }

        /// <summary>
        /// Interface para permitir que objetos sejam validados.
        /// </summary>
        abstract class Validatable<T> : IValidatable where T : class
        {
            public Dictionary<object, string> ValidationMessages { get; set; }
            internal abstract void Validate(T o);

            public Validatable()
            {
                ValidationMessages = new Dictionary<object, string>();
            }

            public void Validate(object o)
            {
                this.Validate(o as T);
            }
        }

        /// <summary>
        /// Validação para pessoa.
        /// </summary>
        class PessoaValidator : Validatable<Pessoa>
        {

            internal override void Validate(Pessoa o)
            {
                Console.WriteLine(o.Name + '-' + this.GetType().Name);
            }
        }

        /// <summary>
        /// Validação para uma pessoa física
        /// </summary>
        class PessoaFisicaValidator : Validatable<PessoaFisica>
        {

            internal override void Validate(PessoaFisica o)
            {
                Console.WriteLine(o.Name + '-' + this.GetType().Name);
                checkCpf(o);
            }


            void checkCpf(PessoaFisica o)
            {
                // "333.333.333-33"
                if (o.Cpf.Length != 13)
                    base.ValidationMessages.Add(this, "O CPF não contém o tamanho correto.");
            }

        }

        /// <summary>
        /// Validacão para um pessoa jurídica
        /// </summary>
        class PessoaJuridicaValidator : Validatable<PessoaJuridica>
        {
            internal override void Validate(PessoaJuridica o)
            {
                Console.WriteLine(o.Name + '-' + this.GetType().Name);
                isCnpjValid(o);
            }

            void isCnpjValid(PessoaJuridica o)
            {
                //  99.999.999/9999-99
                if (o.Cnpj.Length != 18)
                    base.ValidationMessages.Add(this, "O tamanho do CNPJ é inválido");
            }
        }

        /// <summary>
        /// Estratégias de validação
        /// </summary>
        class ValidatableStrategy
        {
            Dictionary<Type, IValidatable> map = null;

            public ValidatableStrategy()
            {

                // Cadastro dos vários tipos de validadores
                // para cara tipo de entidade.
                map = new Dictionary<Type, IValidatable>();
                map.Add(typeof(Pessoa), new PessoaValidator());
                map.Add(typeof(PessoaFisica), new PessoaFisicaValidator());
                map.Add(typeof(PessoaJuridica), new PessoaJuridicaValidator());
            }

            public IValidatable Get(Type validateFor)
            {
                return map[validateFor] as IValidatable;
            }
        }
    }


    /// <summary>
    /// Exemplo de validação de objeto.
    /// </summary>
    public class ObjectValidation
    {
        public void Main()
        {
            var pessoas = new List<Pessoa>();
            pessoas.Add(new Pessoa("PessoaBase"));
            pessoas.Add(new PessoaFisica("João", "333.333.33-33"));
            pessoas.Add(new PessoaFisica("José", "333.333.333-33"));
            pessoas.Add(new PessoaJuridica("Maria", "999.99/9999-99"));


            var strategies = new ValidatableStrategy();

            foreach (var p in pessoas)
            {
                var validator = strategies.Get(p.GetType());
                validator.Validate(p);


                if (validator.ValidationMessages.Any())
                {
                    Console.WriteLine("Mensagens de validação:");
                    foreach (var item in validator.ValidationMessages)
                        Console.WriteLine("\t{0} - {1}", item.Key.GetType().Name, item.Value);
                }

                Console.WriteLine();

            }





        }
    }
}

