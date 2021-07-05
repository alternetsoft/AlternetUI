using System;
using System.Reflection;

namespace ApiGenerator.Api
{
    public class SyntheticParameterInfo : ParameterInfo
    {
        private readonly Type parameterType;
        private readonly string name;

        public SyntheticParameterInfo(Type parameterType, string name)
        {
            this.parameterType = parameterType;
            this.name = name;
        }

        public override Type ParameterType => parameterType;

        public override string Name => name;

        public override MemberInfo Member => DeclaringMember!;

        public MemberInfo? DeclaringMember { get; set; }
    }
}