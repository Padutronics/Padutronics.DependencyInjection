using Padutronics.DependencyInjection.Resolution.Activation.ValueProviders;
using Padutronics.Extensions.System;
using Padutronics.Extensions.System.Collections.Generic;
using Padutronics.Reflection.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Padutronics.DependencyInjection.Resolution.Activation.Activators;

internal sealed class FactoryActivator : IActivator
{
    private static readonly Lazy<ModuleBuilder> moduleBuilder = new(CreateModuleBuilder);

    private readonly Type interfaceType;
    private readonly IDictionary<Type, ProxyFactory> serviceTypeToProxyFactoryMappings = new Dictionary<Type, ProxyFactory>();

    public FactoryActivator(Type interfaceType)
    {
        if (!interfaceType.IsInterface)
        {
            throw new ArgumentException($"Type {interfaceType} is not an interface.", nameof(interfaceType));
        }

        this.interfaceType = interfaceType;
    }

    private static ModuleBuilder CreateModuleBuilder()
    {
        AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
            new AssemblyName(DynamicAssembly.Name),
            AssemblyBuilderAccess.Run
        );

        return assemblyBuilder.DefineDynamicModule("Module");
    }

    private void AddConstructor(TypeBuilder typeBuilder, FieldBuilder fieldBuilder)
    {
        ConstructorInfo? objectConstructor = typeof(object).GetDefaultConstructor();
        if (objectConstructor is null)
        {
            throw new InvalidOperationException("Default constructor is not found.");
        }

        ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(
            MethodAttributes.Public,
            CallingConventions.Standard,
            parameterTypes: TypeArray.Create<IContainerContext>()
        );

        ILGenerator generator = constructorBuilder.GetILGenerator();
        generator.Emit(OpCodes.Ldarg_0);
        generator.Emit(OpCodes.Call, objectConstructor);
        generator.Emit(OpCodes.Ldarg_0);
        generator.Emit(OpCodes.Ldarg_1);
        generator.Emit(OpCodes.Stfld, fieldBuilder);
        generator.Emit(OpCodes.Ret);
    }

    public bool CanGetInstance(ActivationSession session)
    {
        return true;
    }

    private FieldBuilder CreateFieldBuilder(TypeBuilder typeBuilder)
    {
        return typeBuilder.DefineField(
            fieldName: "context",
            type: typeof(IContainerContext),
            FieldAttributes.InitOnly | FieldAttributes.Private
        );
    }

    private ProxyFactory CreateProxyFactory(Type interfaceType)
    {
        Type proxyType = CreateProxyType(interfaceType);

        ConstructorInfo? proxyConstructor = proxyType.GetConstructor<IContainerContext>();
        if (proxyConstructor is null)
        {
            throw new InvalidOperationException($"Constructor that receives {typeof(IContainerContext)} is not found.");
        }

        var proxyFactoryMethod = new DynamicMethod(
            name: string.Empty,
            returnType: proxyType,
            parameterTypes: TypeArray.Create<IContainerContext>(),
            owner: proxyType
        );

        ILGenerator generator = proxyFactoryMethod.GetILGenerator();
        generator.DeclareLocal(proxyType);
        generator.Emit(OpCodes.Ldarg_0);
        generator.Emit(OpCodes.Newobj, proxyConstructor);
        generator.Emit(OpCodes.Stloc_0);
        generator.Emit(OpCodes.Ldloc_0);
        generator.Emit(OpCodes.Ret);

        return new ProxyFactory(proxyType, proxyFactoryMethod.CreateDelegate<Func<IContainerContext, object>>());
    }

    private Type CreateProxyType(Type interfaceType)
    {
        // Client source code:
        //  public interface IServiceFactory
        //  {
        //      Service CreateService(int value1, double value2);
        //  }
        //
        // Generated source code:
        //  internal sealed class ServiceFactory : IServiceFactory
        //  {
        //      private readonly IContainerContext context;
        //
        //      public ServiceFactory(IContainerContext context)
        //      {
        //          this.context = context;
        //      }
        //
        //      Service IServiceFactory.CreateService(int value1, double value2)
        //      {
        //          return context.Resolve<Service>(
        //              new IParameter[]
        //              {
        //                  new NamedParameter<int>(new ParameterTargetName(nameof(value1)), value1),
        //                  new NamedParameter<double>(new ParameterTargetName(nameof(value2)), value2)
        //              }
        //          );
        //      }
        //  }

        TypeBuilder typeBuilder = CreateTypeBuilder(interfaceType);
        FieldBuilder fieldBuilder = CreateFieldBuilder(typeBuilder);

        AddConstructor(typeBuilder, fieldBuilder);
        ImplementInterface(interfaceType, typeBuilder, fieldBuilder);

        try
        {
            return typeBuilder.CreateTypeInfo()
                ?? throw new InvalidOperationException("Type info is not created.");
        }
        catch (TypeLoadException exception) when (exception.Message.Contains("attempting to implement an inaccessible interface"))
        {
            throw new ArgumentException($"Type {interfaceType} is not accessible.", nameof(interfaceType));
        }
    }

    private TypeBuilder CreateTypeBuilder(Type interfaceType)
    {
        return moduleBuilder.Value.DefineType($"{interfaceType}Proxy_{Guid.NewGuid():N}", TypeAttributes.NotPublic | TypeAttributes.Sealed);
    }

    public object GetInstance(ActivationSession session)
    {
        ProxyFactory proxyFactory = GetProxyFactory(session.ServiceType);

        return proxyFactory.CreateInstance(session.ContainerContext);
    }

    public Type GetInstanceType(ActivationSession session)
    {
        return GetProxyFactory(session.ServiceType).Type;
    }

    private ProxyFactory GetProxyFactory(Type serviceType)
    {
        if (!serviceTypeToProxyFactoryMappings.TryGetValue(serviceType, out ProxyFactory? proxyFactory))
        {
            Type interfaceType = GetTargetInterfaceType(serviceType);

            proxyFactory = CreateProxyFactory(interfaceType);

            serviceTypeToProxyFactoryMappings.Add(serviceType, proxyFactory);
        }

        return proxyFactory;
    }

    private Type GetTargetInterfaceType(Type serviceType)
    {
        return interfaceType.IsGenericTypeDefinition
            ? interfaceType.MakeGenericType(serviceType.GetGenericArguments())
            : interfaceType;
    }

    private void ImplementInterface(Type interfaceType, TypeBuilder typeBuilder, FieldBuilder fieldBuilder)
    {
        typeBuilder.AddInterfaceImplementation(interfaceType);

        foreach (MethodInfo interfaceMethod in interfaceType.GetMethods())
        {
            if (interfaceMethod.ReturnType == typeof(void))
            {
                throw new InvalidOperationException($"Factory type {interfaceMethod.DeclaringType} contains method {interfaceMethod} that returns void.");
            }

            ParameterInfo[] parameters = interfaceMethod.GetParameters();
            Type[] parameterTypes = interfaceMethod
                .GetParameters()
                .Select(parameter => parameter.ParameterType)
                .ToArray();

            MethodBuilder methodBuilder = typeBuilder.DefineMethod(
                interfaceMethod.Name,
                MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Private | MethodAttributes.Virtual,
                interfaceMethod.ReturnType,
                parameterTypes
            );

            if (interfaceMethod.IsGenericMethodDefinition)
            {
                Type[] genericArguments = interfaceMethod.GetGenericArguments();

                string[] typeArgumentNames = genericArguments
                    .Select(genericArgument => genericArgument.Name)
                    .ToArray();

                methodBuilder
                    .DefineGenericParameters(typeArgumentNames)
                    .ForEach((genericParameterBuilder, genericParameterBuilderIndex) =>
                    {
                        Type genericArgument = genericArguments[genericParameterBuilderIndex];

                        genericParameterBuilder.SetGenericParameterAttributes(genericArgument.GenericParameterAttributes);
                        genericParameterBuilder.SetInterfaceConstraints(genericArgument.GetGenericParameterConstraints());
                    });
            }

            ILGenerator generator = methodBuilder.GetILGenerator();
            generator.DeclareLocal(interfaceMethod.ReturnType);
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, fieldBuilder);
            generator.Emit(OpCodes.Ldc_I4, parameters.Length);
            generator.Emit(OpCodes.Newarr, typeof(IValueProvider));

            for (int index = 0; index < parameters.Length; ++index)
            {
                string parameterName = parameters[index].Name ?? throw new InvalidOperationException("Parameter name is null.");
                Type parameterType = parameters[index].ParameterType;

                Type namedValueProviderType = typeof(NamedValueProvider<>).MakeGenericType(parameterType);

                ConstructorInfo? namedValueProviderConstructor = namedValueProviderType.GetConstructor(new[] { typeof(string), parameterType });
                if (namedValueProviderConstructor is null)
                {
                    throw new InvalidOperationException($"Public instance constructor for type {parameterType} is not found.");
                }

                generator.Emit(OpCodes.Dup);
                generator.Emit(OpCodes.Ldc_I4, index);
                generator.Emit(OpCodes.Ldstr, parameterName);
                generator.Emit(OpCodes.Ldarg, index + 1);
                generator.Emit(OpCodes.Newobj, namedValueProviderConstructor);
                generator.Emit(OpCodes.Stelem_Ref);
            }

            MethodInfo? resolveMethodDefinition = typeof(IContainerContext).GetMethod<IValueProvider[]>(nameof(IContainerContext.Resolve));
            if (resolveMethodDefinition is null)
            {
                throw new InvalidOperationException("Resolve method is not found.");
            }

            MethodInfo resolveMethod = resolveMethodDefinition.MakeGenericMethod(interfaceMethod.ReturnType);

            generator.Emit(OpCodes.Callvirt, resolveMethod);
            generator.Emit(OpCodes.Stloc_0);
            generator.Emit(OpCodes.Ldloc_0);
            generator.Emit(OpCodes.Ret);

            typeBuilder.DefineMethodOverride(methodBuilder, interfaceMethod);
        }
    }
}