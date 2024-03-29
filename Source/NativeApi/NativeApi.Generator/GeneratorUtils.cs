﻿using System;

namespace ApiGenerator
{
    internal static class GeneratorUtils
    {
        public static string Copyright => $"Copyright (c) {DateTime.Now.Year} AlterNET Software";
        public static string HeaderText =>
            $"// <auto-generated> DO NOT MODIFY MANUALLY. {Copyright}.</auto-generated>";
    }
}