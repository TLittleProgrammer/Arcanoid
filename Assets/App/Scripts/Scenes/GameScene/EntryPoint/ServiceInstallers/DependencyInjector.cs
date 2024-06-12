using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint.ServiceInstallers
{
    public static class DependencyInjector
    {
        public static TService CreateInstanceWithDependencies<TService>(TService service, DiContainer container)
        {
            Type type = service.GetType();

            ConstructorInfo constructor = type.GetConstructors()
                .OrderByDescending(c => c.GetParameters().Length)
                .First();
            
            ParameterInfo[] parameters = constructor.GetParameters();

            for (int i = 0; i < parameters.Length; i++)
            {
                string fieldName = "_" + parameters[i].Name;
                FieldInfo field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);

                field.SetValue(service, container.Resolve(field.FieldType));
            }
            
            MethodInfo methodInfo = type.GetMethod("Initialize", BindingFlags.Public | BindingFlags.Instance);
            
            if (methodInfo != null && methodInfo.ReturnType == typeof(void) && methodInfo.GetParameters().Length == 0)
            {
                methodInfo.Invoke(service, null);
            }

            return service;
        }
    }
}