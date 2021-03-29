using NaughtyAttributes;
using System;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class CallableListAttribute : SpecialCaseDrawerAttribute { }
