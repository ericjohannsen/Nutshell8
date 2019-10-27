using System;
using System.Collections;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using IronPython.Hosting;
using System.Text;

namespace ch20
{
    public class ScriptingDemo
    {
        static public void RunCalc()
        {
            int result = (int)Calculate("2 * 3");
            Console.WriteLine(result);              // 6

            var list = (IEnumerable)Calculate("[1, 2, 3] + [4, 5]");
            foreach (int n in list) Console.Write(n);  // 12345
        }

        static public void RunPassingState()
        {
            // The following string could come from a file or database:
            string auditRule = "taxPaidLastYear / taxPaidThisYear > 2";

            ScriptEngine engine = Python.CreateEngine();

            ScriptScope scope = engine.CreateScope();
            scope.SetVariable("taxPaidLastYear", 20000m);
            scope.SetVariable("taxPaidThisYear", 8000m);

            ScriptSource source = engine.CreateScriptSourceFromString(auditRule, SourceCodeKind.Expression);

            bool auditRequired = (bool)source.Execute(scope);
            Console.WriteLine(auditRequired);   // True
        }
        static public void RunReturn()
        {
            string code = "result = input * 3";

            ScriptEngine engine = Python.CreateEngine();

            ScriptScope scope = engine.CreateScope();
            scope.SetVariable("input", 2);

            ScriptSource source = engine.CreateScriptSourceFromString(code, SourceCodeKind.SingleStatement);
            source.Execute(scope);
            Console.WriteLine(scope.GetVariable("result"));   // 6            
        }

        static public void RunMarshal()
        {
            string code = @"sb.Append (""World"")";

            ScriptEngine engine = Python.CreateEngine();

            ScriptScope scope = engine.CreateScope();
            var sb = new StringBuilder("Hello");
            scope.SetVariable("sb", sb);

            ScriptSource source = engine.CreateScriptSourceFromString(code, SourceCodeKind.SingleStatement);
            source.Execute(scope);
            string s = sb.ToString();
            Console.WriteLine(s);
        }

        static object Calculate(string expression)
        {
            ScriptEngine engine = Python.CreateEngine();
            return engine.Execute(expression);
        }
    }
}