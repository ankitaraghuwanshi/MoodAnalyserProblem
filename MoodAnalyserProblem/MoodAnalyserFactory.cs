﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MoodAnalyserSpace
{
    public class MoodAnalyserFactory
    {
        public static object GetMoodAnalyserObject(string ClassName, string ConstructorName)
        {
            string pattern = @"." + ConstructorName + "$";
            Match result = Regex.Match(ClassName, pattern);
            if (result.Success)
            {
                try
                {
                    Assembly assembly = Assembly.GetExecutingAssembly();
                    Type MoodAnalyserType = assembly.GetType(ClassName);
                    return Activator.CreateInstance(MoodAnalyserType);
                }
                catch (ArgumentNullException)
                {
                    throw new MoodAnalyserException(MoodAnalyserException.ExceptionType.NO_SUCH_CLASS, "class not found");
                }
            }
            else
                throw new MoodAnalyserException(MoodAnalyserException.ExceptionType.NO_SUCH_METHOD, "constructor not found");
        }
        public static object GetMoodAnalyserObjectWithParamterizedConstructor(string ClassName, string ConstructorName, string Message)
        {
            Type type = typeof(MoodAnalyser);
            if (type.Name.Equals(ClassName) || type.FullName.Equals(ClassName))
            {
                if (type.Name.Equals(ConstructorName))
                {
                    ConstructorInfo ctr = type.GetConstructor(new[] { typeof(string) });
                    object instance = ctr.Invoke(new object[] { Message });
                    return instance;
                }
                else
                {
                    throw new MoodAnalyserException(MoodAnalyserException.ExceptionType.NO_SUCH_METHOD, "constructor not found");
                }
            }
            else
                throw new MoodAnalyserException(MoodAnalyserException.ExceptionType.NO_SUCH_CLASS, "class not found");
        }
        public static string InvokeAnalyseMood(string Message, string MethodName)
        {
            try
            {
                Type type = Type.GetType("MoodAnalyserSpace.MoodAnalyser");
                object MoodAnalyserObject = GetMoodAnalyserObjectWithParamterizedConstructor("MoodAnalyserSpace.MoodAnalyser", "MoodAnalyser", Message);
                MethodInfo AnalyseMoodInfo = type.GetMethod(MethodName);
                object mood = AnalyseMoodInfo.Invoke(MoodAnalyserObject, null);
                return mood.ToString();
            }
            catch (NullReferenceException)
            {
                throw new MoodAnalyserException(MoodAnalyserException.ExceptionType.NO_SUCH_METHOD, "constructor not found");
            }
        }
        public static string SetField(string Message, string FieldName)
        {
            try
            {
                MoodAnalyser moodAnalyser = new MoodAnalyser();
                Type type = typeof(MoodAnalyser);
                if (Message == null)
                {
                    throw new MoodAnalyserException(MoodAnalyserException.ExceptionType.NULL_MOOD, "Mood should not be null");
                }
                FieldInfo Field = type.GetField(FieldName, BindingFlags.Public | BindingFlags.Instance);
                Field.SetValue(moodAnalyser, Message);
                return moodAnalyser.mood();
            }
            catch (NullReferenceException)
            {
                throw new MoodAnalyserException(MoodAnalyserException.ExceptionType.NO_SUCH_FIELDS, "field not found");
            }
        }
    }
}