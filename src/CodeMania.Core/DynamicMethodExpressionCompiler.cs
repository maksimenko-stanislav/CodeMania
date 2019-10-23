#if !NETSTANDARD2_0 && !NETCOREAPP

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace CodeMania.Core
{

	[SuppressMessage("ReSharper", "RedundantToStringCallForValueType")]
	public class DynamicMethodExpressionCompiler : IExpressionCompiler
	{
		private static readonly DefaultExpressionCompiler FallbackCompiler = DefaultExpressionCompiler.Instance;
		private readonly ModuleBuilder moduleBuilder;

#if DEBUG
		private readonly AssemblyBuilder assemblyBuilder;
#endif

		public DynamicMethodExpressionCompiler(string assemblyNameSuffix = null)
			: this(CreateAssemblyBuilder(assemblyNameSuffix))
		{
		}

		private static AssemblyBuilder CreateAssemblyBuilder(string assemblyNameSuffix)
		{
			var assemblyBuilderAccess =
#if DEBUG
				AssemblyBuilderAccess.RunAndSave;
#else
				AssemblyBuilderAccess.Run;
#endif

			return AppDomain.CurrentDomain.DefineDynamicAssembly(
				new AssemblyName("DynamicAssembly_" + (assemblyNameSuffix ?? Guid.NewGuid().ToString("N"))), assemblyBuilderAccess);
		}


		public DynamicMethodExpressionCompiler(AssemblyBuilder assemblyBuilder)
			: this(CheckForNull(assemblyBuilder, nameof(assemblyBuilder)).DefineDynamicModule(assemblyBuilder.FullName))
		{
#if DEBUG
			this.assemblyBuilder = assemblyBuilder;
#endif
		}

		public DynamicMethodExpressionCompiler(ModuleBuilder moduleBuilder)
		{
			this.moduleBuilder = moduleBuilder;
		}

		public TDelegate Compile<TDelegate>(Expression<TDelegate> expression)
			where TDelegate : class
		{
			if (expression == null) throw new ArgumentNullException(nameof(expression));

			try
			{
				var typeBuilder = moduleBuilder.DefineType("Dynamic.DynamicType_" + Guid.NewGuid().ToString("N"), TypeAttributes.Public);

				var methodBuilder = typeBuilder.DefineMethod("DynamicMethod_" + Guid.NewGuid().ToString("N"), MethodAttributes.Public | MethodAttributes.Static);

				expression.CompileToMethod(methodBuilder);

				var type = typeBuilder.CreateTypeInfo();

				var methodInfo = type.GetMethod(methodBuilder.Name);

				if (methodInfo == null)
				{
					throw new InvalidOperationException($"Can't get method {methodBuilder.Name} of type {type.FullName}");
				}

				// Force method JIT compilation for debug purposes, we need fail as soon as possible if something goes wrong
				System.Runtime.CompilerServices.RuntimeHelpers.PrepareMethod(methodInfo.MethodHandle);

				return (TDelegate) (object) Delegate.CreateDelegate(typeof(TDelegate), methodInfo);
			}
			catch (Exception)
			{
				return FallbackCompiler.Compile(expression);
			}
		}

		public Delegate Compile(Type delegateType, LambdaExpression expression)
		{
			if (delegateType == null) throw new ArgumentNullException(nameof(delegateType));
			if (expression == null) throw new ArgumentNullException(nameof(expression));
			if (!typeof(Delegate).IsAssignableFrom(delegateType))
				throw new ArgumentException("Type is not delegate.", nameof(delegateType));

			try
			{
				var typeBuilder = moduleBuilder.DefineType("Dynamic.DynamicType_" + Guid.NewGuid().ToString("N"), TypeAttributes.Public);

				var methodBuilder = typeBuilder.DefineMethod("DynamicMethod_" + Guid.NewGuid().ToString("N"), MethodAttributes.Public | MethodAttributes.Static);

				expression.CompileToMethod(methodBuilder);

				var type = typeBuilder.CreateTypeInfo();

				var methodInfo = type.GetMethod(methodBuilder.Name);

				if (methodInfo == null)
				{
					throw new InvalidOperationException($"Can't get method {methodBuilder.Name} of type {type.FullName}");
				}

				// Force method JIT compilation for debug purposes, we need fail as soon as possible if something goes wrong
				System.Runtime.CompilerServices.RuntimeHelpers.PrepareMethod(methodInfo.MethodHandle);

				return Delegate.CreateDelegate(delegateType, methodInfo);
			}
			catch (Exception)
			{
				return FallbackCompiler.Compile(delegateType, expression);
			}
		}

		private static T CheckForNull<T>(T value, string paramName)
			where T : class
		{
			if (value == null) throw new ArgumentNullException(paramName);

			return value;
		}
	}
}

#endif
