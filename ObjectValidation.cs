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
			String name;

			public Pessoa(String name)
			{
				this.name = name;
			}

			public String getName()
			{
				return name;
			}
		}

		/// <summary>
		/// Representa uma pessoa física
		/// </summary>
		class PessoaFisica: Pessoa
		{
			public PessoaFisica(String name)
				: base(name)
			{
			}
		}

		/// <summary>
		/// Representa uma pessoa jurídica
		/// </summary>
		class PessoaJuridica: Pessoa
		{
			public PessoaJuridica(String name)
				: base(name)
			{
			}
		}
	}

	namespace Validations
	{


		/// <summary>
		/// Interface para permitir que objetos sejam validados.
		/// </summary>
		interface IValidatable<T> where T: class
		{
			void Validate(T o) ;
		}

		/// <summary>
		/// Validação para pessoa.
		/// </summary>
		class PessoaValidator: IValidatable<Pessoa>
		{

			public void Validate(Pessoa o)
			{
				Console.WriteLine(o.getName() + '-' + this.GetType().Name);
			}
		}

		/// <summary>
		/// Validação para uma pessoa física
		/// </summary>
		class PessoaFisicaValidator: IValidatable<PessoaFisica>
		{

			public void Validate(PessoaFisica o)
			{
				Console.WriteLine(o.getName() + '-' + this.GetType().Name);
			}
		}

		/// <summary>
		/// Validacão para um pessoa jurídica
		/// </summary>
		class PessoaJuridicaValidator: IValidatable<PessoaJuridica>
		{
			public void Validate(PessoaJuridica o)
			{
				Console.WriteLine(o.getName() + '-' + this.GetType().Name);
			}
		}

		/// <summary>
		/// Estratégias de validação
		/// </summary>
		class ValidatorStrategy
		{
			Dictionary<Type, object> map = null;

			public ValidatorStrategy()
			{

				// Cadastro dos vários tipos de validadores
				// para cara tipo de entidade.
				map = new Dictionary<Type, object>();
				map.Add(typeof(Pessoa), new PessoaValidator());
				map.Add(typeof(PessoaFisica), new PessoaFisicaValidator());
				map.Add(typeof(PessoaJuridica), new PessoaJuridicaValidator());
			}

			public IValidatable<T> Get<T>() where T: class
			{
				return map[typeof(T)] as IValidatable<T>;
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
			pessoas.Add(new Pessoa("Uma pessoa simples"));
			pessoas.Add(new PessoaFisica("Joãoo"));
			pessoas.Add(new PessoaFisica("José"));
			pessoas.Add(new PessoaJuridica("Maria"));

			var validator = new ValidatorStrategy();
			foreach (var p in pessoas) {
				var v = validator.Get(p.GetType());
				v.Validate(p);

			}

		}
	}
}

