using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace PumpService
{
    public class ScriptService : IScriptService
    {
        private CompilerResults _results;
        private readonly IStatisticService _statistic;
        private readonly ISettingsService _settings;
        private readonly IPumpServiceCallback _callback;
        public ScriptService(IStatisticService statistic, ISettingsService settings, IPumpServiceCallback callback)
        {
            _statistic = statistic;
            _settings = settings;
            _callback = callback;
        }
        public bool Compile()
        {
            try
            {
                CompilerParameters compilerParameters = new CompilerParameters();
                compilerParameters.GenerateInMemory = true;

                compilerParameters.ReferencedAssemblies.Add("System.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Core.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Data.dll");
                compilerParameters.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
                compilerParameters.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);

                FileStream fs = new FileStream(_settings.FileName, FileMode.Open);
                byte[] buffer;
                try
                {
                    int length = (int)fs.Length;
                    buffer = new byte[length];
                    int count, sum = 0;
                    while ((count = fs.Read(buffer, sum, length - sum)) > 0)
                        sum += count;
                }
                finally { fs.Close(); }

                CSharpCodeProvider provider = new CSharpCodeProvider();
                _results = provider.CompileAssemblyFromSource(compilerParameters, Encoding.UTF8.GetString(buffer));
                if (_results.Errors != null && _results.Errors.Count > 0)
                {
                    string errors = string.Empty;
                    StringBuilder sb = new StringBuilder();
                    foreach (string error in _results.Errors)
                    {
                        sb.Append(error + "\n");
                    }
                    errors = sb.ToString();
                    return false;
                }
                return true;

            }
            catch (Exception e)
            {
                return false;
            }            
        }

        public void Run(int count)
        {
            if (_results == null || (_results !=null&&_results.Errors != null && _results.Errors.Count>0))
                if (Compile()==false)
                    return;
            
            Type t = _results.CompiledAssembly.GetType("Sample.SampleScript");
            if (t == null) { return; }

            MethodInfo entry = t.GetMethod("EntryPoint");
            if (entry == null) { return; }

            Task.Run(() =>
            {
                for (int i = 0; i < count; i++)
                {
                    if ((bool)entry.Invoke(Activator.CreateInstance(t), null))
                        _statistic.SuccessTacts++;
                    else
                        _statistic.ErrorTacts++;

                    _statistic.AllTacts++;
                    _callback.UpadateStatistic((StatisticService)_statistic);
                    Thread.Sleep(1000);
                }
            });
        }
    }
}
            //compilerParameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");