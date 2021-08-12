using System;
using System.Globalization;
using System.Reflection;

namespace ApiGenerator.Api
{
    internal class SyntheticMethodInfo : MethodInfo
    {
        private Type? declaringType;
        private string name;
        private readonly bool isStatic;
        private SyntheticParameterInfo[] parameters;
        private Type returnType;

        public SyntheticMethodInfo(Type? declaringType, string name, bool isStatic, SyntheticParameterInfo[] parameters, Type returnType)
        {
            this.declaringType = declaringType;
            this.name = name;
            this.isStatic = isStatic;
            this.parameters = parameters;
            this.returnType = returnType;

            foreach (var parameter in this.parameters)
                parameter.DeclaringMember = this;
        }

        public override ParameterInfo ReturnParameter
        {
            get
            {
                var result = new SyntheticParameterInfo(returnType, "");
                result.DeclaringMember = this;
                return result;
            }
        }

        public override ICustomAttributeProvider ReturnTypeCustomAttributes => throw new NotImplementedException();

        public override RuntimeMethodHandle MethodHandle => throw new NotImplementedException();

        public override MethodAttributes Attributes => MethodAttributes.Public | (isStatic ? MethodAttributes.Static : 0);

        public override Type ReflectedType => throw new NotImplementedException();

        public override Type? DeclaringType => declaringType;

        public override string Name => name;

        public override MethodInfo GetBaseDefinition()
        {
            throw new NotImplementedException();
        }

        public override Type ReturnType => returnType;

        public override object[] GetCustomAttributes(bool inherit)
        {
            return new object[0];
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        public override MethodImplAttributes GetMethodImplementationFlags()
        {
            throw new NotImplementedException();
        }

        public override ParameterInfo[] GetParameters()
        {
            return parameters;
        }

        public override object? Invoke(object? obj, BindingFlags invokeAttr, Binder? binder, object?[]? parameters, CultureInfo? culture)
        {
            throw new NotImplementedException();
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }
    }
}